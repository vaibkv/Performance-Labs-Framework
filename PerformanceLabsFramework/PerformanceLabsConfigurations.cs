// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace PerformanceLabsFramework
{
    public static class PerformanceLabsConfigurations
    {
        public static String PerfLabsStopWatchName = "CCCStopWatch";

        public static String DirReportCreation = @"D:\CCCRepository\PerformanceLabsFramework\PerformanceReport\PerformanceReport.html";

        public static String connectionString = @"Data Source=192.168.16.142;Initial Catalog=PerformanceLabsDB;Integrated Security=SSPI;MultipleActiveResultSets=True;";
            
        //when response time is greater than these values then only shall they be persisted in the database otherwise they will be discarded
        public static Decimal TrivialDurationThresholdMillisecondsMiniProfiler = 1;
        public static Decimal TrivialDurationThresholdMillisecondsStopWatch = 1;

        //these values are used to figure out which of the persisted values show to be considered as 'slow' for different stuff like 'code', 'db operation' and 'some special\specific operations' 
        public static Decimal ThresholdTrivialServerCodeTime = 1;
        public static Decimal ThresholdTrivialDbOpTime = 1;
        public static Decimal ThresholdTrivialSpecificOpTime = 1;

        //for Regression analysis
        public static Double ThresholdZScore = 3;
        public static Double MultiplicationFactor = 30;

        public static PerformanceLabDBContextType contextType = PerformanceLabDBContextType.MainContext;

        public static Boolean ShowDbRegression = false;
        public static Boolean ShowSlowDbOperations = false;

        public static Boolean GenerateReport = true;

        public static String ProductName = "Crimson Continuum of Care";
    }

    public enum PerformanceLabDBContextType
    {
        MainContext = 1, TestContext = 2
    }
}