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
        }
    }
}
