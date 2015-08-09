

namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    using CollectingProductionDataSystem.Common;
    using CollectingProductionDataSystem.Data.Repositories;
    using CollectingProductionDataSystem.Models;
    using CollectingProductionDataSystem.Phd2SqlProductionData;
    using CollectingProductionDataSystem.Data;
    using log4net;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using System.Threading.Tasks;
    using Uniformance.PHD;
    using System.Data;

    static class Phd2SqlProductionDataMain
    {
        private static ILog logger;

        static Phd2SqlProductionDataMain()
        {
            logger = LogManager.GetLogger("CollectingProductionDataSystem.Phd2SqlProductionData");
        }

        internal static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Phd2SqlProductionDataService(logger) 
            };
            ServiceBase.Run(ServicesToRun);
        }

        internal static void ProcessInformationLogsData()
        {
            try
            {
                logger.Info("Sync inspection points started!");

                using (var context = new ProductionData(new CollectingDataSystemDbContext()))
                {
                    var units = context.Units.AllActive().Where(u => u.IsInspectionPoint == true).Select(u =>
                        new
                        {
                            Id = u.Id,
                            TagName = u.CurrentInspectionDataTag,
                            Value = 0m,
                            RecordTime = DateTime.MinValue
                        });

                    Tags tags = new Tags();
                    foreach (var unit in units)
                    {
                        tags.Add(new Tag(unit.TagName));
                    }

                    using(PHDHistorian oPhd = new PHDHistorian())
                    {
                        using (PHDServer defaultServer = new PHDServer(Properties.Settings.Default.PHD_HOST))
                        {
                            defaultServer.Port = 3150;
                            defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
                            oPhd.DefaultServer = defaultServer;
                            oPhd.StartTime = "NOW - 2M";
                            oPhd.EndTime = "NOW - 2M";
                            oPhd.Sampletype = SAMPLETYPE.Snapshot;
                            oPhd.MinimumConfidence = 49;
                            oPhd.ReductionType = REDUCTIONTYPE.Average;
                            oPhd.MaximumRows = 1;

                            DataSet dsGrid = oPhd.FetchRowData(tags);
                            foreach (DataRow row in dsGrid.Tables[0].Rows)
                            {
                                foreach (DataColumn dc in dsGrid.Tables[0].Columns)
                                {
                                    if (dc.ColumnName.Equals("Tolerance") || dc.ColumnName.Equals("HostName"))
                                    {
                                        continue;
                                    }

                                    var field1 = row[dc].ToString();
                                    logger.Info(dc.ColumnName + ":" + field1 + "  ");
                                }

                                logger.Info(" ");
                            }
                        }
                    }
                }

                logger.Info("Done!");
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }
    }
}
