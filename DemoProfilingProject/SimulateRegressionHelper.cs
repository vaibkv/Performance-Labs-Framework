// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PerformanceLabsFramework;
using PerformanceLabsFramework.Models;

namespace DemoProfilingProject
{
    public static class SimulateRegressionHelper
    {
        private static PerformanceLabs perfLabs;

        //NUnit - less debugging Helper method
        public static void Main(String[] args)
        {
            SetupTests st = new SetupTests();
            st.SetUp();
            DemoADONetCode cs = new DemoADONetCode();
            cs.Init();
            //cs.SqlConnectionTestOperation1();
            //cs.SqlConnectionTestOperation2();
            cs.SqlConnectionTestOperation3();

            //DemoCSharpCode cs1 = new DemoCSharpCode();
            //cs1.Init();
            //cs1.CSharpTestOperation1();
            //cs1.CSharpTestOperation2();
            //cs1.CSharpTestOperation3();
            //cs1.CSharpTestOperation4();
            //cs1.CSharpTestOperation5();
            //cs1.CSharpTestOperation6();
            
            st.Teardown();
        }

        static SimulateRegressionHelper()
        {
            if (SetupTests.perfLabs == null)
            {
                perfLabs = new PerformanceLabs();
            }
            else
            {
                perfLabs = SetupTests.perfLabs;
            }
        }

        public static void ProfileMultipleTimes(String operationName, Action operation, Int32 numOfInvocations, Profiler profilingMethod)
        {
            for (Int32 i = 0; i < numOfInvocations; i++)
            {
                perfLabs.StartProfilers();
                using (perfLabs.Step(profilingMethod, operationName))
                {
                    operation.Invoke();
                }
                if (profilingMethod == Profiler.MVCMiniProfiler)
                {
                    perfLabs.SaveToDataBase();
                }
                perfLabs.StopProfilers();
            }
            perfLabs.StartProfilers();
        }
    }
}
