namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    using System.Linq;
    using log4net;
    using System;
    using System.ServiceProcess;
    using Uniformance.PHD;
    using System.Data;
    using CollectingProductionDataSystem.Phd2SqlProductionData.Models;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Models.Productions;
    using System.Globalization;

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
                                                  UnitId = u.Id,
                                                  InspectionPointTag = u.CurrentInspectionDataTag
                                              });

                    if (units.Count() > 0)
                    {
                        using (PHDHistorian oPhd = new PHDHistorian())
                        {
                            using (PHDServer defaultServer = new PHDServer(Properties.Settings.Default.PHD_HOST))
                            {
                                defaultServer.Port = Properties.Settings.Default.PHD_PORT;
                                defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
                                oPhd.DefaultServer = defaultServer;
                                oPhd.StartTime = "NOW - 2M";
                                oPhd.EndTime = "NOW - 2M";
                                oPhd.Sampletype = Properties.Settings.Default.INSPECTION_DATA_SAMPLETYPE;
                                oPhd.MinimumConfidence = Properties.Settings.Default.INSPECTION_DATA_MINIMUM_CONFIDENCE;
                                oPhd.MaximumRows = Properties.Settings.Default.INSPECTION_DATA_MAX_ROWS;

                                // get all inspection data
                                foreach (var item in units)
                                {
                                    var unitInspectionPointData = new UnitsInspectionData();
                                    unitInspectionPointData.UnitConfigId = item.UnitId;
                                    DataSet dsGrid = oPhd.FetchRowData(item.InspectionPointTag);
                                    var confidence = 100;
                                    foreach (DataRow row in dsGrid.Tables[0].Rows)
                                    {
                                        foreach (DataColumn dc in dsGrid.Tables[0].Columns)
                                        {
                                            if (dc.ColumnName.Equals("Tolerance") || dc.ColumnName.Equals("HostName"))
                                            {
                                                continue;
                                            }
                                            else if (dc.ColumnName.Equals("Confidence"))
                                            {
                                                confidence = Convert.ToInt32(row[dc]);
                                                break;
                                            }
                                            else if (dc.ColumnName.Equals("Value"))
                                            {
                                                unitInspectionPointData.Value = Convert.ToDecimal(row[dc]);
                                            }
                                            else if (dc.ColumnName.Equals("TimeStamp"))
                                            {
                                                var recordTimestamp = DateTime.ParseExact(row[dc].ToString(), "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                                if (TimeZoneInfo.Local.IsDaylightSavingTime(recordTimestamp))
                                                {
                                                    recordTimestamp = recordTimestamp.AddHours(-1);
                                                }
                                                unitInspectionPointData.RecordTimestamp = recordTimestamp;
                                            }
                                        }
                                    }
                                    if (confidence > Properties.Settings.Default.INSPECTION_DATA_MINIMUM_CONFIDENCE)
                                    {
                                        //var udExists = context.UnitsInspectionData
                                        //    .All()
                                        //    .Where(u => u.RecordTimestamp.CompareTo(unitInspectionPointData.RecordTimestamp) == 0)
                                        //    .Select(u => u.UnitConfigId == unitInspectionPointData.UnitConfigId)
                                        //    .FirstOrDefault();
                                        //if (!udExists)
                                        //{
                                            context.UnitsInspectionData.Add(unitInspectionPointData);                                            
                                        //}
                                    }
                                }

                                context.SaveChanges("PHD2SQLInspectionData");
                            }
                        }
                    }

                    logger.Info("Sync inspection points finished!");
                }
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
                    var units = context.Units.All().Where(u => u.PreviousShiftTag != null).Select(u => new PreviousShift()
                    {
                        UnitId = u.Id,
                        PreviousShiftTag = u.PreviousShiftTag
                    });

                    using (PHDHistorian oPhd = new PHDHistorian())
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

                            // get all inspection data
                            foreach (var item in units)
                            {
                                var unitData = new UnitsData();
                                unitData.UnitConfigId = item.UnitId;
                                DataSet dsGrid = oPhd.FetchRowData(item.PreviousShiftTag);
                                var confidence = 100;
                                foreach (DataRow row in dsGrid.Tables[0].Rows)
                                {
                                    foreach (DataColumn dc in dsGrid.Tables[0].Columns)
                                    {
                                        if (dc.ColumnName.Equals("Tolerance") || dc.ColumnName.Equals("HostName"))
                                        {
                                            continue;
                                        }
                                        else if (dc.ColumnName.Equals("Confidence") && !row[dc].ToString().Equals("100"))
                                        {
                                            confidence = 0;
                                            break;    
                                        }
                                        else if (dc.ColumnName.Equals("Value"))
                                        {
                                            unitData.Value = Convert.ToDecimal(row[dc]);
                                        }
                                        else if (dc.ColumnName.Equals("TimeStamp"))
                                        {
                                            var recordTimestamp = DateTime.ParseExact(row[dc].ToString(), "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                            if (TimeZoneInfo.Local.IsDaylightSavingTime(recordTimestamp))
                                            {
                                                recordTimestamp = recordTimestamp.AddHours(-1);    
                                            }
                                            unitData.RecordTimestamp = recordTimestamp; 
                                        }
                                    }
                                }
                                if (confidence == 100)
                                {
                                    context.UnitsData.Add(unitData);   
                                }
                            }
                            context.SaveChanges("PHD2SQLPreviousShift");
                        }
                    }
                }

                logger.Info("Sync primary data finished!");
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
                logger.Info("Sync inventory tanks data started!");

                using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister())))
                {
                    DateTime recordTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
                    if (context.TanksData.All().Where(td => td.RecordTimestamp.CompareTo(recordTime) == 0).Any())
                    {
                        logger.InfoFormat("There are already a records for that time of the day: {0:yyyy-MM-dd HH:ss:mm}!", recordTime);
                        return;
                    }

                    var tanks = context.Tanks.All().Select(t => new Tank()
                    {
                        TankId = t.Id,
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
                        using (PHDHistorian oPhd = new PHDHistorian())
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
                                    tankData.TankConfigId = t.TankId;
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
                                        else if (tagName.EndsWith(".LL") || tagName.EndsWith(".LEVEL_MM"))
                                        {
                                            tankData.LiquidLevel = tagValue;
                                        }
                                        else if (tagName.EndsWith(".LL_FWL") || tagName.EndsWith(".LEVEL_FWL"))
                                        {
                                            tankData.ProductLevel = tagValue;
                                        }
                                        else if (tagName.EndsWith(".FWL"))
                                        {
                                            tankData.FreeWaterLevel = tagValue;  
                                        }
                                        else if (tagName.EndsWith(".DREF") || tagName.EndsWith(".REF_DENS"))
                                        {
                                            tankData.ReferenceDensity = tagValue;
                                        }
                                        else if (tagName.EndsWith(".NSV"))
                                        {
                                            tankData.NetStandardVolume = tagValue;
                                        }
                                        else if (tagName.EndsWith(".WIA"))
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

                    logger.Info("Sync inventory tanks data finished!");
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