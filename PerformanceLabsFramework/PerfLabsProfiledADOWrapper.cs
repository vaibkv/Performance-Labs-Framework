// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System.Data.Common;

namespace PerformanceLabsFramework
{
    public static class PerfLabsProfiledADOWrapper
    {
        public static dynamic PerfLabsDbConnection(DbConnection connection)
        {
            return new ProfiledDbConnection(connection, MiniProfiler.Current);
        }
    }
}
