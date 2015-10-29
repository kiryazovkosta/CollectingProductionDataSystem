namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Common;
    using CollectingProductionDataSystem.PhdApplication.PrimaryDataServices;
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
    using CollectingProductionDataSystem.Models.Transactions;
    using System.Collections.Generic;

    static class Phd2SqlProductionDataMain
    {
        private static readonly ILog logger;

        private static readonly NinjectConfig ninject;

        static Phd2SqlProductionDataMain()
        {
            logger = LogManager.GetLogger("CollectingProductionDataSystem.Phd2SqlProductionData");
            ninject = new NinjectConfig();

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

        internal static void ProcessPrimaryProductionData()
        {
            try
            {
                logger.Info("Sync primary data started!");
                var kernel = ninject.Kernel;
                var service = kernel.GetService(typeof(PhdPrimaryDataService)) as PhdPrimaryDataService;
                var insertedRecords = service.ReadAndSaveUnitsDataForShift();
                logger.InfoFormat("Successfully added {0} records to CollectingPrimaryDataSystem", insertedRecords);
                logger.Info("Sync primary data finished!");
            }
            catch (DataException validationException)
            {
                LogValidationDataException(validationException);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private static void LogValidationDataException(DataException validationException)
        {
            var dbEntityException = validationException.InnerException as DbEntityValidationException;
            if (dbEntityException != null)
            {
                foreach (var validationErrors in dbEntityException.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        logger.ErrorFormat("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            else
            {
                logger.Error(validationException.ToString());
            }
        }

        internal static void ProcessInventoryTanksData()
        {
            try
            {
                logger.Info("Sync inventory tanks data started!");
                var now = DateTime.Now;

                using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister())))
                {
                    DateTime recordTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
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
                        PhdTagWeightInAir = t.PhdTagWeightInAir,
                        PhdTagWeightInVaccum = t.PhdTagWeightInVacuum
                    });
                    
                    // TODO: Mechanism to get all data for past period
                    for (int hours = Properties.Settings.Default.MIN_GET_INVENTORY_HOURS_INTERVAL; hours < Properties.Settings.Default.MAX_GET_INVENTORY_HOURS_INTERVAL; hours++)
                    {
                        var checkedDateTime = recordTime.AddHours(-hours);
                        var ts = recordTime - checkedDateTime;
                        var getRecordTimestamp = String.Format("NOW-{0}H{1}M", ts.TotalHours, now.Minute - 2);

                        if (context.TanksData.All().Where(td => td.RecordTimestamp == checkedDateTime).Any())
                        {
                            logger.InfoFormat("There are already a records for that time of the day: {0:yyyy-MM-dd HH:ss:mm}!", checkedDateTime);
                            continue;
                        }

                        if (tanks.Count() > 0)
                        {
                            using (PHDHistorian oPhd = new PHDHistorian())
                            {
                                using (PHDServer defaultServer = new PHDServer(Properties.Settings.Default.PHD_HOST))
                                {
                                    defaultServer.Port = Properties.Settings.Default.PHD_PORT;
                                    defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
                                    oPhd.DefaultServer = defaultServer;
                                    oPhd.StartTime = getRecordTimestamp;
                                    oPhd.EndTime = getRecordTimestamp;
                                    oPhd.Sampletype = SAMPLETYPE.Raw;
                                    oPhd.MinimumConfidence = 100;
                                    oPhd.MaximumRows = 1;

                                    var tanksDataList = new List<TankData>();

                                    foreach (var t in tanks)
                                    {
                                        var tankData = new TankData();
                                        tankData.RecordTimestamp = checkedDateTime;
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
                                        SetPhdTag(t.PhdTagWeightInVaccum, tags);
    
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
                                                var prId = Convert.ToInt32(tagValue);
                                                var product = context.TankMasterProducts.All().Where(x => x.TankMasterProductCode == prId).FirstOrDefault();
                                                if (product != null)
                                                {
                                                    tankData.ProductId = product.Id;
                                                    var pr = context.Products.All().Where(p => p.Id == product.Id).FirstOrDefault();
                                                    if (pr != null)
                                                    {
                                                        tankData.ProductName = pr.Name;  
                                                    }
                                                    else
                                                    {
                                                        tankData.ProductName = "N/A";
                                                    }
                                                }
                                                else 
                                                { 
                                                    tankData.ProductId = Convert.ToInt32(tagValue);
                                                    tankData.ProductName = "N/A";
                                                }
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
                                            else if (tagName.EndsWith(".WIV"))
                                            {
                                                tankData.WeightInVacuum = tagValue;
                                            }
                                        }

                                        tanksDataList.Add(tankData);
                                    }

                                    if (tanksDataList.Count > 0)
                                    {
                                        context.TanksData.BulkInsert(tanksDataList, "Phd2SqlLoading");
                                        var status = context.SaveChanges("Phd2SqlLoading");
                                        logger.InfoFormat("Inserted {0} records for: {1:yyyy-MM-dd HH:ss:mm}!", status.ResultRecordsCount, checkedDateTime);
                                    }
                                }
                            }
                        }

                        //ProcessInventoryTankForSpecificDateTime(checkedDateTime);
                    }

                    

                    

                    logger.Info("Sync inventory tanks data finished!");
                }
            }
            catch (DataException validationException)
            {
                LogValidationDataException(validationException);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }

        private static void ProcessInventoryTankForSpecificDateTime(DateTime checkedDateTime)
        {
            throw new NotImplementedException();
        }

        internal static void ProcessMeasuringPointsData()
        {
            try
            {
                logger.Info("Sync measurements points data started!");
                var now = DateTime.Now;
                var today = DateTime.Today;
                var fiveOClock = today.AddHours(5);
                
                var ts = now - fiveOClock;
                if (ts.TotalMinutes > 2 && ts.Hours == 0)
                {
                    using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister())))
                    {
                        var currentDate = today.AddDays(-1);
                        if (context.MeasurementPointsProductsDatas.All().Where(x => x.RecordTimestamp == currentDate).Any())
                        {
                            logger.InfoFormat("There is already an active transaction data for {0}", currentDate);
                            logger.Info("Sync measurements points data finished!");
                            return;
                        }

                        var measuringPoints = context.MeasuringPointConfigs.All().Where(x => !string.IsNullOrEmpty(x.TotalizerCurrentValueTag));

                        if (measuringPoints.Count() > 0)
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

                                    foreach (var item in measuringPoints)
                                    {
                                        var measurementPointData = new MeasuringPointProductsData();
                                        measurementPointData.MeasuringPointConfigId = item.Id;
                                        DataSet dsGrid = oPhd.FetchRowData(item.TotalizerCurrentValueTag);
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
                                                    if (!string.IsNullOrWhiteSpace(row[dc].ToString()))
                                                    {
                                                        confidence = Convert.ToInt32(row[dc]);
                                                    }
                                                    else
                                                    {
                                                        confidence = 0;
                                                        break;
                                                    }
                                                }
                                                else if (dc.ColumnName.Equals("Value"))
                                                {
                                                    if (!string.IsNullOrWhiteSpace(row[dc].ToString()))
                                                    {
                                                        measurementPointData.Value = Convert.ToDecimal(row[dc]);
                                                    }
                                                }
                                            }
                                        }
                                        if (confidence > Properties.Settings.Default.INSPECTION_DATA_MINIMUM_CONFIDENCE &&
                                            measurementPointData.RecordTimestamp != null)
                                        {
                                            measurementPointData.RecordTimestamp = currentDate;
                                            context.MeasurementPointsProductsDatas.Add(measurementPointData);
                                        }
                                    }

                                    context.SaveChanges("Phd2Sql");
                                }
                            }
                        }
                    }
                }

                logger.Info("Sync measurements points data finished!");
            }
            catch (DataException validationException)
            {
                LogValidationDataException(validationException);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
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