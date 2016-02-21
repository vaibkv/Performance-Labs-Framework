// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StackExchange.Profiling;
using PerformanceLabsFramework.Models;

namespace PerformanceLabsFramework
{
    public class MiniProfilerStepPerfLabs : IDisposable
    {
        private readonly IDisposable disposableStep;
        private Int64 ThresholdOperationTiming;
        private String OperationName;

        public MiniProfilerStepPerfLabs(String operationName, Int64 thresholdOperationTiming = 0)
        {
            ThresholdOperationTiming = thresholdOperationTiming == 0 ? (long)PerformanceLabsConfigurations.ThresholdTrivialServerCodeTime : thresholdOperationTiming;
            OperationName = operationName;
            disposableStep = PerformanceLabs.MiniProfiler.Step(operationName);
        }

        public void Dispose()
        {
            disposableStep.Dispose();
            SaveOperationThreshold();
        }

        private void SaveOperationThreshold()
        {
            Guid MiniProfilerId;

            MiniProfilerId = PerformanceLabs.MiniProfiler.Id;

            //MiniProfiler.Settings.Storage.Save(PerformanceLabs.MiniProfiler);

            Type t = (PerformanceLabsConfigurations.contextType == PerformanceLabDBContextType.MainContext) ? typeof(PerformanceLabsDBContext) : typeof(Test_PerformanceLabsDBContext);

            using (IPerformanceDBProxy context = (IPerformanceDBProxy)Activator.CreateInstance(t))
            {
                //ParentId = (from mpt in context.MiniProfilerTimings
                //            join mp in context.MiniProfilers
                //            on mpt.MiniProfilerId equals mp.Id
                //            where mpt.Name != null && mpt.Name == OperationName
                //            orderby mp.Started descending, mpt.RowId descending
                //            select mpt.Id).First();

                MiniProfilerOperationThreshold mmpThreshold = new MiniProfilerOperationThreshold()
                {
                    MiniProfilerId = MiniProfilerId,
                    OperationName = OperationName,
                    ParentId = null,
                    ThresholdOperationTiming = ThresholdOperationTiming  
                };

                context.MiniProfilerOperationThresholds.Add(mmpThreshold);
                context.SaveChanges(); 
            }
        }
    }
}