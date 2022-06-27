using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Fazilat.Models;

namespace Fazilat.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<UserInformation> Information { get; set; }
        public virtual DbSet<EducationalFile> EducationalFiles { get; set; }
        public virtual DbSet<Curriculum> Curricula { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Adviser> Advisers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var adminUserId = Guid.NewGuid().ToString();
            var adminPasswordHash = "AQAAAAEAACcQAAAAEFYvVCymYdM4AxjjbasiBtTpgjb4mE7ITeWVcro877XzxNMbbbatKvNQy/PfbvTrLg==";

            var adminRoleId = Guid.NewGuid().ToString();
            var managerRoleId = Guid.NewGuid().ToString();
            var adviserRoleId = Guid.NewGuid().ToString();
            var userRoleId = Guid.NewGuid().ToString();

            modelBuilder.Entity<IdentityRole>(b =>
            {
                b.ToTable("Role");

                b.HasData(
                    new IdentityRole
                    {
                        Id = adminRoleId,
                        Name = "Administrator",
                        NormalizedName = "ADMINISTRATOR"
                    },
                    new IdentityRole
                    {
                        Id = managerRoleId,
                        Name = "Manager",
                        NormalizedName = "MANAGER"
                    },
                    new IdentityRole
                    {
                        Id = adviserRoleId,
                        Name = "Adviser",
                        NormalizedName = "ADVISER"
                    },
                    new IdentityRole
                    {
                        Id = userRoleId,
                        Name = "User",
                        NormalizedName = "USER"
                    });
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("RoleClaim");
            });

            modelBuilder.Entity<ApplicationUser>(b =>
            {
                b.ToTable("User");

                b.HasOne(u => u.Information)
                    .WithOne(i => i.User)
                    .HasForeignKey<UserInformation>(ui => ui.UserId)
                    .IsRequired();

                b.HasOne(u => u.EducationalFile)
                    .WithOne(ef => ef.User)
                    .HasForeignKey<EducationalFile>(uef => uef.UserId)
                    .IsRequired();

                b.HasMany(u => u.Curriculums)
                    .WithOne(c => c.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                b.HasMany(u => u.Messages)
                    .WithOne(m => m.Sender)
                    .HasForeignKey(um => um.SenderId)
                    .OnDelete(DeleteBehavior.NoAction)
                    .IsRequired();

                b.HasData(
                    new ApplicationUser()
                    {
                        Id = adminUserId,
                        UserName = "mortizanourani@gmail.com",
                        NormalizedUserName = "MORTIZANOURANI@GMAIL.COM",
                        Email = "mortizanourani@gmail.com",
                        NormalizedEmail = "MORTIZANOURANI@GMAIL.COM",
                        EmailConfirmed = true,
                        PasswordHash = adminPasswordHash,
                        PhoneNumber = "+989116069878",
                        PhoneNumberConfirmed = true,
                        AccessFailedCount = 0,
                        LockoutEnabled = false,
                        TwoFactorEnabled = false,
                    });
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("UserClaim");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("UserLogin");
            });

            modelBuilder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("UserRole");

                b.HasData(
                    new IdentityUserRole<string>
                    {
                        UserId = adminUserId,
                        RoleId = adminRoleId
                    },
                    new IdentityUserRole<string>
                    {
                        UserId = adminUserId,
                        RoleId = managerRoleId
                    },
                    new IdentityUserRole<string>
                    {
                        UserId = adminUserId,
                        RoleId = adviserRoleId
                    },
                    new IdentityUserRole<string>
                    {
                        UserId = adminUserId,
                        RoleId = userRoleId
                    });
            });

            modelBuilder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("UserToken");
            });

            modelBuilder.Entity<UserInformation>(b =>
            {
                b.HasKey(e => e.UserId);

                b.Property(e => e.NationalCode)
                    .HasColumnType("nvarchar(10)")
                    .HasMaxLength(10);

                b.Property(e => e.FirstName)
                    .HasColumnType("nvarchar(256)")
                    .HasMaxLength(256);

                b.Property(e => e.LastName)
                    .HasColumnType("nvarchar(256)")
                    .HasMaxLength(256);

                b.Property(e => e.BirthDate)
                    .HasColumnType("date");

                b.HasIndex(e => e.NationalCode)
                    .IsUnique();

                b.ToTable("UserInformation");
            });

            modelBuilder.Entity<EducationalFile>(b =>
            {
                b.HasKey(e => e.UserId);

                b.Property(e => e.Grade)
                    .HasColumnType("nvarchar(2)")
                    .HasMaxLength(2);

                b.Property(e => e.LastAvg)
                    .HasColumnType("nvarchar(5)")
                    .HasMaxLength(4);

                b.ToTable("UserEducationalFile");
            });

            modelBuilder.Entity<Curriculum>(b =>
            {
                b.HasKey(e => e.Id);

                b.Property(e => e.Title)
                    .HasColumnType("nvarchar(256)")
                    .HasMaxLength(256)
                    .IsRequired();

                b.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasDefaultValue(DateTime.Now.Date)
                    .IsRequired();

                b.HasMany(c => c.Courses)
                    .WithOne(i => i.Curriculum)
                    .HasForeignKey(ci => ci.CurriculumId)
                    .IsRequired();

                b.ToTable("Curriculum");
            });

            modelBuilder.Entity<Course>(b =>
            {
                b.HasKey(e => e.Id);

                b.Property(e => e.Title)
                    .HasColumnType("nvarchar(256)")
                    .HasMaxLength(256);

                b.Property(e => e.Accomplished)
                    .HasDefaultValue(false);

                b.ToTable("Course");
            });

            modelBuilder.Entity<Message>(b =>
            {
                b.HasKey(e => e.Id);

                b.Property(e => e.SenderId)
                    .IsRequired();

                b.Property(e => e.ReceiverId)
                    .IsRequired();

                b.Property(e => e.Text)
                    .HasColumnType("text")
                    .IsRequired();

                b.Property(e => e.Created)
                    .HasColumnType("datetime")
                    .HasDefaultValue(DateTime.Now.Date)
                    .IsRequired();

                b.ToTable("Message");
            });

            modelBuilder.Entity<Adviser>(b =>
            {
                b.HasKey(e => e.Id);

                b.Property(e => e.AdviserId)
                    .HasColumnType("nvarchar(450)")
                    .HasMaxLength(450)
                    .IsRequired();

                b.Property(e => e.StudentId)
                    .HasColumnType("nvarchar(450)")
                    .HasMaxLength(450)
                    .IsRequired();

                b.ToTable("Adviser");
            });
        }
    }
}
