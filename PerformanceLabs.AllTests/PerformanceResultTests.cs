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

namespace PerformanceLabs.AllTests
{
    [TestFixture]
    public class PerformanceResultTests
    {
        private PerformanceResult testClass = null;
        private static PerformanceLabsFramework.PerformanceLabs perfLabs;

        [TestFixtureSetUp]
        public void Init()
        {
            perfLabs = SetupTests.perfLabs;
            testClass = new PerformanceResult();
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
    }
}