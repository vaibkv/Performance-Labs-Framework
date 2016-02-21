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
using PerformanceLabsFramework.Helpers;
using System.Web;
using System.Data.SqlClient;
using StackExchange.Profiling.Storage;
using StackExchange.Profiling.SqlFormatters;

namespace PerformanceLabsFramework
{
    public class MVCMiniProfiler //: IPerfLabsProfiler
    {
        private static MVCMiniProfiler _mvcMiniProfiler;
        private static MiniProfiler _miniProfiler;

        private MVCMiniProfiler() { }

        internal void StartProfiler()
        {
            SetDefaultContext();
            StartMiniProfiler();
            SetDefaultProperties();
        }

        internal void StopProfiler()
        {
            MiniProfiler.Stop(false);
        }

        public static MVCMiniProfiler GetMVCMiniProfiler()
        {
            if (_mvcMiniProfiler == null)
            {
                _mvcMiniProfiler = new MVCMiniProfiler();
            }
            return _mvcMiniProfiler;
        }

        private void StartMiniProfiler()
        {
            MiniProfiler.Start();
            _miniProfiler = MiniProfiler.Current;
            MiniProfiler.Current.Name = "MVC Mini Profiler";
        }

        public MiniProfiler GetMiniProfiler()
        {
            return _miniProfiler;
        }

        private void SetDefaultProperties()
        {
            if (_miniProfiler != null)
            {
                MiniProfiler.Settings.ExcludeStackTraceSnippetFromSqlTimings = false;
                MiniProfiler.Settings.ShowControls = false;
                MiniProfiler.Settings.TrivialDurationThresholdMilliseconds = PerformanceLabsConfigurations.TrivialDurationThresholdMillisecondsMiniProfiler;
                MiniProfiler.Settings.Storage = new SqlServerStorage(PerformanceLabsConfigurations.connectionString);
                MiniProfiler.Settings.SqlFormatter = new SqlServerFormatter();
            }
        }

        private void SetDefaultContext()
        {
            HttpSimulator simulator = new HttpSimulator();
            simulator.SimulateRequest(new Uri("http://localhost/fakeContext.htm"));
        }

        public void SaveToDataBase()
        {
            if (_miniProfiler != null)
            {
                MiniProfiler.Settings.Storage.Save(_miniProfiler);
            }
        }
    }
}
