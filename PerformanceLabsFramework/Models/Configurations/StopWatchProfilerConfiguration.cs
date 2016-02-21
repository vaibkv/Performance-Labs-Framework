// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace PerformanceLabsFramework.Models.Configurations
{
    public class StopWatchProfilerConfiguration : EntityTypeConfiguration<StopWatchProfiler>
    {
        public StopWatchProfilerConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.RowId);

            // Properties
            this.Property(t => t.ProfilerName)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.MachineName)
                .HasMaxLength(100);

            this.Property(t => t.User)
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("StopWatchProfilers");
            this.Property(t => t.RowId).HasColumnName("RowId");
            this.Property(t => t.ProfilerName).HasColumnName("ProfilerName");
            this.Property(t => t.Started).HasColumnName("Started");
            this.Property(t => t.MachineName).HasColumnName("MachineName");
            this.Property(t => t.User).HasColumnName("User");
            this.Property(t => t.TrivialDurationThresholdMilliseconds).HasColumnName("TrivialDurationThresholdMilliseconds");
        }
    }
}
