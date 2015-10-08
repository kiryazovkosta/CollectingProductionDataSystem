namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    using System.Data.Entity;
    using System.Data.Entity.Validation;
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
    using CollectingProductionDataSystem.Models.Transactions;
    using System.Collections.Generic;

    static class Phd2SqlProductionDataMain
    {
        private static readonly int ZERO = 0;
        private static readonly int ONE = 1;
        private static readonly int FIVE = 5;
        private static readonly int THIRTEEN = 13;
        private static readonly int TWENTY_ONE = 21;

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
            //try
            //{
            //    logger.Info("Sync inspection points started!");

            //    using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister())))
            //    {
            //        var units = context.UnitConfigs
            //            .All()
            //            .Where(u => u.IsInspectionPoint == true)
            //            .Select(u => new InspectionPoint()
            //            {
            //                UnitId = u.Id,
            //                InspectionPointTag = u.CurrentInspectionDataTag
            //            });

            //        if (units.Count() > 0)
            //        {
            //            using (PHDHistorian oPhd = new PHDHistorian())
            //            {
            //                using (PHDServer defaultServer = new PHDServer(Properties.Settings.Default.PHD_HOST))
            //                {
            //                    defaultServer.Port = Properties.Settings.Default.PHD_PORT;
            //                    defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
            //                    oPhd.DefaultServer = defaultServer;
            //                    oPhd.StartTime = "NOW - 2M";
            //                    oPhd.EndTime = "NOW - 2M";
            //                    oPhd.Sampletype = Properties.Settings.Default.INSPECTION_DATA_SAMPLETYPE;
            //                    oPhd.MinimumConfidence = Properties.Settings.Default.INSPECTION_DATA_MINIMUM_CONFIDENCE;
            //                    oPhd.MaximumRows = Properties.Settings.Default.INSPECTION_DATA_MAX_ROWS;

            //                    // get all inspection data
            //                    foreach (var item in units)
            //                    {
            //                        var unitInspectionPointData = new UnitsInspectionData();
            //                        unitInspectionPointData.UnitConfigId = item.UnitId;
            //                        DataSet dsGrid = oPhd.FetchRowData(item.InspectionPointTag);
            //                        var confidence = 100;
            //                        foreach (DataRow row in dsGrid.Tables[0].Rows)
            //                        {
            //                            foreach (DataColumn dc in dsGrid.Tables[0].Columns)
            //                            {
            //                                if (dc.ColumnName.Equals("Tolerance") || dc.ColumnName.Equals("HostName"))
            //                                {
            //                                    continue;
            //                                }
            //                                else if (dc.ColumnName.Equals("Confidence"))
            //                                {
            //                                    if (!string.IsNullOrWhiteSpace(row[dc].ToString()))
            //                                    {
            //                                        confidence = Convert.ToInt32(row[dc]);
            //                                    }
            //                                    else
            //                                    {
            //                                        confidence = 0;
            //                                        break;
            //                                    }
                                                
            //                                }
            //                                else if (dc.ColumnName.Equals("Value"))
            //                                {
            //                                    if (!string.IsNullOrWhiteSpace(row[dc].ToString()))
            //                                    {
            //                                        unitInspectionPointData.Value = Convert.ToDecimal(row[dc]);
            //                                    }
            //                                }
            //                                else if (dc.ColumnName.Equals("TimeStamp"))
            //                                {
            //                                    var recordDt = row[dc].ToString();
            //                                    if (!string.IsNullOrWhiteSpace(recordDt))
            //                                    {
            //                                         var recordTimestamp = DateTime.ParseExact(recordDt, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //                                        if (TimeZoneInfo.Local.IsDaylightSavingTime(recordTimestamp))
            //                                        {
            //                                            recordTimestamp = recordTimestamp.AddHours(-1);
            //                                        }
            //                                        unitInspectionPointData.RecordTimestamp = recordTimestamp;   
            //                                    }
                                               
            //                                }
            //                            }
            //                        }
            //                        if (confidence > Properties.Settings.Default.INSPECTION_DATA_MINIMUM_CONFIDENCE
            //                            && unitInspectionPointData.RecordTimestamp != null)
            //                        {
            //                            context.UnitsInspectionData.Add(unitInspectionPointData);                                            
            //                        }
            //                    }

            //                    context.SaveChanges("PHD2SQLInspectionData");
            //                }
            //            }
            //        }

            //        logger.Info("Sync inspection points finished!");
            //    }
            //}
            //catch (DataException validationException)
            //{
            //    var dbEntityException = validationException.InnerException as DbEntityValidationException;
            //    if (dbEntityException != null)
            //    {
            //        foreach (var validationErrors in dbEntityException.EntityValidationErrors)
            //        {
            //            foreach (var validationError in validationErrors.ValidationErrors)
            //            {
            //                logger.ErrorFormat("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    logger.Error(ex);
            //}
        }

        internal static void ProcessPrimaryProductionData()
        {
            try
            {
                logger.Info("Sync primary data started!");

                using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister())))
                {
                    using (PHDHistorian oPhd = new PHDHistorian())
                    {
                        using (PHDServer defaultServer = new PHDServer(Properties.Settings.Default.PHD_HOST))
                        {
                            SetPhdConnectionSettings(oPhd, defaultServer);

                            var now = DateTime.Now;
                            var recordDataTime = GetRecordTimestamp(now);
                            var shift = GetShift(now);

                            var unitsConfigsList = context.UnitConfigs.All().ToList();
                            var unitsData = context.UnitsData.All().Where(x => x.RecordTimestamp == recordDataTime && x.ShiftId == shift).ToList();

                            foreach (var unitConfig in unitsConfigsList)
                            {
                                if (unitConfig.CollectingDataMechanism.ToUpper() == "A")
                                {
                                    var confidence = 0;

                                    var s = unitsData.Where(x => x.UnitConfigId == unitConfig.Id).FirstOrDefault();
                                    if (s != null)
                                    {
                                        logger.InfoFormat("Exists: {0} : {1} : {2}", unitConfig.Id, unitConfig.PreviousShiftTag, s.Value);
                                        continue;
                                    }

                                    var unitData = GetUnitData(unitConfig, oPhd, out confidence);
                                    if (confidence > Properties.Settings.Default.INSPECTION_DATA_MINIMUM_CONFIDENCE && unitData.RecordTimestamp != null)
                                    {
                                        if (now.Hour < 13)
                                        {
                                            var prevDay = unitData.RecordTimestamp.AddDays(-1).Date;
                                            unitData.RecordTimestamp = prevDay;
                                        }

                                        if (unitsData.Where(x => x.RecordTimestamp == unitData.RecordTimestamp && x.ShiftId == shift).FirstOrDefault() == null)
                                        {
                                            unitData.ShiftId = shift;
                                            unitData.RecordTimestamp = unitData.RecordTimestamp.Date;
                                            context.UnitsData.Add(unitData); 
                                        }
                                    }
                                    else
                                    {
                                        SetDefaultValue(context, recordDataTime, shift, unitsData, unitConfig);
                                    }

                                    logger.InfoFormat("New : {0} : {1} : {2}", unitConfig.Id, unitConfig.PreviousShiftTag, unitData.Value);
                                }
                                else 
                                {
                                    SetDefaultValue(context, recordDataTime, shift, unitsData, unitConfig);
                                }
                            }

                            context.SaveChanges("PHD2SQLPreviousShift");
                        }
                    }
                }

                logger.Info("Sync primary data finished!");
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
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
        }

        private static void SetDefaultValue(ProductionData context, DateTime recordDataTime, ShiftType shift, List<UnitsData> unitsData, UnitConfig unitConfig)
        {
            var u = unitsData.Where(x => x.UnitConfigId == unitConfig.Id).FirstOrDefault();
            if (u == null)
            {
                context.UnitsData.Add(
                new UnitsData
                {
                    UnitConfigId = unitConfig.Id,
                    Value = null,
                    RecordTimestamp = recordDataTime,
                    ShiftId = shift
                });
            }
        }

        private static DateTime GetRecordTimestamp(DateTime recordDateTime)
        {
            var result = new DateTime(recordDateTime.Year, recordDateTime.Month, recordDateTime.Day, 0, 0, 0);

            if (/*recordDateTime.Hour >= 5 && */recordDateTime.Hour < 13)
            {
                result = result.AddDays(-1);
            }

            return result;
        }

        private static ShiftType GetShift(DateTime recordDateTime)
        {
            if (recordDateTime.Hour >= 5 && recordDateTime.Hour < 13)
            {
                return ShiftType.Third;
            }
            else if (recordDateTime.Hour >= 13 && recordDateTime.Hour < 21)
            {
                return ShiftType.First;
            }
            else
            {
                return ShiftType.Second;
            }
        }
 
        private static void SetPrimaryDataInRange(DateTime now, ProductionData context, UnitsData unitData, ShiftType shiftType, int startHour, int endHour)
        {
            var startDate = new DateTime(now.Year, now.Month, now.Day, startHour, ONE, ZERO);
            var endDate = new DateTime(now.Year, now.Month, now.Day, endHour, ZERO, ZERO);
            var range = new DateRange(startDate, endDate);
            var s = context.UnitsData
                           .All()
                           .ToList()
                           .FirstOrDefault(x => x.UnitConfigId == unitData.UnitConfigId &&
                                                range.Includes(x.RecordTimestamp));
            if (s == null)
            {
                unitData.ShiftId = shiftType;
                context.UnitsData.Add(unitData);   
            }
            else
            {
                logger.InfoFormat("[ProcessPrimaryProductionData][{0}][{1}][{2}]-[{3}][{4}][{5}] already exists", s.RecordTimestamp, s.UnitConfigId, s.Value, unitData.RecordTimestamp, unitData.UnitConfigId, unitData.Value);
            }
        }

        private static void SetPhdConnectionSettings(PHDHistorian oPhd, PHDServer defaultServer)
        {
            defaultServer.Port = Properties.Settings.Default.PHD_PORT;
            defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
            oPhd.DefaultServer = defaultServer;
            oPhd.StartTime = "NOW - 2M";
            oPhd.EndTime = "NOW - 2M";
            oPhd.Sampletype = Properties.Settings.Default.INSPECTION_DATA_SAMPLETYPE;
            oPhd.MinimumConfidence = Properties.Settings.Default.INSPECTION_DATA_MINIMUM_CONFIDENCE;
            oPhd.MaximumRows = Properties.Settings.Default.INSPECTION_DATA_MAX_ROWS;
        }
 
        private static UnitsData GetUnitData(UnitConfig unitConfig, PHDHistorian oPhd, out int confidence)
        {
            var unitData = new UnitsData();
            unitData.UnitConfigId = unitConfig.Id;
            DataSet dsGrid = oPhd.FetchRowData(unitConfig.PreviousShiftTag);
            confidence = 100;
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
                           unitData.Value = Convert.ToDecimal(row[dc]); 
                        }
                    }
                    else if (dc.ColumnName.Equals("TimeStamp"))
                    {
                        if (!string.IsNullOrEmpty(row[dc].ToString()))
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
            }

            return unitData;
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

                                context.SaveChanges("PHD2SQLInventory");
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
                if (DateTime.Now.Hour < 5)
                {
                    logger.Info("Sync measurements points data : hours are before 5o'clock");
                    return;
                }

                using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister())))
                {
                    var measuringPoints = context.MeasurementPointsProductConfigs.All();

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

                                // get all inspection data
                                foreach (var item in measuringPoints)
                                {
                                    var measurementPointData = new MeasurementPointsProductsData();
                                    measurementPointData.MeasurementPointsProductsConfigId = item.Id;
                                    DataSet dsGrid = oPhd.FetchRowData(item.PhdTotalCounterTag);
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
                                            else if (dc.ColumnName.Equals("TimeStamp"))
                                            {
                                                var recordDt = row[dc].ToString();
                                                if (!string.IsNullOrWhiteSpace(recordDt))
                                                {
                                                     var recordTimestamp = DateTime.ParseExact(recordDt, "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                                    if (TimeZoneInfo.Local.IsDaylightSavingTime(recordTimestamp))
                                                    {
                                                        recordTimestamp = recordTimestamp.AddHours(-1);
                                                    }
                                                    measurementPointData.RecordTimestamp = recordTimestamp;   
                                                }
                                               
                                            }
                                        }
                                    }
                                    if (confidence > Properties.Settings.Default.INSPECTION_DATA_MINIMUM_CONFIDENCE
                                        && measurementPointData.RecordTimestamp != null)
                                    {
                                        context.MeasurementPointsProductsDatas.Add(measurementPointData);                                            
                                    }
                                }

                                context.SaveChanges("x");
                            }
                        }
                    }

                    logger.Info("Sync measurements points data finished!");
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