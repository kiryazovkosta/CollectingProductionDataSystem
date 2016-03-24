namespace CollectingProductionDataSystem.PhdApplication.PrimaryDataServices
{
    using System.Configuration;
    using System.Data;
    using System.Data.Entity;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Transactions;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Application.ProductionDataServices;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Common;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Enumerations;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.PhdApplication.Models;
    using Ninject;
    using Uniformance.PHD;
    using CollectingProductionDataSystem.PhdApplication.Contracts;
    using System.Reflection;
    using log4net;
    using System.Data.Entity;
    using log4net.Core;

    public class PhdPrimaryDataService : IPhdPrimaryDataService
    {
        private readonly ILog logger;
        private readonly IMailerService mailer;
        private TransactionOptions transantionOption;
        private IProductionData data;

        public PhdPrimaryDataService(IProductionData dataParam, ILog loggerParam, IMailerService mailerServiceParam)
        {
            this.data = dataParam;
            this.logger = loggerParam;
            this.mailer = mailerServiceParam;
            this.transantionOption = DefaultTransactionOptions.Instance.TransactionOptions;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.data.Dispose();
        }

        /// <summary>
        /// Gets the shift by id.
        /// </summary>
        /// <param name="shiftId">The shift id.</param>
        /// <returns></returns>
        public Shift GetShiftById(int shiftId)
        {
            return this.data.Shifts.GetById(shiftId);
        }

        /// <summary>
        /// Clears the temporary data.
        /// </summary>
        public void ClearTemporaryData()
        {
            this.data.DbContext.DbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [UnitDataTemporaryRecords]");
        }

        private int ReadAndSaveUnitsDataForShift(DateTime targetRecordTimestamp,
            Shift targetShift,
            PrimaryDataSourceType dataSource,
            bool isForcedResultCalculation,
            ref bool lastOperationSucceeded,
            TreeState isFirstPhdInteraceCompleted)
        {
            var timer = new Stopwatch();
            var saveChangesTimer = new Stopwatch();
            timer.Start();
            logger.InfoFormat("-------------------------------------------------------- Begin Interface Iteration For {0} Shift {1} -------------------------------------------------------- ", dataSource.ToString(), targetShift.Id);

            var exeFileName = Assembly.GetEntryAssembly().Location;
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(exeFileName);
            ConfigurationSectionGroup appSettingsGroup = configuration.GetSectionGroup("applicationSettings");
            ConfigurationSection appSettingsSection = appSettingsGroup.Sections[0];
            ClientSettingsSection settings = appSettingsSection as ClientSettingsSection;
            int expectedNumberOfRecords = 0;
            int realNumberOfRecords = 0;
            var unitDatasToAdd = new List<UnitDatasTemp>();
            var unitsConfigsList = this.data.UnitConfigs.All()
                .Include(x => x.RelatedUnitConfigs)
                .Include(x => x.RelatedUnitConfigs.Select(y => y.UnitConfig))
                .Include(x => x.RelatedUnitConfigs.Select(z => z.RelatedUnitConfig).Select(w => w.UnitDatasTemps))
                .Where(x => x.DataSource == dataSource).ToList();
            var targetRecordTimestampDate = targetRecordTimestamp.Date;

            try
            {
                using (PHDHistorian oPhd = new PHDHistorian())
                {
                    using (PHDServer defaultServer = new PHDServer(settings.Settings.Get("PHD_HOST" + (int)dataSource).Value.ValueXml.InnerText))
                    {
                        SetPhdConnectionSettings(oPhd, defaultServer, targetRecordTimestamp, targetShift);
                        var unitsData = this.data.UnitDatasTemps.All().Where(x => x.RecordTimestamp == targetRecordTimestampDate && x.ShiftId == targetShift.Id).ToList();

                        var newRecords = ProcessAutomaticUnits(unitsConfigsList, unitsData, oPhd, targetRecordTimestampDate, targetShift.Id, ref expectedNumberOfRecords);
                        realNumberOfRecords = newRecords.Count();
                        unitDatasToAdd.AddRange(newRecords);
                        LogConsistencyMessage("Processing Automatic Units Records", expectedNumberOfRecords, realNumberOfRecords);

                        newRecords = ProcessAutomaticDeltaUnits(unitsConfigsList, unitsData, oPhd, targetRecordTimestampDate, targetShift, ref expectedNumberOfRecords);
                        realNumberOfRecords = newRecords.Count();
                        unitDatasToAdd.AddRange(newRecords);
                        LogConsistencyMessage("Processing Automatic Delta Records", expectedNumberOfRecords, realNumberOfRecords);

                        newRecords = ProcessAutomaticCalulatedUnits(unitsConfigsList, unitsData, oPhd, targetRecordTimestampDate, targetShift, ref expectedNumberOfRecords);
                        realNumberOfRecords = newRecords.Count();
                        unitDatasToAdd.AddRange(newRecords);
                        LogConsistencyMessage("Processing Automatic Calculated Units Records", expectedNumberOfRecords, realNumberOfRecords);

                        newRecords = ProcessManualUnits(unitsConfigsList, targetRecordTimestampDate, targetShift.Id, unitsData, ref expectedNumberOfRecords);
                        realNumberOfRecords = newRecords.Count();
                        unitDatasToAdd.AddRange(newRecords);
                        LogConsistencyMessage("Processing Manual Units Records", expectedNumberOfRecords, realNumberOfRecords);

                        newRecords = ProcessCalculatedByAutomaticUnits(unitsConfigsList, oPhd, targetRecordTimestampDate, targetShift, unitsData, ref expectedNumberOfRecords);
                        realNumberOfRecords = newRecords.Count();
                        unitDatasToAdd.AddRange(newRecords);
                        LogConsistencyMessage("Processing Calculated By Automatic Records", expectedNumberOfRecords, realNumberOfRecords);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + ex.StackTrace);
                mailer.SendMail(ex.Message + ex.StackTrace, "Phd2Interface Error");
            }

            var totalInsertedRecords = 0;
            saveChangesTimer.Start();
            // persisting received data and incorporate the data with records get from second PHD

            try
            {
                if (unitDatasToAdd.Count > 0)
                {
                    this.data.UnitDatasTemps.BulkInsert(unitDatasToAdd, "Phd2SqlLoader");
                    this.data.SaveChanges("Phd2SqlLoader");

                    totalInsertedRecords += unitDatasToAdd.Count;
                }

                totalInsertedRecords += GetCalculatedUnits(targetShift, unitsConfigsList, targetRecordTimestampDate);

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + ex.StackTrace, ex);

            }

            List<UnitDatasTemp> resultUnitData;
            lastOperationSucceeded = CheckIfLastOperationSucceded(unitsConfigsList, targetRecordTimestampDate, targetShift.Id, out resultUnitData);
            //if (isForcedResultCalculation && !lastOperationSucceeded)
            //{
            //    var additionalRecords = CreateMissingRecords(targetRecordTimestamp, targetShift);
            //    if (additionalRecords.Count() > 0)
            //    {
            //        this.data.UnitDatasTemps.BulkInsert(additionalRecords, "Phd2SqlLoader");
            //        totalInsertedRecords += additionalRecords.Count();
            //        LogConsistencyMessage("Added Missing Records", additionalRecords.Count(), additionalRecords.Count());
            //    }

            //    lastOperationSucceeded = true;
            //}

            //if (lastOperationSucceeded == true)
            //{
            //    //ToDo: Commented for the test
            //    //totalInsertedRecords = FlashDataToOriginalUnitData(out expectedNumberOfRecords);
            //    //LogConsistencyMessage("Records flashed to original UnitsData", expectedNumberOfRecords, totalInsertedRecords);
            //}

            saveChangesTimer.Stop();
            timer.Stop();
            logger.InfoFormat("\tEstimated time for data fetching: {0} s", timer.Elapsed - saveChangesTimer.Elapsed);
            logger.InfoFormat("\tEstimated time for Iteration result saving: {0} s", saveChangesTimer.Elapsed);
            logger.InfoFormat("\tTotal Estimated time for Iteration: {0} s", timer.Elapsed);
            logger.InfoFormat("\tTotal number of persisted records: {0}", totalInsertedRecords);
            logger.InfoFormat("-------------------------------------------------------  End Interface Iteration For {0} Shift {1} -------------------------------------------------------- ", dataSource.ToString(), targetShift.Id);

            return totalInsertedRecords;
        }


        /// <summary>
        /// Gets the calculated units.
        /// </summary>
        /// <param name="targetShift">The target shift.</param>
        /// <param name="unitsConfigsList">The units configs list.</param>
        /// <param name="targetRecordTimestampDate">The target record timestamp date.</param>
        /// <returns></returns>
        private int GetCalculatedUnits(Shift targetShift, List<UnitConfig> unitsConfigsList, DateTime targetRecordTimestampDate)
        {
            int totalInsertedRecords = 0;
            int expectedNumberOfRecords = 0;
            int realNumberOfRecords = 0;
            var unitsTempData = this.data.UnitDatasTemps.All().Where(x => x.RecordTimestamp == targetRecordTimestampDate && x.ShiftId == targetShift.Id).ToList();

            var calculatedUnitDatas = ProcessCalculatedUnits(unitsConfigsList,
                                                             targetRecordTimestampDate,
                                                             targetShift.Id,
                                                             unitsTempData,
                                                             ref expectedNumberOfRecords);

            realNumberOfRecords = calculatedUnitDatas.Count();
            LogConsistencyMessage("Processing Calculated Records", expectedNumberOfRecords, realNumberOfRecords);

            if (calculatedUnitDatas.Count() > 0)
            {
                this.data.UnitDatasTemps.BulkInsert(calculatedUnitDatas, "Phd2SqlLoader");
                this.data.SaveChanges("Phd2SqlLoader");

                totalInsertedRecords += calculatedUnitDatas.Count();
            }

            return totalInsertedRecords;
        }

        /// <summary>
        /// Checks if last operation succeded.
        /// </summary>
        /// <param name="unitsConfigsList">The units configs list.</param>
        /// <param name="targetDate">The target date.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        private bool CheckIfLastOperationSucceded(List<UnitConfig> unitsConfigsList,
                                                    DateTime targetDate,
                                                    int targetShiftId,
                                                    out List<UnitDatasTemp> resultUnitData)
        {
            resultUnitData = this.data.UnitDatasTemps.All().Include(x => x.UnitConfig)
                                        .Where(x => x.RecordTimestamp == targetDate
                                        && x.ShiftId == targetShiftId).ToList();
            return unitsConfigsList.Count == resultUnitData.Count;

        }

        /// <summary>
        /// Gets the observed shift by date time.
        /// </summary>
        /// <param name="targetDateTime">The target date time.</param>
        /// <returns></returns>
        public Shift GetObservedShiftByDateTime(DateTime targetDateTime)
        {
            var baseDate = targetDateTime.Date;
            var resultShift = this.data.Shifts.All().ToList().FirstOrDefault(x =>
                                            (baseDate + x.ReadOffset) <= targetDateTime
                                            && targetDateTime <= (baseDate + x.ReadOffset + x.ReadPollTimeSlot));

            return resultShift;
        }

        class BeginEnd
        {
            public int Id { get; set; }
            public DateTime Begin { get; set; }
            public DateTime End { get; set; }

            public override string ToString()
            {
                return string.Format("{0} Begin: {1} End: {2}",
                    Id,
                    Begin.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture),
                    End.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
            }
        }

        /// <summary>
        /// Finalizes the shift observation.
        /// </summary>
        /// <param name="targetRecordTimestamp">The target record timestamp.</param>
        /// <param name="targetShift">The target shift.</param>
        public void FinalizeShiftObservation(DateTime targetRecordTimestamp, Shift targetShift)
        {
            int expectedNumberOfRecords = 0;

            var additionalRecords = CreateMissingRecords(targetRecordTimestamp, targetShift, ref expectedNumberOfRecords);
            if (additionalRecords.Count() > 0)
            {
                this.data.UnitDatasTemps.BulkInsert(additionalRecords, "Phd2SqlLoader");
                LogConsistencyMessage("Added Missing Records", additionalRecords.Count(), additionalRecords.Count());
                this.mailer.SendMail(string.Format("Successfully Added Missing {0} records to database", additionalRecords.Count()), "SAPO - Shift data");
            }

            var addedRecords = this.FlashDataToOriginalUnitData();
            LogConsistencyMessage("Finally Flashed Records To Units Data", expectedNumberOfRecords, addedRecords);
            this.mailer.SendMail(string.Format("Successfully Finally Flashed {0} records to UnitsData", addedRecords), "SAPO - Shift data");

        }
      
        /// <summary>
        /// Creates the missing records.
        /// </summary>
        /// <param name="targetRecordTimestamp">The target record timestamp.</param>
        /// <param name="shift">The shift.</param>
        /// <param name="expectedNumberOfRecords">The expected number of records.</param>
        /// <returns></returns>
        private IEnumerable<UnitDatasTemp> CreateMissingRecords(
            DateTime targetRecordTimestamp,
            Shift shift,
            ref int expectedNumberOfRecords)
        {
            var unitsConfigsList = this.data.UnitConfigs.All().ToList();
            var unitDatas = this.data.UnitDatasTemps.All().ToDictionary(x => x.UnitConfigId);
            var targetDate = targetRecordTimestamp.Date;
            var confidense = 0;
            var result = new List<UnitDatasTemp>();

            foreach (var position in unitsConfigsList)
            {
                if (!unitDatas.ContainsKey(position.Id))
                {
                    result.Add(this.SetDefaultUnitsDataValue(targetDate, shift.Id, position, confidense));
                }
            }

            expectedNumberOfRecords = unitsConfigsList.Count;
            return result;
        }

        /// <summary>
        /// Flashes the data to original unit data.
        /// </summary>
        /// <param name="expectedNumberOfRecords">The ecpected number of records.</param>
        /// <returns></returns>
        private int FlashDataToOriginalUnitData()
        {
            var timer = new Stopwatch();
            int resultNumberOfRecords = 0;
            using (var transaction = new TransactionScope(TransactionScopeOption.Required, this.transantionOption))
            {
                try
                {
                    var preparedUnitDatasTemps = this.data.UnitDatasTemps.All().ToList();
                    var preparedUnitsData = new List<UnitsData>();
                    foreach (var unitDataTemp in preparedUnitDatasTemps)
                    {
                        preparedUnitsData.Add(new UnitsData()
                        {
                            RecordTimestamp = unitDataTemp.RecordTimestamp,
                            UnitConfigId = unitDataTemp.UnitConfigId,
                            ShiftId = unitDataTemp.ShiftId,
                            Value = unitDataTemp.Value,
                            Confidence = unitDataTemp.Confidence
                        });
                    }

                    resultNumberOfRecords = preparedUnitsData.Count();
                    this.data.UnitsData.BulkInsert(preparedUnitsData, "Phd2SqlLoader");
                    this.data.DbContext.DbContext.Database.ExecuteSqlCommand("TRUNCATE TABLE [UnitDataTemporaryRecords]");
                    transaction.Complete();
                }
                catch (Exception ex)
                {
                    logger.Error(ex + ex.StackTrace);
                    throw ex;
                }
            }
            return resultNumberOfRecords;
        }

        /// <summary>
        /// Logs the consistency message.
        /// </summary>
        /// <param name="stepName">Name of the step.</param>
        /// <param name="expectedNumberOfRecords">The expected number of records.</param>
        /// <param name="realNumberOfRecords">The real number of records.</param>
        private void LogConsistencyMessage(object stepName, int expectedRecordsCount, int generatedRecordsCount)
        {
            logger.InfoFormat("\tOn step {0}: \n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tExpected number of records: {1} \n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tThe number of the generated records:{2}", stepName, expectedRecordsCount, generatedRecordsCount);
        }


        private IEnumerable<UnitDatasTemp> ProcessCalculatedUnits(List<UnitConfig> unitsConfigsList,
                                                                    DateTime recordDataTime,
                                                                    int shift, List<UnitDatasTemp> unitsTempData,
                                                                    ref int expectedNumberOfRecords)
        {
            var currentUnitDatas = new Dictionary<int, UnitDatasTemp>();

            var observedUnitConfigs = unitsConfigsList.Where(x => x.CollectingDataMechanism == "C");

            expectedNumberOfRecords = observedUnitConfigs.Count();

            foreach (var unitConfig in observedUnitConfigs)
            {
                try
                {
                    if (unitConfig.CalculatedFormula.Equals("C9"))
                    {
                        CalculateByMathExpression(unitConfig, recordDataTime, shift, unitsTempData, currentUnitDatas);
                    }
                    else
                    {
                        var formulaCode = unitConfig.CalculatedFormula ?? string.Empty;
                        var arguments = new FormulaArguments();
                        arguments.MaximumFlow = (double?)unitConfig.MaximumFlow;
                        arguments.EstimatedDensity = (double?)unitConfig.EstimatedDensity;
                        arguments.EstimatedPressure = (double?)unitConfig.EstimatedPressure;
                        arguments.EstimatedTemperature = (double?)unitConfig.EstimatedTemperature;
                        arguments.EstimatedCompressibilityFactor = (double?)unitConfig.EstimatedCompressibilityFactor;
                        arguments.CalculationPercentage = (double?)unitConfig.CalculationPercentage;
                        arguments.CustomFormulaExpression = unitConfig.CustomFormulaExpression;

                        var relatedUnitConfigs = unitConfig.RelatedUnitConfigs.ToList();
                        var confidence = 100;
                        var allRelatedUnitDataExsists = true;

                        foreach (var relatedUnitConfig in relatedUnitConfigs)
                        {
                            if (allRelatedUnitDataExsists == true)
                            {
                                var parameterType = relatedUnitConfig.RelatedUnitConfig.AggregateGroup;
                                var element = unitsTempData
                                    .Where(x => x.RecordTimestamp == recordDataTime)
                                    .Where(x => x.ShiftId == shift)
                                    .Where(x => x.UnitConfigId == relatedUnitConfig.RelatedUnitConfigId)
                                    .FirstOrDefault();

                                if (element == null)
                                {
                                    if (currentUnitDatas.ContainsKey(relatedUnitConfig.RelatedUnitConfigId))
                                    {
                                        element = currentUnitDatas[relatedUnitConfig.RelatedUnitConfigId];
                                    }

                                }

                                if (element != null)
                                {
                                    var inputValue = element.RealValue;

                                    if (element.Confidence != 100)
                                    {
                                        confidence = element.Confidence;
                                    }

                                    if (parameterType == "I+")
                                    {
                                        var exsistingValue = arguments.InputValue.HasValue ? arguments.InputValue.Value : 0.0;
                                        arguments.InputValue = exsistingValue + inputValue;
                                    }
                                    if (parameterType == "I-")
                                    {
                                        var exsistingValue = arguments.InputValue.HasValue ? arguments.InputValue.Value : 0.0;
                                        arguments.InputValue = exsistingValue - inputValue;
                                    }
                                    else if (parameterType == "T")
                                    {
                                        arguments.Temperature = inputValue;
                                    }
                                    else if (parameterType == "P")
                                    {
                                        arguments.Pressure = inputValue;
                                    }
                                    else if (parameterType == "D")
                                    {
                                        arguments.Density = inputValue;
                                    }
                                    else if (parameterType == "I/")
                                    {
                                        arguments.CalculationPercentage = inputValue;
                                    }
                                    else if (parameterType == "I*")
                                    {
                                        arguments.CalculationPercentage = inputValue;
                                    }
                                }
                                else
                                {
                                    allRelatedUnitDataExsists = false;
                                }
                            }

                        }

                        if (allRelatedUnitDataExsists == true)
                        {
                            var calculator = new ProductionDataCalculatorService(this.data);
                            var result = calculator.Calculate(formulaCode, arguments);

                            if (!unitsTempData.Where(x => x.RecordTimestamp == recordDataTime && x.ShiftId == shift && x.UnitConfigId == unitConfig.Id).Any())
                            {
                                currentUnitDatas.Add(unitConfig.Id,
                                                        new UnitDatasTemp
                                                        {
                                                            UnitConfigId = unitConfig.Id,
                                                            RecordTimestamp = recordDataTime,
                                                            ShiftId = shift,
                                                            Value = (double.IsNaN(result) || double.IsInfinity(result)) ? 0.0m : (decimal)result,
                                                            Confidence = confidence
                                                        });
                            }
                        }


                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("UnitConfigId: {0} \n [{1} \n {2}]", unitConfig.Id, ex.Message, ex.ToString()), ex);
                }
            }

            return currentUnitDatas.Values.ToList();
        }

        private void CalculateByMathExpression(UnitConfig unitConfig, DateTime recordDataTime,
                                                int shift,
                                                List<UnitDatasTemp> unitsData,
                                                Dictionary<int, UnitDatasTemp> calculatedUnitsData)
        {
            var inputParams = new Dictionary<string, double>();
            int indexCounter = 0;

            var mathExpression = unitConfig.CustomFormulaExpression;
            var relatedunitConfigs = unitConfig.RelatedUnitConfigs.ToList();
            var confidence = 100;
            foreach (var relatedunitConfig in relatedunitConfigs)
            {
                var element = data.UnitsData
                                  .All()
                                  .Where(x => x.RecordTimestamp == recordDataTime)
                                  .Where(x => x.ShiftId == shift)
                                  .Where(x => x.UnitConfigId == relatedunitConfig.RelatedUnitConfigId)
                                  .FirstOrDefault();
                var inputValue = (element != null) ? element.RealValue : 0.0;
                if (inputValue == 0.0)
                {
                    if (calculatedUnitsData.ContainsKey(relatedunitConfig.RelatedUnitConfigId))
                    {
                        inputValue = calculatedUnitsData[relatedunitConfig.RelatedUnitConfigId].RealValue;
                        if (calculatedUnitsData[relatedunitConfig.RelatedUnitConfigId].Confidence != 100)
                        {
                            confidence = calculatedUnitsData[relatedunitConfig.RelatedUnitConfigId].Confidence;
                        }
                    }
                }
                else
                {
                    if (element != null && element.Confidence != 100)
                    {
                        confidence = element.Confidence;
                    }
                }

                inputParams.Add(string.Format("p{0}", indexCounter), inputValue);
                indexCounter++;
            }

            double result = new ProductionDataCalculatorService(this.data).Calculate(mathExpression, "p", inputParams);
            if (!unitsData.Where(x => x.RecordTimestamp == recordDataTime && x.ShiftId == shift && x.UnitConfigId == unitConfig.Id).Any())
            {
                calculatedUnitsData.Add(
                    unitConfig.Id,
                    new UnitDatasTemp
                    {
                        UnitConfigId = unitConfig.Id,
                        RecordTimestamp = recordDataTime,
                        ShiftId = shift,
                        Value = (double.IsNaN(result) || double.IsInfinity(result)) ? 0.0m : (decimal)result,
                        Confidence = confidence,
                    });
            }
        }

        /// <summary>
        /// Processes the manual units.
        /// </summary>
        /// <param name="unitsConfigsList">The units configs list.</param>
        /// <param name="targetRecordTimestamp">The record data time.</param>
        /// <param name="shift">The shift.</param>
        /// <param name="unitsData">The units data.</param>
        /// <returns></returns>
        private IEnumerable<UnitDatasTemp> ProcessManualUnits(List<UnitConfig> unitsConfigsList,
                                                                DateTime targetRecordTimestamp,
                                                                int shiftId,
                                                                List<UnitDatasTemp> unitsData,
                                                                ref int expectedNumberOfRecords)
        {
            var currentUnitDatas = new List<UnitDatasTemp>();

            var observedUnitConfigs = unitsConfigsList.Where(x => x.CollectingDataMechanism == "M"
                                                                || x.CollectingDataMechanism == "MC"
                                                                || x.CollectingDataMechanism == "MD"
                                                                || x.CollectingDataMechanism == "MS");

            expectedNumberOfRecords = observedUnitConfigs.Count();

            foreach (var unitConfig in observedUnitConfigs)
            {
                var isRecordExists = unitsData.Any(x => x.RecordTimestamp == targetRecordTimestamp
                                                    && x.ShiftId == shiftId
                                                    && x.UnitConfigId == unitConfig.Id);
                if (!isRecordExists)
                {
                    currentUnitDatas.Add(SetDefaultUnitsDataValue(targetRecordTimestamp, shiftId, unitConfig, 100));
                }
            }

            return currentUnitDatas;
        }

        /// <summary>
        /// Processes the automatic units.
        /// </summary>
        /// <param name="unitsConfigsList">The units configs list.</param>
        /// <param name="unitsData">The units data.</param>
        /// <param name="oPhd">The o PHD.</param>
        /// <param name="recordTimestamp">The record data time.</param>
        /// <param name="shift">The shift.</param>
        /// <param name="expectedNumberOfRecords">The expected number of records.</param>
        /// <returns></returns>
        private IEnumerable<UnitDatasTemp> ProcessAutomaticUnits(List<UnitConfig> unitsConfigsList, List<UnitDatasTemp> unitsData,
                                                                PHDHistorian oPhd, DateTime recordTimestamp,
                                                                int shift, ref int expectedNumberOfRecords)
        {
            var currentUnitDatas = new List<UnitDatasTemp>();

            //ToDo: Find correct request
            var shifts = this.data.Shifts.All().ToList();
            var lastShift = shifts.FirstOrDefault(x => x.EndTime == shifts.Max(y => y.EndTime));

            var observedUnitConfigs = unitsConfigsList.Where(x => x.CollectingDataMechanism == "A");
            expectedNumberOfRecords = observedUnitConfigs.Count();

            foreach (var unitConfig in observedUnitConfigs)
            {
                var isRecordExists = unitsData.Any(x => x.RecordTimestamp == recordTimestamp
                                                     && x.ShiftId == shift
                                                     && x.UnitConfigId == unitConfig.Id);

                if (!isRecordExists)
                {
                    if (unitConfig.NeedToGetOnlyLastShiftValue == 1 && shift != lastShift.Id)
                    {
                        currentUnitDatas.Add(SetDefaultUnitsDataValue(recordTimestamp, shift, unitConfig, 100));
                    }
                    else
                    {
                        var confidence = 0;
                        var unitData = GetUnitDataFromPhd(unitConfig, oPhd, recordTimestamp, out confidence);
                        if (confidence >= Properties.Settings.Default.PHD_DATA_MIN_CONFIDENCE && unitData.RecordTimestamp != null)
                        {
                            unitData.ShiftId = shift;
                            unitData.RecordTimestamp = unitData.RecordTimestamp.Date;
                            unitData.Confidence = confidence;
                            currentUnitDatas.Add(unitData);
                        }
                        //else
                        //{
                        //    currentUnitDatas.Add(SetDefaultUnitsDataValue(recordTimestamp, shift, unitConfig, confidence));
                        //}
                    }
                }
            }

            return currentUnitDatas;
        }

        /// <summary>
        /// Processes the automatic delta units.
        /// </summary>
        /// <param name="unitsConfigsList">The units configs list.</param>
        /// <param name="unitsData">The units data.</param>
        /// <param name="oPhd">The o PHD.</param>
        /// <param name="targetRecordTimestamp">The record data time.</param>
        /// <param name="shift">The shift.</param>
        /// <param name="expectedNumberOfRecords">The expected number of records.</param>
        /// <returns></returns>
        private IEnumerable<UnitDatasTemp> ProcessAutomaticDeltaUnits(List<UnitConfig> unitsConfigsList, List<UnitDatasTemp> unitsData,
                                                                    PHDHistorian oPhd, DateTime targetRecordTimestamp,
                                                                    Shift shiftData, ref int expectedNumberOfRecords)
        {
            var currentUnitDatas = new List<UnitDatasTemp>();
            var baseDate = targetRecordTimestamp.Date;

            var observedUnitConfigs = unitsConfigsList.Where(x => x.CollectingDataMechanism == "AA");
            expectedNumberOfRecords = observedUnitConfigs.Count();

            foreach (var unitConfig in observedUnitConfigs)
            {
                if (!unitsData.Any(x => x.UnitConfigId == unitConfig.Id))
                {

                    if (shiftData != null)
                    {
                        var endShiftDateTime = baseDate + shiftData.EndTime;
                        var beginShiftDateTime = endShiftDateTime - shiftData.ShiftDuration;
                        //// Todo: remove after tests
                        //logger.InfoFormat("ProcessAutomaticDeltaUnits begin shift time: {0}", beginShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                        //logger.InfoFormat("ProcessAutomaticDeltaUnits end shift time: {0}", endShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                        ////
                        var beginConfidence = 100;
                        var endConfidence = 100;

                        oPhd.StartTime = string.Format("{0}", endShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                        oPhd.EndTime = oPhd.StartTime;

                        var result = oPhd.FetchRowData(unitConfig.PreviousShiftTag);
                        var row = result.Tables[0].Rows[0];
                        var endValue = Convert.ToInt64(row["Value"]);
                        endConfidence = Convert.ToInt32(row["Confidence"]);

                        oPhd.StartTime = string.Format("{0}", beginShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                        oPhd.EndTime = oPhd.StartTime;
                        result = oPhd.FetchRowData(unitConfig.PreviousShiftTag);
                        row = result.Tables[0].Rows[0];
                        var beginValue = Convert.ToInt64(row["Value"]);
                        beginConfidence = Convert.ToInt32(row["Confidence"]);

                        currentUnitDatas.Add(
                            new UnitDatasTemp
                            {
                                UnitConfigId = unitConfig.Id,
                                Value = (endValue - beginValue) / (unitConfig.EstimatedCompressibilityFactor ?? 1000.00m),
                                ShiftId = shiftData.Id,
                                RecordTimestamp = targetRecordTimestamp,
                                Confidence = (beginConfidence + endConfidence) / 2
                            });
                    }
                }
            }

            return currentUnitDatas;
        }

        /// <summary>
        /// Processes the automatic calulated units.
        /// </summary>
        /// <param name="unitsConfigsList">The units configs list.</param>
        /// <param name="unitsData">The units data.</param>
        /// <param name="oPhd">The o PHD.</param>
        /// <param name="targetRecordTimestamp">The record data time.</param>
        /// <param name="shift">The shift.</param>
        /// <param name="expectedNumberOfRecords">The expected number of records.</param>
        /// <returns></returns>
        private IEnumerable<UnitDatasTemp> ProcessAutomaticCalulatedUnits(List<UnitConfig> unitsConfigsList,
                                                                        List<UnitDatasTemp> unitsData,
                                                                        PHDHistorian oPhd,
                                                                        DateTime targetRecordTimestamp,
                                                                        Shift shiftData,
                                                                        ref int expectedNumberOfRecords)
        {
            var currentUnitDatas = new List<UnitDatasTemp>();
            var baseDate = targetRecordTimestamp.Date;

            var observedUnitConfigs = unitsConfigsList.Where(x => x.CollectingDataMechanism == "AC");
            expectedNumberOfRecords = observedUnitConfigs.Count();

            foreach (var unitConfig in observedUnitConfigs)
            {
                if (!unitsData.Any(x => x.UnitConfigId == unitConfig.Id))
                {
                    if (shiftData != null)
                    {
                        var tags = unitConfig.PreviousShiftTag.Split('@');

                        var endShiftDateTime = baseDate + shiftData.EndTime;
                        var beginShiftDateTime = endShiftDateTime - shiftData.ShiftDuration;

                        //// Todo: remove after tests
                        //logger.InfoFormat("ProcessAutomaticCalulatedUnits begin shift time: {0}", beginShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                        //logger.InfoFormat("ProcessAutomaticCalulatedUnits end shift time: {0}", endShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                        ////

                        var beginConfidence = 100;
                        var endConfidence = 100;

                        oPhd.StartTime = string.Format("{0}", endShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                        oPhd.EndTime = oPhd.StartTime;
                        var result = oPhd.FetchRowData(tags[0]);
                        var row = result.Tables[0].Rows[0];
                        var endValue = Convert.ToInt64(row["Value"]);
                        endConfidence = Convert.ToInt32(row["Confidence"]);

                        result = oPhd.FetchRowData(tags[1]);
                        row = result.Tables[0].Rows[0];
                        var pressure = Convert.ToDecimal(row["Value"]);

                        oPhd.StartTime = string.Format("{0}", beginShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                        oPhd.EndTime = oPhd.StartTime;
                        result = oPhd.FetchRowData(tags[0]);
                        row = result.Tables[0].Rows[0];
                        var beginValue = Convert.ToInt64(row["Value"]);
                        beginConfidence = Convert.ToInt32(row["Confidence"]);

                        currentUnitDatas.Add(
                            new UnitDatasTemp
                            {
                                UnitConfigId = unitConfig.Id,
                                Value = ((endValue - beginValue) * pressure) / Convert.ToDecimal(tags[2]),
                                ShiftId = shiftData.Id,
                                RecordTimestamp = targetRecordTimestamp,
                                Confidence = (beginConfidence + endConfidence) / 2
                            });
                    }
                }
            }

            return currentUnitDatas;
        }

        /// <summary>
        /// Processes the calculated by automatic units.
        /// </summary>
        /// <param name="unitsConfigsList">The units configs list.</param>
        /// <param name="oPhd">The o PHD.</param>
        /// <param name="targetRecordTimestamp">The record data time.</param>
        /// <param name="shift">The shift.</param>
        /// <param name="unitsData">The units data.</param>
        /// <param name="expectedNumberOfRecords">The expected number of records.</param>
        /// <returns></returns>
        private IEnumerable<UnitDatasTemp> ProcessCalculatedByAutomaticUnits(List<UnitConfig> unitsConfigsList,
                                                                         PHDHistorian oPhd,
                                                                         DateTime targetRecordTimestamp,
                                                                         Shift shift,
                                                                         List<UnitDatasTemp> unitsData,
                                                                         ref int expectedNumberOfRecords)
        {
            var currentUnitDatas = new List<UnitDatasTemp>();

            var observedUnitConfigs = unitsConfigsList.Where(x => x.CollectingDataMechanism == "CC");
            expectedNumberOfRecords = observedUnitConfigs.Count();

            SetPhdConnectionSettings(oPhd, oPhd.DefaultServer, targetRecordTimestamp, shift);

            foreach (var unitConfig in observedUnitConfigs)
            {
                try
                {
                    var confidence = 0;
                    var inputParams = new Dictionary<string, double>();
                    var formula = unitConfig.CalculatedFormula;
                    var arguments = unitConfig.PreviousShiftTag.Split(new char[] { '@' });
                    var argumentIndex = 0;
                    foreach (var item in arguments)
                    {
                        var row = oPhd.FetchRowData(item).Tables[0].Rows[0];
                        var val = Convert.ToDouble(row["Value"]);
                        confidence += Convert.ToInt32(row["Confidence"]);
                        inputParams.Add(string.Format("p{0}", argumentIndex), val);
                        argumentIndex++;
                    }

                    if (unitConfig.CalculationPercentage.HasValue)
                    {
                        inputParams.Add(string.Format("p{0}", argumentIndex), (double)unitConfig.CalculationPercentage.Value);
                    }

                    var result = new ProductionDataCalculatorService(this.data).Calculate(formula, "p", inputParams);
                    if (!unitsData.Any(x => x.RecordTimestamp == targetRecordTimestamp && x.ShiftId == shift.Id && x.UnitConfigId == unitConfig.Id))
                    {
                        currentUnitDatas.Add(
                            new UnitDatasTemp
                            {
                                UnitConfigId = unitConfig.Id,
                                RecordTimestamp = targetRecordTimestamp,
                                ShiftId = shift.Id,
                                Value = (double.IsNaN(result) || double.IsInfinity(result)) ? 0.0m : (decimal)result,
                                Confidence = confidence / arguments.Count()
                            });
                    }
                }
                catch (Exception ex)
                {
                    string message = string.Format("UnitConfigId: {0} [{1}]", unitConfig.Id, ex.Message);
                    logger.Error(message);
                    throw new Exception(message, ex);
                }
            }

            return currentUnitDatas;
        }

        private static void SetPhdConnectionSettings(PHDHistorian oPhd, PHDServer defaultServer, DateTime targetRecordTimestamp, Shift targetShift)
        {
            var exeFileName = Assembly.GetEntryAssembly().Location;
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(exeFileName);
            ConfigurationSectionGroup appSettingsGroup = configuration.GetSectionGroup("applicationSettings");
            ConfigurationSection appSettingsSection = appSettingsGroup.Sections[0];
            ClientSettingsSection settings = appSettingsSection as ClientSettingsSection;
            defaultServer.Port = Convert.ToInt32(settings.Settings.Get("PHD_PORT").Value.ValueXml.InnerText);
            defaultServer.APIVersion = SERVERVERSION.RAPI200;
            oPhd.DefaultServer = defaultServer;
            var beginShiftDateTime = targetRecordTimestamp.Date + targetShift.EndTime;
            oPhd.StartTime = string.Format("{0}", beginShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
            oPhd.EndTime = oPhd.StartTime;
            oPhd.Sampletype = SAMPLETYPE.Snapshot;
            oPhd.MinimumConfidence = Convert.ToInt32(settings.Settings.Get("PHD_DATA_MIN_CONFIDENCE").Value.ValueXml.InnerText);
            oPhd.MaximumRows = Convert.ToUInt32(settings.Settings.Get("PHD_DATA_MAX_ROWS").Value.ValueXml.InnerText);
            //oPhd.Offset = Convert.ToInt32(settings.Settings.Get("PHD_OFFSET").Value.ValueXml.InnerText);
        }

        /// <summary>
        /// Sets the default units data value.
        /// </summary>
        /// <param name="targetRecordTimestamp">The record data time.</param>
        /// <param name="shiftId">The shiftId.</param>
        /// <param name="unitsData">The units data.</param>
        /// <param name="unitConfig">The unit config.</param>
        /// <param name="confidence">The confidence.</param>
        /// <returns></returns>
        private UnitDatasTemp SetDefaultUnitsDataValue(DateTime targetRecordTimestamp, int shiftId, UnitConfig unitConfig, int confidence)
        {
            return new UnitDatasTemp
                    {
                        UnitConfigId = unitConfig.Id,
                        Value = null,
                        RecordTimestamp = targetRecordTimestamp.Date,
                        ShiftId = shiftId,
                        Confidence = confidence
                    };
        }

        private static UnitDatasTemp GetUnitDataFromPhd(UnitConfig unitConfig, PHDHistorian oPhd, DateTime targetRecordTimestamp, out int confidence)
        {
            var unitData = new UnitDatasTemp();
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
                        unitData.RecordTimestamp = targetRecordTimestamp;
                    }
                }
            }

            return unitData;
        }


        public bool ProcessPrimaryProductionData(PrimaryDataSourceType dataSource,
                                                  DateTime targetRecordTimestamp,
                                                  Shift shift,
                                                  bool isForcedResultCalculation,
                                                  TreeState isFirstPhdInteraceCompleted)
        {
            bool lastOperationSucceeded = false;
            try
            {
                logger.Info("Sync primary data started!");

                var insertedRecords = ReadAndSaveUnitsDataForShift(targetRecordTimestamp, shift, dataSource, isForcedResultCalculation, ref lastOperationSucceeded, isFirstPhdInteraceCompleted);
                logger.InfoFormat("Successfully added {0} UnitsData records to CollectingPrimaryDataSystem", insertedRecords);
                if (insertedRecords > 0)
                {
                    this.mailer.SendMail(string.Format("Successfully added {0} records to database", insertedRecords), "SAPO - Shift data");
                }
                logger.Info("Sync primary data finished!");
            }
            catch (DataException validationException)
            {
                this.LogValidationDataException(validationException);
                this.mailer.SendMail(validationException.Message, "SAPO - ERROR");
            }
            catch (Exception ex)
            {
                this.logger.Error(ex.Message + ex.StackTrace);
                this.mailer.SendMail(ex.Message, "SAPO - ERROR");
            }

            return lastOperationSucceeded;
        }

        /// <summary>
        /// Gets the shifts.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Shift> GetShifts()
        {
            return this.data.Shifts.All().ToList();
        }

        private void LogValidationDataException(DataException validationException)
        {
            var dbEntityException = validationException.InnerException as DbEntityValidationException;
            if (dbEntityException != null)
            {
                foreach (var validationErrors in dbEntityException.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        this.logger.ErrorFormat("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            else
            {
                logger.Error(validationException.ToString());
            }
        }

        public void ProcessInventoryTanksData()
        {
            var now = DateTime.Now;
            if (2 <= now.Minute && now.Minute <= 10)
            {
                var exeFileName = Assembly.GetEntryAssembly().Location;
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(exeFileName);
                ConfigurationSectionGroup appSettingsGroup = configuration.GetSectionGroup("applicationSettings");
                ConfigurationSection appSettingsSection = appSettingsGroup.Sections[0];
                ClientSettingsSection settings = appSettingsSection as ClientSettingsSection;

                var inventoryMinHoursInterval = Convert.ToInt32(settings.Settings.Get("MIN_GET_INVENTORY_HOURS_INTERVAL").Value.ValueXml.InnerText);
                var inventoryHoursInterval = Convert.ToInt32(settings.Settings.Get("MAX_GET_INVENTORY_HOURS_INTERVAL").Value.ValueXml.InnerText);
                for (int hours = inventoryMinHoursInterval; hours < inventoryHoursInterval; hours++)
                {
                    var recordTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
                    var checkedDateTime = recordTime.AddHours(-hours);
                    var targetRecordDateTime = new DateTime(checkedDateTime.Year,
                                                            checkedDateTime.Month,
                                                            checkedDateTime.Day,
                                                            checkedDateTime.Hour,
                                                            0, 
                                                            0);
                    if (this.data.TanksData.All().Where(t => t.RecordTimestamp == checkedDateTime).Any())
                    {
                        logger.InfoFormat("There are already a records for that time of the day: {0:yyyy-MM-dd HH:ss:mm}!", checkedDateTime);
                        continue;
                    }

                    try
                    {
                        logger.InfoFormat("-------------------------------------------------------  Begin processing inventory tanks data for {0:yyyy-MM-dd HH:ss:mm}!", checkedDateTime);
                        var tanks = this.data.Tanks.All().Select(t => new Tank()
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

                        if (tanks.Count() > 0)
                        {
                            using (PHDHistorian oPhd = new PHDHistorian())
                            {
                                var phdServer = settings.Settings.Get("PHD_HOST").Value.ValueXml.InnerText;
                                using (PHDServer defaultServer = new PHDServer(phdServer))
                                {
                                    var getRecordTimestamp = string.Format("{0}", targetRecordDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
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
                                        try
                                        {
                                            var tankData = new TankData();
                                            tankData.RecordTimestamp = targetRecordDateTime;
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

                                            var confedence = 100;
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
                                                        confedence = Convert.ToInt32(row[dc]);
                                                        break;
                                                    }
                                                    else if (dc.ColumnName.Equals("TagName"))
                                                    {
                                                        tagName = row[dc].ToString();
                                                    }
                                                    else if (dc.ColumnName.Equals("Value"))
                                                    {
                                                        if (!row.IsNull("Value"))
                                                        {
                                                            tagValue = Convert.ToDecimal(row[dc]);
                                                        }
                                                    }
                                                }

                                                if (confedence != 100)
                                                {
                                                    continue;
                                                }

                                                if (tagName.Contains(".PROD_ID"))
                                                {
                                                    var prId = Convert.ToInt32(tagValue);
                                                    var product = this.data.TankMasterProducts.All().Where(x => x.TankMasterProductCode == prId).FirstOrDefault();
                                                    if (product != null)
                                                    {
                                                        tankData.ProductId = product.Id;
                                                        var pr = this.data.Products.All().Where(p => p.Id == product.Id).FirstOrDefault();
                                                        if (pr != null)
                                                        {
                                                            tankData.ProductName = pr.Name;
                                                        }
                                                        else
                                                        {
                                                            tankData.ProductName = "N/A";
                                                            this.mailer.SendMail("There is a product with unknown id in TankMaster configuration", "Phd2Sql Inventory");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        tankData.ProductId = Convert.ToInt32(tagValue);
                                                        tankData.ProductName = "N/A";
                                                        this.mailer.SendMail("There is a product with unknown id in TankMaster configuration", "Phd2Sql Inventory");
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
                                            tankData.Confidence = confedence;
                                            tanksDataList.Add(tankData);
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.ErrorFormat("Tank Id [{0}] Exception:\n\n\n{1}", t.TankId, ex.ToString());
                                        }
                                    }

                                    if (tanksDataList.Count > 0)
                                    {
                                        this.data.TanksData.BulkInsert(tanksDataList, "Phd2SqlLoading");
                                        this.data.SaveChanges("Phd2SqlLoading");
                                        logger.InfoFormat("Successfully added {0} TanksData records for: {1:yyyy-MM-dd HH:ss:mm}!", tanksDataList.Count, checkedDateTime);
                                    }
                                }
                            }
                        }

                        logger.InfoFormat("Finish processing inventory tanks data for {0:yyyy-MM-dd HH:ss:mm}!", checkedDateTime);

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
            }
        }

        internal static void ProcessMeasuringPointsData()
        {
            //try
            //{
            //    logger.Info("Sync measurements points data started!");
            //    //var now = DateTime.Now;
            //    //var today = DateTime.Today;
            //    //var fiveOClock = today.AddHours(5);

            //    //var ts = now - fiveOClock;
            //    //if (ts.TotalMinutes > 2 && ts.Hours == 0)
            //    {
            //        using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister(),new Logger())))
            //        {
            //            var measuringPoints = context.MeasuringPointConfigs.All().Where(x => !string.IsNullOrEmpty(x.TotalizerCurrentValueTag));

            //            if (measuringPoints.Count() > 0)
            //            {
            //                using(PHDHistorian oPhd = new PHDHistorian())
            //                { 
            //                    using(PHDServer defaultServer = new PHDServer("srv-vm-mes-phd"))
            //                    {
            //                        defaultServer.Port = 3150;
            //                        defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
            //                        oPhd.DefaultServer = defaultServer;
            //                        oPhd.StartTime = "NOW - 2M";
            //                        oPhd.EndTime = "NOW - 2M";
            //                        oPhd.Sampletype = SAMPLETYPE.Snapshot;
            //                        oPhd.MinimumConfidence = 49;
            //                        oPhd.ReductionType = REDUCTIONTYPE.Average;
            //                        oPhd.MaximumRows = 1;

            //                        foreach (var item in measuringPoints)
            //                        {
            //                            var measurementPointData = new MeasuringPointProductsData();
            //                            measurementPointData.MeasuringPointConfigId = item.Id;
            //                            DataSet dsGrid = oPhd.FetchRowData(item.TotalizerCurrentValueTag);
            //                            foreach (DataRow row in dsGrid.Tables[0].Rows)
            //                            {
            //                                foreach (DataColumn dc in dsGrid.Tables[0].Columns)
            //                                {
            //                                    if (dc.ColumnName.Equals("Tolerance") || dc.ColumnName.Equals("HostName"))
            //                                    {
            //                                        continue;
            //                                    }
            //                                    else if (dc.ColumnName.Equals("Value"))
            //                                    {
            //                                        if (!string.IsNullOrWhiteSpace(row[dc].ToString()))
            //                                        {
            //                                            measurementPointData.Value = Convert.ToDecimal(row[dc]);
            //                                            logger.InfoFormat("Value: {0}", measurementPointData.Value ?? 0);
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                }


            //                //using (PHDHistorian oPhd = new PHDHistorian())
            //                //{
            //                //    using (PHDServer oPhd = new PHDServer("srv-vm-mes-phd""))
            //                //    {
            //                //        oPhd.Port = 3150;
            //                //        defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
            //                //        oPhd.DefaultServer = defaultServer;
            //                //        oPhd.StartTime = "NOW - 2M";
            //                //        oPhd.EndTime = "NOW - 2M";
            //                //        oPhd.Sampletype = SAMPLETYPE.Snapshot;
            //                //        oPhd.MinimumConfidence = 49;
            //                //        oPhd.ReductionType = REDUCTIONTYPE.Average;
            //                //        oPhd.MaximumRows = 1;

            //                //        foreach (var item in measuringPoints)
            //                //        {
            //                //            var measurementPointData = new MeasuringPointProductsData();
            //                //            measurementPointData.MeasuringPointConfigId = item.Id;
            //                //            DataSet dsGrid = oPhd.FetchRowData(item.TotalizerCurrentValueTag);
            //                //            var confidence = 100;
            //                //            foreach (DataRow row in dsGrid.Tables[0].Rows)
            //                //            {
            //                //                foreach (DataColumn dc in dsGrid.Tables[0].Columns)
            //                //                {
            //                //                    if (dc.ColumnName.Equals("Tolerance") || dc.ColumnName.Equals("HostName"))
            //                //                    {
            //                //                        continue;
            //                //                    }
            //                //                    else if (dc.ColumnName.Equals("Confidence"))
            //                //                    {
            //                //                        if (!string.IsNullOrWhiteSpace(row[dc].ToString()))
            //                //                        {
            //                //                            confidence = Convert.ToInt32(row[dc]);
            //                //                        }
            //                //                        else
            //                //                        {
            //                //                            confidence = 0;
            //                //                            break;
            //                //                        }
            //                //                    }
            //                //                    else if (dc.ColumnName.Equals("Value"))
            //                //                    {
            //                //                        if (!string.IsNullOrWhiteSpace(row[dc].ToString()))
            //                //                        {
            //                //                            measurementPointData.Value = Convert.ToDecimal(row[dc]);
            //                //                        }
            //                //                    }
            //                //                }
            //                //            }
            //                //            if (confidence > Properties.Settings.Default.INSPECTION_DATA_MINIMUM_CONFIDENCE &&
            //                //                measurementPointData.RecordTimestamp != null)
            //                //            {
            //                //                measurementPointData.RecordTimestamp = currentDate;
            //                //                context.MeasurementPointsProductsDatas.Add(measurementPointData);
            //                //            }
            //                //        }

            //                //        //context.SaveChanges("Phd2Sql");
            //                //    }
            //                //}
            //            }
            //        }
            //    }

            //    logger.Info("Sync measurements points data finished!");
            //}
            //catch (DataException validationException)
            //{
            //    LogValidationDataException(validationException);
            //}
            //catch (Exception ex)
            //{
            //    logger.Error(ex);
            //}
        }

        private IProductionData CreateProductionData()
        {
            return new ProductionData(new CollectingDataSystemDbContext());
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