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

namespace PerformanceLabs.AllTests.SampleProfilingProject
{
    public class SampleCSharpCode
    {
        public void CSharpTestOperation1(Boolean smallTime = true)
        {
            Thread.Sleep(smallTime ? Constants.smallTime : Constants.bigTime);//simulate doing work
        }
    }
}
