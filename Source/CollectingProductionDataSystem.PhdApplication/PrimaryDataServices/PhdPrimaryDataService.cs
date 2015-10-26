namespace CollectingProductionDataSystem.PhdApplication.PrimaryDataServices
{
    using System.Data;
    using System.Globalization;
    using CollectingProductionDataSystem.Application.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Data.Common;
    using Uniformance.PHD;

    public class PhdPrimaryDataService : IPrimaryDataService
    {
        private readonly IProductionData data;

        public PhdPrimaryDataService(IProductionData dataParam)
        {
            this.data = dataParam;
        }

        public IEfStatus ReadAndSaveUnitsDataForShift()
        {
            // using (PHDHistorian oPhd = new PHDHistorian())
            //{
            //    using (PHDServer defaultServer = new PHDServer(Properties.Settings.Default.PHD_HOST))
            //    {
            //        SetPhdConnectionSettings(oPhd, defaultServer);

            //        var now = DateTime.Now;
            //        var today = DateTime.Today;

            //        var shift = GetShift(now);
            //        var recordDataTime = GetRecordTimestamp(now);
                    

            //        var unitsConfigsList = this.data.UnitConfigs.All().ToList();
            //        var unitsData = this.data.UnitsData.All().Where(x => x.RecordTimestamp == recordDataTime && x.ShiftId == shift).ToList();

            //        foreach (var unitConfig in unitsConfigsList)
            //        {
            //            if (unitConfig.CollectingDataMechanism.ToUpper() == "A")
            //            {
            //                var confidence = 0;
            //                var s = unitsData.Where(x => x.UnitConfigId == unitConfig.Id).FirstOrDefault();
            //                if (s != null)
            //                {
            //                    continue;
            //                }

            //                var unitData = GetUnitData(unitConfig, oPhd, today, out confidence);
            //                if (confidence > Properties.Settings.Default.PHD_DATA_MIN_CONFIDENCE && unitData.RecordTimestamp != null)
            //                {
            //                    if (now.Hour < 13)
            //                    {
            //                        var prevDay = unitData.RecordTimestamp.AddDays(-1).Date;
            //                        unitData.RecordTimestamp = prevDay;
            //                    }

            //                    if (unitsData.Where(x => x.RecordTimestamp == unitData.RecordTimestamp && x.ShiftId == shift).FirstOrDefault() == null)
            //                    {
            //                        unitData.ShiftId = shift;
            //                        unitData.RecordTimestamp = unitData.RecordTimestamp.Date;
            //                        this.data.UnitsData.Add(unitData);
            //                    }
            //                }
            //                else
            //                {
            //                    SetDefaultValue(recordDataTime, shift, unitsData, unitConfig);
            //                }
            //            }
            //            else
            //            {
            //                SetDefaultValue(recordDataTime, shift, unitsData, unitConfig);
            //            }
            //        }
            //    }
            //}

            var status = new EfStatus();
            ////status = this.data.SaveChanges("Phd2SqlLoader");
            return status;
        }

        private static void SetPhdConnectionSettings(PHDHistorian oPhd, PHDServer defaultServer)
        {
            //defaultServer.Port = Properties.Settings.Default.PHD_PORT;
            //defaultServer.APIVersion = Properties.Settings.Default.PHD_API_VERSION;
            //oPhd.DefaultServer = defaultServer;
            //oPhd.StartTime = "NOW - 2M";
            //oPhd.EndTime = "NOW - 2M";
            //oPhd.Sampletype = Properties.Settings.Default.PHD_SAMPLETYPE;
            //oPhd.MinimumConfidence = Properties.Settings.Default.PHD_DATA_MIN_CONFIDENCE;
            //oPhd.MaximumRows = Properties.Settings.Default.PHD_DATA_MAX_ROWS;
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
                        //if (!string.IsNullOrEmpty(row[dc].ToString()))
                        //{
                        //    var recordTimestamp = DateTime.ParseExact(row[dc].ToString(), "dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                        //    if (TimeZoneInfo.Local.IsDaylightSavingTime(recordTimestamp))
                        //    {
                        //        recordTimestamp = recordTimestamp.AddHours(-1);    
                        //    }

                            unitData.RecordTimestamp = timeStamp; 
                        //}
                    }
                }
            }

            return unitData;
        }

    }
}