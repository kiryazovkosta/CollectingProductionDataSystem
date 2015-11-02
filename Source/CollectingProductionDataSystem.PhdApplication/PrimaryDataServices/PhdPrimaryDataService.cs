namespace CollectingProductionDataSystem.PhdApplication.PrimaryDataServices
{
    using System.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using MathExpressions.Application;
    using Uniformance.PHD;
    using CollectingProductionDataSystem.PhdApplication.Contracts;

    public class PhdPrimaryDataService : IPrimaryDataService
    {
        private readonly IProductionData data;

        public PhdPrimaryDataService(IProductionData dataParam)
        {
            this.data = dataParam;
        }

        public int ReadAndSaveUnitsDataForShift()
        {
             using (PHDHistorian oPhd = new PHDHistorian())
            {
                using (PHDServer defaultServer = new PHDServer(Properties.Settings.Default.PHD_HOST))
                {
                    var totalInsertedRecords = 0;
                    SetPhdConnectionSettings(oPhd, defaultServer);

                    var now = DateTime.Now;
                    var today = DateTime.Today;

                    var shift = GetShift(now);
                    var recordDataTime = GetRecordTimestamp(now);

                    var unitsConfigsList = this.data.UnitConfigs.All().ToList();
                    var unitsData = this.data.UnitsData.All().Where(x => x.RecordTimestamp == recordDataTime && x.ShiftId == shift).ToList();

                    foreach (var unitConfig in unitsConfigsList.Where(x=>x.CollectingDataMechanism == "A"))
                    {
                        var confidence = 0;
                        if (!unitsData.Where(x => x.UnitConfigId == unitConfig.Id).Any())
                        {

                            var unitData = GetUnitData(unitConfig, oPhd, today, out confidence);
                            if (confidence > Properties.Settings.Default.PHD_DATA_MIN_CONFIDENCE && unitData.RecordTimestamp != null)
                            {
                                if (now.Hour < 13)
                                {
                                    var prevDay = unitData.RecordTimestamp.AddDays(-1).Date;
                                    unitData.RecordTimestamp = prevDay;
                                }

                                if (!unitsData.Where(x => x.RecordTimestamp == unitData.RecordTimestamp && x.ShiftId == shift).Any())
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
                    totalInsertedRecords += this.data.SaveChanges("Phd2SqlLoader").ResultRecordsCount;

                    foreach (var unitConfig in unitsConfigsList.Where(x=>x.CollectingDataMechanism == "M"))
                    {
                        SetDefaultValue(recordDataTime, shift, unitsData, unitConfig);    
                    }
                    totalInsertedRecords += this.data.SaveChanges("Phd2SqlLoader").ResultRecordsCount;

                    foreach (var unitConfig in unitsConfigsList.Where(x=>x.CollectingDataMechanism == "C"))
                    {
                        var formulaCode = unitConfig.CalculatedFormula ?? string.Empty;
                        var arguments = new FormulaArguments();
                        arguments.MaximumFlow = (double?) unitConfig.MaximumFlow;
                        arguments.EstimatedDensity = (double?) unitConfig.EstimatedDensity;
                        arguments.EstimatedPressure = (double?) unitConfig.EstimatedPressure;
                        arguments.EstimatedTemperature = (double?) unitConfig.EstimatedTemperature;
                        arguments.EstimatedCompressibilityFactor = (double?) unitConfig.EstimatedCompressibilityFactor;

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
                                arguments.InputValue = inputValue;   
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

                        var result = ProductionDataCalculator.Calculate(formulaCode, arguments);
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
                    totalInsertedRecords += this.data.SaveChanges("Phd2SqlLoader").ResultRecordsCount;
                    return totalInsertedRecords;
                }
            }
            
        }

        private static void SetPhdConnectionSettings(PHDHistorian oPhd, PHDServer defaultServer)
        {
            defaultServer.Port = Properties.Settings.Default.PHD_PORT;
            defaultServer.APIVersion = Properties.Settings.Default.PHD_API_VERSION;
            oPhd.DefaultServer = defaultServer;
            oPhd.StartTime = "NOW - 2M";
            oPhd.EndTime = "NOW - 2M";
            oPhd.Sampletype = Properties.Settings.Default.PHD_SAMPLETYPE;
            oPhd.MinimumConfidence = Properties.Settings.Default.PHD_DATA_MIN_CONFIDENCE;
            oPhd.MaximumRows = Properties.Settings.Default.PHD_DATA_MAX_ROWS;
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