using System;
using System.Collections.Generic;
using Fazilat.Models;
using Microsoft.EntityFrameworkCore;

namespace Fazilat.Data;

public partial class FazilatContext : DbContext
{
    public FazilatContext()
    {
    }

    public FazilatContext(DbContextOptions<FazilatContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityReport> ActivityReports { get; set; }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Field> Fields { get; set; }

    public virtual DbSet<Grade> Grades { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<Media> Medias { get; set; }

    public virtual DbSet<Province> Provinces { get; set; }

    public virtual DbSet<Receipt> Receipts { get; set; }

    public virtual DbSet<Schedule> Schedules { get; set; }

    public virtual DbSet<ScheduleTask> ScheduleTasks { get; set; }

    public virtual DbSet<SleepReport> SleepReports { get; set; }

    public virtual DbSet<Slide> Slides { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ActivityReport>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.Day, e.TaskId });

            entity.Property(e => e.Day)
                .HasMaxLength(10)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd','fa-IR'))");

            entity.HasOne(d => d.Task).WithMany(p => p.ActivityReports)
                .HasForeignKey(d => d.TaskId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ActivityReports_Tasks_TaskId");

            entity.HasOne(d => d.User).WithMany(p => p.ActivityReports)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasIndex(e => e.PostalCode, "IX_Addresses_PostalCode").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.PostalCode).HasMaxLength(10);

            entity.HasOne(d => d.City).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull);
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

            entity.HasMany(d => d.Counsellors).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserCounsellor",
                    r => r.HasOne<AspNetUser>().WithMany()
                        .HasForeignKey("CounsellorId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<AspNetUser>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("UserId", "CounsellorId");
                        j.ToTable("UserCounsellors");
                    });

            entity.HasMany(d => d.Departments).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserDepartment",
                    r => r.HasOne<Department>().WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<AspNetUser>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("UserId", "DepartmentId");
                        j.ToTable("UserDepartments");
                    });

            entity.HasMany(d => d.Grades).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "UserGrade",
                    r => r.HasOne<Grade>().WithMany()
                        .HasForeignKey("GradeId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<AspNetUser>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("UserId", "GradeId");
                        j.ToTable("UserGrades");
                    });

            entity.HasMany(d => d.Parents).WithMany(p => p.UsersNavigation)
                .UsingEntity<Dictionary<string, object>>(
                    "UserParent",
                    r => r.HasOne<AspNetUser>().WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<AspNetUser>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("UserId", "ParentId");
                        j.ToTable("UserParents");
                    });

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

            entity.HasMany(d => d.Users).WithMany(p => p.Counsellors)
                .UsingEntity<Dictionary<string, object>>(
                    "UserCounsellor",
                    r => r.HasOne<AspNetUser>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<AspNetUser>().WithMany()
                        .HasForeignKey("CounsellorId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("UserId", "CounsellorId");
                        j.ToTable("UserCounsellors");
                    });

            entity.HasMany(d => d.UsersNavigation).WithMany(p => p.Parents)
                .UsingEntity<Dictionary<string, object>>(
                    "UserParent",
                    r => r.HasOne<AspNetUser>().WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    l => l.HasOne<AspNetUser>().WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.ClientSetNull),
                    j =>
                    {
                        j.HasKey("UserId", "ParentId");
                        j.ToTable("UserParents");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "IX_Categories_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
            entity.Property(e => e.PersianName).HasMaxLength(256);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasIndex(e => e.Name, "IX_Cities_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(256);

            entity.HasOne(d => d.Province).WithMany(p => p.Cities)
                .HasForeignKey(d => d.ProvinceId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasIndex(e => new { e.GradeId, e.Title }, "IX_Courses_Title").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Title).HasMaxLength(256);

            entity.HasOne(d => d.Grade).WithMany(p => p.Courses)
                .HasForeignKey(d => d.GradeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "IX_Departments_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
            entity.Property(e => e.PersianName).HasMaxLength(256);
        });

        modelBuilder.Entity<Field>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "IX_Fields_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
            entity.Property(e => e.PersianName).HasMaxLength(256);

            entity.HasOne(d => d.Department).WithMany(p => p.Fields)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Grade>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "IX_Grades_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
            entity.Property(e => e.PersianName).HasMaxLength(256);

            entity.HasOne(d => d.Field).WithMany(p => p.Grades)
                .HasForeignKey(d => d.FieldId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CustomerId).HasMaxLength(450);
            entity.Property(e => e.Discount).HasDefaultValueSql("((0))");
            entity.Property(e => e.DueDate).HasDefaultValueSql("(format(dateadd(day,(7),getdate()),'yyyy-MM-dd','fa-IR'))");
            entity.Property(e => e.Issued).HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd','fa-IR'))");

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Invoices_AspNetUsers_PayerId");
        });

        modelBuilder.Entity<Media>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Uploaded).HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd','fa-IR'))");

            entity.HasOne(d => d.Category).WithMany(p => p.Media)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Province>(entity =>
        {
            entity.HasIndex(e => e.Name, "IX_Provinces_Name").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Name).HasMaxLength(256);
        });

        modelBuilder.Entity<Receipt>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

            entity.HasOne(d => d.Transaction).WithMany(p => p.Receipts)
                .HasForeignKey(d => d.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Receipts_TransactionId");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CounsellorId).HasMaxLength(450);
            entity.Property(e => e.Finish).HasDefaultValueSql("(format(dateadd(day,(7),getdate()),'yyyy-MM-dd','fa-IR'))");
            entity.Property(e => e.Start).HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd','fa-IR'))");
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.Counsellor).WithMany(p => p.ScheduleCounsellors)
                .HasForeignKey(d => d.CounsellorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Schedules_AspNetUsers_ConsellorId");

            entity.HasOne(d => d.User).WithMany(p => p.ScheduleUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ScheduleTask>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Tasks");

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.IsDone).HasColumnName("isDone");

            entity.HasOne(d => d.Course).WithMany(p => p.ScheduleTasks)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Courses_CourseId");

            entity.HasOne(d => d.Schedule).WithMany(p => p.ScheduleTasks)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Schedules_ScheduleId");
        });

        modelBuilder.Entity<SleepReport>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.Day });

            entity.Property(e => e.Day)
                .HasMaxLength(10)
                .HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd','fa-IR'))");

            entity.HasOne(d => d.User).WithMany(p => p.SleepReports)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Slide>(entity =>
        {
            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Created).HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd','fa-IR'))");
            entity.Property(e => e.IsVisible).HasColumnName("isVisible");

            entity.HasOne(d => d.Image).WithMany(p => p.Slides)
                .HasForeignKey(d => d.ImageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Slides_Media_ImageId");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasIndex(e => e.TrackingCode, "IX_Transactions_TrackingCode").IsUnique();

            entity.Property(e => e.Id).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Paid).HasDefaultValueSql("(format(getdate(),'yyyy-MM-dd hh:mm:ss','fa-IR'))");
            entity.Property(e => e.Status).HasDefaultValueSql("((-1))");
            entity.Property(e => e.TrackingCode).HasMaxLength(256);

            entity.HasOne(d => d.Invoice).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Transactions_InvoiceId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
