namespace CollectingProductionDataSystem.Application.PrimaryDataServices
{
    using CollectingProductionDataSystem.Application.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Data.Common;

    public class PhdPrimaryDataService : IPrimaryDataService
    {
        private readonly IProductionData data;

        public PhdPrimaryDataService(IProductionData dataParam)
        {
            this.data = dataParam;
        }

        public IEfStatus ReadAndSaveUnitsDataForShift()
        {
            //var today = DateTime.Today;
            //var now = DateTime.Now;
            //var ts = now - today;

            //var shift = this.data.Shifts.All().Where(s => s.BeginTicks <= ts.Ticks && s.ReadOffsetTicks > ts.Ticks).FirstOrDefault();
            //if (shift != null)
            //{
            //    var recordDataTime = shift.BeginTicks > 0 ? today : today.AddDays(-1);

            //    var unitsConfigsList = this.data.UnitConfigs.All().ToList();
            //    var unitsData = this.data.UnitsData.All().Where(x => x.RecordTimestamp == recordDataTime && x.ShiftId == shift.Id).ToList();

            //    foreach (var unitConfig in unitsConfigsList)
            //    {
            //        if (unitConfig.CollectingDataMechanism.ToUpper() == "A")
            //        {
            //            var confidence = 0;
            //            var s = this.data.UnitsData.All().Where(x => x.UnitConfigId == unitConfig.Id).FirstOrDefault();
            //            if (s == null)
            //            {
            //                var unitData = GetUnitData(unitConfig, oPhd, out confidence);
            //                if (confidence > 50 && unitData.RecordTimestamp != null)
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
            //                        context.UnitsData.Add(unitData); 
            //                    }
            //                }
            //                else
            //                {
            //                    var u = unitsData.Where(x => x.UnitConfigId == unitConfig.Id).FirstOrDefault();
            //                    if (u == null)
            //                    {
            //                        this.data.UnitsData.Add(
            //                            new UnitsData
            //                            {
            //                                UnitConfigId = unitConfig.Id,
            //                                Value = null,
            //                                RecordTimestamp = recordDataTime,
            //                                ShiftId = (ShiftType)shift.Id
            //                            });
            //                    }
            //                }
            //            }
            //        }
            //        else 
            //        {
            //            SetDefaultValue(context, recordDataTime, shift, unitsData, unitConfig);
            //        }
            //    }
            //}

            var status = new EfStatus();
            //status = this.data.SaveChanges("Phd2SqlLoader");
            return status;
        }

        private void SetDefaultValue(DateTime recordDataTime, ShiftType shift, List<UnitsData> unitsData, UnitConfig unitConfig)
        {
            //var u = unitsData.Where(x => x.UnitConfigId == unitConfig.Id).FirstOrDefault();
            //if (u == null)
            //{
            //    context.UnitsData.Add(
            //        new UnitsData
            //        {
            //            UnitConfigId = unitConfig.Id,
            //            Value = null,
            //            RecordTimestamp = recordDataTime,
            //            ShiftId = shift
            //        });
            //}
        }

    }
}