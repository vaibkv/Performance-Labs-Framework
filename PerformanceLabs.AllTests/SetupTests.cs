// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using NUnit.Framework;
using System.Configuration;
using PerformanceLabsFramework;
using System.Reflection;
using System.IO;
using PerformanceLabsFramework.Models;
using System.Data.SqlClient;
using PerformanceLabs.AllTests.SampleProfilingProject;

namespace PerformanceLabs.AllTests
{
    [SetUpFixture]
    public class SetupTests
    {
        public static PerformanceLabsFramework.PerformanceLabs perfLabs;
        public TestDBBlogContext context = null;
        public Test_PerformanceLabsDBContext contextTestDBPLabs = null;
        
        [SetUp]
        public void SetUp()
        {
            ///DO NOT DELETE THIS COMMENTED CODE
            //string nunitConfig = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            //string appConfig = Path.GetFileName(Assembly.GetExecutingAssembly().Location) + ".config";
            //File.Copy(appConfig, nunitConfig, true);
            
            context = new TestDBBlogContext();
            context.Database.CreateIfNotExists();
           
            contextTestDBPLabs = new Test_PerformanceLabsDBContext();
            contextTestDBPLabs.Database.CreateIfNotExists();
            
            perfLabs = new PerformanceLabsFramework.PerformanceLabs();
            MakeProfilerSettings();
            perfLabs.StartProfilers();
        }    

        private void MakeProfilerSettings()
        {
            PerformanceLabsConfigurations.connectionString = ConfigurationManager.ConnectionStrings["Test_PerformanceLabsDBContext"].ConnectionString;
            PerformanceLabsConfigurations.DirReportCreation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            PerformanceLabsConfigurations.TrivialDurationThresholdMillisecondsStopWatch = 0;
            PerformanceLabsConfigurations.TrivialDurationThresholdMillisecondsMiniProfiler = 0;
            PerformanceLabsConfigurations.ThresholdZScore = 3;
            PerformanceLabsConfigurations.ThresholdTrivialSpecificOpTime = 0;
            PerformanceLabsConfigurations.ThresholdTrivialServerCodeTime = 0;
            PerformanceLabsConfigurations.ThresholdTrivialDbOpTime = 0;
            PerformanceLabsConfigurations.MultiplicationFactor = 30;
            PerformanceLabsConfigurations.contextType = PerformanceLabDBContextType.TestContext;
            PerformanceLabsConfigurations.ShowDbRegression = true;
            PerformanceLabsConfigurations.ShowSlowDbOperations = true;
            PerformanceLabsConfigurations.GenerateReport = false;
        }

        [TearDown]
        public void Teardown()
        {
            if (context.Database.Exists())
            {
                SqlConnection.ClearAllPools();
                context.Database.Delete();
            }

            if(contextTestDBPLabs.Database.Exists())
            {
                SqlConnection.ClearAllPools();
                contextTestDBPLabs.Database.Delete();
            }
        }
    }
}