// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace PerformanceLabsFramework.Models
{
    public class MiniProfilerClientTiming
    {
        public System.Guid MiniProfilerId { get; set; }
        public string Name { get; set; }
        public Nullable<decimal> Start { get; set; }
        public Nullable<decimal> Duration { get; set; }
    }
}
