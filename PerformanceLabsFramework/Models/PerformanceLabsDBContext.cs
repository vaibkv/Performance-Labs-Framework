// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using PerformanceLabsFramework.Models.Configurations;
using PerformanceLabsFramework.Migrations;

namespace PerformanceLabsFramework.Models
{
    public class PerformanceLabsDBContext : DbContext, IPerformanceDBProxy
    {
        static PerformanceLabsDBContext()
        {
            Database.SetInitializer<PerformanceLabsDBContext>(new CreateDatabaseIfNotExists<PerformanceLabsDBContext>());
        }

        public PerformanceLabsDBContext()
            : base("PerformanceLabsDBContext")
        {
        }

        public DbSet<MiniProfilerClientTiming> MiniProfilerClientTimings { get; set; }
        public DbSet<MiniProfilers> MiniProfilers { get; set; }
        public DbSet<MiniProfilerSqlTimingParameter> MiniProfilerSqlTimingParameters { get; set; }
        public DbSet<MiniProfilerSqlTiming> MiniProfilerSqlTimings { get; set; }
        public DbSet<MiniProfilerTiming> MiniProfilerTimings { get; set; }
        public DbSet<RegressionArchive> RegressionArchives { get; set; }
        public DbSet<StopWatchProfiler> StopWatchProfilers { get; set; }
        public DbSet<StopWatchProfilerTiming> StopWatchProfilerTimings { get; set; }
        public DbSet<MiniProfilerOperationThreshold> MiniProfilerOperationThresholds { get; set; } 

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new MiniProfilerClientTimingConfiguration());
            modelBuilder.Configurations.Add(new MiniProfilerConfiguration());
            modelBuilder.Configurations.Add(new MiniProfilerSqlTimingParameterConfiguration());
            modelBuilder.Configurations.Add(new MiniProfilerSqlTimingConfiguration());
            modelBuilder.Configurations.Add(new MiniProfilerTimingConfiguration());
            modelBuilder.Configurations.Add(new RegressionArchiveConfiguration());
            modelBuilder.Configurations.Add(new StopWatchProfilerConfiguration());
            modelBuilder.Configurations.Add(new StopWatchProfilerTimingConfiguration());
            modelBuilder.Configurations.Add(new MiniProfilerOperationThresholdConfiguration());
        }
    }
}
