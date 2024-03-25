using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Fazilat.Models;

public partial class FazilatContext : DbContext
{
    public FazilatContext()
    {
    }

    public FazilatContext(DbContextOptions<FazilatContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Adviser> Advisers { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<BlogPost> BlogPosts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Curriculum> Curricula { get; set; }

    public virtual DbSet<FinancialRecord> FinancialRecords { get; set; }

    public virtual DbSet<Media> Medias { get; set; }

    public virtual DbSet<Meeting> Meetings { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleClaim> RoleClaims { get; set; }

    public virtual DbSet<Slide> Slides { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<TicketInstruction> TicketInstructions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserClaim> UserClaims { get; set; }

    public virtual DbSet<UserEducationalFile> UserEducationalFiles { get; set; }

    public virtual DbSet<UserInformation> UserInformations { get; set; }

    public virtual DbSet<UserLimitation> UserLimitations { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<UserToken> UserTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Persian_100_CI_AI");

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasIndex(e => e.PostalCode, "IX_Addresses_PostalCode").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AddressLine).IsRequired();
            entity.Property(e => e.PostalCode)
                .IsRequired()
                .HasMaxLength(10);

            entity.HasOne(d => d.City).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Adviser>(entity =>
        {
            entity.ToTable("Adviser");

            entity.Property(e => e.AdviserId)
                .IsRequired()
                .HasMaxLength(450);
            entity.Property(e => e.StudentId)
                .IsRequired()
                .HasMaxLength(450);
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.Property(e => e.RoleId).IsRequired();

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);
            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.ToTable("BlogPost");

            entity.Property(e => e.Content)
                .IsRequired()
                .HasColumnType("text");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.IsVisible).HasColumnName("isVisible");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(256);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "IX_Categories_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256);
            entity.Property(e => e.NormalizedName)
                .IsRequired()
                .HasMaxLength(256);
            entity.Property(e => e.PersianName)
                .IsRequired()
                .HasMaxLength(256);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasIndex(e => e.Name, "IX_Cities_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256);

            entity.HasOne(d => d.Province).WithMany(p => p.Cities)
                .HasForeignKey(d => d.ProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.ToTable("Course");

            entity.HasIndex(e => e.CurriculumId, "IX_Course_CurriculumId");

            entity.Property(e => e.CurriculumId).IsRequired();
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(256);
            entity.Property(e => e.Topics).IsRequired();

            entity.HasOne(d => d.Curriculum).WithMany(p => p.Courses).HasForeignKey(d => d.CurriculumId);
        });

        modelBuilder.Entity<Curriculum>(entity =>
        {
            entity.ToTable("Curriculum");

            entity.HasIndex(e => e.UserId, "IX_Curriculum_UserId");

            entity.Property(e => e.StartDate).HasDefaultValue(new DateOnly(2022, 7, 27));
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(256);
            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(d => d.User).WithMany(p => p.Curricula).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<FinancialRecord>(entity =>
        {
            entity.ToTable("FinancialRecord");

            entity.HasIndex(e => e.UserId, "IX_FinancialRecord_UserId");

            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.TrackingCode).IsRequired();
            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(d => d.User).WithMany(p => p.FinancialRecords).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Media>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Extension).IsRequired();
            entity.Property(e => e.FileName).IsRequired();
            entity.Property(e => e.Uploaded)
                .IsRequired()
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd','fa-IR'))");

            entity.HasOne(d => d.Category).WithMany(p => p.Media)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Meeting>(entity =>
        {
            entity.ToTable("Meeting");

            entity.HasIndex(e => e.TicketId, "IX_Meeting_TicketId").IsUnique();

            entity.Property(e => e.Major).IsRequired();
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256);
            entity.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(11);
            entity.Property(e => e.TicketId).IsRequired();
            entity.Property(e => e.Type)
                .IsRequired()
                .HasDefaultValue("Online");

            entity.HasOne(d => d.Ticket).WithOne(p => p.Meeting).HasForeignKey<Meeting>(d => d.TicketId);
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.ToTable("Message");

            entity.HasIndex(e => e.ReceiverId, "IX_Message_ReceiverId");

            entity.HasIndex(e => e.SenderId, "IX_Message_SenderId");

            entity.Property(e => e.Created)
                .HasDefaultValue(new DateTime(2022, 7, 27, 0, 0, 0, 0, DateTimeKind.Unspecified))
                .HasColumnType("datetime");
            entity.Property(e => e.ReceiverId).IsRequired();
            entity.Property(e => e.SenderId).IsRequired();
            entity.Property(e => e.Text)
                .IsRequired()
                .HasColumnType("text");

            entity.HasOne(d => d.Receiver).WithMany(p => p.MessageReceivers).HasForeignKey(d => d.ReceiverId);

            entity.HasOne(d => d.Sender).WithMany(p => p.MessageSenders)
                .HasForeignKey(d => d.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasIndex(e => e.Name, "IX_Provinces_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("Role");

            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<RoleClaim>(entity =>
        {
            entity.ToTable("RoleClaim");

            entity.HasIndex(e => e.RoleId, "IX_RoleClaim_RoleId");

            entity.Property(e => e.RoleId).IsRequired();

            entity.HasOne(d => d.Role).WithMany(p => p.RoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<Slide>(entity =>
        {
            entity.ToTable("Slide");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Ticket");

            entity.Property(e => e.Day).IsRequired();
        });

        modelBuilder.Entity<TicketInstruction>(entity =>
        {
            entity.ToTable("TicketInstruction");

            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(256);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.Email, "IX_User_Email")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");

            entity.HasIndex(e => e.PhoneNumber, "IX_User_PhoneNumber")
                .IsUnique()
                .HasFilter("([PhoneNumber] IS NOT NULL)");

            entity.HasIndex(e => e.UserName, "IX_User_UserName")
                .IsUnique()
                .HasFilter("([UserName] IS NOT NULL)");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRole",
                    r => r.HasOne<Role>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<User>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("UserRole");
                        j.HasIndex(new[] { "RoleId" }, "IX_UserRole_RoleId");
                    });
        });

        modelBuilder.Entity<UserClaim>(entity =>
        {
            entity.ToTable("UserClaim");

            entity.HasIndex(e => e.UserId, "IX_UserClaim_UserId");

            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(d => d.User).WithMany(p => p.UserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<UserEducationalFile>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("UserEducationalFile");

            entity.Property(e => e.Grade).HasMaxLength(2);
            entity.Property(e => e.LastAvg).HasMaxLength(5);

            entity.HasOne(d => d.User).WithOne(p => p.UserEducationalFile).HasForeignKey<UserEducationalFile>(d => d.UserId);
        });

        modelBuilder.Entity<UserInformation>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("UserInformation");

            entity.Property(e => e.FirstName).HasMaxLength(256);
            entity.Property(e => e.LastName).HasMaxLength(256);

            entity.HasOne(d => d.User).WithOne(p => p.UserInformation).HasForeignKey<UserInformation>(d => d.UserId);
        });

        modelBuilder.Entity<UserLimitation>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("UserLimitation");

            entity.Property(e => e.Expiration).HasDefaultValue(new DateOnly(2022, 8, 6));

            entity.HasOne(d => d.User).WithOne(p => p.UserLimitation).HasForeignKey<UserLimitation>(d => d.UserId);
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.ToTable("UserLogin");

            entity.HasIndex(e => e.UserId, "IX_UserLogin_UserId");

            entity.Property(e => e.UserId).IsRequired();

            entity.HasOne(d => d.User).WithMany(p => p.UserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.ToTable("UserToken");

            entity.HasOne(d => d.User).WithMany(p => p.UserTokens).HasForeignKey(d => d.UserId);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
