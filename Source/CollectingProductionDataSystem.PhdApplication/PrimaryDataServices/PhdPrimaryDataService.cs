namespace CollectingProductionDataSystem.PhdApplication.PrimaryDataServices
{
    using System.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using Uniformance.PHD;
    using CollectingProductionDataSystem.PhdApplication.Contracts;
    using CollectingProductionDataSystem.Application.ProductionDataServices;

    public class PhdPrimaryDataService : IPrimaryDataService
    {
        private readonly IProductionData data;

        public PhdPrimaryDataService(IProductionData dataParam)
        {
            this.data = dataParam;
        }

        public int ReadAndSaveUnitsDataForShift(DateTime currentDateTime, int offsetInHours)
        {
            var totalInsertedRecords = 0;

            var now = currentDateTime;
            var record = DateTime.Now.AddHours(offsetInHours); 
            var shift = GetShift(record);
            var recordDataTime = GetRecordTimestamp(record);

            using (PHDHistorian oPhd = new PHDHistorian())
            {
                using (PHDServer defaultServer = new PHDServer("srv-vm-mes-phd"))
                {
                    var hours = Math.Truncate((now - record).TotalHours);
                    SetPhdConnectionSettings(oPhd, defaultServer, hours);

                    var unitsConfigsList = this.data.UnitConfigs.All().ToList();
                    var unitsData = this.data.UnitsData.All().Where(x => x.RecordTimestamp == recordDataTime && x.ShiftId == shift).ToList();

                    ProcessAutomaticUnits(unitsConfigsList, unitsData, oPhd, recordDataTime, shift);
                    totalInsertedRecords += this.data.SaveChanges("Phd2SqlLoader").ResultRecordsCount;

                    ProcessManualUnits(unitsConfigsList, recordDataTime, shift, unitsData);
                    totalInsertedRecords += this.data.SaveChanges("Phd2SqlLoader").ResultRecordsCount;

                    ProcessCalculatedUnits(unitsConfigsList, recordDataTime, shift, unitsData);
                    totalInsertedRecords += this.data.SaveChanges("Phd2SqlLoader").ResultRecordsCount;
                }
            }

            return totalInsertedRecords;
        }
 
        private void ProcessCalculatedUnits(List<UnitConfig> unitsConfigsList, DateTime recordDataTime, ShiftType shift, List<UnitsData> unitsData)
        {
            foreach (var unitConfig in unitsConfigsList.Where(x => x.CollectingDataMechanism == "C"))
            {
                try 
	            {	        
		            var formulaCode = unitConfig.CalculatedFormula ?? string.Empty;
                    var arguments = new FormulaArguments();
                    arguments.MaximumFlow = (double?)unitConfig.MaximumFlow;
                    arguments.EstimatedDensity = (double?)unitConfig.EstimatedDensity;
                    arguments.EstimatedPressure = (double?)unitConfig.EstimatedPressure;
                    arguments.EstimatedTemperature = (double?)unitConfig.EstimatedTemperature;
                    arguments.EstimatedCompressibilityFactor = (double?)unitConfig.EstimatedCompressibilityFactor;
                    arguments.CalculationPercentage = (double?)unitConfig.CalculationPercentage;

                    var ruc = unitConfig.RelatedUnitConfigs.ToList();
                    foreach (var ru in ruc)
                    {
                        var parameterType = ru.RelatedUnitConfig.AggregateGroup;
                        var inputValue = data.UnitsData
                                                .All()
                                                .Where(x => x.RecordTimestamp == recordDataTime)
                                                .Where(x => x.ShiftId == shift)
                                                .Where(x => x.UnitConfigId == ru.RelatedUnitConfigId)
                                                .FirstOrDefault()
                                                .RealValue;
                        if (parameterType == "I")
                        {
                            var exsistingValue = arguments.InputValue.HasValue ? arguments.InputValue.Value : 0.0;
                            arguments.InputValue = exsistingValue + inputValue; 
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
                    }

                    var result = new ProductionDataCalculatorService(this.data).Calculate(formulaCode, arguments);
                    if (!unitsData.Where(x => x.RecordTimestamp == recordDataTime && x.ShiftId == shift && x.UnitConfigId == unitConfig.Id).Any())
                    {
                        this.data.UnitsData.Add(
                            new UnitsData
                            {
                                UnitConfigId = unitConfig.Id,
                                RecordTimestamp = recordDataTime,
                                ShiftId = shift,
                                Value = (decimal)result
                            });
                    }
	            }
	            catch (Exception ex)
	            {
		            throw new Exception(string.Format("UnitConfigId: {0} [{1}]", unitConfig.Id, ex.Message), ex);
	            }
            }
        }

        private void ProcessManualUnits(List<UnitConfig> unitsConfigsList, DateTime recordDataTime, ShiftType shift, List<UnitsData> unitsData)
        {
            foreach (var unitConfig in unitsConfigsList.Where(x => x.CollectingDataMechanism == "M" || 
                x.CollectingDataMechanism == "MC" || 
                x.CollectingDataMechanism == "MD" || 
                x.CollectingDataMechanism == "MS"  ))
            {
                SetDefaultValue(recordDataTime, shift, unitsData, unitConfig);    
            }
        }
 
        private void ProcessAutomaticUnits(List<UnitConfig> unitsConfigsList, List<UnitsData> unitsData, PHDHistorian oPhd, DateTime recordDataTime, ShiftType shift)
        {
            foreach (var unitConfig in unitsConfigsList.Where(x => x.CollectingDataMechanism == "A"))
            {
                var confidence = 0;
                if (!unitsData.Where(x => x.UnitConfigId == unitConfig.Id).Any())
                {
                    var unitData = GetUnitData(unitConfig, oPhd, recordDataTime, out confidence);
                    if (confidence > Properties.Settings.Default.PHD_DATA_MIN_CONFIDENCE && unitData.RecordTimestamp != null)
                    {
                        if (!unitsData.Where(x => x.RecordTimestamp == unitData.RecordTimestamp && x.ShiftId == shift && x.UnitConfigId == unitConfig.Id).Any())
                        {
                            unitData.ShiftId = shift;
                            unitData.RecordTimestamp = unitData.RecordTimestamp.Date;
                            this.data.UnitsData.Add(unitData);
                        }
                    }
                    else
                    {
                        SetDefaultValue(recordDataTime, shift, unitsData, unitConfig);
                    }
                }
            }
        }

        private static void SetPhdConnectionSettings(PHDHistorian oPhd, PHDServer defaultServer, double offsetInHours)
        {
            defaultServer.Port = 3150;
            defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
            oPhd.DefaultServer = defaultServer;
            oPhd.StartTime = string.Format("NOW - {0}H2M", offsetInHours);
            oPhd.EndTime = string.Format("NOW - {0}H2M", offsetInHours);
            oPhd.Sampletype = SAMPLETYPE.Snapshot;
            oPhd.MinimumConfidence = 49;
            oPhd.MaximumRows = 1;
        }

        private void SetDefaultValue(DateTime recordDataTime, ShiftType shift, List<UnitsData> unitsData, UnitConfig unitConfig)
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
                        ShiftId = shift
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