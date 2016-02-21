// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace PerformanceLabsFramework.Models
{
    public class MiniProfilerSqlTiming
    {
        public int RowId { get; set; }
        public System.Guid Id { get; set; }
        public System.Guid MiniProfilerId { get; set; }
        public System.Guid ParentTimingId { get; set; }
        public byte ExecuteType { get; set; }
        public decimal StartMilliseconds { get; set; }
        public decimal DurationMilliseconds { get; set; }
        public Nullable<decimal> FirstFetchDurationMilliseconds { get; set; }
        public bool IsDuplicate { get; set; }
        public string StackTraceSnippet { get; set; }
        public string CommandString { get; set; }
    }
}
