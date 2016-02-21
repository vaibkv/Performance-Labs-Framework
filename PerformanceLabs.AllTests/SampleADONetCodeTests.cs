// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using PerformanceLabs.AllTests.SampleProfilingProject;
using PerformanceLabsFramework;
using System.Data.Common;
using PerformanceLabsFramework.Models;
using PerformanceLabsFramework.Models.ValueObjects;

namespace PerformanceLabs.AllTests
{
    [TestFixture]
    public class SampleADONetCodeTests
    {
        private SampleADONetCode sampleCode = null;
        private static PerformanceLabsFramework.PerformanceLabs perfLabs;

        [TestFixtureSetUp]
        public void Init()
        {
            perfLabs = SetupTests.perfLabs;
            sampleCode = new SampleADONetCode();
        }

        [SetUp]
        public void Setup()
        {
            perfLabs.StartProfilers();
        }

        [TearDown]
        public void TearDown()
        {
            perfLabs.StopProfilers();
            TestHelper.Cleanup();
        }

        [Test]
        public void SqlConnectionTestOperation_SmallTimeTrue_SqlShouldBeCapturedByProfiler()
        {
            using (perfLabs.Step(Profiler.MVCMiniProfiler, "SqlConnectionTestOperation"))
            {
                sampleCode.SqlConnectionTestOperation(true);
            }

            perfLabs.SaveToDataBase();

            using (var context = new Test_PerformanceLabsDBContext())
            {
                Int32 count = (from mpct in context.MiniProfilerTimings
                               join mpst in context.MiniProfilerSqlTimings
                               on mpct.Id equals mpst.ParentTimingId
                               where mpct.Name == "SqlConnectionTestOperation"
                               select mpst.Id).Count();
                Assert.AreEqual(count, 1);
            }
        }

        [Test]
        public void SqlConnectionTestOperation_SmallTimeAndBigTimeForRegression_DbRegressionShouldBeCapturedByMiniProfiler()
        {
            //profiling 10 times using smallTime -> emulating several correct response times
            TestHelper.ProfileMultipleTimes("SqlConnectionTestOperation1", () => sampleCode.SqlConnectionTestOperation(true), 15, Profiler.MVCMiniProfiler);

            //emulating spike in response time
            TestHelper.ProfileMultipleTimes("SqlConnectionTestOperation1", () => sampleCode.SqlConnectionTestOperation(false), 1, Profiler.MVCMiniProfiler);

            PerformanceResult pResult = new PerformanceResult();
            IEnumerable<RegressionResult3> regressionResult3 = pResult.GetRegressionResult3();
            pResult.PopulateListForDbRegressions(regressionResult3);

            Assert.IsTrue(pResult.ListDbRegression.First().Name == "SqlConnectionTestOperation1");
            pResult.Dispose();
        }

        [Test]
        public void SqlConnectionTestOperation_OperationLevelThresholdsGiven_ShouldSaveThresholdDataInTableMiniProfilerOperationThresholds()
        {
            Int32 timing = 20;
            using (perfLabs.Step(Profiler.MVCMiniProfiler, "SqlConnectionTestOperation2", timing))
            {
                sampleCode.SqlConnectionTestOperation(true);
            }

            using (var context = new Test_PerformanceLabsDBContext())
            {
                Int32 count = (from mpot in context.MiniProfilerOperationThresholds
                               where mpot.OperationName == "SqlConnectionTestOperation2" && mpot.ThresholdOperationTiming == timing
                               select mpot.OperationName).Count();

                Assert.AreEqual(count, 1);
            }
        }

        [Test]
        public void SqlConnectionTestOperation_OperationLevelThresholdsGiven_ShouldSaveThresholdDataInStopWatchTimingColumn()
        {
            Int32 timing = 20;
            using (perfLabs.Step(Profiler.PerfLabsStopWatch, "SqlConnectionTestOperation3", timing))
            {
                sampleCode.SqlConnectionTestOperation(true);
            }

            perfLabs.SaveToDataBase();

            using (var context = new Test_PerformanceLabsDBContext())
            {
                Int32 count = (from swpt in context.StopWatchProfilerTimings
                               where swpt.Name == "SqlConnectionTestOperation3" && swpt.ThresholdOperationTiming == timing
                               select swpt.Name).Count();

                Assert.AreEqual(count, 1);
            }
        }
    }
}