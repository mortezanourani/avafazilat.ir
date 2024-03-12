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

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Media> Medias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Persian_100_CI_AI");

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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
