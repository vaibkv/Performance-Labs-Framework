// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace PerformanceLabsFramework.Models.Configurations
{
    public class MiniProfilerSqlTimingParameterConfiguration : EntityTypeConfiguration<MiniProfilerSqlTimingParameter>
    {
        public MiniProfilerSqlTimingParameterConfiguration()
        {
            // Primary Key
            this.HasKey(t => new { t.MiniProfilerId, t.ParentSqlTimingId, t.Name });

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(130);

            this.Property(t => t.DbType)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("MiniProfilerSqlTimingParameters");
            this.Property(t => t.MiniProfilerId).HasColumnName("MiniProfilerId");
            this.Property(t => t.ParentSqlTimingId).HasColumnName("ParentSqlTimingId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.DbType).HasColumnName("DbType");
            this.Property(t => t.Size).HasColumnName("Size");
            this.Property(t => t.Value).HasColumnName("Value");
        }
    }
}
