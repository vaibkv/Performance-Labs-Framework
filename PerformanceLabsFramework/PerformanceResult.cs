// --------------------------------------------------------------------------------------------------------------------
// <copyright company="The Advisory Board Company">
// Copyright © 2012 The Advisory Board Company
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PerformanceLabsFramework.Helpers;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using PerformanceLabsFramework.Properties;
using System.Reflection;
using Microsoft.VisualBasic.Devices;
using System.Web;
using PerformanceLabsFramework.Models;
using PerformanceLabsFramework.Models.ValueObjects;
using System.ComponentModel;

namespace PerformanceLabsFramework
{
    public class PerformanceResult : IDisposable
    {
        private Decimal _thresholdTrivialServerCodeTime;
        private Decimal _thresholdTrivialDbOpTime;
        private Decimal _thresholdTrivialSpecificOpTime;
        private Boolean _anyResultsAvailable = false;
        private List<CodeRegression> listCodeRegressions;
        private List<DbRegression> listDbRegression;
        private IEnumerable<NamedSlowCode> namedSlowCode;
        private IEnumerable<MiniProfilerSqlTimingValueObject> namedSlowSqlMiniProfiler;
        private IEnumerable<MiniProfilerSqlTimingValueObject> namedSqlMiniProfiler;
        private IEnumerable<MiniProfilerSqlTimingParameterValueObject> miniProfilerSqlParameters;
        private IEnumerable<RegressionResult1> regressionResult1;
        private IEnumerable<RegressionResult2> regressionResult2;
        private IEnumerable<RegressionResult3> regressionResult3;
        private IEnumerable<LatestMiniProfiler> latestMiniProfiler;
        private Int32 rowId;
        private DateTime Started_SW;
        private IPerformanceDBProxy context;
        private System.Guid MiniProfilerId;
        private DateTime Started;

        public void Dispose()
        {
            context.Dispose();
        }

        public PerformanceResult()
        {
            Type t = (PerformanceLabsConfigurations.contextType == PerformanceLabDBContextType.MainContext) ? typeof(PerformanceLabsDBContext) : typeof(Test_PerformanceLabsDBContext);
            context = (IPerformanceDBProxy)Activator.CreateInstance(t);
            listCodeRegressions = new List<CodeRegression>();
            listDbRegression = new List<DbRegression>();
        }

        public List<DbRegression> ListDbRegression
        {
            get
            {
                return listDbRegression;
            }
        }

        public List<CodeRegression> ListCodeRegressions
        {
            get
            {
                return listCodeRegressions;
            }
        }

        public Decimal ThresholdTrivialSpecificOpTime
        {
            get
            {
                if (_thresholdTrivialSpecificOpTime == 0)
                {
                    _thresholdTrivialSpecificOpTime = PerformanceLabsConfigurations.ThresholdTrivialSpecificOpTime;
                }
                return _thresholdTrivialSpecificOpTime;
            }
            set
            {
                _thresholdTrivialSpecificOpTime = value;
            }
        }

        public Decimal ThresholdTrivialServerCodeTime
        {
            get
            {
                if (_thresholdTrivialServerCodeTime == 0)
                {
                    _thresholdTrivialServerCodeTime = PerformanceLabsConfigurations.ThresholdTrivialServerCodeTime;
                }
                return _thresholdTrivialServerCodeTime;
            }
            set
            {
                _thresholdTrivialServerCodeTime = value;
            }
        }

        public Decimal ThresholdTrivialDbOpTime
        {
            get
            {
                if (_thresholdTrivialDbOpTime == 0)
                {
                    _thresholdTrivialDbOpTime = PerformanceLabsConfigurations.ThresholdTrivialDbOpTime;
                }
                return _thresholdTrivialDbOpTime;
            }
            set
            {
                _thresholdTrivialDbOpTime = value;
            }
        }

        private void GetAllResults()
        {
            rowId = (from swProfiler in context.StopWatchProfilers
                     orderby swProfiler.Started descending
                     select swProfiler.RowId).First();

            Started_SW = (from swProfiler in context.StopWatchProfilers
                          where swProfiler.RowId == rowId
                          select swProfiler.Started).First();

            latestMiniProfiler = (from miniProfiler in context.MiniProfilers
                                  orderby miniProfiler.Started descending
                                  select new LatestMiniProfiler()
                                      {
                                          Id = miniProfiler.Id,
                                          Started = miniProfiler.Started
                                      }).ToList<LatestMiniProfiler>(); ;

            MiniProfilerId = latestMiniProfiler.First().Id;
            Started = latestMiniProfiler.First().Started;

            //for backwards compatibility -> namedSlowCode = _dsAllResults.Tables[0]
            namedSlowCode = GetNamedSlowCode();

            //namedSlowSqlMiniProfiler = _dsAllResults.Tables[1]
            namedSlowSqlMiniProfiler = GetNamedSlowSqlMiniProfiler();

            namedSqlMiniProfiler = GetNamedSqlMiniProfiler();

            //miniProfilerSqlParameters = _dsAllResults.Tables[2]
            miniProfilerSqlParameters = GetMiniProfilerSqlParameters();

            //regressionResult1 = _dsAllResults.Tables[3]
            regressionResult1 = GetRegressionResult1();

            //regressionResult2 = _dsAllResults.Tables[4]
            regressionResult2 = GetRegressionResult2();

            //regressionResult3 = _dsAllResults.Tables[5]
            regressionResult3 = GetRegressionResult3();
        }

        public IEnumerable<RegressionResult3> GetRegressionResult3()
        {
            IQueryable<Int32> parentids = from regressionArchive in context.RegressionArchives where regressionArchive.RegressionType == 3 select regressionArchive.ParentId;

            var query = (from miniProfiler in context.MiniProfilers
                         join miniProfilerSqlTiming in context.MiniProfilerSqlTimings
                         on miniProfiler.Id equals miniProfilerSqlTiming.MiniProfilerId
                         join miniProfilerTiming in context.MiniProfilerTimings
                         on miniProfilerSqlTiming.ParentTimingId equals miniProfilerTiming.Id
                         where (miniProfilerTiming.HasSqlTimings == true && (!(parentids).Contains(miniProfilerSqlTiming.RowId)))
                         orderby miniProfiler.Started descending
                         select new RegressionResult3()
                         {
                             Name = miniProfilerTiming.Name,
                             RowId = miniProfilerSqlTiming.RowId,
                             CommandString = miniProfilerSqlTiming.CommandString,
                             DurationMilliseconds = miniProfilerSqlTiming.DurationMilliseconds,
                             StackTraceSnippet = miniProfilerSqlTiming.StackTraceSnippet,
                             Started = miniProfiler.Started
                         });

            return query.ToList<RegressionResult3>();
        }

        public IEnumerable<RegressionResult2> GetRegressionResult2()
        {
            IQueryable<Int32> parentids = from regressionArchive in context.RegressionArchives where regressionArchive.RegressionType == 2 select regressionArchive.ParentId;

            var query = (from miniProfiler in context.MiniProfilers
                         join miniProfilerTiming in context.MiniProfilerTimings
                         on miniProfiler.Id equals miniProfilerTiming.MiniProfilerId
                         ///left outer join commented out
                         //join miniProfilerSqlTiming in context.MiniProfilerSqlTimings
                         //on miniProfilerTiming.Id equals miniProfilerSqlTiming.ParentTimingId into mpst
                         //from st in mpst.DefaultIfEmpty()
                         where (miniProfilerTiming.ParentTimingId != null && (!(parentids).Contains(miniProfilerTiming.RowId)))
                         orderby miniProfilerTiming.Name, miniProfiler.Started descending
                         select new RegressionResult2()
                         {
                             RowId = miniProfilerTiming.RowId,
                             Name = miniProfilerTiming.Name,
                             DurationMilliseconds = miniProfilerTiming.DurationMilliseconds,
                             AvgDurationMilliseconds = miniProfilerTiming.DurationMilliseconds,
                             MethodRunCounts = 1,
                             SqlTimingsDurationMilliseconds = miniProfilerTiming.SqlTimingsDurationMilliseconds,
                             HasSqlTimings = miniProfilerTiming.HasSqlTimings,
                             HasDuplicateSqlTimings = miniProfilerTiming.HasDuplicateSqlTimings,
                             Started = miniProfiler.Started,
                             Id = miniProfilerTiming.Id
                         });

            return query.ToList<RegressionResult2>();
        }

