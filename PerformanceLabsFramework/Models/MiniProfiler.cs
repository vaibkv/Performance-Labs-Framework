// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace PerformanceLabsFramework.Models
{
    public class MiniProfilers
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public System.DateTime Started { get; set; }
        public string MachineName { get; set; }
        public string User { get; set; }
        public Nullable<byte> Level { get; set; }
        public Nullable<System.Guid> RootTimingId { get; set; }
        public decimal DurationMilliseconds { get; set; }
        public Nullable<decimal> DurationMillisecondsInSql { get; set; }
        public bool HasSqlTimings { get; set; }
        public bool HasDuplicateSqlTimings { get; set; }
        public bool HasTrivialTimings { get; set; }
        public bool HasAllTrivialTimings { get; set; }
        public Nullable<decimal> TrivialDurationThresholdMilliseconds { get; set; }
        public bool HasUserViewed { get; set; }
    }
}
