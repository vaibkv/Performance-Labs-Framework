// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerformanceLabsFramework.Models.ValueObjects
{
    public class NamedSlowCode
    {
        public Nullable<System.Guid> Id { get; set; }
        public Nullable<System.Guid> MiniProfilerId { get; set; }
        public Nullable<System.Guid> ParentTimingId { get; set; }
        public String Name { get; set; }
        public Nullable<Int32> MethodRunCounts { get; set; }
        public Nullable<Decimal> DurationMilliseconds { get; set; }
        public Nullable<Decimal> AvgDurationMilliseconds { get; set; }
        public Nullable<Decimal> DurationWithoutChildrenMilliseconds { get; set; }
        public Nullable<Decimal> SqlTimingsDurationMilliseconds { get; set; }
        public Boolean HasSqlTimings { get; set; }
        public Boolean HasDuplicateSqlTimings { get; set; }
        public DateTime Started { get; set; }
        public Nullable<Int64> ThresholdOperationTiming { get; set; }
    }
}