        public IEnumerable<RegressionResult1> GetRegressionResult1()
        {
            IQueryable<Int32> parentids = from regressionArchive in context.RegressionArchives
                                          where regressionArchive.RegressionType == 1
                                          select regressionArchive.ParentId;

            var query = (from stopWatchProfilerTiming in context.StopWatchProfilerTimings
                         join stopWatchProfiler in context.StopWatchProfilers
                         on stopWatchProfilerTiming.ParentId equals stopWatchProfiler.RowId
                         where !(parentids).Contains(stopWatchProfilerTiming.RowId)
                         orderby stopWatchProfilerTiming.Name, stopWatchProfiler.Started descending
                         select new RegressionResult1()
                         {
                             RowId = stopWatchProfilerTiming.RowId,
                             Name = stopWatchProfilerTiming.Name,
                             DurationMilliseconds = stopWatchProfilerTiming.AvgDurationMilliseconds,
                             AvgDurationMilliseconds = stopWatchProfilerTiming.AvgDurationMilliseconds,
                             MethodRunCounts = stopWatchProfilerTiming.MethodRunCounts,
                             SqlTimingsDurationMilliseconds = 0,
                             HasSqlTimings = false,
                             HasDuplicateSqlTimings = false,
                             ParentId = stopWatchProfilerTiming.ParentId,
                             Started = stopWatchProfiler.Started,
                             Id = new Guid()
                         });

            return query.ToList<RegressionResult1>();
        }

        public IEnumerable<MiniProfilerSqlTimingParameterValueObject> GetMiniProfilerSqlParameters()
        {
            var query = (from miniProfilerSqlTimingParameter in context.MiniProfilerSqlTimingParameters
                         where miniProfilerSqlTimingParameter.MiniProfilerId == MiniProfilerId
                         select new MiniProfilerSqlTimingParameterValueObject()
                         {
                             ParentSqlTimingId = miniProfilerSqlTimingParameter.ParentSqlTimingId,
                             Name = miniProfilerSqlTimingParameter.Name,
                             DbType = miniProfilerSqlTimingParameter.DbType,
                             Size = miniProfilerSqlTimingParameter.Size,
                             Value = miniProfilerSqlTimingParameter.Value
                         });

            return query.ToList<MiniProfilerSqlTimingParameterValueObject>();
        }

        public IEnumerable<MiniProfilerSqlTimingValueObject> GetNamedSqlMiniProfiler()
        {
            var query = (from miniProfilerSqlTiming in context.MiniProfilerSqlTimings
                         join mpt in context.MiniProfilerTimings
                         on miniProfilerSqlTiming.ParentTimingId equals mpt.Id
                         where miniProfilerSqlTiming.MiniProfilerId == MiniProfilerId
                         select new MiniProfilerSqlTimingValueObject()
                         {
                             Id = miniProfilerSqlTiming.Id,
                             ParentTimingId = miniProfilerSqlTiming.ParentTimingId,
                             CommandString = miniProfilerSqlTiming.CommandString,
                             DurationMilliseconds = miniProfilerSqlTiming.DurationMilliseconds,
                             FirstFetchDurationMilliseconds = miniProfilerSqlTiming.FirstFetchDurationMilliseconds,
                             IsDuplicate = miniProfilerSqlTiming.IsDuplicate,
                             StackTraceSnippet = miniProfilerSqlTiming.StackTraceSnippet,
                             Name = mpt.Name
                         });

            return query.ToList<MiniProfilerSqlTimingValueObject>();
        }

        public IEnumerable<MiniProfilerSqlTimingValueObject> GetNamedSlowSqlMiniProfiler()
        {
            var query = (from miniProfilerSqlTiming in context.MiniProfilerSqlTimings
                         where miniProfilerSqlTiming.MiniProfilerId == MiniProfilerId
                         select new MiniProfilerSqlTimingValueObject()
                         {
                             Id = miniProfilerSqlTiming.Id,
                             ParentTimingId = miniProfilerSqlTiming.ParentTimingId,
                             CommandString = miniProfilerSqlTiming.CommandString,
                             DurationMilliseconds = miniProfilerSqlTiming.DurationMilliseconds,
                             FirstFetchDurationMilliseconds = miniProfilerSqlTiming.FirstFetchDurationMilliseconds,
                             IsDuplicate = miniProfilerSqlTiming.IsDuplicate,
                             StackTraceSnippet = miniProfilerSqlTiming.StackTraceSnippet
                         }).Where(x => x.DurationMilliseconds > ThresholdTrivialDbOpTime);

            return query.ToList<MiniProfilerSqlTimingValueObject>();
        }

        public IEnumerable<NamedSlowCode> GetNamedSlowCode()
        {
            var query = (from stopWatchProfilerTiming in context.StopWatchProfilerTimings
                         where stopWatchProfilerTiming.ParentId == rowId && stopWatchProfilerTiming.AvgDurationMilliseconds >= stopWatchProfilerTiming.ThresholdOperationTiming
                         select new NamedSlowCode()
                         {
                             Id = null,
                             MiniProfilerId = null,
                             ParentTimingId = null,
                             Name = stopWatchProfilerTiming.Name,
                             DurationMilliseconds = stopWatchProfilerTiming.DurationMilliseconds,
                             AvgDurationMilliseconds = stopWatchProfilerTiming.AvgDurationMilliseconds,
                             DurationWithoutChildrenMilliseconds = stopWatchProfilerTiming.AvgDurationMilliseconds,
                             MethodRunCounts = stopWatchProfilerTiming.MethodRunCounts,
                             SqlTimingsDurationMilliseconds = 0,
                             HasSqlTimings = false,
                             HasDuplicateSqlTimings = false,
                             Started = Started_SW,
                             ThresholdOperationTiming = stopWatchProfilerTiming.ThresholdOperationTiming
                         }).Union 
                            (from miniProfilerTiming in context.MiniProfilerTimings
                             join mmpThreshold in context.MiniProfilerOperationThresholds
                             on miniProfilerTiming.Name equals mmpThreshold.OperationName
                             where miniProfilerTiming.MiniProfilerId == MiniProfilerId && miniProfilerTiming.DurationMilliseconds != null && miniProfilerTiming.ParentTimingId != null
                             && miniProfilerTiming.DurationMilliseconds >= mmpThreshold.ThresholdOperationTiming && mmpThreshold.MiniProfilerId == MiniProfilerId
                             select new NamedSlowCode()
                             { 
                                 Id = miniProfilerTiming.Id,
                                 MiniProfilerId = miniProfilerTiming.MiniProfilerId,
                                 ParentTimingId = miniProfilerTiming.ParentTimingId,
                                 Name = miniProfilerTiming.Name,
                                 DurationMilliseconds = miniProfilerTiming.DurationMilliseconds,
                                 AvgDurationMilliseconds = miniProfilerTiming.DurationMilliseconds,
                                 DurationWithoutChildrenMilliseconds = miniProfilerTiming.DurationWithoutChildrenMilliseconds,
                                 MethodRunCounts = 1,
                                 SqlTimingsDurationMilliseconds = miniProfilerTiming.SqlTimingsDurationMilliseconds,
                                 HasSqlTimings = miniProfilerTiming.HasSqlTimings,
                                 HasDuplicateSqlTimings = miniProfilerTiming.HasDuplicateSqlTimings,
                                 Started = Started,
                                 ThresholdOperationTiming = mmpThreshold.ThresholdOperationTiming
                             });

            return query.ToList<NamedSlowCode>();
        }

