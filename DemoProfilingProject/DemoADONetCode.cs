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
using System.Data.SqlClient;
using NUnit.Framework;
using PerformanceLabsFramework;
using System.Reflection;

namespace DemoProfilingProject
{
    [TestFixture]
    public class DemoADONetCode
    {
        private PerformanceLabs _perfLabs;

        [TestFixtureSetUp]
        public void Init()
        {
            _perfLabs = SetupTests.perfLabs;
        }

        [Test]
        public void SqlConnectionTestOperation1()
        {
            using (_perfLabs.Step(Profiler.MVCMiniProfiler, "SqlConnectionTestOperation1"))
            {
                Thread.Sleep(Constants.bigTime);//simulate some work before db operation
                dynamic cnn = PerfLabsProfiledADOWrapper.PerfLabsDbConnection(new SqlConnection(Constants.demoDBConnectionString));
                cnn.Open();
                var cmd = cnn.CreateCommand();
                cmd.CommandText = Constants.bigQuery;
                cmd.ExecuteNonQuery();
                Thread.Sleep(Constants.bigTime);//simulate some work after db operation
                cnn.Close();
            }
        }

        [Test]
        public void SqlConnectionTestOperation2()
        {
            using (_perfLabs.Step(Profiler.MVCMiniProfiler, "SqlConnectionTestOperation2", 6000))
            {
                Thread.Sleep(Constants.bigTime);//simulate some work before db operation
                dynamic cnn = PerfLabsProfiledADOWrapper.PerfLabsDbConnection(new SqlConnection(Constants.demoDBConnectionString));
                cnn.Open();
                var cmd = cnn.CreateCommand();
                cmd.CommandText = Constants.bigQuery;
                cmd.ExecuteNonQuery();
                Thread.Sleep(Constants.bigTime);//simulate some work after db operation
                cnn.Close();
            }
        }

        [Test]
        public void SqlConnectionTestOperation3()
        {
            //profiling 1 times using smallTime -> emulating correct response time
            using (_perfLabs.Step(Profiler.MVCMiniProfiler, "SqlConnectionTestOperation3"))
            {
                SampleSqlConnectionTestOperation(true);
            }

            //cheating to generate Regression by clearing OperationNames. Not needed in actual scenario.
            _perfLabs.GetType().GetProperty("OperationNames").SetValue(_perfLabs, new HashSet<String>(), null);

            //emulating spike in response time
            using (_perfLabs.Step(Profiler.MVCMiniProfiler, "SqlConnectionTestOperation3"))
            {
                SampleSqlConnectionTestOperation(false);
            }
        }

        private void SampleSqlConnectionTestOperation(Boolean smallTime)
        {
            Thread.Sleep(smallTime ? Constants.smallTime : Constants.bigTime);//simulate some work before db operation
            dynamic cnn = PerfLabsProfiledADOWrapper.PerfLabsDbConnection(new SqlConnection(Constants.demoDBConnectionString));
            cnn.Open();
            var cmd = cnn.CreateCommand();
            cmd.CommandText = smallTime ? Constants.smallQuery : Constants.bigQuery;
            cmd.ExecuteNonQuery();
            Thread.Sleep(smallTime ? Constants.smallTime : Constants.bigTime);//simulate some work after db operation
            cnn.Close();
        }
    }
}