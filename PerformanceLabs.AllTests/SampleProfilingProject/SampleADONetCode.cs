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
using PerformanceLabsFramework;
using System.Configuration;

namespace PerformanceLabs.AllTests.SampleProfilingProject
{
    public class SampleADONetCode
    {
        public void SqlConnectionTestOperation(Boolean smallTime = true)
        {
            Thread.Sleep(smallTime ? Constants.smallTime : Constants.bigTime);//simulate some work before db operation
            dynamic cnn = PerfLabsProfiledADOWrapper.PerfLabsDbConnection(new SqlConnection(ConfigurationManager.ConnectionStrings["TestDBBlogContext"].ConnectionString));
            cnn.Open();
            var cmd = cnn.CreateCommand();
            cmd.CommandText = smallTime ? Constants.smallQuery : Constants.bigQuery;
            cmd.ExecuteNonQuery();
            Thread.Sleep(smallTime ? Constants.smallTime : Constants.bigTime);//simulate some work after db operation
            cnn.Close();
        }
    }
}