        public void GenerateHtmlResultFile()
        {
            StringBuilder _sbTableResults = new StringBuilder();
            GetAllResults();

            _sbTableResults.Append(GenerateHtmlForSpecificOperationsProfiled());
            _sbTableResults.Append(GenerateHtmlForRegressions());
            _sbTableResults.Append(GenerateHtmlForSlowOperations());
            _sbTableResults.Append(GenerateHtmlForSlowSqlTimings());
            _sbTableResults.Append(GenerateHtmlForSqlDrillDown());
            _sbTableResults.Append(GenerateHtmlForSqlParameters());

            if (_sbTableResults != null && _anyResultsAvailable == true)
            {
                CreateAndSaveHtmlFileToFileSystem(_sbTableResults.ToString());
            }
        }

        private void CreateAndSaveHtmlFileToFileSystem(String htmlStringTables)
        {
            using (FileStream fs = new FileStream(PerformanceLabsConfigurations.DirReportCreation, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(UXConstants.StartingHTML);
                    w.WriteLine(UXConstants.StyleTag);
                    w.WriteLine(Resources.bootstrap);
                    w.WriteLine(UXConstants.StyleTagClose);
                    w.WriteLine(UXConstants.StyleFollowingBootstrp);
                    w.WriteLine(UXConstants.StyleTag);
                    w.WriteLine(Resources.bootstrap_responsive);
                    w.WriteLine(UXConstants.StyleTagClose);
                    w.WriteLine(UXConstants.ColumnHeaderTagClose);
                    w.WriteLine(UXConstants.BodyTag);
                    w.WriteLine(UXConstants.DivHeader);
                    w.WriteLine(UXConstants.DivTagContainer);

                    w.WriteLine(GenerateHtmlForEvaluationEnvironment());
                    w.WriteLine(htmlStringTables);

                    w.WriteLine(UXConstants.DivTagClose);
                    w.WriteLine(UXConstants.DivModal);
                    w.WriteLine(UXConstants.DivModalRegression);
                    w.WriteLine(UXConstants.JQueryUICssCDN);
                    w.WriteLine(UXConstants.JQueryCDN);
                    w.WriteLine(UXConstants.JQueryUICDN);
                    w.WriteLine(UXConstants.ScriptTag);
                    w.WriteLine(Resources.bootstrap_modal);
                    w.WriteLine(UXConstants.ScriptTagClose);
                    w.WriteLine(UXConstants.BodyTagClose);
                    w.WriteLine(UXConstants.HtmlTagClose);
                }
            }
        }

        private String GenerateHtmlForEvaluationEnvironment()
        {
            Dictionary<String, String> dictEnvironment = GetEvaluationEnvironmentValues();

            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append(UXConstants.Header2Tag);
            sbHtml.Append(UXConstants.EvaluationEnvironmentHeader);
            sbHtml.Append(UXConstants.Header2TagClose);
            sbHtml.Append(UXConstants.BreakTag);
            sbHtml.Append(UXConstants.TableTagStyled);
            sbHtml.Append(UXConstants.THeaderTag);

            //generating header row
            sbHtml.Append(UXConstants.RowTag);

            sbHtml.Append(UXConstants.ColumnHeaderTag);
            sbHtml.Append("Name");
            sbHtml.Append(UXConstants.ColumnHeaderTagClose);

            sbHtml.Append(UXConstants.ColumnHeaderTag);
            sbHtml.Append("Value");
            sbHtml.Append(UXConstants.ColumnHeaderTagClose);

            sbHtml.Append(UXConstants.RowTagClose);
            sbHtml.Append(UXConstants.THeaderTagClose);

            sbHtml.Append(UXConstants.TBodyTag);

            foreach (KeyValuePair<String, String> kv in dictEnvironment)
            {
                sbHtml.Append(UXConstants.RowTag);

                sbHtml.Append(UXConstants.ColumnTag);
                sbHtml.Append(kv.Key);
                sbHtml.Append(UXConstants.ColumnTagClose);

                sbHtml.Append(UXConstants.ColumnTag);
                sbHtml.Append(kv.Value);
                sbHtml.Append(UXConstants.ColumnTagClose);

                sbHtml.Append(UXConstants.RowTagClose);
            }
            sbHtml.Append(UXConstants.TBodyTagClose);
            sbHtml.Append(UXConstants.TableTagClose);
            return sbHtml.ToString();
        }

        private Dictionary<String, String> GetEvaluationEnvironmentValues()
        {
            ComputerInfo computerInfo = new ComputerInfo();
            Dictionary<String, String> dictEnvironment = new Dictionary<string, string>();

            dictEnvironment.Add("Machine Name", Environment.MachineName);
            dictEnvironment.Add("Operating System", computerInfo.OSFullName + " | " + Environment.OSVersion.VersionString + " | " + (Environment.Is64BitOperatingSystem ? "64 Bit" : "32 Bit"));
            dictEnvironment.Add("CLR Version", Environment.Version.ToString());
            dictEnvironment.Add("Total Physical Memory", Math.Round((Convert.ToDecimal(computerInfo.TotalPhysicalMemory) / (1024 * 1024 * 1024)), 4).ToString() + " GB");
            dictEnvironment.Add("Available Physical Memory", Math.Round((Convert.ToDecimal(computerInfo.AvailablePhysicalMemory) / (1024 * 1024 * 1024)), 4).ToString() + " GB");
            dictEnvironment.Add("Processor Count", Environment.ProcessorCount.ToString());
            dictEnvironment.Add("System Page Size", Environment.SystemPageSize.ToString() + " Bytes");

            return dictEnvironment;
        }

        private String GenerateHtmlForSpecificOperationsProfiled()
        {
            StringBuilder sbHtml = new StringBuilder();
            return sbHtml.ToString();
        }

        private String GenerateHtmlForRegressions()
        {
            StringBuilder sbHtml = new StringBuilder();
            sbHtml.Append(GenerateHtmlForDbReressions());
            sbHtml.Append(GenerateHtmlForCodeReressions());
            return sbHtml.ToString();
        }

        public void PopulateListForDbRegressions(IEnumerable<RegressionResult3> regressionResult3)
        {
            PerformanceLabsMathUtility mathUtil = new PerformanceLabsMathUtility();

            if (regressionResult3.Count() > 0 && PerformanceLabsConfigurations.ShowDbRegression)
            {
                DataTable dt = CollectionToDataTable.GetDataTable<RegressionResult3>(regressionResult3);
                var uniqueCommandStrings = dt.AsEnumerable().Select(row => new { Name = row.Field<String>("Name") }).Distinct();

                foreach (var commandString in uniqueCommandStrings)
                {
                    var sortedCommandStringByDatetime = dt.Select("Name = '" + commandString.Name + "'").OrderByDescending(c => c.Field<DateTime>("Started"));

                    if (sortedCommandStringByDatetime.Count() > 1)
                    {
                        Double currentMultiplicationFactor;

                        Double latestResponseTime = Convert.ToDouble(sortedCommandStringByDatetime.First().Field<Decimal>("DurationMilliseconds"));
                        List<Decimal> listResponseTimes = sortedCommandStringByDatetime.Select(r => r.Field<Decimal>("DurationMilliseconds")).ToList();

                        Double zScore = mathUtil.CalculateZScore(listResponseTimes.Select(item => Convert.ToDouble(item)).ToList(), latestResponseTime);

                        Boolean isOverTimed = IsOverTimed(sortedCommandStringByDatetime, "DurationMilliseconds", out currentMultiplicationFactor);

                        //generating data row if regression is present
                        if ((zScore > PerformanceLabsConfigurations.ThresholdZScore) || isOverTimed)
                        {
                            _anyResultsAvailable = true;
                            var regressionRow = sortedCommandStringByDatetime.First();

                            RegressionArchive regArchive = new RegressionArchive()
                            {
                                ParentId = regressionRow.Field<Int32>("RowId"),
                                NameOrCommandString = regressionRow.Field<String>("CommandString"),
                                DurationMilliseconds = regressionRow.Field<Decimal>("DurationMilliseconds"),
                                RegressionType = (Int32)RegressionType.MiniProfilerDBRegression,
                                Started = regressionRow.Field<DateTime>("Started")
                            };
                            context.RegressionArchives.Add(regArchive);
                            context.SaveChanges();

                            listDbRegression.Add(new DbRegression { CommandString = regressionRow["CommandString"].ToString(), DurationMilliseconds = Convert.ToDecimal(regressionRow["DurationMilliseconds"]), Started = DateTime.Parse(regressionRow["Started"].ToString()).ToShortDateString(), RowId = Convert.ToInt32(regressionRow["RowId"]), Name = regressionRow["Name"].ToString() });
                        }
                    }
                }
            }
        }

