// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace PerformanceLabsFramework.Models.Configurations
{
    public class MiniProfilerClientTimingConfiguration : EntityTypeConfiguration<MiniProfilerClientTiming>
    {
        public MiniProfilerClientTimingConfiguration()
        {
            // Primary Key
            this.HasKey(t => new { t.MiniProfilerId, t.Name });

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("MiniProfilerClientTimings");
            this.Property(t => t.MiniProfilerId).HasColumnName("MiniProfilerId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Start).HasColumnName("Start");
            this.Property(t => t.Duration).HasColumnName("Duration");
        }
    }
}
