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
    using System.Web.Configuration;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Application.ProductionDataServices;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Common;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Enumerations;
    using CollectingProductionDataSystem.Infrastructure.Contracts;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using Uniformance.PHD;
    using CollectingProductionDataSystem.PhdApplication.Contracts;
    using System.Reflection;
    using System.Text;

    public class PhdPrimaryDataService : IPhdPrimaryDataService
    {
        private readonly log4net.ILog logger;
        private readonly IMailerService mailer;
        private readonly TransactionOptions transantionOption;
        private readonly IProductionData data;

        public PhdPrimaryDataService(IProductionData dataParam, log4net.ILog loggerParam, IMailerService mailerServiceParam)
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
            this.data.DbContext.DbContext.Database.ExecuteSqlCommand(sql: "TRUNCATE TABLE [UnitDataTemporaryRecords]");
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
            logger.Info($"-------------------------------------------------------- Begin Interface Iteration For {dataSource.ToString()} Shift {targetShift.Id} -------------------------------------------------------- ");

            string exeFileName = Assembly.GetEntryAssembly().Location;
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(exeFileName);
            ConfigurationSectionGroup appSettingsGroup = configuration.GetSectionGroup(sectionGroupName: "applicationSettings");
            ConfigurationSection appSettingsSection = appSettingsGroup.Sections[0];
            var settings = appSettingsSection as ClientSettingsSection;
            int expectedNumberOfRecords = 0;
            int realNumberOfRecords = 0;
            var unitDatasToAdd = new List<UnitDatasTemp>();
            List<UnitConfig> unitsConfigsList = this.data.UnitConfigs.All()
                .Include(x => x.RelatedUnitConfigs)
                .Include(x => x.RelatedUnitConfigs.Select(y => y.UnitConfig))
                .Include(x => x.RelatedUnitConfigs.Select(z => z.RelatedUnitConfig).Select(w => w.UnitDatasTemps))
                .Where(x => x.DataSource == dataSource).ToList();
            DateTime targetRecordTimestampDate = targetRecordTimestamp.Date;

            try
            {
                using (PHDHistorian oPhd = new PHDHistorian())
                {
                    using (PHDServer defaultServer = new PHDServer(settings.Settings.Get("PHD_HOST" + (int) dataSource).Value.ValueXml.InnerText))
                    {
                        SetPhdConnectionSettings(oPhd, defaultServer, targetRecordTimestamp, targetShift);
                        List<UnitDatasTemp> unitsData = this.data.UnitDatasTemps.All().Where(x => x.RecordTimestamp == targetRecordTimestampDate && x.ShiftId == targetShift.Id).ToList();

                        var newRecords = ProcessAutomaticUnits(unitsConfigsList, unitsData, oPhd, targetRecordTimestampDate, targetShift.Id, ref expectedNumberOfRecords);
                        realNumberOfRecords = newRecords.Count();
                        unitDatasToAdd.AddRange(newRecords);
                        LogConsistencyMessage(stepName: "Processing Automatic Units Records", expectedRecordsCount: expectedNumberOfRecords, generatedRecordsCount: realNumberOfRecords);

                        newRecords = ProcessAutomaticDeltaUnits(unitsConfigsList, unitsData, oPhd, targetRecordTimestampDate, targetShift, ref expectedNumberOfRecords);
                        realNumberOfRecords = newRecords.Count();
                        unitDatasToAdd.AddRange(newRecords);
                        LogConsistencyMessage(stepName: "Processing Automatic Delta Records", expectedRecordsCount: expectedNumberOfRecords, generatedRecordsCount: realNumberOfRecords);

                        newRecords = ProcessAutomaticDeltaTwoSourcesUnits(unitsConfigsList, unitsData, oPhd, targetRecordTimestampDate, targetShift, ref expectedNumberOfRecords);
                        realNumberOfRecords = newRecords.Count();
                        unitDatasToAdd.AddRange(newRecords);
                        LogConsistencyMessage(stepName: "Processing Automatic Delta Records From Two Sources", expectedRecordsCount: expectedNumberOfRecords, generatedRecordsCount: realNumberOfRecords);

                        newRecords = ProcessAutomaticCalulatedUnits(unitsConfigsList, unitsData, oPhd, targetRecordTimestampDate, targetShift, ref expectedNumberOfRecords);
                        realNumberOfRecords = newRecords.Count();
                        unitDatasToAdd.AddRange(newRecords);
                        LogConsistencyMessage(stepName: "Processing Automatic Calculated Units Records", expectedRecordsCount: expectedNumberOfRecords, generatedRecordsCount: realNumberOfRecords);

                        newRecords = ProcessManualUnits(unitsConfigsList, targetRecordTimestampDate, targetShift.Id, unitsData, ref expectedNumberOfRecords);
                        realNumberOfRecords = newRecords.Count();
                        unitDatasToAdd.AddRange(newRecords);
                        LogConsistencyMessage(stepName: "Processing Manual Units Records", expectedRecordsCount: expectedNumberOfRecords, generatedRecordsCount: realNumberOfRecords);

                        newRecords = ProcessCalculatedByAutomaticUnits(unitsConfigsList, oPhd, targetRecordTimestampDate, targetShift, unitsData, ref expectedNumberOfRecords);
                        realNumberOfRecords = newRecords.Count();
                        unitDatasToAdd.AddRange(newRecords);
                        LogConsistencyMessage(stepName: "Processing Calculated By Automatic Records", expectedRecordsCount: expectedNumberOfRecords, generatedRecordsCount: realNumberOfRecords);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + ex.StackTrace);
                mailer.SendMail(ex.Message + ex.StackTrace, title: "Phd2Interface Error");
            }

            int totalInsertedRecords = 0;
            saveChangesTimer.Start();
            // persisting received data and incorporate the data with records get from second PHD

            try
            {
                if (unitDatasToAdd.Count > 0)
                {
                    this.data.UnitDatasTemps.BulkInsert(unitDatasToAdd, CommonConstants.Phd2SqlDefaultUserName);
                    this.data.SaveChanges(CommonConstants.Phd2SqlDefaultUserName);

                    totalInsertedRecords += unitDatasToAdd.Count;
                }

                totalInsertedRecords += GetCalculatedUnits(targetShift, unitsConfigsList, targetRecordTimestampDate, calculateDailyInfoRecord: false);

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + ex.StackTrace, ex);

            }

            List<UnitDatasTemp> resultUnitData;
            lastOperationSucceeded = CheckIfLastOperationSucceded(unitsConfigsList, targetRecordTimestampDate, targetShift.Id, out resultUnitData);

            saveChangesTimer.Stop();
            timer.Stop();
            logger.InfoFormat(format: "\tEstimated time for data fetching: {0} s", arg0: timer.Elapsed - saveChangesTimer.Elapsed);
            logger.InfoFormat(format: "\tEstimated time for Iteration result saving: {0} s", arg0: saveChangesTimer.Elapsed);
            logger.InfoFormat(format: "\tTotal Estimated time for Iteration: {0} s", arg0: timer.Elapsed);
            logger.InfoFormat(format: "\tTotal number of persisted records: {0}", arg0: totalInsertedRecords);
            logger.InfoFormat(format: "-------------------------------------------------------  End Interface Iteration For {0} Shift {1} -------------------------------------------------------- ", arg0: dataSource.ToString(), arg1: targetShift.Id);

            return totalInsertedRecords;
        }

        /// <summary>
        /// Gets the calculated units.
        /// </summary>
        /// <param name="targetShift">The target shift.</param>
        /// <param name="unitsConfigsList">The units configs list.</param>
        /// <param name="targetRecordTimestampDate">The target record timestamp date.</param>
        /// <returns></returns>
        private int GetCalculatedUnits(Shift targetShift, List<UnitConfig> unitsConfigsList, DateTime targetRecordTimestampDate, bool calculateDailyInfoRecord)
        {
            int totalInsertedRecords = 0;
            int expectedNumberOfRecords = 0;
            int realNumberOfRecords = 0;
            List<UnitDatasTemp> unitsTempData = this.data.UnitDatasTemps.All()
                .Where(x => x.RecordTimestamp == targetRecordTimestampDate
                && x.ShiftId == targetShift.Id
                ).ToList();
            IEnumerable<UnitDatasTemp> calculatedUnitDatas = ProcessCalculatedUnits(unitsConfigsList,
                targetRecordTimestampDate,
                targetShift.Id,
                unitsTempData,
                ref expectedNumberOfRecords,
                calculateDailyInfoRecord);

            realNumberOfRecords = calculatedUnitDatas.Count();
            LogConsistencyMessage(stepName: "Processing Calculated Records", expectedRecordsCount: expectedNumberOfRecords, generatedRecordsCount: realNumberOfRecords);

            if (calculatedUnitDatas.Count() > 0)
            {
                this.data.UnitDatasTemps.BulkInsert(calculatedUnitDatas, CommonConstants.Phd2SqlDefaultUserName);
                this.data.SaveChanges(CommonConstants.Phd2SqlDefaultUserName);

                totalInsertedRecords += calculatedUnitDatas.Count();
            }

            return totalInsertedRecords;
        }


        private IEnumerable<UnitDatasTemp> ReadUnitsDataForShift(DateTime targetRecordTimestamp,
            Shift targetShift,
            string hostName,
            PrimaryDataSourceType dataSource,
            IEnumerable<UnitDatasTemp> unitDatasToAdd = null)
        {
            var timer = new Stopwatch();
            var saveChangesTimer = new Stopwatch();
            timer.Start();
            logger.Info($"-------------------------------------------------------- Begin Interface Iteration For {dataSource.ToString()} Shift {targetShift.Id} -------------------------------------------------------- ");

            int expectedNumberOfRecords = 0;
            int realNumberOfRecords = 0;
            unitDatasToAdd = unitDatasToAdd ?? new List<UnitDatasTemp>();
            List<UnitDatasTemp> unitDatasToAddList = unitDatasToAdd.ToList();

            List<UnitConfig> unitsConfigsList = this.data.UnitConfigs.All()
                .Include(x => x.RelatedUnitConfigs)
                .Include(x => x.RelatedUnitConfigs.Select(y => y.UnitConfig))
                .Include(x => x.RelatedUnitConfigs.Select(z => z.RelatedUnitConfig).Select(w => w.UnitDatasTemps))
                .Where(x => x.DataSource == dataSource).ToList();
            DateTime targetRecordTimestampDate = targetRecordTimestamp.Date;

            try
            {
                using (PHDHistorian oPhd = new PHDHistorian())
                {
                    using (PHDServer defaultServer = new PHDServer(hostName))
                    {
                        SetPhdConnectionSettings(oPhd, defaultServer, targetRecordTimestamp, targetShift);
                        List<UnitDatasTemp> unitsData = this.data.UnitDatasTemps.All().Where(x => x.RecordTimestamp == targetRecordTimestampDate && x.ShiftId == targetShift.Id).ToList();

                        IEnumerable<UnitDatasTemp> newRecords = ProcessAutomaticUnits(unitsConfigsList, unitsData, oPhd, targetRecordTimestampDate, targetShift.Id, ref expectedNumberOfRecords);
                        realNumberOfRecords = newRecords.Count();
                        unitDatasToAddList.AddRange(newRecords);
                        LogConsistencyMessage(new ProgressMessage(message: "Processing Automatic Units Records", progressValue: realNumberOfRecords), expectedNumberOfRecords, realNumberOfRecords);

                        newRecords = ProcessAutomaticDeltaUnits(unitsConfigsList, unitsData, oPhd, targetRecordTimestampDate, targetShift, ref expectedNumberOfRecords);
                        realNumberOfRecords = newRecords.Count();
                        unitDatasToAddList.AddRange(newRecords);
                        LogConsistencyMessage(new ProgressMessage(message: "Processing Automatic Delta Records", progressValue: realNumberOfRecords), expectedNumberOfRecords, realNumberOfRecords);

                        newRecords = ProcessAutomaticCalulatedUnits(unitsConfigsList, unitsData, oPhd, targetRecordTimestampDate, targetShift, ref expectedNumberOfRecords);
                        realNumberOfRecords = newRecords.Count();
                        unitDatasToAddList.AddRange(newRecords);
                        LogConsistencyMessage(new ProgressMessage(message: "Processing Automatic Calculated Units Records", progressValue: realNumberOfRecords), expectedNumberOfRecords, realNumberOfRecords);

                        newRecords = ProcessManualUnits(unitsConfigsList, targetRecordTimestampDate, targetShift.Id, unitsData, ref expectedNumberOfRecords);
                        realNumberOfRecords = newRecords.Count();
                        unitDatasToAddList.AddRange(newRecords);
                        LogConsistencyMessage(new ProgressMessage(message: "Processing Manual Units Records", progressValue: realNumberOfRecords), expectedNumberOfRecords, realNumberOfRecords);

                        newRecords = ProcessCalculatedByAutomaticUnits(unitsConfigsList, oPhd, targetRecordTimestampDate, targetShift, unitsData, ref expectedNumberOfRecords);
                        realNumberOfRecords = newRecords.Count();
                        unitDatasToAddList.AddRange(newRecords);
                        LogConsistencyMessage(new ProgressMessage(message: "Processing Calculated By Automatic Records", progressValue: realNumberOfRecords), expectedNumberOfRecords, realNumberOfRecords);

                        newRecords = ProcessCalculatedUnits(unitsConfigsList, targetRecordTimestampDate, targetShift.Id, unitDatasToAddList, ref realNumberOfRecords, false);
                        realNumberOfRecords = newRecords.Count();
                        unitDatasToAddList.AddRange(newRecords);
                        LogConsistencyMessage(new ProgressMessage(message: "Processing Calculated By Automatic Records", progressValue: realNumberOfRecords), expectedNumberOfRecords, realNumberOfRecords);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message + ex.StackTrace);
                mailer.SendMail(ex.Message + ex.StackTrace, title: "Phd2Interface Error");
            }

            return unitDatasToAddList;
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
            DateTime baseDate = targetDateTime.Date;
            Shift resultShift = this.data.Shifts.All().ToList().FirstOrDefault(x =>
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
            var message = new StringBuilder();

            IEnumerable<UnitDatasTemp> additionalRecords = CreateMissingRecords(targetRecordTimestamp, targetShift, ref expectedNumberOfRecords, generateDailyInfoRecords: false);
            if (additionalRecords.Count() > 0)
            {
                this.data.UnitDatasTemps.BulkInsert(additionalRecords, CommonConstants.Phd2SqlDefaultUserName);
                LogConsistencyMessage(stepName: "Added Missing Records", expectedRecordsCount: additionalRecords.Count(), generatedRecordsCount: additionalRecords.Count());
                message.AppendLine($"Successfully Added Missing {additionalRecords.Count()} records to database.<br/>");
            }

            bool calculateDailyInfoRecords = true;
            List<UnitConfig> unitsConfigsList = this.data.UnitConfigs.All().ToList();
            int dailyInfoRecords = GetCalculatedUnits(targetShift, unitsConfigsList, targetRecordTimestamp, calculateDailyInfoRecords);
            if (dailyInfoRecords > 0)
            {
                message.AppendLine($"Successfully Added DailyInfo {dailyInfoRecords} records to database.<br/>");
            }

            IEnumerable<UnitDatasTemp> extraAdditionalRecords = CreateMissingRecords(targetRecordTimestamp, targetShift, ref expectedNumberOfRecords, true);
            if (extraAdditionalRecords.Count() > 0)
            {
                this.data.UnitDatasTemps.BulkInsert(extraAdditionalRecords, CommonConstants.Phd2SqlDefaultUserName);
                LogConsistencyMessage(stepName: "Added Missing Records", expectedRecordsCount: extraAdditionalRecords.Count(), generatedRecordsCount: extraAdditionalRecords.Count());
                message.AppendLine($"Successfully Added Missing {extraAdditionalRecords.Count()} records to database.<br/>");
            }

            int addedRecords = this.FlashDataToOriginalUnitData();
            LogConsistencyMessage(stepName: "Finally Flashed Records To Units Data", expectedRecordsCount: expectedNumberOfRecords, generatedRecordsCount: addedRecords);
            message.AppendLine($"Successfully Finally Flashed {addedRecords} records to UnitsData.<br/>");
            this.mailer.SendMail(message.ToString(), title: "SAPO - Shift data");
        }

        /// <summary>
        /// Creates the missing records.
        /// </summary>
        /// <param name="targetRecordTimestamp">The target record time stamp.</param>
        /// <param name="shift">The shift.</param>
        /// <param name="expectedNumberOfRecords">The expected number of records.</param>
        /// <returns></returns>
        private IEnumerable<UnitDatasTemp> CreateMissingRecords(
            DateTime targetRecordTimestamp,
            Shift shift,
            ref int expectedNumberOfRecords,
            bool generateDailyInfoRecords)
        {
            List<UnitConfig> unitsConfigsList = this.data.UnitConfigs.All().ToList();
            Dictionary<int, UnitDatasTemp> unitDatas = this.data.UnitDatasTemps.All().ToDictionary(x => x.UnitConfigId);
            DateTime targetDate = targetRecordTimestamp.Date;
            int confidense = 0;
            var result = new List<UnitDatasTemp>();

            foreach (var position in unitsConfigsList)
            {
                if (!unitDatas.ContainsKey(position.Id))
                {
                    if (generateDailyInfoRecords == false && position.ShiftProductTypeId == CommonConstants.DailyInfoDailyInfoHydrocarbonsShiftTypeId)
                    {
                        logger.Info($"SKIP DAILY MISSING POSITION {position.Code} {position.Name}");
                        continue;
                    }

                    result.Add(this.SetDefaultUnitsDataValue(targetDate, shift.Id, position, confidense));
                    logger.Info($"CREATE MISSING POSITION {position.Code} {position.Name}");
                }
            }

            expectedNumberOfRecords = unitsConfigsList.Count;
            return result;
        }

        /// <summary>
        /// Flashes the data to original unit data.
        /// </summary>
        /// <param name="expectedNumberOfRecords">The expected number of records.</param>
        /// <returns></returns>
        private int FlashDataToOriginalUnitData()
        {
            var timer = new Stopwatch();
            int resultNumberOfRecords = 0;
            using (var transaction = new TransactionScope(TransactionScopeOption.Required, this.transantionOption))
            {
                try
                {
                    List<UnitDatasTemp> preparedUnitDatasTemps = this.data.UnitDatasTemps.All().ToList();
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
                    this.data.UnitsData.BulkInsert(preparedUnitsData, CommonConstants.Phd2SqlDefaultUserName);
                    this.data.DbContext.DbContext.Database.ExecuteSqlCommand(sql: "TRUNCATE TABLE [UnitDataTemporaryRecords]");
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
            if (stepName is IProgressMessage)
            {
                logger.Info($"\tOn step {generatedRecordsCount}: \n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tExpected number of records: {expectedRecordsCount} \n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tThe number of the generated records:{generatedRecordsCount}");
            }
            else
            {
                logger.Info($"\tOn step {stepName}: \n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tExpected number of records: {expectedRecordsCount} \n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\tThe number of the generated records:{generatedRecordsCount}");
            }
        }


        public IEnumerable<UnitDatasTemp> ProcessCalculatedUnits(List<UnitConfig> unitsConfigsList,
                                                                    DateTime recordDataTime,
                                                                    int shift, List<UnitDatasTemp> unitsTempData,
                                                                    ref int expectedNumberOfRecords,
                                                                    bool calculateDailyInfoRecord)
        {
            var currentUnitDatas = new Dictionary<int, UnitDatasTemp>();
            logger.Info($"CALCULATE DAILY INFO RECORD: {calculateDailyInfoRecord}");

            var observedUnitConfigs = unitsConfigsList.Where(x => x.CollectingDataMechanism == "C");
            if (calculateDailyInfoRecord == true)
            {
                observedUnitConfigs = observedUnitConfigs.Where(x => x.ShiftProductTypeId == CommonConstants.DailyInfoDailyInfoHydrocarbonsShiftTypeId).ToList();
                logger.Info($"OBSERED UNIT CONFIG: {observedUnitConfigs.Count()}");
            }
            else
            {
                observedUnitConfigs = observedUnitConfigs.Where(x => x.ShiftProductTypeId != CommonConstants.DailyInfoDailyInfoHydrocarbonsShiftTypeId).ToList();
            }

            expectedNumberOfRecords = observedUnitConfigs.Count();

            foreach (var unitConfig in observedUnitConfigs)
            {
                try
                {
                    if (unitConfig.CalculatedFormula.Equals(value: "C9"))
                    {
                        //this.logger.ErrorFormat("Id {0} calculate by math expression", unitConfig.Id);
                        CalculateByMathExpression(unitConfig, recordDataTime, shift, unitsTempData, currentUnitDatas);
                    }
                    else
                    {
                        var formulaCode = unitConfig.CalculatedFormula ?? string.Empty;
                        var arguments = new FormulaArguments();
                        arguments.MaximumFlow = (double?) unitConfig.MaximumFlow;
                        arguments.EstimatedDensity = (double?) unitConfig.EstimatedDensity;
                        arguments.EstimatedPressure = (double?) unitConfig.EstimatedPressure;
                        arguments.EstimatedTemperature = (double?) unitConfig.EstimatedTemperature;
                        arguments.EstimatedCompressibilityFactor = (double?) unitConfig.EstimatedCompressibilityFactor;
                        arguments.CalculationPercentage = (double?) unitConfig.CalculationPercentage;
                        arguments.CustomFormulaExpression = unitConfig.CustomFormulaExpression;
                        arguments.Code = unitConfig.Code;
                        //arguments.UnitConfigId = unitConfig.Id;

                        List<RelatedUnitConfigs> relatedUnitConfigs = unitConfig.RelatedUnitConfigs.ToList();
                        int confidence = 100;
                        bool allRelatedUnitDataExsists = true;

                        foreach (var relatedUnitConfig in relatedUnitConfigs)
                        {
                            if (allRelatedUnitDataExsists == true)
                            {
                                string parameterType = relatedUnitConfig.RelatedUnitConfig.AggregateGroup;
                                UnitDatasTemp element = unitsTempData
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

                                if (element != null || unitConfig.ShiftProductTypeId == CommonConstants.DailyInfoDailyInfoHydrocarbonsShiftTypeId)
                                {
                                    double inputValue = element?.RealValue ?? 0;
                                    confidence = element?.Confidence ?? 0;

                                    if (parameterType == "I+")
                                    {
                                        double exsistingValue = arguments.InputValue.HasValue ? arguments.InputValue.Value : 0.0;
                                        arguments.InputValue = exsistingValue + inputValue;
                                    }
                                    if (parameterType == "I-")
                                    {
                                        double exsistingValue = arguments.InputValue.HasValue ? arguments.InputValue.Value : 0.0;
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
                            double result = calculator.Calculate(formulaCode, arguments);

                            if (!unitsTempData.Where(x => x.RecordTimestamp == recordDataTime && x.ShiftId == shift && x.UnitConfigId == unitConfig.Id).Any())
                            {
                                if (result < 0 && unitConfig.NeedToSetValueToZero == true)
                                {
                                    result = 0;
                                }

                                currentUnitDatas.Add(unitConfig.Id,
                                                        new UnitDatasTemp
                                                        {
                                                            UnitConfigId = unitConfig.Id,
                                                            RecordTimestamp = recordDataTime,
                                                            ShiftId = shift,
                                                            Value = (double.IsNaN(result) || double.IsInfinity(result)) ? 0.0m : (decimal) result,
                                                            Confidence = confidence
                                                        });
                                if (unitConfig.ShiftProductTypeId == CommonConstants.DailyInfoDailyInfoHydrocarbonsShiftTypeId)
                                {
                                    logger.Info($"DAILY SUCCESS CALCULATION: {unitConfig.Code} {unitConfig.Name}");
                                }
                            }
                        }
                        else
                        {
                            if (unitConfig.ShiftProductTypeId == CommonConstants.DailyInfoDailyInfoHydrocarbonsShiftTypeId)
                            {
                                logger.Error($"DAILY ERROR CALCULATION: {unitConfig.Code} {unitConfig.Name}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    var errorMessage = string.Format("UnitConfigId: {0} \n [{1} \n {2}]", unitConfig.Id, ex.Message, ex.ToString());
                    this.logger.ErrorFormat(errorMessage);
                    this.mailer.SendMail(errorMessage, title: "Phd2Interface Error");
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
            bool allRelatedRecordsExists = true;

            foreach (var relatedunitConfig in relatedunitConfigs)
            {
                if (allRelatedRecordsExists == true)
                {
                    var element = unitsData
                                  .Where(x => x.RecordTimestamp == recordDataTime)
                                  .Where(x => x.ShiftId == shift)
                                  .Where(x => x.UnitConfigId == relatedunitConfig.RelatedUnitConfigId)
                                  .FirstOrDefault();
                    if (element != null)
                    {
                        //this.logger.ErrorFormat("Id {0} EXSIST RELATED CONFIG {1}", element.UnitConfigId, element.RealValue);
                        double inputValue = element.RealValue;
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
                            if (element.Confidence != 100)
                            {
                                confidence = element.Confidence;
                            }
                        }

                        inputParams.Add(string.Format("p{0}", indexCounter), inputValue);
                        indexCounter++;
                    }
                    else
                    {
                        //this.logger.ErrorFormat("Id {0} MISSING RELATED CONFIG {1}", relatedunitConfig.RelatedUnitConfigId, relatedunitConfig.RelatedUnitConfig.Code);
                        allRelatedRecordsExists = false;
                    }
                }

            }

            if (allRelatedRecordsExists == true)
            {
                double result = new ProductionDataCalculatorService(this.data).Calculate(mathExpression, "p", inputParams, unitConfig.Code);
                if (!unitsData.Where(x => x.RecordTimestamp == recordDataTime && x.ShiftId == shift && x.UnitConfigId == unitConfig.Id).Any())
                {
                    calculatedUnitsData.Add(
                        unitConfig.Id,
                        new UnitDatasTemp
                        {
                            UnitConfigId = unitConfig.Id,
                            RecordTimestamp = recordDataTime,
                            ShiftId = shift,
                            Value = (double.IsNaN(result) || double.IsInfinity(result)) ? 0.0m : (decimal) result,
                            Confidence = confidence,
                        });
                }
            }

        }

        /// <summary>
        /// Processes the manual units.
        /// </summary>
        /// <param name="unitsConfigsList">The units configs list.</param>
        /// <param name="targetRecordTimestamp">The record data time.</param>
        /// <param name="shiftId">The Id of the shift.</param>
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
                        var recordTimestamp = new DateTime(2016, 1, 1, 1, 0, 0);

                        oPhd.StartTime = string.Format("{0}", endShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                        oPhd.EndTime = oPhd.StartTime;

                        var result = oPhd.FetchRowData(unitConfig.PreviousShiftTag);
                        var row = result.Tables[0].Rows[0];
                        var endValue = row.IsNull("Value") ? 0 : Convert.ToInt64(row["Value"]);
                        endConfidence = row.IsNull("Confidence") ? 0 : Convert.ToInt32(row["Confidence"]);
                        if (!row.IsNull("Timestamp")) { recordTimestamp = Convert.ToDateTime(row["Timestamp"]); }
                        logger.DebugFormat("{0} {1} : Tag:{2} Value:{3} Confidence:{4} Timestamp {5}", unitConfig.Code, unitConfig.Name, unitConfig.PreviousShiftTag, endValue, endConfidence, recordTimestamp);

                        oPhd.StartTime = string.Format("{0}", beginShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                        oPhd.EndTime = oPhd.StartTime;
                        result = oPhd.FetchRowData(unitConfig.PreviousShiftTag);
                        row = result.Tables[0].Rows[0];
                        var beginValue = row.IsNull("Value") ? 0 : Convert.ToInt64(row["Value"]);
                        beginConfidence = row.IsNull("Confidence") ? 0 : Convert.ToInt32(row["Confidence"]);
                        if (!row.IsNull("Timestamp")) { recordTimestamp = Convert.ToDateTime(row["Timestamp"]); }
                        logger.DebugFormat("{0} {1} : Tag:{2} Value:{3} Confidence:{4} Timestamp {5}", unitConfig.Code, unitConfig.Name, unitConfig.PreviousShiftTag, beginValue, beginConfidence, recordTimestamp);

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

        private IEnumerable<UnitDatasTemp> ProcessAutomaticDeltaTwoSourcesUnits(List<UnitConfig> unitsConfigsList, List<UnitDatasTemp> unitsData,
                                                                    PHDHistorian oPhd, DateTime targetRecordTimestamp,
                                                                    Shift shiftData, ref int expectedNumberOfRecords)
        {
            var currentUnitDatas = new List<UnitDatasTemp>();
            var baseDate = targetRecordTimestamp.Date;

            var observedUnitConfigs = unitsConfigsList.Where(x => x.CollectingDataMechanism == "AS");
            expectedNumberOfRecords = observedUnitConfigs.Count();

            foreach (var unitConfig in observedUnitConfigs)
            {
                if (!unitsData.Any(x => x.UnitConfigId == unitConfig.Id))
                {
                    if (shiftData != null)
                    {
                        DateTime endShiftDateTime = baseDate + shiftData.EndTime;
                        DateTime beginShiftDateTime = endShiftDateTime - shiftData.ShiftDuration;

                        string mathExpression = unitConfig.CustomFormulaExpression;
                        var phdTags = new String[]
                        {
                            "TSN_BTG_QN_T.PV",
                            "TSN_BTG1_QN_T.PV",
                            "610FQI547.TOTALIZER_S.OLDAV",
                            "SU400FQI436.TOTALIZER_S.OLDAV"
                        };
                        var phdTagsConfidences = new int[phdTags.GetLength(dimension: 0)];
                        var phdTagsValues = new decimal[phdTags.GetLength(dimension: 0)];
                        for (int i = 0; i < phdTagsConfidences.GetLength(dimension: 0); i++)
                        {
                            phdTagsConfidences[i] = 100;
                        }

                        try
                        {
                            var beginConfidence = 100;
                            var endConfidence = 100;
                            var deltaValue = 0L;

                            using (PHDHistorian oPhdOld = new PHDHistorian())
                            {
                                using (PHDServer defaultServer = new PHDServer("srv-vm-mes-phd"))
                                //using (PHDServer defaultServer = new PHDServer("10.94.0.213"))
                                {
                                    SetPhdConnectionSettings(oPhdOld, defaultServer, targetRecordTimestamp, shiftData);
                                    var recordTimestamp = new DateTime(2016, 1, 1, 1, 0, 0);

                                    oPhdOld.StartTime = string.Format("{0}", endShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                                    oPhdOld.EndTime = oPhdOld.StartTime;

                                    for (int phdTagIndex = 0; phdTagIndex < 2; phdTagIndex++)
                                    {
                                        var result = oPhdOld.FetchRowData(phdTags[phdTagIndex]);
                                        var row = result.Tables[0].Rows[0];
                                        var endValue = row.IsNull("Value") ? 0 : Convert.ToInt64(row["Value"]);
                                        endConfidence = row.IsNull("Confidence") ? 0 : Convert.ToInt32(row["Confidence"]);
                                        if (!row.IsNull("Timestamp")) { recordTimestamp = Convert.ToDateTime(row["Timestamp"]); }
                                        logger.DebugFormat("{0} {1} : Tag:{2} Value:{3} Confidence:{4} Timestamp {5}", unitConfig.Code, unitConfig.Name, unitConfig.PreviousShiftTag, endValue, endConfidence, recordTimestamp);

                                        oPhdOld.StartTime = string.Format("{0}", beginShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                                        oPhdOld.EndTime = oPhdOld.StartTime;
                                        result = oPhdOld.FetchRowData(phdTags[phdTagIndex]);
                                        row = result.Tables[0].Rows[0];
                                        var beginValue = row.IsNull("Value") ? 0 : Convert.ToInt64(row["Value"]);
                                        beginConfidence = row.IsNull("Confidence") ? 0 : Convert.ToInt32(row["Confidence"]);
                                        if (!row.IsNull("Timestamp")) { recordTimestamp = Convert.ToDateTime(row["Timestamp"]); }
                                        logger.DebugFormat("{0} {1} : Tag:{2} Value:{3} Confidence:{4} Timestamp {5}", unitConfig.Code, unitConfig.Name, unitConfig.PreviousShiftTag, beginValue, beginConfidence, recordTimestamp);

                                        phdTagsValues[phdTagIndex] = (decimal) (endValue - beginValue);
                                        phdTagsConfidences[phdTagIndex] = (beginConfidence + endConfidence) / 2;
                                    }
                                }
                            }

                            using (PHDHistorian oPhdNew = new PHDHistorian())
                            {
                                using (PHDServer defaultServer = new PHDServer("phd-l35-1"))
                                //using (PHDServer defaultServer = new PHDServer("10.94.0.195"))
                                {
                                    SetPhdConnectionSettings(oPhdNew, defaultServer, targetRecordTimestamp, shiftData);

                                    for (int i = 2; i < phdTags.GetLength(dimension: 0); i++)
                                    {
                                        DataSet dsGrid = oPhdNew.FetchRowData(phdTags[i]);
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
                                                        phdTagsConfidences[i] = Convert.ToInt32(row[dc]);
                                                    }
                                                    else
                                                    {
                                                        phdTagsConfidences[i] = 0;
                                                        break;
                                                    }
                                                }
                                                else if (dc.ColumnName.Equals("Value"))
                                                {
                                                    if (!string.IsNullOrWhiteSpace(row[dc].ToString()))
                                                    {
                                                        phdTagsValues[i] = Convert.ToDecimal(row[dc]);
                                                    }
                                                }
                                                else if (dc.ColumnName.Equals("TimeStamp"))
                                                {
                                                    //unitData.RecordTimestamp = targetRecordTimestamp;
                                                }
                                            }

                                            var currentTimestamp = new DateTime(2016, 1, 1, 1, 0, 0);
                                            var currentValue = row.IsNull("Value") ? 0 : Convert.ToInt64(row["Value"]);
                                            var currentConfidence = row.IsNull("Confidence") ? 0 : Convert.ToInt32(row["Confidence"]);
                                            if (!row.IsNull("Timestamp")) { currentTimestamp = Convert.ToDateTime(row["Timestamp"]); }
                                            logger.DebugFormat("Automatic {0} {1} : Tag:{2} Value:{3} Confidence:{4} Timestamp:{5}",
                                                                unitConfig.Code,
                                                                unitConfig.Name,
                                                                phdTags[i],
                                                                currentValue,
                                                                currentConfidence,
                                                                currentTimestamp);
                                        }
                                    }
                                }
                            }

                            var realValue = phdTagsValues[0];
                            for (int i = 1; i < phdTagsValues.GetLength(dimension: 0); i++)
                            {
                                realValue -= phdTagsValues[i];
                            }
                            if (realValue < 0)
                            {
                                realValue = 0;
                            }

                            var confidence = phdTagsConfidences.Sum() / phdTagsConfidences.GetLength(dimension: 0);

                            currentUnitDatas.Add(
                                new UnitDatasTemp
                                {
                                    UnitConfigId = unitConfig.Id,
                                    Value = realValue,
                                    ShiftId = shiftData.Id,
                                    RecordTimestamp = targetRecordTimestamp,
                                    Confidence = confidence
                                });
                        }
                        catch (Exception ex)
                        {
                            var errorMessage = string.Format("UnitConfigId: {0} \n [{1} \n {2}]", unitConfig.Id, ex.Message, ex.ToString());
                            this.logger.ErrorFormat(errorMessage);
                            this.mailer.SendMail(errorMessage, "Phd2Interface Error");
                        }
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
                        var recordTimestamp = new DateTime(2016, 1, 1, 1, 0, 0);

                        oPhd.StartTime = string.Format("{0}", endShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                        oPhd.EndTime = oPhd.StartTime;
                        var result = oPhd.FetchRowData(tags[0]);
                        var row = result.Tables[0].Rows[0];
                        var endValue = row.IsNull("Value") ? 0 : Convert.ToInt64(row["Value"]);
                        endConfidence = row.IsNull("Confidence") ? 0 : Convert.ToInt32(row["Confidence"]);
                        if (!row.IsNull("Timestamp")) { recordTimestamp = Convert.ToDateTime(row["Timestamp"]); }
                        logger.DebugFormat("{0} {1} : Tag:{2} Value:{3} Confidence:{4} Timestamp:{5}", unitConfig.Code, unitConfig.Name, tags[0], endValue, endConfidence, recordTimestamp);

                        result = oPhd.FetchRowData(tags[1]);
                        row = result.Tables[0].Rows[0];
                        var pressure = Convert.ToDecimal(row["Value"]);

                        oPhd.StartTime = string.Format("{0}", beginShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                        oPhd.EndTime = oPhd.StartTime;
                        result = oPhd.FetchRowData(tags[0]);
                        row = result.Tables[0].Rows[0];
                        var beginValue = row.IsNull("Value") ? 0 : Convert.ToInt64(row["Value"]);
                        beginConfidence = row.IsNull("Confidence") ? 0 : Convert.ToInt32(row["Confidence"]);
                        if (!row.IsNull("Timestamp")) { recordTimestamp = Convert.ToDateTime(row["Timestamp"]); }
                        logger.DebugFormat("{0} {1} : Tag:{2} Value:{3} Confidence:{4} Timestamp:{5}", unitConfig.Code, unitConfig.Name, tags[0], beginValue, beginConfidence, recordTimestamp);

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
                        var val = row.IsNull("Value") ? 0 : Convert.ToDouble(row["Value"]);
                        var currentConf = row.IsNull("Confidence") ? 0 : Convert.ToInt32(row["Confidence"]);
                        var recordTimestamp = new DateTime(2016, 1, 1, 1, 0, 0);
                        if (!row.IsNull("Timestamp")) { recordTimestamp = Convert.ToDateTime(row["Timestamp"]); }
                        confidence += currentConf;
                        inputParams.Add(string.Format("p{0}", argumentIndex), val);
                        argumentIndex++;
                        logger.DebugFormat("{0} {1} : Tag:{2} Value:{3} Confidence:{4} Timestamp:{5}", unitConfig.Code, unitConfig.Name, item, val, currentConf, recordTimestamp);
                    }

                    if (unitConfig.CalculationPercentage.HasValue)
                    {
                        inputParams.Add(string.Format("p{0}", argumentIndex), (double) unitConfig.CalculationPercentage.Value);
                    }

                    var result = new ProductionDataCalculatorService(this.data).Calculate(formula, "p", inputParams, unitConfig.Code);
                    if (!unitsData.Any(x => x.RecordTimestamp == targetRecordTimestamp && x.ShiftId == shift.Id && x.UnitConfigId == unitConfig.Id))
                    {
                        currentUnitDatas.Add(
                            new UnitDatasTemp
                            {
                                UnitConfigId = unitConfig.Id,
                                RecordTimestamp = targetRecordTimestamp,
                                ShiftId = shift.Id,
                                Value = (double.IsNaN(result) || double.IsInfinity(result)) ? 0.0m : (decimal) result,
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
            if (Assembly.GetEntryAssembly() != null)
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
                //oPhd.Sampletype = SAMPLETYPE.Raw;
                oPhd.MinimumConfidence = Convert.ToInt32(settings.Settings.Get("PHD_DATA_MIN_CONFIDENCE").Value.ValueXml.InnerText);
                oPhd.MaximumRows = Convert.ToUInt32(settings.Settings.Get("PHD_DATA_MAX_ROWS").Value.ValueXml.InnerText);
                //oPhd.Offset = Convert.ToInt32(settings.Settings.Get("PHD_OFFSET").Value.ValueXml.InnerText);
            }
            else
            {
                var settings = WebConfigurationManager.AppSettings;
                defaultServer.Port = Convert.ToInt32(settings["PHD_PORT"]);
                defaultServer.APIVersion = SERVERVERSION.RAPI200;
                oPhd.DefaultServer = defaultServer;
                var beginShiftDateTime = targetRecordTimestamp.Date + targetShift.EndTime;
                oPhd.StartTime = string.Format("{0}", beginShiftDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                oPhd.EndTime = oPhd.StartTime;
                oPhd.Sampletype = SAMPLETYPE.Snapshot;
                oPhd.MinimumConfidence = Convert.ToInt32(settings["PHD_DATA_MIN_CONFIDENCE"]);
                oPhd.MaximumRows = Convert.ToUInt32(settings["PHD_DATA_MAX_ROWS"]);
                //oPhd.Offset = Convert.ToInt32(settings.Settings.Get("PHD_OFFSET").Value.ValueXml.InnerText);
            }

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

        private UnitDatasTemp GetUnitDataFromPhd(UnitConfig unitConfig, PHDHistorian oPhd, DateTime targetRecordTimestamp, out int confidence)
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

                var currentTimestamp = new DateTime(year: 2016, month: 1, day: 1, hour: 1, minute: 0, second: 0);
                var currentValue = row.IsNull("Value") ? 0 : Convert.ToInt64(row["Value"]);
                var currentConfidence = row.IsNull("Confidence") ? 0 : Convert.ToInt32(row["Confidence"]);
                if (!row.IsNull("Timestamp")) { currentTimestamp = Convert.ToDateTime(row["Timestamp"]); }
                logger.DebugFormat("Automatic {0} {1} : Tag:{2} Value:{3} Confidence:{4} Timestamp:{5}",
                                    unitConfig.Code,
                                    unitConfig.Name,
                                    unitConfig.PreviousShiftTag,
                                    currentValue,
                                    currentConfidence,
                                    currentTimestamp);
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

        public IEnumerable<UnitDatasTemp> GetPrimaryProductionData(PrimaryDataSourceType dataSource,
                                                 string hostName,
                                                 DateTime targetRecordTimestamp,
                                                 Shift shift,
                                                 IEnumerable<UnitDatasTemp> lastIterationData = null)
        {
            IEnumerable<UnitDatasTemp> preparedRecords = new List<UnitDatasTemp>();
            try
            {
                logger.Info("Sync primary data started!");

                preparedRecords = ReadUnitsDataForShift(targetRecordTimestamp, shift, hostName, dataSource, lastIterationData);
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

            return preparedRecords;
        }

        /// <summary>
        /// Creates the missing records.
        /// </summary>
        /// <param name="targetRecordTimestamp">The target record timestamp.</param>
        /// <param name="shift">The shift.</param>
        /// <param name="existTempUnitData">The exist temp unit data.</param>
        /// <param name="expectedNumberOfRecords">The expected number of records.</param>
        /// <returns></returns>
        public IEnumerable<UnitDatasTemp> CreateMissingRecords(DateTime targetRecordTimestamp, Shift shift, IEnumerable<UnitDatasTemp> existTempUnitData)
        {
            var unitsConfigsList = this.data.UnitConfigs.All().ToList();
            var unitDatas = existTempUnitData.ToDictionary(x => x.UnitConfigId);
            var targetDate = targetRecordTimestamp.Date;
            var confidense = 0;
            var result = new List<UnitDatasTemp>();

            foreach (var position in unitsConfigsList)
            {
                if (!unitDatas.ContainsKey(position.Id))
                {
                    result.Add(this.SetDefaultUnitsDataValue(targetDate, shift.Id, position, confidense));
                    logger.Info($"CREATE MISSING POSITION WITHOUT CHECK {position.Code} {position.Name}");
                }
            }

            return result;
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

        public void ProcessInventoryTanksData(PrimaryDataSourceType dataSource)
        {
            DateTime now = DateTime.Now;
            if (2 <= now.Minute && now.Minute <= 10)
            {
                string exeFileName = Assembly.GetEntryAssembly().Location;
                Configuration configuration = ConfigurationManager.OpenExeConfiguration(exeFileName);
                ConfigurationSectionGroup appSettingsGroup = configuration.GetSectionGroup(sectionGroupName: "applicationSettings");
                ConfigurationSection appSettingsSection = appSettingsGroup.Sections[0];
                var settings = appSettingsSection as ClientSettingsSection;

                int inventoryMinHoursInterval = Convert.ToInt32(settings.Settings.Get(elementKey: "MIN_GET_INVENTORY_HOURS_INTERVAL").Value.ValueXml.InnerText);
                int inventoryHoursInterval = Convert.ToInt32(settings.Settings.Get(elementKey: "MAX_GET_INVENTORY_HOURS_INTERVAL").Value.ValueXml.InnerText);
                for (int hours = inventoryMinHoursInterval; hours < inventoryHoursInterval; hours++)
                {
                    var recordTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, minute: 0, second: 0);
                    DateTime checkedDateTime = recordTime.AddHours(-hours);
                    var targetRecordDateTime = new DateTime(checkedDateTime.Year, checkedDateTime.Month, checkedDateTime.Day, checkedDateTime.Hour, 0, second: 0);
                    if (this.data.TanksData.All().Include(t => t.TankConfig).Where(t => t.RecordTimestamp == checkedDateTime && t.TankConfig.DataSource == dataSource).Any())
                    {
                        logger.Debug($"There are already a records from {dataSource.ToString()} for: {checkedDateTime:yyyy-MM-dd HH:ss:mm}!");
                        continue;
                    }

                    try
                    {
                        logger.Info($"--------------------------------  Begin processing inventory tanks data from {dataSource.ToString()} for {checkedDateTime:yyyy-MM-dd HH:ss:mm}!");
                        var tanksPhdConfigs = new Dictionary<string, int>();
                        var tanksData = new Dictionary<int, TankData>();

                        List<TankConfig> tanks = data.Tanks.All().Include(x => x.Park).Where(x => x.DataSource == dataSource).ToList();
                        foreach (var tank in tanks)
                        {
                            var tankData = new TankData();
                            tankData.RecordTimestamp = targetRecordDateTime;
                            tankData.TankConfigId = tank.Id;
                            tankData.ParkId = tank.ParkId;
                            tankData.NetStandardVolume = tank.UnusableResidueLevel;
                            tankData.Confidence = 100;

                            SetTankPhdConfigValue(tank.PhdTagProductId, tank.Id, tanksPhdConfigs);
                            SetTankPhdConfigValue(tank.PhdTagLiquidLevel, tank.Id, tanksPhdConfigs);
                            SetTankPhdConfigValue(tank.PhdTagProductLevel, tank.Id, tanksPhdConfigs);
                            SetTankPhdConfigValue(tank.PhdTagFreeWaterLevel, tank.Id, tanksPhdConfigs);
                            SetTankPhdConfigValue(tank.PhdTagReferenceDensity, tank.Id, tanksPhdConfigs);
                            SetTankPhdConfigValue(tank.PhdTagNetStandardVolume, tank.Id, tanksPhdConfigs);
                            SetTankPhdConfigValue(tank.PhdTagWeightInAir, tank.Id, tanksPhdConfigs);
                            SetTankPhdConfigValue(tank.PhdTagWeightInVacuum, tank.Id, tanksPhdConfigs);
                            tanksData.Add(tank.Id, tankData);
                        }

                        List<string> tags = tanksPhdConfigs.Keys.ToList();
                        if (tags.Count() > 0)
                        {
                            using (PHDHistorian oPhd = new PHDHistorian())
                            {
                                string phdServer = settings.Settings.Get("PHD_HOST" + (int) dataSource).Value.ValueXml.InnerText;
                                using (PHDServer defaultServer = new PHDServer(phdServer))
                                {
                                    string getRecordTimestamp = string.Format(format: "{0}", arg0: targetRecordDateTime.ToString(CommonConstants.PhdDateTimeFormat, CultureInfo.InvariantCulture));
                                    defaultServer.Port = Properties.Settings.Default.PHD_PORT;
                                    defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
                                    oPhd.DefaultServer = defaultServer;
                                    oPhd.StartTime = getRecordTimestamp;
                                    oPhd.EndTime = getRecordTimestamp;
                                    oPhd.Sampletype = SAMPLETYPE.Raw;
                                    oPhd.MinimumConfidence = 100;
                                    oPhd.MaximumRows = 1;

                                    var oTags = new Tags();
                                    foreach (var tag in tags)
                                    {
                                        oTags.Add(new Tag(tag));
                                    }

                                    DataSet result = oPhd.FetchRowData(oTags);
                                    ProcessingPhdTankData(result.Tables[0], tanksData, tanksPhdConfigs, targetRecordDateTime, data);
                                    if (tanksData.Count() > 0)
                                    {
                                        this.data.TanksData.BulkInsert(tanksData.Values.ToList(), userName: "Phd2SqlLoading");
                                        this.data.SaveChanges(userName: "Phd2SqlLoading");
                                        logger.Info($"Successfully added {tanksData.Count()} TanksData records from {dataSource.ToString()} for: {checkedDateTime:yyyy-MM-dd HH:ss:mm}!");
                                    }

                                    result = null;
                                }
                            }
                        }

                        logger.Info($"Finish processing inventory tanks data from {dataSource.ToString()} for {checkedDateTime:yyyy-MM-dd HH:ss:mm}!");

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

        private static void SetTankPhdConfigValue(string phdTagName, int tankId, Dictionary<string, int> tanksPhdConfigs)
        {
            if (!string.IsNullOrEmpty(phdTagName))
            {
                tanksPhdConfigs.Add(phdTagName, tankId);
            }
        }

        private void ProcessingPhdTankData(DataTable oPhdData, Dictionary<int, TankData> tanksData, Dictionary<string, int> tanksPhdConfigs, DateTime targetRecordDateTime, IProductionData data)
        {
            int confedence = 100;
            string tagName = string.Empty;
            decimal tagValue = 0m;

            DataRow row = null;

            var products = data.Products.All().Select(x => new
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name
            });


            for (int i = 0; i < oPhdData.Rows.Count; i++)
            {
                row = oPhdData.Rows[i];
                int tankId = tanksPhdConfigs[row["TagName"].ToString().Trim()];
                TankData tankData = tanksData[tankId];

                try
                {
                    foreach (DataColumn dc in oPhdData.Columns)
                    {
                        if (dc.ColumnName.Equals(value: "Tolerance") || dc.ColumnName.Equals(value: "HostName") || dc.ColumnName.Equals(value: "Units"))
                        {
                            continue;
                        }
                        else if (dc.ColumnName.Equals(value: "Confidence") /*&& !row[dc].ToString().Equals(value: "100")*/)
                        {
                            if (!row.IsNull(columnName: "Confidence"))
                            {
                                confedence = Convert.ToInt32(row[dc]);
                            }
                            else
                            {
                                confedence = 0;
                            }
                            //break;
                        }
                        else if (dc.ColumnName.Equals(value: "TagName"))
                        {
                            tagName = row[dc].ToString();
                        }
                        else if (dc.ColumnName.Equals(value: "TimeStamp"))
                        {
                            DateTime dt = !row.IsNull(columnName: "TimeStamp") ? Convert.ToDateTime(row[dc]) : DateTime.MinValue;
                            TimeSpan difference = targetRecordDateTime - dt;
                            string currentTagName = !row.IsNull(columnName: "TagName") ? row["TagName"].ToString() : string.Empty;
                            string currentTagValue = !row.IsNull(columnName: "Value") ? row["Value"].ToString() : string.Empty;
                            if (difference.TotalMinutes > 120)
                            {
                                confedence = 0;
                            }
                        }
                        else if (dc.ColumnName.Equals(value: "Value"))
                        {
                            if (!row.IsNull(columnName: "Value"))
                            {
                                tagValue = Convert.ToDecimal(row[dc]);
                            }
                        }
                    }

                    //if (confedence != 100)
                    //{
                    //    logger.Error($"TankId {tankData.TankConfigId}");
                    //    tankData.Confidence = confedence;
                    //    continue;
                    //}

                    if (tagName.Contains(value: ".PROD_ID"))
                    {
                        int prId = Convert.ToInt32(tagValue);
                        TankMasterProduct product = data.TankMasterProducts.All().Where(x => x.TankMasterProductCode == prId).FirstOrDefault();
                        if (product != null)
                        {
                            tankData.ProductId = product.Id;
                            var pr = products.Where(p => p.Id == product.Id).FirstOrDefault();
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
                    else if (tagName.EndsWith(value: ".LL") || tagName.EndsWith(value: ".LEVEL_MM"))
                    {
                        tankData.LiquidLevel = tagValue;
                    }
                    else if (tagName.EndsWith(value: ".LL_FWL") || tagName.EndsWith(value: ".LEVEL_FWL"))
                    {
                        tankData.ProductLevel = tagValue;
                    }
                    else if (tagName.EndsWith(value: ".FWL"))
                    {
                        tankData.FreeWaterLevel = tagValue;
                    }
                    else if (tagName.EndsWith(value: ".DREF") || tagName.EndsWith(value: ".REF_DENS"))
                    {
                        tankData.ReferenceDensity = tagValue;
                    }
                    else if (tagName.EndsWith(value: ".NSV"))
                    {
                        tankData.NetStandardVolume = tagValue;
                    }
                    else if (tagName.EndsWith(value: ".WIA"))
                    {
                        tankData.WeightInAir = tagValue;
                    }
                    else if (tagName.EndsWith(value: ".WIV"))
                    {
                        tankData.WeightInVacuum = tagValue;
                    }

                    tankData.Confidence = tankData.Confidence == 100 ? confedence : tankData.Confidence;
                    tanksData[tankId] = tankData;
                }
                catch (Exception exception)
                {
                    this.logger.Error($"Tank Id [{tankData.TankConfigId}] Exception:\n\n\n{exception.ToString()}");
                    this.mailer.SendMail($"There is a exception [{exception.Message}] with Tank {tankData.TankConfigId}", title: "Phd2Sql Inventory");
                    tankData.Confidence = -1;
                    tanksData[tankId] = tankData;
                }
            }
        }
    }
}