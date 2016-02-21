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
using System.Data.Common;
using PerformanceLabsFramework;
using PerformanceLabsFramework.Models.ValueObjects;
using PerformanceLabsFramework.Models;
using System.Data.Entity.Infrastructure;
using System.Transactions;
using System.Threading;

namespace PerformanceLabs.AllTests
{
    [TestFixture]
    public class SampleCSharpCodeTests
    {
        private SampleCSharpCode sampleCode = null;
        private static PerformanceLabsFramework.PerformanceLabs perfLabs;

        [TestFixtureSetUp]
        public void Init()
        {
            perfLabs = SetupTests.perfLabs;
            sampleCode = new SampleCSharpCode();
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
        public void CSharpTestOperation1_SmallTimeTrue_ShouldBeCapturedByProfiler()
        {
            using (perfLabs.Step(Profiler.MVCMiniProfiler, "CSharpTestOperation1"))
            {
                sampleCode.CSharpTestOperation1(true);
            }

            perfLabs.SaveToDataBase();

            using (var context = new Test_PerformanceLabsDBContext())
            {
                Int32 count = (from mpt in context.MiniProfilerTimings
                               where mpt.Name == "CSharpTestOperation1"
                               select mpt.Name).Count();
                Assert.AreEqual(count, 1);
            }
        }

        [Test]
        public void CSharpTestOperation1_SmallTimeTrue_ShouldBeCapturedByStopWatchProfiler()
        {
            using (perfLabs.Step(Profiler.PerfLabsStopWatch, "CSharpTestOperation2"))
            {
                sampleCode.CSharpTestOperation1(true);
            }

            using (var context = new Test_PerformanceLabsDBContext())
            {
                Int32 count = (from swpt in context.StopWatchProfilerTimings
                               where swpt.Name == "CSharpTestOperation2"
                               select swpt.Name).Count();

                Assert.AreEqual(count, 1);
            }
        }

        [Test]
        public void CSharpTestOperation1_SmallTimeTrueWithMultipleRuns_ShouldBeCapturedByStopWatchProfiler()
        {
            Int32 methodRunCount = 5;
            using (perfLabs.Step(Profiler.PerfLabsStopWatch, "CSharpTestOperation3", 0, methodRunCount))
            {
                sampleCode.CSharpTestOperation1(true);
            }

            using (var context = new Test_PerformanceLabsDBContext())
            {
                Int32 count = (from swpt in context.StopWatchProfilerTimings
                               where swpt.Name == "CSharpTestOperation3" && swpt.MethodRunCounts == methodRunCount
                               select swpt.Name).Count();

                Assert.AreEqual(count, 1);
            }
        }

        [Test]
        public void CSharpTestOperation1_SmallTimeAndBigTimeForRegression_ShouldBeCapturedByStopWatchProfiler()
        {
            //profiling 10 times using smallTime -> emulating several correct response times
            TestHelper.ProfileMultipleTimes("CSharpTestOperation4", () => sampleCode.CSharpTestOperation1(true), 10, Profiler.PerfLabsStopWatch);

            //emulating spike in response time
            TestHelper.ProfileMultipleTimes("CSharpTestOperation4", () => sampleCode.CSharpTestOperation1(false), 1, Profiler.PerfLabsStopWatch);

            PerformanceResult pResult = new PerformanceResult();
            IEnumerable<RegressionResult1> regressionResult1 = pResult.GetRegressionResult1();
            pResult.PopulateListForSWCodeRegressions(regressionResult1);

            Assert.IsTrue(pResult.ListCodeRegressions.First().Name == "CSharpTestOperation4");
            pResult.Dispose();
        }

        [Test]
        public void CSharpTestOperation1_SmallTimeAndBigTimeForRegression_ShouldBeCapturedByMiniProfiler()
        {
            //profiling 10 times using smallTime -> emulating several correct response times
            TestHelper.ProfileMultipleTimes("CSharpTestOperation5", () => sampleCode.CSharpTestOperation1(true), 10, Profiler.MVCMiniProfiler);

            //emulating spike in response time
            TestHelper.ProfileMultipleTimes("CSharpTestOperation5", () => sampleCode.CSharpTestOperation1(false), 1, Profiler.MVCMiniProfiler);

            PerformanceResult pResult = new PerformanceResult();
            IEnumerable<RegressionResult2> regressionResult2 = pResult.GetRegressionResult2();
            pResult.PopulateListForMiniProfilerCodeRegressions(regressionResult2);

            Assert.IsTrue(pResult.ListCodeRegressions.First().Name == "CSharpTestOperation5");
            pResult.Dispose();
        }

        [Test]
        public void CSharpTestOperation1_SmallTimeMultipleRunPassingFunction_ShouldBeCapturedByStopWatchProfiler()
        {
            using (perfLabs.Step(Profiler.PerfLabsStopWatch, "AnonymousOperation1", 0, 2, () => sampleCode.CSharpTestOperation1())) { }

            using (var context = new Test_PerformanceLabsDBContext())
            {
                Int32 count = (from swpt in context.StopWatchProfilerTimings
                               where swpt.Name == "AnonymousOperation1" && swpt.MethodRunCounts == 2
                               select swpt.Name).Count();

                Assert.AreEqual(count, 1);
            }
        }

        [Test]
        public void CSharpTestOperation1_OperationLevelThresholdsGiven_ShouldSaveThresholdDataInStopWatchTimingColumn()
        {
            Int32 timing = 20;
            using (perfLabs.Step(Profiler.PerfLabsStopWatch, "CSharpTestOperation6", timing))
            {
                sampleCode.CSharpTestOperation1(true);
            }

            using (var context = new Test_PerformanceLabsDBContext())
            {
                Int32 count = (from swpt in context.StopWatchProfilerTimings
                               where swpt.Name == "CSharpTestOperation6" && swpt.ThresholdOperationTiming == timing
                               select swpt.Name).Count();

                Assert.AreEqual(count, 1);
            }
        }

        [Test]
        public void CSharpTestOperation1_OperationLevelThresholdsGiven_ShouldSaveThresholdDataInTableMiniProfilerOperationThresholds()
        {
            Int32 timing = 20;
            using (perfLabs.Step(Profiler.MVCMiniProfiler, "CSharpTestOperation7", timing))
            {
                sampleCode.CSharpTestOperation1(true);
            }

            perfLabs.SaveToDataBase();

            using (var context = new Test_PerformanceLabsDBContext())
            {
                Int32 count = (from mpot in context.MiniProfilerOperationThresholds
                               where mpot.OperationName == "CSharpTestOperation7" && mpot.ThresholdOperationTiming == timing
                               select mpot.OperationName).Count();

                Assert.AreEqual(count, 1);
            }
        }
    }
}