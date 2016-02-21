// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using PerformanceLabsFramework.Helpers;
using PerformanceLabsFramework.Models;
using System.Data.Entity;

namespace PerformanceLabsFramework
{
    public class PerfLabsStopWatch : IDisposable
    {
        private static Decimal _trivialDurationThresholdMillisecondsStopWatch;
        private static String _user;
        private static String _profilerName;
        private static Stopwatch _stopWatch;
        private Int64 _startTicks;
        private static String _machineName;
        private static DateTime _started;

        public static Decimal DurationMilliseconds { get; set; }

        public static Int32 NumOfInvocations { get; set; }

        public static Int64 ThresholdOperationTiming { get; set; }

        public static Decimal AvgDurationMilliseconds { get; set; }

        public static String OperationName {get;set;}
        
        public static String ProfilerName
        {
            get
            {
                if (_profilerName == null)
                {
                    _profilerName = PerformanceLabsConfigurations.PerfLabsStopWatchName;
                }
                return _profilerName;
            }
            set
            {
                _profilerName = value;
            }
        }

        public static DateTime Started
        {
            get
            {
                if (_started == null)
                {
                    _started = DateTime.Now;
                }
                return _started;
            }
            set
            {
                _started = value;
            }
        }

        public static String MachineName 
        {
            get
            {
                if (String.IsNullOrEmpty(_machineName))
                {
                    _machineName = Environment.MachineName;
                }
                return _machineName;
            }
            set
            {
                _machineName = value;
            }
        }

        public static String User
        {
            get
            {
                if (String.IsNullOrEmpty(_user))
                {
                    _user = "127.0.0.1";
                }
                return _user;
            }
            set
            {
                _machineName = value;
            }
        }

        public static Decimal TrivialDurationThresholdMillisecondsStopWatch
        {
            get
            {
                if (_trivialDurationThresholdMillisecondsStopWatch == 0m)
                {
                    _trivialDurationThresholdMillisecondsStopWatch = PerformanceLabsConfigurations.TrivialDurationThresholdMillisecondsStopWatch;
                }
                return _trivialDurationThresholdMillisecondsStopWatch;
            }
            set
            {
                _trivialDurationThresholdMillisecondsStopWatch = value;
            }
        }

        static PerfLabsStopWatch()
        {
            _stopWatch = new Stopwatch();
        }

        public PerfLabsStopWatch(String operationName, Int64 thresholdOperationTiming, Int32 numOfInvocations, Action operation)
        {
            OperationName = operationName;
            NumOfInvocations = numOfInvocations;
            ThresholdOperationTiming = thresholdOperationTiming == 0 ? (long)PerformanceLabsConfigurations.ThresholdTrivialServerCodeTime : thresholdOperationTiming;
            _stopWatch.Restart();

            if (numOfInvocations > 1 && operation != null)
            {
                for (int i = 0; i < numOfInvocations; i++)
                {
                    operation();
                }
                //Dispose();
            }
        }

        public PerfLabsStopWatch() { }

        public static void SaveToDataBase()
        {
            if (DurationMilliseconds > TrivialDurationThresholdMillisecondsStopWatch)
            {
                Type t = (PerformanceLabsConfigurations.contextType == PerformanceLabDBContextType.MainContext) ? typeof(PerformanceLabsDBContext) : typeof(Test_PerformanceLabsDBContext);
                using (IPerformanceDBProxy context = (IPerformanceDBProxy)Activator.CreateInstance(t))
                {
                    var parentId = (from swProfiler in context.StopWatchProfilers
                                    orderby swProfiler.Started descending
                                    select swProfiler.RowId).First();
                    if (parentId != 0)
                    {
                        StopWatchProfilerTiming swProfilerTiming = new StopWatchProfilerTiming()
                        {
                            ParentId = parentId, 
                            Name = OperationName, 
                            MethodRunCounts = NumOfInvocations, 
                            DurationMilliseconds = DurationMilliseconds,
                            AvgDurationMilliseconds = AvgDurationMilliseconds,
                            ThresholdOperationTiming = ThresholdOperationTiming
                        };
                        context.StopWatchProfilerTimings.Add(swProfilerTiming);
                        context.SaveChanges();
                    }
                }
            }
        }

        public static Stopwatch GetStopWatch()
        {
            return _stopWatch;
        }

        public static void StopProfiler()
        {
            if (_stopWatch != null)
            {
                if (_stopWatch.IsRunning)
                {
                    _stopWatch.Reset();
                    _stopWatch.Stop();
                }
            }
        }

        public void Dispose()
        {
            if (_stopWatch != null)
            {
                _stopWatch.Stop();

                _startTicks = _stopWatch.ElapsedTicks;
                DurationMilliseconds = PerfLabsStopWatch.GetRoundedMilliseconds(_startTicks);
                AvgDurationMilliseconds = DurationMilliseconds / Convert.ToDecimal(NumOfInvocations);
                SaveToDataBase();
            }
        }

        private static Decimal GetRoundedMilliseconds(Int64 ticks)
        {
            Int64 z = 10000 * ticks;
            Decimal msTimesTen = (Int32)(z / Stopwatch.Frequency);
            return msTimesTen / 10;
        }

        public static void StartProfiler()
        {
            Type t = (PerformanceLabsConfigurations.contextType == PerformanceLabDBContextType.MainContext) ? typeof(PerformanceLabsDBContext) : typeof(Test_PerformanceLabsDBContext);
            using (IPerformanceDBProxy context = (IPerformanceDBProxy)Activator.CreateInstance(t))
            {
                StopWatchProfiler swProfiler = new StopWatchProfiler()
                {
                    ProfilerName = ProfilerName,
                    Started = DateTime.UtcNow,
                    MachineName = MachineName,
                    User = User,
                    TrivialDurationThresholdMilliseconds = TrivialDurationThresholdMillisecondsStopWatch
                };

                context.StopWatchProfilers.Add(swProfiler);
                context.SaveChanges();
            }
        }
    }
}
