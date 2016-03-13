namespace CollectingProductionDataSystem.PhdApplication.PrimaryDataServices
{
    using System.Configuration;
    using System.Data;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Transactions;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Common;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using Uniformance.PHD;
    using CollectingProductionDataSystem.PhdApplication.Contracts;
    using CollectingProductionDataSystem.Application.ProductionDataServices;
    using System.Reflection;
    using log4net;
    using System.Data.Entity;
    using log4net.Core;

    public class PhdPrimaryDataService : IPrimaryDataService
    {
        private readonly IProductionData data;
        private readonly ILog logger;
        private TransactionOptions transantionOption;


        public PhdPrimaryDataService(IProductionData dataParam, ILog loggerParam)
        {
            this.data = dataParam;
            this.logger = loggerParam;
            this.transantionOption = DefaultTransactionOptions.Instance.TransactionOptions;
        }

        public int ReadAndSaveUnitsDataForShift(DateTime targetDateTime,
                                                 Shift targetShift,
                                                 PrimaryDataSourceType dataSource,
                                                 bool isForcedResultCalculation,
                                                 ref bool lastOperationSucceeded)
        {
            var timer = new Stopwatch();
            var saveChangesTimer = new Stopwatch();
            timer.Start();
            logger.Info("-------------------------------------------------------- Begin Interface Iteration -------------------------------------------------------- ");

            var totalInsertedRecords = 0;

            var exeFileName = Assembly.GetEntryAssembly().Location;
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(exeFileName);
            ConfigurationSectionGroup appSettingsGroup = configuration.GetSectionGroup("applicationSettings");
            ConfigurationSection appSettingsSection = appSettingsGroup.Sections[0];
            ClientSettingsSection settings = appSettingsSection as ClientSettingsSection;
            int expectedNumberOfRecords = 0;
            int realNumberOfRecords = 0;
            var unitDatasToAdd = new List<UnitsData>();
            var unitsConfigsList = this.data.UnitConfigs.All().Where(x => x.DataSource == dataSource).ToList();
            var targetDate = targetDateTime.Date;

            using (PHDHistorian oPhd = new PHDHistorian())
            {
                using (PHDServer defaultServer = new PHDServer(settings.Settings.Get("PHD_HOST").Value.ValueXml.InnerText))
                {
                    SetPhdConnectionSettings(oPhd, defaultServer, targetDateTime, targetShift);
                    var unitsData = this.data.UnitsData.All().Where(x => x.RecordTimestamp == targetDate && x.ShiftId == targetShift.Id).ToList();

                    var newRecords = ProcessAutomaticUnits(unitsConfigsList, unitsData, oPhd, targetDate, targetShift.Id, ref expectedNumberOfRecords);
                    realNumberOfRecords = newRecords.Count();
                    unitDatasToAdd.AddRange(newRecords);
                    LogConsistencyMessage("Processing Automatic Units Records", expectedNumberOfRecords, realNumberOfRecords);

                    newRecords = ProcessAutomaticDeltaUnits(unitsConfigsList, unitsData, oPhd, targetDate, targetShift, ref expectedNumberOfRecords);
                    realNumberOfRecords = newRecords.Count();
                    unitDatasToAdd.AddRange(newRecords);
                    LogConsistencyMessage("Processing Automatic Delta Records", expectedNumberOfRecords, realNumberOfRecords);

                    newRecords = ProcessAutomaticCalulatedUnits(unitsConfigsList, unitsData, oPhd, targetDate, targetShift, ref expectedNumberOfRecords);
                    realNumberOfRecords = newRecords.Count();
                    unitDatasToAdd.AddRange(newRecords);
                    LogConsistencyMessage("Processing Automatic Calculated Units Records", expectedNumberOfRecords, realNumberOfRecords);

                    newRecords = ProcessManualUnits(unitsConfigsList, targetDate, targetShift.Id, unitsData, ref expectedNumberOfRecords);
                    realNumberOfRecords = newRecords.Count();
                    unitDatasToAdd.AddRange(newRecords);
                    LogConsistencyMessage("Processing Manual Units Records", expectedNumberOfRecords, realNumberOfRecords);

                    newRecords = ProcessCalculatedByAutomaticUnits(unitsConfigsList, oPhd, targetDate, targetShift, unitsData, ref expectedNumberOfRecords);
                    realNumberOfRecords = newRecords.Count();
                    unitDatasToAdd.AddRange(newRecords);
                    LogConsistencyMessage("Processing Calculated By Automatic Records", expectedNumberOfRecords, realNumberOfRecords);
                }
            }

            saveChangesTimer.Start();
            // persisting received data and incorporate the data with records get from second PHD
            using (var transaction = new TransactionScope(TransactionScopeOption.Required, this.transantionOption))
            {
                if (unitDatasToAdd.Count > 0)
                {
                    this.data.UnitsData.BulkInsert(unitDatasToAdd, "Phd2SqlLoader");
                }

                totalInsertedRecords += unitDatasToAdd.Count;

                if (Properties.Settings.Default.IsTwoPHDS)
                {
                    var unitsData = this.data.UnitsData.All().Where(x => x.RecordTimestamp == targetDate && x.ShiftId == targetShift.Id).ToList();

                    var calculatedUnitDatas = ProcessCalculatedUnits(unitsConfigsList, targetDate, targetShift.Id, unitsData, ref expectedNumberOfRecords);

                    realNumberOfRecords = calculatedUnitDatas.Count();
                    LogConsistencyMessage("Processing Calculated Records", expectedNumberOfRecords, realNumberOfRecords);

                    if (calculatedUnitDatas.Count() > 0)
                    {
                        this.data.UnitsData.BulkInsert(calculatedUnitDatas, "Phd2SqlLoader");
                        totalInsertedRecords += calculatedUnitDatas.Count();
                    }

                    List<UnitsData> resultUnitData;
                    lastOperationSucceeded = CheckIfLastOperationSucceded(unitsConfigsList, targetDate, targetShift.Id, out resultUnitData);
                    if (isForcedResultCalculation && !lastOperationSucceeded)
                    {
                       var additionalRecords =  CreateMissingRecords(unitsConfigsList, resultUnitData, targetShift);
                       if (additionalRecords.Count() > 0)
                       {
                           this.data.UnitsData.BulkInsert(additionalRecords, "Phd2SqlLoader");
                           totalInsertedRecords += additionalRecords.Count();
                       }

                       lastOperationSucceeded = true;
                    }
                }

                transaction.Complete();
            }

            saveChangesTimer.Stop();
            timer.Stop();
            logger.InfoFormat("\tEstimated time for data fetching: {0} s", timer.Elapsed - saveChangesTimer.Elapsed);
            logger.InfoFormat("\tEstimated time for Iteration result saving: {0} s", saveChangesTimer.Elapsed);
            logger.InfoFormat("\tTotal Estimated time for Iteration: {0} s", timer.Elapsed);
            logger.InfoFormat("\tTotal number of persisted records: {0}", totalInsertedRecords);
            logger.Info("-------------------------------------------------------  End Interface Iteration -------------------------------------------------------- ");


            return totalInsertedRecords;
        }

        private IEnumerable<UnitsData> CreateMissingRecords(List<UnitConfig> unitsConfigsList, IEnumerable<UnitsData> resultUnitData, Shift shift)
        {
            var UnitDatas = resultUnitData.ToDictionary(x => x.UnitConfig.Code);
            var targetDate = (DateTime.Now.Date+shift.BeginTime).Date;
            var confidense = 100;
            var result = new List<UnitsData>();


            foreach (var position in unitsConfigsList)
            {
                if (!UnitDatas.ContainsKey(position.Code))
                {
                    result.Add(this.SetDefaultUnitsDataValue(targetDate, shift.Id, position, confidense));
                }
            }

            return result;
        }

        /// <summary>
        /// Checks if last operation succeded.
        /// </summary>
        /// <param name="unitsConfigsList">The units configs list.</param>
        /// <param name="targetDate">The target date.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        private bool CheckIfLastOperationSucceded(List<UnitConfig> unitsConfigsList, DateTime targetDate, int targetShiftId, out List<UnitsData> resultUnitData)
        {
            resultUnitData = this.data.UnitsData.All().Include(x => x.UnitConfig)
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
            var resultShift = data.Shifts.All().ToList().FirstOrDefault(x =>
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
        /// Logs the consistency message.
        /// </summary>
        /// <param name="stepName">Name of the step.</param>
        /// <param name="expectedNumberOfRecords">The expected number of records.</param>
        /// <param name="realNumberOfRecords">The real number of records.</param>
        private void LogConsistencyMessage(object stepName, int expectedRecordsCount, int generatedRecordsCount)
        {
            logger.InfoFormat("\tOn step {0}: \n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tExpected number of records: {1} \n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tThe number of the generated records:{2}", stepName, expectedRecordsCount, generatedRecordsCount);
        }


        private IEnumerable<UnitsData> ProcessCalculatedUnits(List<UnitConfig> unitsConfigsList, DateTime recordDataTime, int shift, List<UnitsData> unitsData, ref int expectedNumberOfRecords)
        {
            var currentUnitDatas = new Dictionary<int, UnitsData>();

            var observedUnitConfigs = unitsConfigsList.Where(x => x.CollectingDataMechanism == "C");

            expectedNumberOfRecords = observedUnitConfigs.Count();

            foreach (var unitConfig in observedUnitConfigs)
            {
                try
                {
                    if (unitConfig.CalculatedFormula.Equals("C9"))
                    {
                        CalculateByMathExpression(unitConfig, recordDataTime, shift, unitsData, currentUnitDatas);
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

                        var ruc = unitConfig.RelatedUnitConfigs.ToList();
                        var confidence = 100;
                        foreach (var ru in ruc)
                        {
                            var parameterType = ru.RelatedUnitConfig.AggregateGroup;
                            var element = data.UnitsData.All()
                                .Where(x => x.RecordTimestamp == recordDataTime)
                                .Where(x => x.ShiftId == shift)
                                .Where(x => x.UnitConfigId == ru.RelatedUnitConfigId)
                                .FirstOrDefault();
                            var inputValue = (element != null) ? element.RealValue : 0.0;
                            if (inputValue == 0.0)
                            {
                                if (currentUnitDatas.ContainsKey(ru.RelatedUnitConfigId))
                                {
                                    inputValue = currentUnitDatas[ru.RelatedUnitConfigId].RealValue;
                                }
                            }

                            try
                            {
                                if (element != null && element.Confidence != 100)
                                {
                                    confidence = element.Confidence;
                                }
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("CONFIDENCE ERROR: " + ex.Message, ex);
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

                        var result = new ProductionDataCalculatorService(this.data).Calculate(formulaCode, arguments);
                        if (!unitsData.Where(x => x.RecordTimestamp == recordDataTime && x.ShiftId == shift && x.UnitConfigId == unitConfig.Id).Any())
                        {
                            currentUnitDatas.Add(unitConfig.Id,
                                                    new UnitsData
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
                catch (Exception ex)
                {
                    throw new Exception(string.Format("UnitConfigId: {0} \n [{1} \n {2}]", unitConfig.Id, ex.Message, ex.ToString()), ex);
                }
            }

            return currentUnitDatas.Values.ToList();
        }

        private void CalculateByMathExpression(UnitConfig unitConfig, DateTime recordDataTime, int shift, List<UnitsData> unitsData, Dictionary<int, UnitsData> calculatedUnitsData)
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
                    new UnitsData
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
        /// <param name="recordDataTime">The record data time.</param>
        /// <param name="shift">The shift.</param>
        /// <param name="unitsData">The units data.</param>
        /// <returns></returns>
        private IEnumerable<UnitsData> ProcessManualUnits(List<UnitConfig> unitsConfigsList, DateTime recordDataTime, int shift, List<UnitsData> unitsData, ref int expectedNumberOfRecords)
        {
            var currentUnitDatas = new List<UnitsData>();

            var observedUnitConfigs = unitsConfigsList.Where(x => x.CollectingDataMechanism == "M"
                                                                || x.CollectingDataMechanism == "MC"
                                                                || x.CollectingDataMechanism == "MD"
                                                                || x.CollectingDataMechanism == "MS");

            expectedNumberOfRecords = observedUnitConfigs.Count();

            foreach (var unitConfig in observedUnitConfigs)
            {
                var isRecordExists = unitsData.Any(x => x.RecordTimestamp == recordDataTime
                                                    && x.ShiftId == shift
                                                    && x.UnitConfigId == unitConfig.Id);
                if (!isRecordExists)
                {
                    currentUnitDatas.Add(SetDefaultUnitsDataValue(recordDataTime, shift, unitConfig, 100));
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
        /// <param name="recordDataTime">The record data time.</param>
        /// <param name="shift">The shift.</param>
        /// <param name="expectedNumberOfRecords">The expected number of records.</param>
        /// <returns></returns>
        private IEnumerable<UnitsData> ProcessAutomaticUnits(List<UnitConfig> unitsConfigsList, List<UnitsData> unitsData,
                                                                PHDHistorian oPhd, DateTime recordDataTime,
                                                                int shift, ref int expectedNumberOfRecords)
        {
            var currentUnitDatas = new List<UnitsData>();

            //ToDo: Find correct request
            var shifts = this.data.Shifts.All().ToList();
            var lastShift = shifts.FirstOrDefault(x => x.EndTime == shifts.Max(y => y.EndTime));

            var observedUnitConfigs = unitsConfigsList.Where(x => x.CollectingDataMechanism == "A");
            expectedNumberOfRecords = observedUnitConfigs.Count();

            foreach (var unitConfig in observedUnitConfigs)
            {
                var isRecordExists = unitsData.Any(x => x.RecordTimestamp == recordDataTime
                                                     && x.ShiftId == shift
                                                     && x.UnitConfigId == unitConfig.Id);

                if (!isRecordExists)
                {
                    if (unitConfig.NeedToGetOnlyLastShiftValue == 1 && shift != lastShift.Id)
                    {
                        currentUnitDatas.Add(SetDefaultUnitsDataValue(recordDataTime, shift, unitConfig, 100));
                    }
                    else
                    {
                        var confidence = 0;
                        var unitData = GetUnitData(unitConfig, oPhd, recordDataTime, out confidence);
                        if (confidence >= Properties.Settings.Default.PHD_DATA_MIN_CONFIDENCE && unitData.RecordTimestamp != null)
                        {
                            unitData.ShiftId = shift;
                            unitData.RecordTimestamp = unitData.RecordTimestamp.Date;
                            unitData.Confidence = confidence;
                            currentUnitDatas.Add(unitData);
                        }
                        else
                        {
                            currentUnitDatas.Add(SetDefaultUnitsDataValue(recordDataTime, shift, unitConfig, confidence));
                        }
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
        /// <param name="recordDataTime">The record data time.</param>
        /// <param name="shift">The shift.</param>
        /// <param name="expectedNumberOfRecords">The expected number of records.</param>
        /// <returns></returns>
        private IEnumerable<UnitsData> ProcessAutomaticDeltaUnits(List<UnitConfig> unitsConfigsList, List<UnitsData> unitsData,
                                                                    PHDHistorian oPhd, DateTime recordDataTime,
                                                                    Shift shiftData, ref int expectedNumberOfRecords)
        {
            var currentUnitDatas = new List<UnitsData>();
            var baseDate = recordDataTime.Date;

            var observedUnitConfigs = unitsConfigsList.Where(x => x.CollectingDataMechanism == "AA");
            expectedNumberOfRecords = observedUnitConfigs.Count();

            foreach (var unitConfig in observedUnitConfigs)
            {
                if (!unitsData.Any(x => x.UnitConfigId == unitConfig.Id))
                {

                    if (shiftData != null)
                    {
                        var beginShiftDateTime = baseDate + shiftData.BeginTime;
                        var endShiftDateTime = baseDate + shiftData.EndTime;

                        var beginConfidence = 100;
                        var endConfidence = 100;

                        oPhd.StartTime = string.Format("{0}", endShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                        oPhd.EndTime = oPhd.StartTime;

                        var result = oPhd.FetchRowData(unitConfig.PreviousShiftTag);
                        var row = result.Tables[0].Rows[0];
                        var endValue = Convert.ToInt64(row["Value"]);
                        endConfidence = Convert.ToInt32(row["Confidence"]);

                        string.Format("{0}", beginShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                        oPhd.EndTime = oPhd.StartTime;
                        result = oPhd.FetchRowData(unitConfig.PreviousShiftTag);
                        row = result.Tables[0].Rows[0];
                        var beginValue = Convert.ToInt64(row["Value"]);
                        beginConfidence = Convert.ToInt32(row["Confidence"]);

                        currentUnitDatas.Add(
                            new UnitsData
                            {
                                UnitConfigId = unitConfig.Id,
                                Value = (endValue - beginValue) / 1000m,
                                ShiftId = shiftData.Id,
                                RecordTimestamp = recordDataTime,
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
        /// <param name="recordDataTime">The record data time.</param>
        /// <param name="shift">The shift.</param>
        /// <param name="expectedNumberOfRecords">The expected number of records.</param>
        /// <returns></returns>
        private IEnumerable<UnitsData> ProcessAutomaticCalulatedUnits(List<UnitConfig> unitsConfigsList, List<UnitsData> unitsData, PHDHistorian oPhd, DateTime recordDataTime, Shift shiftData, ref int expectedNumberOfRecords)
        {
            var currentUnitDatas = new List<UnitsData>();
            var baseDate = recordDataTime.Date;

            var observedUnitConfigs = unitsConfigsList.Where(x => x.CollectingDataMechanism == "AC");
            expectedNumberOfRecords = observedUnitConfigs.Count();

            foreach (var unitConfig in observedUnitConfigs)
            {
                if (!unitsData.Any(x => x.UnitConfigId == unitConfig.Id))
                {
                    if (shiftData != null)
                    {
                        var tags = unitConfig.PreviousShiftTag.Split('@');

                        var beginShiftDateTime = baseDate + shiftData.BeginTime;
                        var endShiftDateTime = baseDate + shiftData.EndTime;

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
                            new UnitsData
                            {
                                UnitConfigId = unitConfig.Id,
                                Value = ((endValue - beginValue) * pressure) / Convert.ToDecimal(tags[2]),
                                ShiftId = shiftData.Id,
                                RecordTimestamp = recordDataTime,
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
        /// <param name="recordDataTime">The record data time.</param>
        /// <param name="shift">The shift.</param>
        /// <param name="unitsData">The units data.</param>
        /// <param name="expectedNumberOfRecords">The expected number of records.</param>
        /// <returns></returns>
        private IEnumerable<UnitsData> ProcessCalculatedByAutomaticUnits(List<UnitConfig> unitsConfigsList, PHDHistorian oPhd, DateTime recordDataTime, Shift shift, List<UnitsData> unitsData, ref int expectedNumberOfRecords)
        {
            var currentUnitDatas = new List<UnitsData>();

            var observedUnitConfigs = unitsConfigsList.Where(x => x.CollectingDataMechanism == "CC");
            expectedNumberOfRecords = observedUnitConfigs.Count();

            SetPhdConnectionSettings(oPhd, oPhd.DefaultServer, recordDataTime, shift);

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
                    if (!unitsData.Any(x => x.RecordTimestamp == recordDataTime && x.ShiftId == shift.Id && x.UnitConfigId == unitConfig.Id))
                    {
                        currentUnitDatas.Add(
                            new UnitsData
                            {
                                UnitConfigId = unitConfig.Id,
                                RecordTimestamp = recordDataTime,
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

        private static void SetPhdConnectionSettings(PHDHistorian oPhd, PHDServer defaultServer, DateTime targetDate, Shift targetShift)
        {
            var exeFileName = Assembly.GetEntryAssembly().Location;
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(exeFileName);
            ConfigurationSectionGroup appSettingsGroup = configuration.GetSectionGroup("applicationSettings");
            ConfigurationSection appSettingsSection = appSettingsGroup.Sections[0];
            ClientSettingsSection settings = appSettingsSection as ClientSettingsSection;
            defaultServer.Port = Convert.ToInt32(settings.Settings.Get("PHD_PORT").Value.ValueXml.InnerText);
            defaultServer.APIVersion = SERVERVERSION.RAPI200;
            oPhd.DefaultServer = defaultServer;
            var beginShiftDateTime = targetDate.Date + targetShift.ReadOffset;
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
        /// <param name="recordDataTime">The record data time.</param>
        /// <param name="shift">The shift.</param>
        /// <param name="unitsData">The units data.</param>
        /// <param name="unitConfig">The unit config.</param>
        /// <param name="confidence">The confidence.</param>
        /// <returns></returns>
        private UnitsData SetDefaultUnitsDataValue(DateTime recordDataTime, int shift, UnitConfig unitConfig, int confidence)
        {
            return new UnitsData
                    {
                        UnitConfigId = unitConfig.Id,
                        Value = null,
                        RecordTimestamp = recordDataTime,
                        ShiftId = shift,
                        Confidence = confidence
                    };
        }

        private DateTime GetRecordTimestamp(DateTime recordDateTime)
        {
            var result = new DateTime(recordDateTime.Year, recordDateTime.Month, recordDateTime.Day, 0, 0, 0);

            if (recordDateTime.Hour < 13)
            {
                result = result.AddDays(-1);
            }

            return result;
        }

        private ShiftType GetShift(DateTime recordDateTime)
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

        private static UnitsData GetUnitData(UnitConfig unitConfig, PHDHistorian oPhd, DateTime timeStamp, out int confidence)
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
                        unitData.RecordTimestamp = timeStamp;
                    }
                }
            }

            return unitData;
        }
    }
}