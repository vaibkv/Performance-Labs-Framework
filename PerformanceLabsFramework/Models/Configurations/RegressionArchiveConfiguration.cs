// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace PerformanceLabsFramework.Models.Configurations
{
    public class RegressionArchiveConfiguration : EntityTypeConfiguration<RegressionArchive>
    {
        public RegressionArchiveConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.RowId);

            // Properties
            this.Property(t => t.NameOrCommandString)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("RegressionArchive");
            this.Property(t => t.RowId).HasColumnName("RowId");
            this.Property(t => t.ParentId).HasColumnName("ParentId");
            this.Property(t => t.NameOrCommandString).HasColumnName("NameOrCommandString");
            this.Property(t => t.DurationMilliseconds).HasColumnName("DurationMilliseconds");
            this.Property(t => t.RegressionType).HasColumnName("RegressionType");
            this.Property(t => t.Started).HasColumnName("Started");
        }
    }
}
