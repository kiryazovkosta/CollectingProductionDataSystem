namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Concrete;
    using log4net;
    using System;
    using System.Linq;
    using System.ServiceProcess;
    using Uniformance.PHD;
    using System.Data;
    using System.Text;

    static class Phd2SqlProductionDataMain
    {
        private static readonly ILog logger;

        static Phd2SqlProductionDataMain()
        {
            logger = LogManager.GetLogger("CollectingProductionDataSystem.Phd2SqlProductionData");
        }

        internal static void Main()
        {
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[] 
            { 
                new Phd2SqlProductionDataService(logger) 
            };
            ServiceBase.Run(servicesToRun);
        }

        internal static void ProcessInformationLogsData()
        {
            try
            {
                logger.Info("Sync inspection points started!");

                using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister())))
                {
                    var units = context.Units.All().Where(u => u.IsInspectionPoint == true).Select(u =>
                        new
                        {
                            Id = u.Id,
                            CurrentInspectionDataTag = u.CurrentInspectionDataTag
                        });

                    Tags tags = new Tags();
                    foreach (var unit in units)
                    {
                        tags.Add(new Tag(unit.CurrentInspectionDataTag));
                    }

                    if (tags.Count > 0)
                    {
                       using(PHDHistorian oPhd = new PHDHistorian())
                        {
                            using (PHDServer defaultServer = new PHDServer(Properties.Settings.Default.PHD_HOST))
                            {
                                defaultServer.Port = Properties.Settings.Default.PHD_PORT;
                                defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
                                oPhd.DefaultServer = defaultServer;
                                oPhd.StartTime = "NOW - 2M";
                                oPhd.EndTime = "NOW - 2M";
                                oPhd.Sampletype = SAMPLETYPE.Raw;
                                oPhd.MinimumConfidence = 100;
                                oPhd.MaximumRows = 1;

                                DataSet dsGrid = oPhd.FetchRowData(tags);
                                foreach (DataRow row in dsGrid.Tables[0].Rows)
                                {
                                    var sb = new StringBuilder();
                                    sb.Append("[CurrentInspectionDataTag]");
                                    foreach (DataColumn dc in dsGrid.Tables[0].Columns)
                                    {
                                        if (dc.ColumnName.Equals("Tolerance") || dc.ColumnName.Equals("HostName"))
                                        {
                                            continue;
                                        }

                                        var field1 = row[dc].ToString();
                                       sb.Append(dc.ColumnName + ":" + field1 + "  ");
                                    }

                                    logger.Info(sb.ToString());
                                }
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

        internal static void ProcessPrimaryProductionData()
        {
            try
            {
                logger.Info("Sync primary data started!");

                using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister())))
                {
                    var units = context.Units.All().Select(u =>
                        new
                        {
                            Id = u.Id,
                            PreviousShiftTag = u.PreviousShiftTag
                        });

                    Tags tags = new Tags();
                    foreach (var unit in units)
                    {
                        if (! string.IsNullOrEmpty(unit.PreviousShiftTag))
                        {
                            tags.Add(new Tag(unit.PreviousShiftTag));
                        }
                    }

                    using(PHDHistorian oPhd = new PHDHistorian())
                    {
                        using (PHDServer defaultServer = new PHDServer(Properties.Settings.Default.PHD_HOST))
                        {
                            defaultServer.Port = Properties.Settings.Default.PHD_PORT;
                            defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
                            oPhd.DefaultServer = defaultServer;
                            oPhd.StartTime = "NOW - 2M";
                            oPhd.EndTime = "NOW - 2M";
                            oPhd.Sampletype = SAMPLETYPE.Raw;
                            oPhd.MinimumConfidence = 100;
                            oPhd.MaximumRows = 1;

                            DataSet dsGrid = oPhd.FetchRowData(tags);
                            foreach (DataRow row in dsGrid.Tables[0].Rows)
                            {
                                var sb = new StringBuilder();
                                sb.Append("[PreviousShiftTag]");
                                foreach (DataColumn dc in dsGrid.Tables[0].Columns)
                                {
                                    if (dc.ColumnName.Equals("Tolerance") || dc.ColumnName.Equals("HostName"))
                                    {
                                        continue;
                                    }

                                    var field1 = row[dc].ToString();
                                   sb.Append(dc.ColumnName + ":" + field1 + "  ");
                                }

                                logger.Info(sb.ToString());
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

        internal static void ProcessInventoryTanksData()
        {
            try
            {
                logger.Info("Sync inspection points started!");
                if (DateTime.Now.Minute < 10)
                {
                    return;
                }

                using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister())))
                {
                    var tanks = context.Tanks.All().Select(t =>
                        new
                        {
                            Id = t.Id,
                            PhdTagProductId = t.PhdTagProductId,
                            PhdTagProductName           = t.PhdTagProductName          ,
                            PhdTagLiquidLevel           = t.PhdTagLiquidLevel          ,
                            PhdTagProductLevel          = t.PhdTagProductLevel         ,
                            PhdTagFreeWaterLevel        = t.PhdTagFreeWaterLevel       ,
                            PhdTagFreeWaterVolume       = t.PhdTagFreeWaterVolume      ,
                            PhdTagObservableDensity     = t.PhdTagObservableDensity    ,
                            PhdTagReferenceDensity      = t.PhdTagReferenceDensity     ,
                            PhdTagGrossObservableVolume = t.PhdTagGrossObservableVolume,
                            PhdTagGrossStandardVolume   = t.PhdTagGrossStandardVolume  ,
                            PhdTagNetStandardVolume     = t.PhdTagNetStandardVolume    ,
                            PhdTagWeightInAir           = t.PhdTagWeightInAir          ,
                            WeightInAirLowExtreme       = t.WeightInAirLowExtreme      ,
                            WeightInAirHighExtreme      = t.WeightInAirHighExtreme     ,
                            PhdTagAverageTemperature    = t.PhdTagAverageTemperature   ,
                            PhdTagTotalObservableVolume = t.PhdTagTotalObservableVolume,
                            PhdTagWeightInVacuum        = t.PhdTagWeightInVacuum       ,
                            PhdTagMaxVolume             = t.PhdTagMaxVolume            ,
                            PhdTagAvailableRoom         = t.PhdTagAvailableRoom        ,
                        });

                    Tags tags = new Tags();
                    foreach (var t in tanks)
                    {
                        tags.Add(new Tag(t.PhdTagProductName));              
                        tags.Add(new Tag(t.PhdTagLiquidLevel));             
                        tags.Add(new Tag(t.PhdTagProductLevel));              
                        tags.Add(new Tag(t.PhdTagFreeWaterLevel));            
                        tags.Add(new Tag(t.PhdTagFreeWaterVolume));          
                        tags.Add(new Tag(t.PhdTagObservableDensity));         
                        tags.Add(new Tag(t.PhdTagReferenceDensity));       
                        tags.Add(new Tag(t.PhdTagGrossObservableVolume));     
                        tags.Add(new Tag(t.PhdTagGrossStandardVolume));       
                        tags.Add(new Tag(t.PhdTagNetStandardVolume));         
                        tags.Add(new Tag(t.PhdTagWeightInAir));                          
                        tags.Add(new Tag(t.PhdTagAverageTemperature));          
                        tags.Add(new Tag(t.PhdTagTotalObservableVolume));     
                        tags.Add(new Tag(t.PhdTagWeightInVacuum));              
                        tags.Add(new Tag(t.PhdTagMaxVolume));                         
                        tags.Add(new Tag(t.PhdTagAvailableRoom));
                    }

                    if (tags.Count > 0)
                    {
                       using(PHDHistorian oPhd = new PHDHistorian())
                        {
                            using (PHDServer defaultServer = new PHDServer(Properties.Settings.Default.PHD_HOST))
                            {
                                defaultServer.Port = Properties.Settings.Default.PHD_PORT;
                                defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
                                oPhd.DefaultServer = defaultServer;
                                oPhd.StartTime = "NOW - 2M";
                                oPhd.EndTime = "NOW - 2M";
                                oPhd.Sampletype = SAMPLETYPE.Raw;
                                oPhd.MinimumConfidence = 100;
                                oPhd.MaximumRows = 1;

                                DataSet dsGrid = oPhd.FetchRowData(tags);
                                foreach (DataRow row in dsGrid.Tables[0].Rows)
                                {
                                    var sb = new StringBuilder();
                                    sb.Append("[Inventory]");
                                    foreach (DataColumn dc in dsGrid.Tables[0].Columns)
                                    {
                                        if (dc.ColumnName.Equals("Tolerance") || dc.ColumnName.Equals("HostName"))
                                        {
                                            continue;
                                        }

                                        var field1 = row[dc].ToString();
                                       sb.Append(dc.ColumnName + ":" + field1 + "  ");
                                    }

                                    logger.Info(sb.ToString());
                                }
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
