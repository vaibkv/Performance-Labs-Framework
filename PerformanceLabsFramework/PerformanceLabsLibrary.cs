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
using System.Diagnostics;
using PerformanceLabsFramework.Helpers;
using System.Data.SqlClient;
using System.Reflection;
using System.IO;
using PerformanceLabsFramework.Properties;
using System.Data.Objects;
using PerformanceLabsFramework.Models;
using PerformanceLabsFramework.PerformanceLabsExceptionsAndLogging;

namespace PerformanceLabsFramework
{
    public class PerformanceLabs
    {
        public static MVCMiniProfiler MvcMiniProfiler { get; set; }
        public static MiniProfiler MiniProfiler { get; set; }
        public static Stopwatch StopWatch { get; set; }
        public HashSet<String> OperationNames { get; private set; }

        public PerformanceLabs()
        {
            ResolveAssemblies();
            GetProfilers();
        }

        private void ResolveAssemblies()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveAssembly);
        }

        private static Assembly ResolveAssembly(Object sender, ResolveEventArgs args)
        {
            String dll = string.Empty;
            var thisAssembly = Assembly.GetExecutingAssembly();
            String[] resNames = thisAssembly.GetManifestResourceNames();
            foreach (String resName in resNames)
            {
                if (resName.EndsWith("MiniProfiler.dll"))
                {
                    dll = resName;
                    break;
                }
            }
            using (var input = thisAssembly.GetManifestResourceStream(dll))
            {
                return input != null
                     ? Assembly.Load(StreamToBytes(input))
                     : null;
            }
        }

        static byte[] StreamToBytes(Stream input)
        {
            var capacity = input.CanSeek ? (int)input.Length : 0;
            using (var output = new MemoryStream(capacity))
            {
                int readLength;
                var buffer = new byte[4096];

                do
                {
                    readLength = input.Read(buffer, 0, buffer.Length);
                    output.Write(buffer, 0, readLength);
                }
                while (readLength != 0);

                return output.ToArray();
            }
        }

        private void GetProfilers()
        {
            if (MvcMiniProfiler == null)
            {
                MvcMiniProfiler = MVCMiniProfiler.GetMVCMiniProfiler();
            }
            if (StopWatch == null)
            {
                StopWatch = PerfLabsStopWatch.GetStopWatch();
            }
        }

        private void EnsureDataBaseExists()
        {
            Type t = (PerformanceLabsConfigurations.contextType == PerformanceLabDBContextType.MainContext) ? typeof(PerformanceLabsDBContext) : typeof(Test_PerformanceLabsDBContext);
            using (IPerformanceDBProxy context = (IPerformanceDBProxy)Activator.CreateInstance(t))
            {
                context.Database.CreateIfNotExists();
                //if (context.Database.Exists())
                //{
                //    context.Database.ExecuteSqlCommand(PerformanceLabsConfigurations.contextType == PerformanceLabDBContextType.MainContext ? PerformanceLabsConfigurations.MakeLoginCredentials : PerformanceLabsConfigurations.MakeLoginCredentialsTestDB);
                //}
            }
        }

        public void DropDataBase()
        {
            SqlConnection.ClearAllPools();
            Type t = (PerformanceLabsConfigurations.contextType == PerformanceLabDBContextType.MainContext) ? typeof(PerformanceLabsDBContext) : typeof(Test_PerformanceLabsDBContext);
            using (IPerformanceDBProxy context = (IPerformanceDBProxy)Activator.CreateInstance(t))
            {
                context.Database.Delete();
            }
        }

        public void StartProfilers()
        {
            EnsureDataBaseExists();
            OperationNames = new HashSet<string>();
            MvcMiniProfiler.StartProfiler();
            MiniProfiler = MvcMiniProfiler.GetMiniProfiler();

            PerfLabsStopWatch.StartProfiler();
        }

        public void StopProfilers()
        {
            PerformanceResult report = null;
            MvcMiniProfiler.StopProfiler();
            PerfLabsStopWatch.StopProfiler();
            SaveToDataBase();
            if (PerformanceLabsConfigurations.GenerateReport)
            {
                report = new PerformanceResult();
                report.GenerateHtmlResultFile();
            }
        }

        public void SaveToDataBase()
        {
            MiniProfiler.Settings.Storage.Save(MiniProfiler);
        }

        public IDisposable Step(Profiler profiler, String operationName, Int64 thresholdOperationTiming = 0, Int32 numofInvocations = 1, Action operation = null)
        {
            if (profiler == Profiler.MVCMiniProfiler)
            {
                if (!OperationNames.Contains<String>(operationName))
                {
                    OperationNames.Add(operationName);
                }
                else
                {
                    throw new OperationNameException(String.Format(ExceptionsAndLoggingMessages.OperationNameException, operationName));
                }
                return new MiniProfilerStepPerfLabs(operationName, thresholdOperationTiming);
            }
            else if (profiler == Profiler.PerfLabsStopWatch)
            {
                if (!OperationNames.Contains<String>(operationName))
                {
                    OperationNames.Add(operationName);
                }
                else
                {
                    throw new OperationNameException(String.Format(ExceptionsAndLoggingMessages.OperationNameException, operationName));
                }
                return new PerfLabsStopWatch(operationName, thresholdOperationTiming, numofInvocations, operation);
            }
            else
            {
                return null;
            }
        }
    }

    public enum Profiler
    {
        MVCMiniProfiler = 1, PerfLabsStopWatch = 2
    }
}