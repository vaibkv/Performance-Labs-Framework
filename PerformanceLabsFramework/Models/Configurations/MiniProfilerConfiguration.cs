// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace PerformanceLabsFramework.Models.Configurations
{
    public class MiniProfilerConfiguration : EntityTypeConfiguration<MiniProfilers>
    {
        public MiniProfilerConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.MachineName)
                .HasMaxLength(100);

            this.Property(t => t.User)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("MiniProfilers");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Started).HasColumnName("Started");
            this.Property(t => t.MachineName).HasColumnName("MachineName");
            this.Property(t => t.User).HasColumnName("User");
            this.Property(t => t.Level).HasColumnName("Level");
            this.Property(t => t.RootTimingId).HasColumnName("RootTimingId");
            this.Property(t => t.DurationMilliseconds).HasColumnName("DurationMilliseconds");
            this.Property(t => t.DurationMillisecondsInSql).HasColumnName("DurationMillisecondsInSql");
            this.Property(t => t.HasSqlTimings).HasColumnName("HasSqlTimings");
            this.Property(t => t.HasDuplicateSqlTimings).HasColumnName("HasDuplicateSqlTimings");
            this.Property(t => t.HasTrivialTimings).HasColumnName("HasTrivialTimings");
            this.Property(t => t.HasAllTrivialTimings).HasColumnName("HasAllTrivialTimings");
            this.Property(t => t.TrivialDurationThresholdMilliseconds).HasColumnName("TrivialDurationThresholdMilliseconds");
            this.Property(t => t.HasUserViewed).HasColumnName("HasUserViewed");
        }
    }
}
