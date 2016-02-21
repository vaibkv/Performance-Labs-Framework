// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;

namespace PerformanceLabsFramework.Models.Configurations
{
    public class MiniProfilerSqlTimingConfiguration : EntityTypeConfiguration<MiniProfilerSqlTiming>
    {
        public MiniProfilerSqlTimingConfiguration()
        {
            // Primary Key
            this.HasKey(t => t.RowId);

            // Properties
            this.Property(t => t.StackTraceSnippet)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.CommandString)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("MiniProfilerSqlTimings");
            this.Property(t => t.RowId).HasColumnName("RowId");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.MiniProfilerId).HasColumnName("MiniProfilerId");
            this.Property(t => t.ParentTimingId).HasColumnName("ParentTimingId");
            this.Property(t => t.ExecuteType).HasColumnName("ExecuteType");
            this.Property(t => t.StartMilliseconds).HasColumnName("StartMilliseconds");
            this.Property(t => t.DurationMilliseconds).HasColumnName("DurationMilliseconds");
            this.Property(t => t.FirstFetchDurationMilliseconds).HasColumnName("FirstFetchDurationMilliseconds");
            this.Property(t => t.IsDuplicate).HasColumnName("IsDuplicate");
            this.Property(t => t.StackTraceSnippet).HasColumnName("StackTraceSnippet");
            this.Property(t => t.CommandString).HasColumnName("CommandString");
        }
    }
}