        private String GenerateHtmlForDbReressions()
        {
            PopulateListForDbRegressions(regressionResult3);

            StringBuilder sbHtml = new StringBuilder();

            if (listDbRegression != null && listDbRegression.Count > 0)
            {
                sbHtml.Append(UXConstants.Header2Tag);
                sbHtml.Append(UXConstants.RegressionsDBOperations);
                sbHtml.Append(UXConstants.Header2TagClose);

                sbHtml.Append(UXConstants.BreakTag);

                sbHtml.Append(UXConstants.TableTagStyled);

                sbHtml.Append(UXConstants.THeaderTag);
                sbHtml.Append(UXConstants.RowTag);

                DataTable dt = CollectionToDataTable.GetDataTable<DbRegression>(listDbRegression);

                //generating header row
                foreach (DataColumn dc in dt.Columns)
                {
                    switch (dc.ColumnName)
                    {
                        case "RowId":
                        case "Name":
                            sbHtml.Append(UXConstants.ColumnHeaderTagDisplayNone);
                            break;
                        default:
                            sbHtml.Append(UXConstants.ColumnHeaderTag);
                            break;
                    }
                    sbHtml.Append(GetDisplayName(dc.ColumnName));
                    sbHtml.Append(UXConstants.ColumnHeaderTagClose);
                }

                sbHtml.Append(UXConstants.RowTagClose);
                sbHtml.Append(UXConstants.THeaderTagClose);

                sbHtml.Append(UXConstants.TBodyTag);

                //generating data rows
                foreach (DataRow dr in dt.Rows)
                {
                    sbHtml.Append(UXConstants.RowTag);
                    foreach (DataColumn dc in dr.Table.Columns)
                    {
                        switch (dc.ColumnName)
                        {
                            case "RowId":
                            case "Name":
                                sbHtml.Append(UXConstants.ColumnTagDisplayNone);
                                sbHtml.Append(dr[dc]);
                                break;
                            case "CommandString":
                            case "StackTraceSnippet":
                                sbHtml.Append(UXConstants.ColumnTag);
                                if (dr[dc].ToString().Length > UXConstants.LengthBreak)
                                {
                                    sbHtml.Append(String.Format(UXConstants.AnchorTagModal, PrepareModalContent(dr[dc].ToString())));
                                    sbHtml.Append(dr[dc].ToString().Substring(0, UXConstants.LengthBreak) + "...");
                                    sbHtml.Append(UXConstants.AnchorTagClose);
                                }
                                else
                                {
                                    sbHtml.Append(dr[dc]);
                                }
                                break;
                            case "Started":
                                sbHtml.Append(UXConstants.ColumnTag);
                                sbHtml.Append(DateTime.Parse(dr[dc].ToString()).ToShortDateString());
                                break;
                            default:
                                sbHtml.Append(UXConstants.ColumnTag);
                                sbHtml.Append(dr[dc]);
                                break;
                        }
                        sbHtml.Append(UXConstants.ColumnTagClose);
                    }

                    sbHtml.Append(UXConstants.RowTagClose);
                }
                sbHtml.Append(UXConstants.TBodyTagClose);
                sbHtml.Append(UXConstants.TableTagClose);
            }
            return sbHtml.ToString();
        }

        private String GenerateHtmlForCodeReressions()
        {
            PopulateListForMiniProfilerCodeRegressions(regressionResult2);
            PopulateListForSWCodeRegressions(regressionResult1);

            StringBuilder sbHtml = new StringBuilder();

            if (listCodeRegressions != null && listCodeRegressions.Count > 0)
            {
                DataTable dt = CollectionToDataTable.GetDataTable<CodeRegression>(listCodeRegressions);

                sbHtml.Append(UXConstants.Header2Tag);
                sbHtml.Append(UXConstants.RegressionsCodeOps);
                sbHtml.Append(UXConstants.Header2TagClose);

                sbHtml.Append(UXConstants.BreakTag);

                sbHtml.Append(UXConstants.TableTagStyled);

                //generating header row
                sbHtml.Append(UXConstants.THeaderTag);
                sbHtml.Append(UXConstants.RowTag);

                foreach (DataColumn dc in dt.Columns)
                {
                    switch (dc.ColumnName)
                    {
                        case "Id":
                        case "ParentTimingId":
                        case "Started":
                        case "ParentId":
                        case "RowId":
                        case "SqlTimingsDurationMilliseconds":
                        case "drAllPreviousTimingsSorted":
                        case "CurrentZScore":
                        case "ZScoreThreshold":
                        case "CurrentMultiplicationFactor":
                        case "MultiplicationFactor":
                            sbHtml.Append(UXConstants.ColumnHeaderTagDisplayNone);
                            break;
                        default:
                            sbHtml.Append(UXConstants.ColumnHeaderTag);
                            break;
                    }
                    sbHtml.Append(GetDisplayName(dc.ColumnName));
                    sbHtml.Append(UXConstants.ColumnHeaderTagClose);
                }

                sbHtml.Append(UXConstants.RowTagClose);
                sbHtml.Append(UXConstants.THeaderTagClose);

                sbHtml.Append(UXConstants.TBodyTag);

                //generating data rows
                foreach (DataRow dr in dt.Rows)
                {
                    sbHtml.Append(UXConstants.RowTag);
                    foreach (DataColumn dc in dr.Table.Columns)
                    {
                        switch (dc.ColumnName)
                        {
                            case "Id":
                            case "ParentTimingId":
                            case "Started":
                            case "ParentId":
                            case "RowId":
                            case "SqlTimingsDurationMilliseconds":
                            case "drAllPreviousTimingsSorted":
                            case "CurrentZScore":
                            case "ZScoreThreshold":
                            case "CurrentMultiplicationFactor":
                            case "MultiplicationFactor":
                                sbHtml.Append(UXConstants.ColumnTagDisplayNone);
                                sbHtml.Append(dr[dc.ColumnName]);
                                break;
                            case "HasSqlTimings":
                                if (Convert.ToBoolean(dr[dc.ColumnName]))
                                {
                                    sbHtml.Append(UXConstants.ColumnTag);
                                    sbHtml.Append(UXConstants.AnchorTagHref.Replace("{0}", dr["Id"].ToString()));
                                    sbHtml.Append(dr[dc.ColumnName]);
                                    sbHtml.Append(UXConstants.AnchorTagClose);
                                }
                                else
                                {
                                    sbHtml.Append(UXConstants.ColumnTag);
                                    sbHtml.Append(dr[dc.ColumnName]);
                                }
                                break;
                            case "Name":
                                sbHtml.Append(UXConstants.ColumnTag);
                                sbHtml.Append(String.Format(UXConstants.AnchorTagModalRegression, PrepareModalContentForRegression(dr)));
                                if (dr[dc.ColumnName].ToString().Length > UXConstants.LengthBreak)
                                {
                                    sbHtml.Append(dr[dc.ColumnName].ToString().Substring(0, UXConstants.LengthBreak) + "...");
                                }
                                else
                                {
                                    sbHtml.Append(dr[dc.ColumnName].ToString());
                                }
                                sbHtml.Append(UXConstants.AnchorTagClose);
                                break;
                            default:
                                sbHtml.Append(UXConstants.ColumnTag);
                                sbHtml.Append(dr[dc.ColumnName]);
                                break;
                        }
                        sbHtml.Append(UXConstants.ColumnTagClose);
                    }
                    sbHtml.Append(UXConstants.RowTagClose);
                }

                sbHtml.Append(UXConstants.TBodyTagClose);
                sbHtml.Append(UXConstants.TableTagClose);
            }
            return sbHtml.ToString();
        }

