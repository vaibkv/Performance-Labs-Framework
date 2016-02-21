// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace PerformanceLabsFramework.Models
{
    public interface IPerformanceDBProxy : IDisposable
    {
        DbSet<MiniProfilerClientTiming> MiniProfilerClientTimings { get; set; }
        DbSet<MiniProfilers> MiniProfilers { get; set; }
        DbSet<MiniProfilerSqlTimingParameter> MiniProfilerSqlTimingParameters { get; set; }
        DbSet<MiniProfilerSqlTiming> MiniProfilerSqlTimings { get; set; }
        DbSet<MiniProfilerTiming> MiniProfilerTimings { get; set; }
        DbSet<RegressionArchive> RegressionArchives { get; set; }
        DbSet<StopWatchProfiler> StopWatchProfilers { get; set; }
        DbSet<StopWatchProfilerTiming> StopWatchProfilerTimings { get; set; }
        DbSet<MiniProfilerOperationThreshold> MiniProfilerOperationThresholds { get; set; }
        Int32 SaveChanges();
        Database Database { get; }
    }
}