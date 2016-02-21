
// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using PerformanceLabsFramework;

namespace DemoProfilingProject
{
    [TestFixture]
    public class DemoCSharpCode
    {
        private PerformanceLabs _perfLabs;

        [TestFixtureSetUp]
        public void Init()
        {
            _perfLabs = SetupTests.perfLabs;
        }

        [Test]
        public void CSharpTestOperation1()
        {
            using (_perfLabs.Step(Profiler.PerfLabsStopWatch, "CSharpTestOperation1"))
            {
                Thread.Sleep(Constants.smallTime);//simulate doing work
            }
        }

        [Test]
        public void CSharpTestOperation2()
        {
            using (_perfLabs.Step(Profiler.PerfLabsStopWatch, "CSharpTestOperation2", 6000))
            {
                Thread.Sleep(Constants.bigTime);//simulate doing work
            }
        }

        [Test]
        public void CSharpTestOperation3()
        {
            using (_perfLabs.Step(Profiler.PerfLabsStopWatch, "CSharpTestOperation3", 0, 5, () => { Thread.Sleep(Constants.smallTime); }))
            { }
        }

        [Test]
        public void CSharpTestOperation4()
        {
            using (_perfLabs.Step(Profiler.PerfLabsStopWatch, "CSharpTestOperation4", 0, 3, () => { Thread.Sleep(Constants.bigTime); }))
            { }
        }

        [Test]
        public void CSharpTestOperation5()
        {
            using (_perfLabs.Step(Profiler.MVCMiniProfiler, "CSharpTestOperation5"))
            {
                Thread.Sleep(Constants.bigTime);
            }
        }

        [Test]
        public void CSharpTestOperation6()
        {
            using (_perfLabs.Step(Profiler.MVCMiniProfiler, "CSharpTestOperation6"))
            {
                Thread.Sleep(Constants.smallTime);
            }
        }

        private void SampleCSharpTestMethod(Boolean smallTime)
        {
            Thread.Sleep(smallTime ? Constants.smallTime : Constants.bigTime);
        }
    }
}