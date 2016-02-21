// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace PerformanceLabsFramework.Models
{
    public class StopWatchProfiler
    {
        public StopWatchProfiler()
        {
            this.StopWatchProfilerTimings = new List<StopWatchProfilerTiming>();
        }

        public int RowId { get; set; }
        public string ProfilerName { get; set; }
        public System.DateTime Started { get; set; }
        public string MachineName { get; set; }
        public string User { get; set; }
        public Nullable<decimal> TrivialDurationThresholdMilliseconds { get; set; }
        public virtual ICollection<StopWatchProfilerTiming> StopWatchProfilerTimings { get; set; }
    }
}