        public void PopulateListForMiniProfilerCodeRegressions(IEnumerable<RegressionResult2> regressionResult2)
        {
            PerformanceLabsMathUtility mathUtil = new PerformanceLabsMathUtility();

            if (regressionResult2.Count() > 0)
            {
                DataTable dt = CollectionToDataTable.GetDataTable<RegressionResult2>(regressionResult2);
                var uniqueMethodNames = dt.AsEnumerable().Select(row => new { Name = row.Field<String>("Name") }).Distinct();

                foreach (var methodName in uniqueMethodNames)
                {
                    var sortedMethodNamesByDatetime = dt.Select("Name = '" + methodName.Name + "'").OrderByDescending(c => c.Field<DateTime>("Started")).OrderByDescending(c => c.Field<Int32>("RowId"));

                    if (sortedMethodNamesByDatetime.Count() > 1)
                    {
                        Double currentMultiplicationFactor;

                        Double latestResponseTime = Convert.ToDouble(sortedMethodNamesByDatetime.First().Field<Decimal>("DurationMilliseconds"));

                        List<Decimal> listResponseTimes = sortedMethodNamesByDatetime.Select(r => r.Field<Decimal>("DurationMilliseconds")).ToList();

                        Double zScore = mathUtil.CalculateZScore(listResponseTimes.Select(x => Convert.ToDouble(x)).ToList(), latestResponseTime);

                        Boolean isOverTimed = IsOverTimed(sortedMethodNamesByDatetime, "DurationMilliseconds", out currentMultiplicationFactor);

                        //populating list if regression is present
                        if ((zScore > PerformanceLabsConfigurations.ThresholdZScore) || isOverTimed)
                        {
                            _anyResultsAvailable = true;
                            var regressionRow = sortedMethodNamesByDatetime.First();

                            RegressionArchive regressionArchive = new RegressionArchive()
                            {
                                ParentId = regressionRow.Field<Int32>("RowId"),
                                NameOrCommandString = regressionRow.Field<String>("Name"),
                                DurationMilliseconds = regressionRow.Field<Decimal>("DurationMilliseconds"),
                                RegressionType = (Int32)RegressionType.MiniProfilerCodeRegression,
                                Started = regressionRow.Field<DateTime>("Started")
                            };
                            context.RegressionArchives.Add(regressionArchive);
                            context.SaveChanges();

                            listCodeRegressions.Add(new CodeRegression
                            {
                                Name = regressionRow["Name"].ToString(),
                                DurationMilliseconds = Convert.ToDecimal(regressionRow["DurationMilliseconds"]),
                                Started = DateTime.Parse(regressionRow["Started"].ToString()).ToShortDateString(),
                                AvgDurationMilliseconds = Convert.ToDecimal(regressionRow["AvgDurationMilliseconds"]),
                                HasDuplicateSqlTimings = Convert.ToBoolean(regressionRow["HasDuplicateSqlTimings"]),
                                HasSqlTimings = Convert.ToBoolean(regressionRow["HasSqlTimings"]),
                                Id = (System.Guid)regressionRow["Id"],
                                MethodRunCounts = Convert.ToInt32(regressionRow["MethodRunCounts"]),
                                RowId = Convert.ToInt32(regressionRow["RowId"]),
                                SqlTimingsDurationMilliseconds = Convert.ToDecimal(regressionRow["RowId"]),
                                drAllPreviousTimingsSorted = sortedMethodNamesByDatetime,
                                ZScoreThreshold = PerformanceLabsConfigurations.ThresholdZScore,
                                CurrentZScore = zScore,
                                CurrentMultiplicationFactor = currentMultiplicationFactor,
                                MultiplicationFactor = PerformanceLabsConfigurations.MultiplicationFactor
                            });
                        }
                    }
                }
            }
        }

        public void PopulateListForSWCodeRegressions(IEnumerable<RegressionResult1> regressionResult1)
        {
            PerformanceLabsMathUtility mathUtil = new PerformanceLabsMathUtility();

            if (regressionResult1.Count() > 0)
            {
                DataTable dt = CollectionToDataTable.GetDataTable<RegressionResult1>(regressionResult1);
                var uniqueMethodNames = dt.AsEnumerable().Select(row => new { Name = row.Field<String>("Name") }).Distinct();

                foreach (var methodName in uniqueMethodNames)
                {
                    var sortedMethodNamesByDatetime = dt.Select("Name = '" + methodName.Name + "'").OrderByDescending(c => c.Field<DateTime>("Started")).OrderByDescending(c => c.Field<Int32>("RowId"));

                    if (sortedMethodNamesByDatetime.Count() > 1)
                    {
                        Double currentMultiplicationFactor;

                        Double latestResponseTime = Convert.ToDouble(sortedMethodNamesByDatetime.First().Field<Decimal>("AvgDurationMilliseconds"));
                        List<Decimal> listResponseTimes = sortedMethodNamesByDatetime.Select(r => r.Field<Decimal>("AvgDurationMilliseconds")).ToList();

                        Double zScore = mathUtil.CalculateZScore(listResponseTimes.Select(x => Convert.ToDouble(x)).ToList(), latestResponseTime);

                        Boolean isOverTimed = IsOverTimed(sortedMethodNamesByDatetime, "AvgDurationMilliseconds", out currentMultiplicationFactor);

                        //populating list if regression is present
                        if ((zScore > PerformanceLabsConfigurations.ThresholdZScore) || isOverTimed)
                        {
                            _anyResultsAvailable = true;
                            var regressionRow = sortedMethodNamesByDatetime.First();

                            RegressionArchive regressionArchive = new RegressionArchive()
                            {
                                ParentId = regressionRow.Field<Int32>("RowId"),
                                NameOrCommandString = regressionRow.Field<String>("Name"),
                                DurationMilliseconds = regressionRow.Field<Decimal>("AvgDurationMilliseconds"),
                                RegressionType = (Int32)RegressionType.MiniProfilerCodeRegression,
                                Started = regressionRow.Field<DateTime>("Started")
                            };
                            context.RegressionArchives.Add(regressionArchive);
                            context.SaveChanges();

                            listCodeRegressions.Add(new CodeRegression
                            {
                                Name = regressionRow["Name"].ToString(),
                                DurationMilliseconds = Convert.ToDecimal(regressionRow["AvgDurationMilliseconds"]),
                                Started = DateTime.Parse(regressionRow["Started"].ToString()).ToShortDateString(),
                                AvgDurationMilliseconds = Convert.ToDecimal(regressionRow["AvgDurationMilliseconds"]),
                                HasDuplicateSqlTimings = false,
                                HasSqlTimings = false,
                                Id = (System.Guid)regressionRow["Id"],
                                MethodRunCounts = Convert.ToInt32(regressionRow["MethodRunCounts"]),
                                RowId = Convert.ToInt32(regressionRow["RowId"]),
                                SqlTimingsDurationMilliseconds = 0,
                                drAllPreviousTimingsSorted = sortedMethodNamesByDatetime,
                                ZScoreThreshold = PerformanceLabsConfigurations.ThresholdZScore,
                                CurrentZScore = zScore,
                                CurrentMultiplicationFactor = currentMultiplicationFactor,
                                MultiplicationFactor = PerformanceLabsConfigurations.MultiplicationFactor
                            });
                        }
                    }
                }
            }
        }

        private Boolean IsOverTimed(IOrderedEnumerable<DataRow> listDataRows, String columnName, out Double currentMultiplicationFactor)
        {
            Boolean result = false;
            Double latestResponseTime = Convert.ToDouble(listDataRows.First()[columnName]);
            Double penultimateResponseTime = Convert.ToDouble(listDataRows.ElementAt(1)[columnName]);
            currentMultiplicationFactor = latestResponseTime / penultimateResponseTime;
            if (latestResponseTime >= (penultimateResponseTime * PerformanceLabsConfigurations.MultiplicationFactor))
            {
                result = true;
            }
            return result;
        }

        private IEnumerable<MiniProfilerSqlTimingValueObject> PopulateDataForSqlToBeGenerated()
        {
            List<System.Guid> ids = new List<Guid>();

            ids.AddRange(listCodeRegressions.Where(x => x.HasSqlTimings == true).Select<CodeRegression, System.Guid>(x => x.Id).Distinct().ToList<System.Guid>());

            ids.AddRange(namedSlowCode.Where(x => x.HasSqlTimings == true).Select<NamedSlowCode, System.Guid>(x => (System.Guid)x.Id).Distinct().ToList<System.Guid>());

            namedSqlMiniProfiler = namedSqlMiniProfiler.Where(x => ids.Contains(x.ParentTimingId));

            return namedSqlMiniProfiler;
        }

