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
    public class MiniProfilerSqlTimingParameterValueObject
    {
        public string Name { get; set; }
        public string DbType { get; set; }
        public Nullable<int> Size { get; set; }
        public string Value { get; set; }
        public System.Guid MiniProfilerId { get; set; }
        public System.Guid ParentSqlTimingId { get; set; }
    }
}
