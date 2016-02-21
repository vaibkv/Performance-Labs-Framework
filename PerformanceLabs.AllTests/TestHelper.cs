// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PerformanceLabsFramework;
using PerformanceLabsFramework.Models;

namespace PerformanceLabs.AllTests
{
    public static class TestHelper
    {
        private static PerformanceLabsFramework.PerformanceLabs perfLabs;

        //NUnit - less debugging Helper method
        public static void Main(String[] args)
        {
            SetupTests st = new SetupTests();
            st.SetUp();
            SampleADONetCodeTests testClass = new SampleADONetCodeTests();
            testClass.Init();
            testClass.Setup();

            testClass.SqlConnectionTestOperation_SmallTimeAndBigTimeForRegression_DbRegressionShouldBeCapturedByMiniProfiler();

            testClass.TearDown();
            //st.Teardown();
        }

        static TestHelper()
        {
            if (SetupTests.perfLabs == null)
            {
                perfLabs = new PerformanceLabsFramework.PerformanceLabs();
            }
            else
            {
                perfLabs = SetupTests.perfLabs;
            }
        }

        public static void ProfileMultipleTimes(String profileName, Action operation, Int32 numOfInvocations, Profiler profilingOperation)
        {
            for (Int32 i = 0; i < numOfInvocations; i++)
            {
                perfLabs.StartProfilers();
                using (perfLabs.Step(profilingOperation, profileName))
                {
                    operation.Invoke();
                }
                perfLabs.StopProfilers();
            }
            perfLabs.StartProfilers();
        }

        public static void Cleanup()
        {
            using (var context = new Test_PerformanceLabsDBContext())
            {
                context.Database.ExecuteSqlCommand("delete from MiniProfilerSqlTimingParameters");
                context.Database.ExecuteSqlCommand("delete from MiniProfilerSqlTimings");
                context.Database.ExecuteSqlCommand("delete from MiniProfilerSqlTimings");
                context.Database.ExecuteSqlCommand("delete from MiniProfilerTimings");
                context.Database.ExecuteSqlCommand("delete from MiniProfilerClientTimings");
                context.Database.ExecuteSqlCommand("delete from MiniProfilers");

                context.Database.ExecuteSqlCommand("delete from StopWatchProfilers");
                context.Database.ExecuteSqlCommand("delete from MiniProfilers");

                context.Database.ExecuteSqlCommand("delete from RegressionArchive");
            }
        }
    }
}