        //show all sql that can be drilled down from SlowCode table or code regressions table
        private String GenerateHtmlForSqlDrillDown()
        {
            StringBuilder sbHtml = new StringBuilder();
            DataRow[] arrParentTimingIds;
            Int32 incrementTable = 0;

            if (PopulateDataForSqlToBeGenerated().Count() > 0)
            {
                DataTable dt = CollectionToDataTable.GetDataTable<MiniProfilerSqlTimingValueObject>(namedSqlMiniProfiler);

                _anyResultsAvailable = true;

                var listUniqueParentTimingIds = dt.AsEnumerable().Select(row => new { ParentTimingId = row.Field<Guid>("ParentTimingId") }).Distinct();

                sbHtml.Append(UXConstants.Header2Tag);
                sbHtml.Append(UXConstants.DbOpsDrillDown);
                sbHtml.Append(UXConstants.Header2TagClose);
                sbHtml.Append(UXConstants.BreakTag);

                foreach (var uniqueParentTimingId in listUniqueParentTimingIds)
                {
                    arrParentTimingIds = dt.Select("ParentTimingId = '" + uniqueParentTimingId.ParentTimingId.ToString() + "'");
                    sbHtml.Append(String.Format(UXConstants.AnchorTagLanding, uniqueParentTimingId.ParentTimingId.ToString(), "Db Operarion | Listing " + ++incrementTable + " for " + arrParentTimingIds[0]["Name"].ToString()));
                    sbHtml.Append(UXConstants.TableTagStyled);
                    sbHtml.Append(UXConstants.THeaderTag);
                    //generating header row
                    sbHtml.Append(UXConstants.RowTag);
                    foreach (DataColumn dc in dt.Columns)
                    {
                        switch (dc.ColumnName)
                        {
                            case "Id":
                            case "ParentTimingId":
                            case "RowId":
                            case "MiniProfilerId":
                            case "StartMilliseconds":
                            case "ExecuteType":
                            case "FirstFetchDurationMilliseconds":
                            case "Name":
                                sbHtml.Append(UXConstants.ColumnHeaderTagDisplayNone);
                                break;
                            default:
                                sbHtml.Append(UXConstants.ColumnHeaderTag);
                                break;
                        }
                        sbHtml.Append(GetDisplayName(dc.ColumnName));
                        sbHtml.Append(UXConstants.ColumnHeaderTagClose);
                    }
                    sbHtml.Append(UXConstants.RowTagClose);
                    sbHtml.Append(UXConstants.THeaderTagClose);

                    sbHtml.Append(UXConstants.TBodyTag);

                    //generating data rows
                    foreach (DataRow dr in arrParentTimingIds)
                    {
                        sbHtml.Append(UXConstants.RowTag);
                        foreach (DataColumn dc in dr.Table.Columns)
                        {
                            switch (dc.ColumnName)
                            {
                                case "Id":
                                case "ParentTimingId":
                                case "RowId":
                                case "MiniProfilerId":
                                case "StartMilliseconds":
                                case "ExecuteType":
                                case "FirstFetchDurationMilliseconds":
                                case "Name":
                                    sbHtml.Append(UXConstants.ColumnTagDisplayNone);
                                    sbHtml.Append(dr[dc.ColumnName]);
                                    break;
                                case "DurationMilliseconds":
                                    if (miniProfilerSqlParameters.Count() > 0 && miniProfilerSqlParameters.Where(x => x.ParentSqlTimingId.ToString() == dr["Id"].ToString()).Count() > 0)
                                    {
                                        sbHtml.Append(UXConstants.ColumnTag);
                                        sbHtml.Append(UXConstants.AnchorTagHref.Replace("{0}", dr["Id"].ToString()));
                                        sbHtml.Append(dr[dc.ColumnName]);
                                        sbHtml.Append(UXConstants.AnchorTagClose);
                                    }
                                    else
                                    {
                                        sbHtml.Append(UXConstants.ColumnTag);
                                        sbHtml.Append(dr[dc.ColumnName]);
                                    }
                                    break;
                                case "CommandString":
                                case "StackTraceSnippet":
                                    sbHtml.Append(UXConstants.ColumnTag);
                                    if (dr[dc.ColumnName].ToString().Length > UXConstants.LengthBreak)
                                    {
                                        sbHtml.Append(String.Format(UXConstants.AnchorTagModal, PrepareModalContent(dr[dc.ColumnName].ToString())));
                                        sbHtml.Append(dr[dc.ColumnName].ToString().Substring(0, UXConstants.LengthBreak) + "...");
                                        sbHtml.Append(UXConstants.AnchorTagClose);
                                    }
                                    else
                                    {
                                        sbHtml.Append(dr[dc.ColumnName]);
                                    }
                                    break;
                                default:
                                    sbHtml.Append(UXConstants.ColumnTag);
                                    sbHtml.Append(dr[dc.ColumnName]);
                                    break;
                            }
                            sbHtml.Append(UXConstants.ColumnTagClose);
                        }
                        sbHtml.Append(UXConstants.RowTagClose);
                    }
                    sbHtml.Append(UXConstants.TBodyTagClose);
                    sbHtml.Append(UXConstants.TableTagClose);
                }
            }
            return sbHtml.ToString();
        }

        private String GenerateHtmlForSlowSqlTimings()
        {
            StringBuilder sbHtml = new StringBuilder();
            DataRow[] arrParentTimingIds;
            Int32 incrementTable = 0;
            if (namedSlowSqlMiniProfiler.Count() > 0 && PerformanceLabsConfigurations.ShowSlowDbOperations)
            {
                DataTable dt = CollectionToDataTable.GetDataTable<MiniProfilerSqlTimingValueObject>(namedSlowSqlMiniProfiler);

                _anyResultsAvailable = true;

                var listUniqueParentTimingIds = dt.AsEnumerable().Select(row => new { ParentTimingId = row.Field<Guid>("ParentTimingId") }).Distinct();

                sbHtml.Append(UXConstants.Header2Tag);
                sbHtml.Append(UXConstants.SlowDbOps);
                sbHtml.Append(UXConstants.Header2TagClose);
                sbHtml.Append(UXConstants.BreakTag);

                foreach (var uniqueParentTimingId in listUniqueParentTimingIds)
                {
                    arrParentTimingIds = dt.Select("ParentTimingId = '" + uniqueParentTimingId.ParentTimingId.ToString() + "'");
                    sbHtml.Append(String.Format(UXConstants.AnchorTagLanding, uniqueParentTimingId.ParentTimingId.ToString(), "Slow Db Operarion | Listing " + ++incrementTable));
                    sbHtml.Append(UXConstants.TableTagStyled);
                    sbHtml.Append(UXConstants.THeaderTag);
                    //generating header row
                    sbHtml.Append(UXConstants.RowTag);
                    foreach (DataColumn dc in dt.Columns)
                    {
                        switch (dc.ColumnName)
                        {
                            case "Id":
                            case "ParentTimingId":
                            case "RowId":
                            case "MiniProfilerId":
                            case "StartMilliseconds":
                            case "ExecuteType":
                            case "FirstFetchDurationMilliseconds":
                                sbHtml.Append(UXConstants.ColumnHeaderTagDisplayNone);
                                break;
                            default:
                                sbHtml.Append(UXConstants.ColumnHeaderTag);
                                break;
                        }
                        sbHtml.Append(GetDisplayName(dc.ColumnName));
                        sbHtml.Append(UXConstants.ColumnHeaderTagClose);
                    }
                    sbHtml.Append(UXConstants.RowTagClose);
                    sbHtml.Append(UXConstants.THeaderTagClose);

                    sbHtml.Append(UXConstants.TBodyTag);

                    //generating data rows
                    foreach (DataRow dr in dt.Rows)
                    {
                        sbHtml.Append(UXConstants.RowTag);
                        foreach (DataColumn dc in dr.Table.Columns)
                        {
                            switch (dc.ColumnName)
                            {
                                case "Id":
                                case "ParentTimingId":
                                case "RowId":
                                case "MiniProfilerId":
                                case "StartMilliseconds":
                                case "ExecuteType":
                                case "FirstFetchDurationMilliseconds":
                                    sbHtml.Append(UXConstants.ColumnTagDisplayNone);
                                    sbHtml.Append(dr[dc.ColumnName]);
                                    break;
                                case "DurationMilliseconds":
                                    if (miniProfilerSqlParameters.Count() > 0 && miniProfilerSqlParameters.Where(x => x.ParentSqlTimingId.ToString() == dr["Id"].ToString()).Count() > 0)
                                    {
                                        sbHtml.Append(UXConstants.ColumnTag);
                                        sbHtml.Append(UXConstants.AnchorTagHref.Replace("{0}", dr["Id"].ToString()));
                                        sbHtml.Append(dr[dc.ColumnName]);
                                        sbHtml.Append(UXConstants.AnchorTagClose);
                                    }
                                    else
                                    {
                                        sbHtml.Append(UXConstants.ColumnTag);
                                        sbHtml.Append(dr[dc.ColumnName]);
                                    }
                                    break;
                                case "CommandString":
                                case "StackTraceSnippet":
                                    sbHtml.Append(UXConstants.ColumnTag);
                                    if (dr[dc.ColumnName].ToString().Length > UXConstants.LengthBreak)
                                    {
                                        sbHtml.Append(String.Format(UXConstants.AnchorTagModal, PrepareModalContent(dr[dc.ColumnName].ToString())));
                                        sbHtml.Append(dr[dc.ColumnName].ToString().Substring(0, UXConstants.LengthBreak) + "...");
                                        sbHtml.Append(UXConstants.AnchorTagClose);
                                    }
                                    else
                                    {
                                        sbHtml.Append(dr[dc.ColumnName]);
                                    }
                                    break;
                                default:
                                    sbHtml.Append(UXConstants.ColumnTag);
                                    sbHtml.Append(dr[dc.ColumnName]);
                                    break;
                            }
                            sbHtml.Append(UXConstants.ColumnTagClose);
                        }
                        sbHtml.Append(UXConstants.RowTagClose);
                    }
                    sbHtml.Append(UXConstants.TBodyTagClose);
                    sbHtml.Append(UXConstants.TableTagClose);
                }
            }
            return sbHtml.ToString();
        }

