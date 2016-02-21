// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace PerformanceLabsFramework.Models
{
    public class MiniProfilerTiming
    {
        public int RowId { get; set; }
        public System.Guid Id { get; set; }
        public System.Guid MiniProfilerId { get; set; }
        public Nullable<System.Guid> ParentTimingId { get; set; }
        public string Name { get; set; }
        public short Depth { get; set; }
        public decimal StartMilliseconds { get; set; }
        public Nullable<decimal> DurationMilliseconds { get; set; }
        public decimal DurationWithoutChildrenMilliseconds { get; set; }
        public Nullable<decimal> SqlTimingsDurationMilliseconds { get; set; }
        public bool IsRoot { get; set; }
        public bool HasChildren { get; set; }
        public bool IsTrivial { get; set; }
        public bool HasSqlTimings { get; set; }
        public bool HasDuplicateSqlTimings { get; set; }
        public short ExecutedReaders { get; set; }
        public short ExecutedScalars { get; set; }
        public short ExecutedNonQueries { get; set; }
    }
}