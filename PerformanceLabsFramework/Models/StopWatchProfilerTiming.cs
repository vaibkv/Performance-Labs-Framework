// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace PerformanceLabsFramework.Models
{
    public class StopWatchProfilerTiming
    {
        public int RowId { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public Nullable<int> MethodRunCounts { get; set; }
        public Nullable<decimal> DurationMilliseconds { get; set; }
        public Nullable<decimal> AvgDurationMilliseconds { get; set; }
        public virtual StopWatchProfiler StopWatchProfiler { get; set; }
        public Nullable<Int64> ThresholdOperationTiming { get; set; }
    }
}
