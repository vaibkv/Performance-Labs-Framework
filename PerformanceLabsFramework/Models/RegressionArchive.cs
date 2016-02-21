// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;

namespace PerformanceLabsFramework.Models
{
    public class RegressionArchive
    {
        public int RowId { get; set; }
        public int ParentId { get; set; }
        public string NameOrCommandString { get; set; }
        public Nullable<decimal> DurationMilliseconds { get; set; }
        public int RegressionType { get; set; }
        public System.DateTime Started { get; set; }
    }
}
