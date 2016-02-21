// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace PerformanceLabsFramework.Models.Configurations
{
    public class MiniProfilerTimingConfiguration : EntityTypeConfiguration<MiniProfilerTiming>
    {
        public MiniProfilerTimingConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.RowId);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("MiniProfilerTimings");
            this.Property(t => t.RowId).HasColumnName("RowId");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MiniProfilerId).HasColumnName("MiniProfilerId");
            this.Property(t => t.ParentTimingId).HasColumnName("ParentTimingId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Depth).HasColumnName("Depth");
            this.Property(t => t.StartMilliseconds).HasColumnName("StartMilliseconds");
            this.Property(t => t.DurationMilliseconds).HasColumnName("DurationMilliseconds");
            this.Property(t => t.DurationWithoutChildrenMilliseconds).HasColumnName("DurationWithoutChildrenMilliseconds");
            this.Property(t => t.SqlTimingsDurationMilliseconds).HasColumnName("SqlTimingsDurationMilliseconds");
            this.Property(t => t.IsRoot).HasColumnName("IsRoot");
            this.Property(t => t.HasChildren).HasColumnName("HasChildren");
            this.Property(t => t.IsTrivial).HasColumnName("IsTrivial");
            this.Property(t => t.HasSqlTimings).HasColumnName("HasSqlTimings");
            this.Property(t => t.HasDuplicateSqlTimings).HasColumnName("HasDuplicateSqlTimings");
            this.Property(t => t.ExecutedReaders).HasColumnName("ExecutedReaders");
            this.Property(t => t.ExecutedScalars).HasColumnName("ExecutedScalars");
            this.Property(t => t.ExecutedNonQueries).HasColumnName("ExecutedNonQueries");
        }
    }
}