        private String GenerateHtmlForSqlParameters()
        {
            StringBuilder sbHtml = new StringBuilder();
            DataRow[] arrParentTimingIds;
            Int32 incrementCount = 0;

            List<Guid> ids = new List<Guid>();

            ids = namedSqlMiniProfiler.Select<MiniProfilerSqlTimingValueObject, Guid>(x => x.Id).ToList<Guid>();

            miniProfilerSqlParameters = miniProfilerSqlParameters.Where(x => ids.Contains(x.ParentSqlTimingId));

            if (miniProfilerSqlParameters.Count() > 0)
            {
                DataTable dt = CollectionToDataTable.GetDataTable<MiniProfilerSqlTimingParameterValueObject>(miniProfilerSqlParameters);

                var listUniqueParentTimingIds = dt.AsEnumerable().Select(row => new { ParentTimingId = row.Field<Guid>("ParentTimingId") }).Distinct();

                sbHtml.Append(UXConstants.Header2Tag);
                sbHtml.Append(UXConstants.SqlParams);
                sbHtml.Append(UXConstants.Header2TagClose);
                sbHtml.Append(UXConstants.BreakTag);

                foreach (var uniqueParentTimingId in listUniqueParentTimingIds)
                {
                    arrParentTimingIds = dt.Select("ParentTimingId = '" + uniqueParentTimingId.ParentTimingId.ToString() + "'");
                    sbHtml.Append(String.Format(UXConstants.AnchorTagLanding, uniqueParentTimingId.ParentTimingId.ToString(), "Sql Parameters | Listing " + ++incrementCount));

                    sbHtml.Append(UXConstants.TableTagStyled);
                    sbHtml.Append(UXConstants.THeaderTag);

                    //generating header row
                    sbHtml.Append(UXConstants.RowTag);
                    foreach (DataColumn dc in dt.Columns)
                    {
                        switch (dc.ColumnName)
                        {
                            case "ParentTimingId":
                                sbHtml.Append(UXConstants.ColumnHeaderTagDisplayNone);
                                break;
                            default:
                                sbHtml.Append(UXConstants.ColumnHeaderTag);
                                break;
                        }
                        sbHtml.Append(dc.ColumnName);
                        sbHtml.Append(UXConstants.ColumnHeaderTagClose);
                    }
                    sbHtml.Append(UXConstants.RowTagClose);
                    sbHtml.Append(UXConstants.THeaderTagClose);

                    sbHtml.Append(UXConstants.TBodyTag);

                    //generating data rows
                    foreach (DataRow dr in dt.Rows)
                    {
                        sbHtml.Append(UXConstants.RowTag);
                        foreach (DataColumn dc in dr.Table.Columns)
                        {
                            switch (dc.ColumnName)
                            {
                                case "ParentTimingId":
                                    sbHtml.Append(UXConstants.ColumnTagDisplayNone);
                                    break;
                                case "Name":
                                    sbHtml.Append(UXConstants.ColumnTag);
                                    if (dr[dc.ColumnName].ToString().Length > UXConstants.LengthBreak)
                                    {
                                        sbHtml.Append(String.Format(UXConstants.AnchorTagModal, PrepareModalContent(dr[dc.ColumnName].ToString())));
                                        sbHtml.Append(dr[dc.ColumnName].ToString().Substring(0, UXConstants.LengthBreak) + "...");
                                        sbHtml.Append(UXConstants.AnchorTagClose);
                                    }
                                    else
                                    {
                                        sbHtml.Append(dr[dc.ColumnName]);
                                    }
                                    break;
                                default:
                                    sbHtml.Append(UXConstants.ColumnTag);
                                    sbHtml.Append(dr[dc.ColumnName]);
                                    break;
                            }
                            sbHtml.Append(UXConstants.ColumnTagClose);
                        }
                        sbHtml.Append(UXConstants.RowTagClose);
                    }
                    sbHtml.Append(UXConstants.TBodyTagClose);
                    sbHtml.Append(UXConstants.TableTagClose);
                }
            }
            return sbHtml.ToString();
        }

