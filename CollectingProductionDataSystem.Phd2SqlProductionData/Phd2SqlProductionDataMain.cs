namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    using System.Collections;
    using log4net;
    using System;
    using System.Linq;
    using System.ServiceProcess;
    using Uniformance.PHD;
    using System.Data;
    using System.Text;
    using CollectingProductionDataSystem.Phd2SqlProductionData.Models;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Concrete;

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

        internal static void ProcessInspectionPointsData()
        {
            try
            {
                logger.Info("Sync inspection points started!");

                using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister())))
                {
                    var units = context.Units
                        .All()
                        .Where(u => u.IsInspectionPoint == true)
                        .Select(u => new InspectionPoint()
                            {
                                Id = u.Id,
                                InspectionPointTag = u.CurrentInspectionDataTag
                            });

                    Tags tags = new Tags();
                    Hashtable ht = new Hashtable();
                    foreach (var unit in units)
                    {
                        ht.Add(unit.InspectionPointTag, unit);
                        tags.Add(new Tag(unit.InspectionPointTag));
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
                                    var tagName = string.Empty;
                                    var tagValue = 0m;

                                    foreach (DataColumn dc in dsGrid.Tables[0].Columns)
                                    {
                                        if (dc.ColumnName.Equals("Tolerance") || dc.ColumnName.Equals("HostName") || dc.ColumnName.Equals("Units"))
                                        {
                                            continue;
                                        }
                                        else if (dc.ColumnName.Equals("Confidence") && !row[dc].ToString().Equals("100"))
                                        {
                                            continue;    
                                        }
                                        else if (dc.ColumnName.Equals("TagName"))
                                        {
                                            tagName = row[dc].ToString();
                                        }
                                        else if (dc.ColumnName.Equals("Value"))
                                        {
                                            tagValue = Convert.ToDecimal(row[dc]);
                                        }
                                    }
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
                var minimumMinutes = 5;
                logger.Info("Sync inventory tanks data!");
                if (DateTime.Now.Minute <= minimumMinutes)
                {
                    logger.InfoFormat("Minutes in the current time are less than {0}!", minimumMinutes);
                    return;
                }

                using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister())))
                {
                    DateTime recordTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
                    if (context.TanksData.All().Where(td => td.RecordTimestamp.CompareTo(recordTime) == 0).Any())
                    {
                        logger.InfoFormat("There are already a records for that time of the day: {0:yyyy-MM-dd HH:ss:mm}!", recordTime);
                        return;
                    }

                    var tanks = context.Tanks.All().Select(t =>
                        new Tank() 
                        {
                            Id = t.Id,
                            ParkId = t.ParkId,
                            ControlPoint = t.ControlPoint,
                            UnusableResidueLevel = 0.05m,
                            PhdTagProductId = t.PhdTagProductId,
                            PhdTagLiquidLevel = t.PhdTagLiquidLevel,
                            PhdTagProductLevel = t.PhdTagProductLevel,
                            PhdTagFreeWaterLevel = t.PhdTagFreeWaterLevel,
                            PhdTagReferenceDensity = t.PhdTagReferenceDensity,
                            PhdTagNetStandardVolume = t.PhdTagNetStandardVolume,
                            PhdTagWeightInAir = t.PhdTagWeightInAir
                        });

                    if (tanks.Count() > 0)
                    {
                        using(PHDHistorian oPhd = new PHDHistorian())
                        {
                            using (PHDServer defaultServer = new PHDServer(Properties.Settings.Default.PHD_HOST))
                            {
                                defaultServer.Port = Properties.Settings.Default.PHD_PORT;
                                defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
                                oPhd.DefaultServer = defaultServer;
                                oPhd.StartTime = string.Format("NOW - {0}M", DateTime.Now.Minute - 2);
                                oPhd.EndTime = string.Format("NOW - {0}M", DateTime.Now.Minute - 2);
                                oPhd.Sampletype = SAMPLETYPE.Raw;
                                oPhd.MinimumConfidence = 100;
                                oPhd.MaximumRows = 1;

                                foreach (var t in tanks)
                                {
                                    var tankData = new TankData();
                                    tankData.RecordTimestamp = recordTime;
                                    tankData.Id = t.Id;
                                    tankData.ParkId = t.ParkId;
                                    tankData.NetStandardVolume = t.UnusableResidueLevel;

                                    Tags tags = new Tags();
                                    SetPhdTag(t.PhdTagProductId, tags);              
                                    SetPhdTag(t.PhdTagLiquidLevel, tags);                   
                                    SetPhdTag(t.PhdTagProductLevel, tags);                   
                                    SetPhdTag(t.PhdTagFreeWaterLevel, tags);                              
                                    SetPhdTag(t.PhdTagReferenceDensity, tags);   
                                    SetPhdTag(t.PhdTagNetStandardVolume, tags);              
                                    SetPhdTag(t.PhdTagWeightInAir, tags);                               
    
                                    DataSet dsGrid = oPhd.FetchRowData(tags);
                                    foreach (DataRow row in dsGrid.Tables[0].Rows)
                                    {
                                        var tagName = string.Empty;
                                        var tagValue = 0m;

                                        foreach (DataColumn dc in dsGrid.Tables[0].Columns)
                                        {
                                            if (dc.ColumnName.Equals("Tolerance") || dc.ColumnName.Equals("HostName") || dc.ColumnName.Equals("Units"))
                                            {
                                                continue;
                                            }
                                            else if (dc.ColumnName.Equals("Confidence") && !row[dc].ToString().Equals("100"))
                                            {
                                                continue;    
                                            }
                                            else if (dc.ColumnName.Equals("TagName"))
                                            {
                                                tagName = row[dc].ToString();
                                            }
                                            else if (dc.ColumnName.Equals("Value"))
                                            {
                                                tagValue = Convert.ToDecimal(row[dc]);
                                            }
                                        }

                                        if (tagName.Contains(".PROD_ID"))
                                        {
                                            // Todo: Need to convert productCode from Tank Master or need to choose a mehanism for product code comparation
                                            // var productId = Convert.ToInt32(tagValue);   
                                            // tankData.ProductId = Convert.ToInt32(productId);
                                            // tankData.ProductName = context.Products.GetById(productId).Name;
                                            tankData.ProductId = 1;
                                            tankData.ProductName = tagValue.ToString();
                                        }
                                        else if (tagName.Contains(".LL"))
                                        {
                                            tankData.LiquidLevel = tagValue;    
                                        }
                                        else if (tagName.Contains(".LEVEL_MM"))
                                        {
                                            tankData.LiquidLevel = tagValue;
                                        }
                                        else if (tagName.Contains(".LL_FWL"))
                                        {
                                            tankData.ProductLevel = tagValue;
                                        }      
                                        else if (tagName.Contains(".LEVEL_FWL"))
                                        {
                                            tankData.ProductLevel = tagValue;
                                        }
                                        else if (tagName.Contains(".FWL"))
                                        {
                                            tankData.FreeWaterLevel = tagValue;  
                                        }
                                        else if (tagName.Contains(".DREF"))
                                        {
                                            tankData.ReferenceDensity = tagValue;
                                        }
                                        else if (tagName.Contains(".REF_DENS"))
                                        {
                                            tankData.ReferenceDensity = tagValue;
                                        }
                                        else if (tagName.Contains(".NSV"))
                                        {
                                            tankData.NetStandardVolume = tagValue;
                                        }
                                        else if (tagName.Contains(".WIA"))
                                        {
                                            tankData.WeightInAir = tagValue;
                                        }
                                    }

                                    context.TanksData.Add(tankData);
                                }

                                context.SaveChanges("PHD2SQLInventory");
                            }
                        } 
                    }
                    logger.Info("Done!");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }
 
        private static void SetPhdTag(string tagName, Tags tags)
        {
            if (!string.IsNullOrEmpty(tagName))
            {
                tags.Add(new Tag(tagName));    
            }
        }
    }
}
