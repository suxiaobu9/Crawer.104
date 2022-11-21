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

        public virtual DbSet<Company> Companies { get; set; } = null!;
        public virtual DbSet<JobDescription> JobDescriptions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(entity =>
            {
                entity.ToTable("Company");

                entity.HasIndex(e => e.CompanyNo, "IX_公司")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CompanyName).HasComment("公司名稱");

                entity.Property(e => e.CompanyNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("公司編號");

                entity.Property(e => e.CompanyUrl).HasComment("公司網址");

                entity.Property(e => e.Ignore).HasComment("忽視");

                entity.Property(e => e.Property)
                    .HasMaxLength(10)
                    .HasComment("性質");

                entity.Property(e => e.Welfare).HasComment("福利");
            });

            modelBuilder.Entity<JobDescription>(entity =>
            {
                entity.ToTable("JobDescription");

                entity.HasIndex(e => e.JobNo, "IX_職缺")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AppearDate)
                    .HasColumnType("datetime")
                    .HasComment("出現日期");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasComment("建立時間");

                entity.Property(e => e.DetailUrl).HasComment("詳細內容網址");

                entity.Property(e => e.HaveRead).HasComment("已讀");

                entity.Property(e => e.HighestSalary)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("最高薪");

                entity.Property(e => e.IsDeleted).HasComment("被刪除");

                entity.Property(e => e.JobName).HasComment("工作名稱");

                entity.Property(e => e.JobNo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasComment("工作編號");

                entity.Property(e => e.JobPlace).HasComment("工作地點");

                entity.Property(e => e.MinimunSalary)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasComment("最低薪");

                entity.Property(e => e.RemoteWork).HasComment("遠端工作");

                entity.Property(e => e.Request).HasComment("要求");

                entity.Property(e => e.SalaryDescription)
                    .HasMaxLength(50)
                    .HasComment("薪水說明");

                entity.Property(e => e.Seniority)
                    .HasMaxLength(10)
                    .HasComment("年資");

                entity.Property(e => e.Tags).HasComment("標記");

                entity.Property(e => e.UpdatedDate)
                    .HasColumnType("datetime")
                    .HasComment("更新時間");

                entity.Property(e => e.WorkContent).HasComment("工作內容");

                entity.Property(e => e.WorkingHour).HasComment("上班時間");

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.JobDescriptions)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_職缺_公司");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
