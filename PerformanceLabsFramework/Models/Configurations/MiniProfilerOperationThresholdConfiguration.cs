// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace PerformanceLabsFramework.Models.Configurations
{
    public class MiniProfilerOperationThresholdConfiguration : EntityTypeConfiguration<MiniProfilerOperationThreshold>
    {
        public MiniProfilerOperationThresholdConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.RowId);

            // Properties
            this.Property(t => t.OperationName)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("MiniProfilerOperationThresholds");
            this.Property(t => t.RowId).HasColumnName("RowId");
            this.Property(t => t.MiniProfilerId).HasColumnName("MiniProfilerId");
            this.Property(t => t.OperationName).HasColumnName("OperationName");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.ThresholdOperationTiming).HasColumnName("ThresholdOperationTiming");


        }
    }
}
