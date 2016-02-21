// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PerformanceLabs.AllTests.SampleProfilingProject.Models;

namespace PerformanceLabs.AllTests.SampleProfilingProject
{
    public static class Constants
    {
        public static String testDBConnectionString = @"SERVER=localhost;DATABASE=TestDBBlog;Integrated Security=SSPI;";

        public static Int32 smallTime = 10;
        public static Int32 bigTime = 1000;

        public static String smallQuery = @"select * from BlogPosts; select * from Categories; WAITFOR DELAY '00:00:01'";
        public static String bigQuery = @"select * from BlogPosts; select * from Categories; WAITFOR DELAY '00:00:20'";
    }
}
