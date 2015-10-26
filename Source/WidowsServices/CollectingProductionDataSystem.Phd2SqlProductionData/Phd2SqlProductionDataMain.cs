namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Common;
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
    using CollectingProductionDataSystem.Application.UnitsDataServices;
    using CollectingProductionDataSystem.Application.PrimaryDataServices;

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
                var result = service.ReadAndSaveUnitsDataForShift();
                logger.InfoFormat("Successfully added {0} records to CollectingPrimaryDataSystem", result.ResultRecordsCount);
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
        }

        internal static void ProcessInventoryTanksData()
        {
            try
            {
                logger.Info("Sync inventory tanks data started!");

                using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister())))
                {
                    DateTime recordTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
                    
                    // TODO: Mechanism to get all data for past period
                    for (int hours = 1; hours < Properties.Settings.Default.UPDATE_INVENTORY_DATA_INTERVAL; hours++)
                    {
                        var checkedDateTime = recordTime.AddHours(-hours);
                        if (!context.TanksData.All().Where(td => td.RecordTimestamp.CompareTo(checkedDateTime) == 0).Any())
                        {
                            logger.InfoFormat("The data for for {0:yyyy-MM-dd HH:ss:mm} does not exsits!", checkedDateTime);
                        }
                    }

                    if (context.TanksData.All().Where(td => td.RecordTimestamp.CompareTo(recordTime) == 0).Any())
                    {
                        logger.InfoFormat("There are already a records for that time of the day: {0:yyyy-MM-dd HH:ss:mm}!", recordTime);
                        logger.Info("Sync inventory tanks data finished!");
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
                        PhdTagWeightInAir = t.PhdTagWeightInAir,
                        PhdTagWeightInVaccum = t.PhdTagWeightInVacuum,
                        CorrectionFactor = t.CorrectionFactor
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
                                    tankData.CorrectionFactor = t.CorrectionFactor;

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

                                    context.TanksData.Add(tankData);
                                }

                                context.SaveChanges("PHD2SQL");
                            }
                        }
                    }

                    logger.Info("Sync inventory tanks data finished!");
                }
            }
            catch (DataException validationException)
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
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
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

        private static bool TimeBetween(DateTime datetime, TimeSpan start, TimeSpan end)
        {
            TimeSpan now = datetime.TimeOfDay;
            if (start < end)
            {
                return start <= now && now <= end;
            }

            return !(end < now && now < start);
        }

        public interface IRange<T>
        {
            T Start { get; }

            T End { get; }

            bool Includes(T value);

            bool Includes(IRange<T> range);
        }

        public class DateRange : IRange<DateTime> 
        {
            public DateRange(DateTime start, DateTime end)
            {
                Start = start;
                End = end;
            }

            public DateTime Start { get; private set; }

            public DateTime End { get; private set; }

            public bool Includes(DateTime value)
            {
                return (Start <= value) && (value <= End);
            }

            public bool Includes(IRange<DateTime> range)
            {
                return (Start <= range.Start) && (range.End <= End);
            }
        }
    }
}