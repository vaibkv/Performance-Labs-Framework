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
    public class DbRegression
    {
        public String CommandString { get; set; }
        public String StackTraceSnippet { get; set; }
        public String Started { get; set; }
        public Decimal DurationMilliseconds { get; set; }
        public Int32 RowId { get; set; }
        public String Name { get; set; }
    }
}