        private String GenerateHtmlForSlowOperations()
        {
            StringBuilder sbHtml = new StringBuilder();
            if (namedSlowCode.Count() > 0)
            {
                DataTable dt = CollectionToDataTable.GetDataTable<NamedSlowCode>(namedSlowCode);
                dt.Columns.Remove("MiniProfilerId");

                _anyResultsAvailable = true;

                sbHtml.Append(UXConstants.Header2Tag);
                sbHtml.Append(UXConstants.SlowCode);
                sbHtml.Append(UXConstants.Header2TagClose);

                sbHtml.Append(UXConstants.BreakTag);

                sbHtml.Append(UXConstants.TableTagStyled);

                //generating header row
                sbHtml.Append(UXConstants.THeaderTag);
                sbHtml.Append(UXConstants.RowTag);
                foreach (DataColumn dc in dt.Columns)
                {
                    switch (dc.ColumnName)
                    {
                        case "Id":
                        case "ParentTimingId":
                        case "DurationWithoutChildrenMilliseconds":
                        case "Started":
                            sbHtml.Append(UXConstants.ColumnHeaderTagDisplayNone);
                            break;
                        default:
                            sbHtml.Append(UXConstants.ColumnHeaderTag);
                            break;
                    }
                    sbHtml.Append(GetDisplayName(dc.ColumnName));
                    sbHtml.Append(UXConstants.ColumnHeaderTagClose);
                }
                sbHtml.Append(UXConstants.RowTagClose);
                sbHtml.Append(UXConstants.THeaderTag);

                sbHtml.Append(UXConstants.TBodyTag);

                //generating data rows
                foreach (DataRow dr in dt.Rows)
                {
                    sbHtml.Append(UXConstants.RowTag);
                    foreach (DataColumn dc in dr.Table.Columns)
                    {
                        switch (dc.ColumnName)
                        {
                            case "Id":
                            case "ParentTimingId":
                            case "DurationWithoutChildrenMilliseconds":
                            case "Started":
                                sbHtml.Append(UXConstants.ColumnTagDisplayNone);
                                sbHtml.Append(dr[dc.ColumnName]);
                                break;
                            case "HasSqlTimings":
                                if (Convert.ToBoolean(dr[dc.ColumnName]) && namedSqlMiniProfiler.Count() > 0 && namedSqlMiniProfiler.Where(x => x.ParentTimingId != null && !x.ParentTimingId.Equals(DBNull.Value) && x.ParentTimingId.ToString() == dr["Id"].ToString()).Count() > 0)
                                {
                                    sbHtml.Append(UXConstants.ColumnTag);
                                    sbHtml.Append(UXConstants.AnchorTagHref.Replace("{0}", dr["Id"].ToString()));
                                    sbHtml.Append(dr[dc.ColumnName]);
                                    sbHtml.Append(UXConstants.AnchorTagClose);
                                }
                                else
                                {
                                    sbHtml.Append(UXConstants.ColumnTag);
                                    sbHtml.Append(dr[dc.ColumnName]);
                                }
                                break;
                            default:
                                sbHtml.Append(UXConstants.ColumnTag);
                                sbHtml.Append(dr[dc.ColumnName]);
                                break;
                        }
                        sbHtml.Append(UXConstants.ColumnTagClose);
                    }
                    sbHtml.Append(UXConstants.RowTagClose);
                }
                sbHtml.Append(UXConstants.TBodyTagClose);
                sbHtml.Append(UXConstants.TableTagClose);
            }

            return sbHtml.ToString();
        }

        private String PrepareModalContent(String data)
        {
            StringBuilder sbHtml = new StringBuilder();

            sbHtml.Append(UXConstants.ParaTag);
            sbHtml.Append(data);
            sbHtml.Append(UXConstants.ParaTagClose);

            return HttpUtility.HtmlEncode(sbHtml.ToString());
        }

        private String PrepareModalContentForRegression(DataRow dr)
        {
            StringBuilder sbHtml = new StringBuilder();

            IEnumerable<DataRow> drAllPreviousTimingsSorted = (IEnumerable<DataRow>)dr["drAllPreviousTimingsSorted"];
            Double CurrentZScore = Math.Round(Convert.ToDouble(dr["CurrentZScore"]), 3);
            Double ZScoreThreshold = Convert.ToDouble(dr["ZScoreThreshold"]);
            Double CurrentMultiplicationFactor = Math.Round(Convert.ToDouble(dr["CurrentMultiplicationFactor"]), 3);
            Double MultiplicationFactor = Convert.ToDouble(dr["MultiplicationFactor"]);

            sbHtml.Append(UXConstants.RegressionConditionExplanation);
            sbHtml.Append(UXConstants.BreakTag);
            sbHtml.Append(UXConstants.BreakTag);

            sbHtml.Append(UXConstants.Header4Tag);
            sbHtml.Append(UXConstants.CurrentResponseTime);
            sbHtml.Append(Math.Round(drAllPreviousTimingsSorted.First().Field<Decimal>("AvgDurationMilliseconds"), 3));
            sbHtml.Append(UXConstants.Header4TagClose);
            sbHtml.Append(UXConstants.BreakTag);

            drAllPreviousTimingsSorted = drAllPreviousTimingsSorted.Skip(1);
            sbHtml.Append(UXConstants.Header4Tag);
            sbHtml.Append(UXConstants.PreviousResponseTimes);
            sbHtml.Append(UXConstants.Header4TagClose);
            sbHtml.Append(UXConstants.BreakTag);

            //generating header row
            sbHtml.Append(UXConstants.TableTagStyled);
            sbHtml.Append(UXConstants.THeaderTag);
            sbHtml.Append(UXConstants.RowTag);

            sbHtml.Append(UXConstants.ColumnHeaderTag);
            sbHtml.Append("Date");
            sbHtml.Append(UXConstants.ColumnHeaderTagClose);

            sbHtml.Append(UXConstants.ColumnHeaderTag);
            sbHtml.Append("Response Time (ms)");
            sbHtml.Append(UXConstants.ColumnHeaderTagClose);

            sbHtml.Append(UXConstants.RowTagClose);
            sbHtml.Append(UXConstants.THeaderTagClose);

            sbHtml.Append(UXConstants.TBodyTag);

            //generating data row
            foreach (DataRow drPreviousResponseTime in drAllPreviousTimingsSorted)
            {
                sbHtml.Append(UXConstants.RowTag);
                foreach (DataColumn dc in drPreviousResponseTime.Table.Columns)
                {
                    switch (dc.ColumnName)
                    {
                        default:
                            sbHtml.Append(UXConstants.ColumnTagDisplayNone);
                            sbHtml.Append(drPreviousResponseTime[dc]);
                            break;
                        case "AvgDurationMilliseconds":
                            sbHtml.Append(UXConstants.ColumnTag);
                            sbHtml.Append(drPreviousResponseTime[dc]);
                            break;
                        case "Started":
                            sbHtml.Append(UXConstants.ColumnTag);
                            sbHtml.Append(DateTime.Parse(drPreviousResponseTime[dc].ToString()).ToShortDateString());
                            break;
                    }
                    sbHtml.Append(UXConstants.ColumnTagClose);
                }
                sbHtml.Append(UXConstants.RowTagClose);
            }
            sbHtml.Append(UXConstants.TBodyTagClose);
            sbHtml.Append(UXConstants.TableTagClose);

            sbHtml.Append(UXConstants.BreakTag);
            sbHtml.Append(UXConstants.Header4Tag);
            sbHtml.Append(UXConstants.CurrentMultiplicationFactor);
            sbHtml.Append(CurrentMultiplicationFactor);
            sbHtml.Append(UXConstants.Header4TagClose);

            sbHtml.Append(UXConstants.BreakTag);
            sbHtml.Append(UXConstants.Header4Tag);
            sbHtml.Append(UXConstants.ThresholdMultiplicationFactor);
            sbHtml.Append(MultiplicationFactor);
            sbHtml.Append(UXConstants.Header4TagClose);

            sbHtml.Append(UXConstants.BreakTag);
            sbHtml.Append(UXConstants.Header4Tag);
            sbHtml.Append(UXConstants.CurrentZScore);
            sbHtml.Append(CurrentZScore);
            sbHtml.Append(UXConstants.Header4TagClose);

            sbHtml.Append(UXConstants.BreakTag);
            sbHtml.Append(UXConstants.Header4Tag);
            sbHtml.Append(UXConstants.ZScoreThreshold);
            sbHtml.Append(ZScoreThreshold);
            sbHtml.Append(UXConstants.Header4TagClose);
            return HttpUtility.HtmlEncode(sbHtml.ToString());
        }

        private String GetDisplayName(String columnName)
        {
            String displayName = String.Empty;
            switch (columnName)
            {
                case "Name": displayName = "Operation Name"; break;
                case "DurationMilliseconds": displayName = "Total Duration(ms)"; break;
                case "AvgDurationMilliseconds": displayName = "Average Duration(ms)"; break;
                case "MethodRunCounts": displayName = "# of Invocations"; break;
                case "HasSqlTimings": displayName = "Has Sql Timings"; break;
                case "HasDuplicateSqlTimings": displayName = "(N+1) Scenario"; break;
                case "SqlTimingsDurationMilliseconds": displayName = "Sql Timing Duration(ms)"; break;
                case "CommandString": displayName = "Query Executed"; break;
                case "StackTraceSnippet": displayName = "Stack Trace Snippet"; break;
                case "IsDuplicate": displayName = "Is Duplicate"; break;
                case "ThresholdOperationTiming": displayName = "Threshold Operation Timing(ms)"; break;
                default: displayName = columnName; break;
            }
            return displayName;
        }
    }

    internal enum RegressionType
    {
        StopWatchRegression = 1, MiniProfilerCodeRegression = 2, MiniProfilerDBRegression = 3
    }
}
