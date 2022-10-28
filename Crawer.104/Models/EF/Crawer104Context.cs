using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Crawer._104.Models.EF
{
    public partial class Crawer104Context : DbContext
    {
        public Crawer104Context()
        {
        }

        public Crawer104Context(DbContextOptions<Crawer104Context> options)
            : base(options)
        {
        }

        public virtual DbSet<公司> 公司s { get; set; } = null!;
        public virtual DbSet<職缺> 職缺s { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<公司>(entity =>
            {
                entity.ToTable("公司");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.公司編號)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.性質).HasMaxLength(10);
            });

            modelBuilder.Entity<職缺>(entity =>
            {
                entity.ToTable("職缺");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.公司id).HasColumnName("公司ID");

                entity.Property(e => e.出現日期).HasColumnType("datetime");

                entity.Property(e => e.工作編號)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.年資).HasMaxLength(10);

                entity.Property(e => e.建立時間).HasColumnType("datetime");

                entity.Property(e => e.更新時間).HasColumnType("datetime");

                entity.Property(e => e.最低薪)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.最高薪)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.薪水說明).HasMaxLength(50);

                entity.HasOne(d => d.公司)
                    .WithMany(p => p.職缺s)
                    .HasForeignKey(d => d.公司id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_職缺_公司");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
