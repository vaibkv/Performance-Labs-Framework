// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace PerformanceLabsFramework.Models.Configurations
{
    public class StopWatchProfilerTimingConfiguration : EntityTypeConfiguration<StopWatchProfilerTiming>
    {
        public StopWatchProfilerTimingConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.RowId);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("StopWatchProfilerTimings");
            this.Property(t => t.RowId).HasColumnName("RowId");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.MethodRunCounts).HasColumnName("MethodRunCounts");
            this.Property(t => t.DurationMilliseconds).HasColumnName("DurationMilliseconds");
            this.Property(t => t.AvgDurationMilliseconds).HasColumnName("AvgDurationMilliseconds");
            this.Property(t => t.ThresholdOperationTiming).HasColumnName("ThresholdOperationTiming");

            // Relationships
            this.HasRequired(t => t.StopWatchProfiler)
                .WithMany(t => t.StopWatchProfilerTimings)
                .HasForeignKey(d => d.ParentId);

        }
    }
}
