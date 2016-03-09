namespace CollectingProductionDataSystem.PhdApplication.PrimaryDataServices
{
    using System.Configuration;
    using System.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using Uniformance.PHD;
    using CollectingProductionDataSystem.PhdApplication.Contracts;
    using CollectingProductionDataSystem.Application.ProductionDataServices;
    using System.Reflection;

    public class PhdPrimaryDataService : IPrimaryDataService
    {
        private readonly IProductionData data;

        public PhdPrimaryDataService(IProductionData dataParam)
        {
            this.data = dataParam;
        }

        public int ReadAndSaveUnitsDataForShift(DateTime currentDateTime, int offsetInHours, PrimaryDataSourceType dataSource)
        {
            var totalInsertedRecords = 0;

            var now = currentDateTime;
            var record = DateTime.Now.AddHours(offsetInHours); 
            var shift = GetShift(record);
            var recordDataTime = GetRecordTimestamp(record);

            var exeFileName = Assembly.GetEntryAssembly().Location;
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(exeFileName);
            ConfigurationSectionGroup appSettingsGroup = configuration.GetSectionGroup("applicationSettings");
            ConfigurationSection appSettingsSection = appSettingsGroup.Sections[0];
            ClientSettingsSection settings = appSettingsSection as ClientSettingsSection;

            using (PHDHistorian oPhd = new PHDHistorian())
            {
                using (PHDServer defaultServer = new PHDServer(settings.Settings.Get("PHD_HOST").Value.ValueXml.InnerText))
                {
                    var hours = Math.Truncate((now - record).TotalHours);
                    SetPhdConnectionSettings(oPhd, defaultServer, hours);

                    var unitsConfigsList = this.data.UnitConfigs.All().Where(x=>x.DataSource == dataSource).ToList();
                    var unitsData = this.data.UnitsData.All().Where(x => x.RecordTimestamp == recordDataTime && x.ShiftId == shift).ToList();

                    ProcessAutomaticUnits(unitsConfigsList, unitsData, oPhd, recordDataTime, shift);
                    totalInsertedRecords += this.data.SaveChanges("Phd2SqlLoader").ResultRecordsCount;

                    ProcessAutomaticDeltaUnits(unitsConfigsList, unitsData, oPhd, recordDataTime, shift);
                    totalInsertedRecords += this.data.SaveChanges("Phd2SqlLoader").ResultRecordsCount;

                    ProcessAutomaticCalulatedUnits(unitsConfigsList, unitsData, oPhd, recordDataTime, shift);
                    totalInsertedRecords += this.data.SaveChanges("Phd2SqlLoader").ResultRecordsCount;

                    ProcessManualUnits(unitsConfigsList, recordDataTime, shift, unitsData);
                    totalInsertedRecords += this.data.SaveChanges("Phd2SqlLoader").ResultRecordsCount;

                    ProcessCalculatedByAutomaticUnits(unitsConfigsList, oPhd, recordDataTime, shift, unitsData);
                    totalInsertedRecords += this.data.SaveChanges("Phd2SqlLoader").ResultRecordsCount;

                    unitsData.Clear();
                    unitsData = this.data.UnitsData.All().Where(x => x.RecordTimestamp == recordDataTime && x.ShiftId == shift).ToList();

                    var calculatedUnitDatas = ProcessCalculatedUnits(unitsConfigsList, recordDataTime, shift, unitsData);
                    if (calculatedUnitDatas.Count > 0)
                    {
                        this.data.UnitsData.BulkInsert(calculatedUnitDatas, "Phd2SqlLoader");
                        totalInsertedRecords += calculatedUnitDatas.Count;
                    }
                }
            }

            return totalInsertedRecords;
        }

        private void ProcessCalculatedByAutomaticUnits(List<UnitConfig> unitsConfigsList, PHDHistorian oPhd, DateTime recordDataTime, ShiftType shift, List<UnitsData> unitsData)
        {
            oPhd.StartTime = "NOW-1M";
            oPhd.EndTime = "NOW-1M";

            foreach (var unitConfig in unitsConfigsList.Where(x => x.CollectingDataMechanism == "CC"))
            {
                try
                {
                    var confidence = 0;
                    var inputParams = new Dictionary<string, double>();
                    var formula = unitConfig.CalculatedFormula;
                    var arguments = unitConfig.PreviousShiftTag.Split(new char[]{'@'});
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
                    if (!unitsData.Where(x => x.RecordTimestamp == recordDataTime && x.ShiftId == shift && x.UnitConfigId == unitConfig.Id).Any())
                    {
                        this.data.UnitsData.Add(
                            new UnitsData
                            {
                                UnitConfigId = unitConfig.Id,
                                RecordTimestamp = recordDataTime,
                                ShiftId = shift,
                                Value = (double.IsNaN(result)||double.IsInfinity(result)) ? 0.0m : (decimal)result,
                                Confidence = confidence / arguments.Count()
                            });
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("UnitConfigId: {0} [{1}]", unitConfig.Id, ex.Message), ex);
                }
            }
        }
 
        private List<UnitsData> ProcessCalculatedUnits(List<UnitConfig> unitsConfigsList, DateTime recordDataTime, ShiftType shift, List<UnitsData> unitsData)
        {
            var calculatedUnitsData = new Dictionary<int, UnitsData>();

            foreach (var unitConfig in unitsConfigsList.Where(x => x.CollectingDataMechanism == "C"))
            {
                try 
	            {
                    if (unitConfig.CalculatedFormula.Equals("C9"))
                    {
                        CalculateByMathExpression(unitConfig, recordDataTime, shift, unitsData, calculatedUnitsData);
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
                                if (calculatedUnitsData.ContainsKey(ru.RelatedUnitConfigId))
                                {
                                    inputValue = calculatedUnitsData[ru.RelatedUnitConfigId].RealValue;   
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
                            calculatedUnitsData.Add(unitConfig.Id,
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

            return calculatedUnitsData.Values.ToList();
        }
 
        private void CalculateByMathExpression(UnitConfig unitConfig, DateTime recordDataTime, ShiftType shift, List<UnitsData> unitsData, Dictionary<int, UnitsData> calculatedUnitsData)
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

        private void ProcessManualUnits(List<UnitConfig> unitsConfigsList, DateTime recordDataTime, ShiftType shift, List<UnitsData> unitsData)
        {
            foreach (var unitConfig in unitsConfigsList.Where(x => x.CollectingDataMechanism == "M" || 
                x.CollectingDataMechanism == "MC" || 
                x.CollectingDataMechanism == "MD" || 
                x.CollectingDataMechanism == "MS"  ))
            {
                SetDefaultValue(recordDataTime, shift, unitsData, unitConfig, 100);    
            }
        }
 
        private void ProcessAutomaticUnits(List<UnitConfig> unitsConfigsList, List<UnitsData> unitsData, PHDHistorian oPhd, DateTime recordDataTime, ShiftType shift)
        {
            foreach (var unitConfig in unitsConfigsList.Where(x => x.CollectingDataMechanism == "A"))
            {
                if (unitConfig.NeedToGetOnlyLastShiftValue == 1 && shift != ShiftType.Third)
                {
                    SetDefaultValue(recordDataTime, shift, unitsData, unitConfig, 100);   
                }
                else
                {
                    var confidence = 0;
                    if (!unitsData.Where(x => x.UnitConfigId == unitConfig.Id).Any())
                    {
                        var unitData = GetUnitData(unitConfig, oPhd, recordDataTime, out confidence);
                        if (confidence >= Properties.Settings.Default.PHD_DATA_MIN_CONFIDENCE && unitData.RecordTimestamp != null)
                        {
                            if (!unitsData.Where(x => x.RecordTimestamp == unitData.RecordTimestamp && x.ShiftId == shift && x.UnitConfigId == unitConfig.Id).Any())
                            {
                                unitData.ShiftId = shift;
                                unitData.RecordTimestamp = unitData.RecordTimestamp.Date;
                                unitData.Confidence = confidence;
                                this.data.UnitsData.Add(unitData);
                            }
                        }
                        else
                        {
                            SetDefaultValue(recordDataTime, shift, unitsData, unitConfig, confidence);
                        }
                    } 
                }
            }
        }

        private void ProcessAutomaticDeltaUnits(List<UnitConfig> unitsConfigsList, List<UnitsData> unitsData, PHDHistorian oPhd, DateTime recordDataTime, ShiftType shift)
        {
            foreach (var unitConfig in unitsConfigsList.Where(x => x.CollectingDataMechanism == "AA"))
            {
                //var confidence = 0;
                if (!unitsData.Where(x => x.UnitConfigId == unitConfig.Id).Any())
                {
                    var shiftData = this.data.Shifts.GetById((int)shift);
                    if (shiftData != null)
                    {
                        var end = recordDataTime.AddTicks(shiftData.EndTicks);
                        var begin = end.AddHours(-8);

                        var beginConfidence = 100;
                        var endConfidence = 100;

                        var endTimestamp = DateTime.Now - end;
                        var beginTimestamp = DateTime.Now - begin;

                        var endPhdTimestamp = string.Format("NOW-{0}H{1}M", Math.Truncate(endTimestamp.TotalHours), endTimestamp.Minutes);
                        var beginPhdTimestamp = string.Format("NOW-{0}H{1}M", Math.Truncate(beginTimestamp.TotalHours), beginTimestamp.Minutes);

                        oPhd.StartTime = endPhdTimestamp;
                        oPhd.EndTime = endPhdTimestamp;
                        var result = oPhd.FetchRowData(unitConfig.PreviousShiftTag);
                        var row = result.Tables[0].Rows[0];
                        var endValue = Convert.ToInt64(row["Value"]);
                        endConfidence = Convert.ToInt32(row["Confidence"]);

                        oPhd.StartTime = beginPhdTimestamp;
                        oPhd.EndTime = beginPhdTimestamp;
                        result = oPhd.FetchRowData(unitConfig.PreviousShiftTag);
                        row = result.Tables[0].Rows[0];
                        var beginValue = Convert.ToInt64(row["Value"]);
                        beginConfidence = Convert.ToInt32(row["Confidence"]);

                        this.data.UnitsData.Add(
                            new UnitsData
                            {
                                UnitConfigId = unitConfig.Id,
                                Value = (endValue - beginValue)/1000m,
                                ShiftId = shift,
                                RecordTimestamp = recordDataTime,
                                Confidence = (beginConfidence+endConfidence)/2
                            });
                    }
                }
            }
        }

        private void ProcessAutomaticCalulatedUnits(List<UnitConfig> unitsConfigsList, List<UnitsData> unitsData, PHDHistorian oPhd, DateTime recordDataTime, ShiftType shift)
        {
            foreach (var unitConfig in unitsConfigsList.Where(x => x.CollectingDataMechanism == "AC")) 
            {
                //var confidence = 0;
                if (!unitsData.Where(x => x.UnitConfigId == unitConfig.Id).Any())
                {
                    var shiftData = this.data.Shifts.GetById((int)shift);
                    if (shiftData != null)
                    {
                        var tags = unitConfig.PreviousShiftTag.Split('@');

                        var end = recordDataTime.AddTicks(shiftData.EndTicks);
                        var begin = end.AddHours(-8);

                        var endTimestamp = DateTime.Now - end;
                        var beginTimestamp = DateTime.Now - begin;

                        var endPhdTimestamp = string.Format("NOW-{0}H{1}M", Math.Truncate(endTimestamp.TotalHours), endTimestamp.Minutes);
                        var beginPhdTimestamp = string.Format("NOW-{0}H{1}M", Math.Truncate(beginTimestamp.TotalHours), beginTimestamp.Minutes);

                        var beginConfidence = 100;
                        var endConfidence = 100;

                        oPhd.StartTime = endPhdTimestamp;
                        oPhd.EndTime = endPhdTimestamp;
                        var result = oPhd.FetchRowData(tags[0]);
                        var row = result.Tables[0].Rows[0];
                        var endValue = Convert.ToInt64(row["Value"]);
                        endConfidence = Convert.ToInt32(row["Confidence"]);

                        result = oPhd.FetchRowData(tags[1]);
                        row = result.Tables[0].Rows[0];
                        var pressure = Convert.ToDecimal(row["Value"]);

                        oPhd.StartTime = beginPhdTimestamp;
                        oPhd.EndTime = beginPhdTimestamp;
                        result = oPhd.FetchRowData(tags[0]);
                        row = result.Tables[0].Rows[0];
                        var beginValue = Convert.ToInt64(row["Value"]);
                        beginConfidence = Convert.ToInt32(row["Confidence"]);

                        this.data.UnitsData.Add(
                            new UnitsData
                            {
                                UnitConfigId = unitConfig.Id,
                                Value = ((endValue - beginValue)*pressure)/Convert.ToDecimal(tags[2]),
                                ShiftId = shift,
                                RecordTimestamp = recordDataTime,
                                Confidence = (beginConfidence + endConfidence)/2
                            });
                    }
                }
            }
        }

        private static void SetPhdConnectionSettings(PHDHistorian oPhd, PHDServer defaultServer, double offsetInHours)
        {
            var exeFileName = Assembly.GetEntryAssembly().Location;
            Configuration configuration = ConfigurationManager.OpenExeConfiguration(exeFileName);
            ConfigurationSectionGroup appSettingsGroup = configuration.GetSectionGroup("applicationSettings");
            ConfigurationSection appSettingsSection = appSettingsGroup.Sections[0];
            ClientSettingsSection settings = appSettingsSection as ClientSettingsSection;

            defaultServer.Port = Convert.ToInt32(settings.Settings.Get("PHD_PORT").Value.ValueXml.InnerText);
            defaultServer.APIVersion = SERVERVERSION.RAPI200;
            oPhd.DefaultServer = defaultServer;
            oPhd.StartTime = string.Format("NOW - {0}H2M", offsetInHours);
            oPhd.EndTime = string.Format("NOW - {0}H2M", offsetInHours);
            oPhd.Sampletype = SAMPLETYPE.Snapshot;
            oPhd.MinimumConfidence = Convert.ToInt32(settings.Settings.Get("PHD_DATA_MIN_CONFIDENCE").Value.ValueXml.InnerText);
            oPhd.MaximumRows = Convert.ToUInt32(settings.Settings.Get("PHD_DATA_MAX_ROWS").Value.ValueXml.InnerText);
            //oPhd.Offset = Convert.ToInt32(settings.Settings.Get("PHD_OFFSET").Value.ValueXml.InnerText);
        }

        private void SetDefaultValue(DateTime recordDataTime, ShiftType shift, List<UnitsData> unitsData, UnitConfig unitConfig, int confidence)
        {
            var u = unitsData.Where(x => x.UnitConfigId == unitConfig.Id).FirstOrDefault();
            if (u == null)
            {
                this.data.UnitsData.Add(
                    new UnitsData
                    {
                        UnitConfigId = unitConfig.Id,
                        Value = null,
                        RecordTimestamp = recordDataTime,
                        ShiftId = shift,
                        Confidence = confidence
                    });
            }
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