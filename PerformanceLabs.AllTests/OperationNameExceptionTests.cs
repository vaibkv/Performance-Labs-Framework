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
using PerformanceLabsFramework;
using PerformanceLabs.AllTests.SampleProfilingProject;
using PerformanceLabsFramework.Models;
using PerformanceLabsFramework.PerformanceLabsExceptionsAndLogging;

namespace PerformanceLabs.AllTests
{
    [TestFixture]
    public class OperationNameExceptionTests
    {
        private SampleCSharpCode sampleCode;
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
        public void OperationNameException_MiniProfilerOptionSameOpNameTwice_ShouldThrowExceptionInClientCode()
        {
            using (perfLabs.Step(Profiler.MVCMiniProfiler, "OperationName1"))
            {
                sampleCode.CSharpTestOperation1(true);
            }

            Assert.Throws<OperationNameException>(() =>
            {
                using (perfLabs.Step(Profiler.MVCMiniProfiler, "OperationName1"))
                {
                    sampleCode.CSharpTestOperation1(true);
                }
            });
        }

        [Test]
        public void OperationNameException_StopWatchOptionSameOpNameTwice_ShouldThrowExceptionInClientCode()
        {
            using (perfLabs.Step(Profiler.PerfLabsStopWatch, "OperationName2"))
            {
                sampleCode.CSharpTestOperation1(true);
            }

            Assert.Throws<OperationNameException>(() =>
                {
                    using (perfLabs.Step(Profiler.PerfLabsStopWatch, "OperationName2"))
                    {
                        sampleCode.CSharpTestOperation1(true);
                    }
                });
        }

        [Test]
        public void OperationNameException_MixedOptionsSameOpNameTwice_ShouldThrowExceptionInClientCode()
        {
            using (perfLabs.Step(Profiler.PerfLabsStopWatch, "OperationName3"))
            {
                sampleCode.CSharpTestOperation1(true);
            }

            Assert.Throws<OperationNameException>(() =>
                {
                    using (perfLabs.Step(Profiler.MVCMiniProfiler, "OperationName3"))
                    {
                        sampleCode.CSharpTestOperation1(true);
                    }
                });
        }
    }
}
