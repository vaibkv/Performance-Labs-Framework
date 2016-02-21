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
using System.Configuration;
using System.IO;
using PerformanceLabsFramework;
using System.Reflection;
using System.Data.SqlClient;
using PerformanceLabsFramework.Models;

namespace DemoProfilingProject
{
    [SetUpFixture]
    public class SetupTests
    {
        public static PerformanceLabs perfLabs;

        [SetUp]
        public void SetUp()
        {
            //This has nothing to do with PeformanceLabs. This is just to create a sample DB for profiling
            CreateDemoDB();

            perfLabs = new PerformanceLabs();
            MakeProfilerSettings();
            perfLabs.StartProfilers();
        }

        [TearDown]
        public void Teardown()
        {
            perfLabs.StopProfilers();

            //This has nothing to do with PeformanceLabs. This is just to delete the sample DB created for profiling
            DeleteDemoDB();
        }

        private void MakeProfilerSettings()
        {
            PerformanceLabsConfigurations.connectionString = ConfigurationManager.ConnectionStrings[Constants.PerformanceLabsDBContext].ConnectionString;
            PerformanceLabsConfigurations.DirReportCreation = Path.Combine(Environment.CurrentDirectory, @"..\..\..\PerformanceReport\DemoReport.html");
            PerformanceLabsConfigurations.TrivialDurationThresholdMillisecondsStopWatch = 10;
            PerformanceLabsConfigurations.TrivialDurationThresholdMillisecondsMiniProfiler = 10;
            PerformanceLabsConfigurations.ThresholdZScore = 1.5;
            PerformanceLabsConfigurations.ThresholdTrivialSpecificOpTime = 2;
            PerformanceLabsConfigurations.ThresholdTrivialServerCodeTime = 5000;
            PerformanceLabsConfigurations.ThresholdTrivialDbOpTime = 0;
            PerformanceLabsConfigurations.MultiplicationFactor = 5;
            PerformanceLabsConfigurations.contextType = PerformanceLabDBContextType.TestContext;
            PerformanceLabsConfigurations.ShowDbRegression = false;
            PerformanceLabsConfigurations.ShowSlowDbOperations = false;
            PerformanceLabsConfigurations.GenerateReport = true;
            PerformanceLabsConfigurations.ProductName = "Demo Project";
        }

        private void CreateDemoDB()
        {
            SqlConnection conn = new SqlConnection(Constants.demoDBConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(Constants.createDemoDB, conn);
            cmd.ExecuteNonQuery();
            cmd.CommandText = Constants.CreateTables;
            cmd.ExecuteNonQuery();
            cmd.CommandText = Constants.populateDemoDB;
            cmd.ExecuteNonQuery();
            conn.Close();
        }

        private void DeleteDemoDB()
        {
            SqlConnection conn = new SqlConnection(Constants.demoDBConnectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand(Constants.deleteDemoDB, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}