// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace PerformanceLabsFramework.Models
{
    public class MiniProfilerOperationThreshold
    {
        public int RowId { get; set; }
        public Guid MiniProfilerId { get; set; }
        public Nullable<Guid> ParentId { get; set; }
        public String OperationName { get; set; }
        public Nullable<Int64> ThresholdOperationTiming { get; set; }
    }
}