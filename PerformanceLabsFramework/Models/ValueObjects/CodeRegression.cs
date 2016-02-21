// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PerformanceLabsFramework
{
    public class CodeRegression
    {
        public String Name { get; set; }
        public Decimal DurationMilliseconds { get; set; }
        public String Started { get; set; }
        public Nullable<Decimal> AvgDurationMilliseconds { get; set; }
        public Nullable<Int32> ParentId { get; set; }
        public Int32 RowId { get; set; }
        public Int32 MethodRunCounts { get; set; }
        public Nullable<Decimal> SqlTimingsDurationMilliseconds { get; set; }
        public Boolean HasSqlTimings { get; set; }
        public Boolean HasDuplicateSqlTimings { get; set; }
        public System.Guid Id { get; set; }
        public IOrderedEnumerable<DataRow> drAllPreviousTimingsSorted { get; set; }
        public Double CurrentZScore { get; set; }
        public Double ZScoreThreshold { get; set; }
        public Double CurrentMultiplicationFactor { get; set; }
        public Double MultiplicationFactor { get; set; }
    }
}