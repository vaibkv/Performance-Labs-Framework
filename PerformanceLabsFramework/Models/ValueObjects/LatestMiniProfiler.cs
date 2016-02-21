// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace PerformanceLabsFramework.Models.ValueObjects
{
    public class LatestMiniProfiler
    {
        public System.Guid Id { get; set; }
        public DateTime Started { get; set; }
    }
}
