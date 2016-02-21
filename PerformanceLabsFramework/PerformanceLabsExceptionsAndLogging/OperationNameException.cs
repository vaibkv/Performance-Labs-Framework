// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerformanceLabsFramework.PerformanceLabsExceptionsAndLogging
{
    [Serializable]
    public class OperationNameException : Exception
    {
        public OperationNameException(string message)
            : base(message) { }

        public OperationNameException(string format, params object[] args)
            : base(string.Format(format, args)) { }
    }
}
