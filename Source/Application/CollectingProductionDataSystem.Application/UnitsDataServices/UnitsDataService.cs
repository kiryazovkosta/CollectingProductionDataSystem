namespace CollectingProductionDataSystem.Application.UnitsDataServices
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Data.Common;

    public class UnitsDataService : IUnitsDataService
    {
        private readonly IProductionData data;
        private readonly IHistoricalService historicalService;

        public UnitsDataService(IProductionData dataParam, IHistoricalService historicalServiceParam)
        {
            this.data = dataParam;
            this.historicalService = historicalServiceParam;
        }

        /// <summary>
        /// Gets the units data for daily record.
        /// </summary>
        /// <param name="unitsData">The units data.</param>
        /// <returns></returns>
        public IEnumerable<UnitsData> GetUnitsDataForDailyRecord(int unitDailyDataId)
        {

            var unitDailyData = this.data.UnitsDailyDatas.GetById(unitDailyDataId);

            var dbResult = this.data.UnitsDailyDatas.All()
                            .Include(x => x.UnitsDailyConfig)
                            .Include(x => x.UnitsDailyConfig.UnitConfigUnitDailyConfigs.Select(y => y.UnitConfig).Select(z => z.UnitsDatas.Select(w => w.UnitsManualData).Select(r => r.EditReason)))
                            .Include(x => x.UnitsDailyConfig.UnitConfigUnitDailyConfigs.Select(y => y.UnitConfig).Select(z => z.ShiftProductType))
                            .Include(x => x.UnitsDailyConfig.UnitConfigUnitDailyConfigs.Select(y => y.UnitConfig).Select(z => z.MeasureUnit))
                            .Include(x => x.UnitsDailyConfig.UnitConfigUnitDailyConfigs.Select(y => y.UnitConfig).Select(z => z.ProcessUnit))
                            .FirstOrDefault(x => x.Id == unitDailyDataId)
                            .UnitsDailyConfig.UnitConfigUnitDailyConfigs
                            .SelectMany(x => x.UnitConfig.UnitsDatas)
                            .Where(y => y.RecordTimestamp == unitDailyData.RecordTimestamp);

            return dbResult;
        }

        /// <summary>
        /// Gets the units data for date time.
        /// </summary>
        /// <param name="dateParam">The date parameter.</param>
        /// <param name="processUnitIdParam">The process unit identifier parameter.</param>
        /// <param name="shiftIdParam">The shift identifier parameter.</param>
        /// <returns></returns>
        public IQueryable<UnitsData> GetUnitsDataForDateTime(DateTime? dateParam, int? processUnitIdParam, int? shiftIdParam)
        {
            var dbResult = GetAllUnitDataIncludeRelations()
                .Where(x =>
                (dateParam == null || x.RecordTimestamp == dateParam.Value)
                && (processUnitIdParam == null || x.UnitConfig.ProcessUnitId == processUnitIdParam.Value)
                && (shiftIdParam == null || x.ShiftId == shiftIdParam.Value)
                ).OrderBy(x => x.UnitConfig.Code);
            return dbResult;
        }

        private IQueryable<UnitsData> GetAllUnitDataIncludeRelations()
        {
            var dbResult = this.data.UnitsData
                               .All()
                               .Include(x => x.UnitConfig)
                               .Include(x => x.UnitConfig.Product)
                               .Include(x => x.UnitConfig.ProcessUnit)
                               .Include(x => x.UnitConfig.ProcessUnit.Factory)
                               .Include(x => x.UnitConfig.MeasureUnit)
                               .Include(x => x.UnitsManualData)
                               .Include(x => x.UnitsManualData.EditReason);
            //.Include(x => x.UnitConfig.UnitConfigUnitDailyConfigs.Select(y => y.UnitDailyConfig).Select(z => z.UnitsDailyDatas));
            return dbResult;
        }

        public IQueryable<UnitsDailyData> GetUnitsDailyDataForDateTime(DateTime? date, int? processUnitId, int? materialType)
        {
            var dbResult = this.data.UnitsDailyDatas
                .All()
                .Include(u => u.UnitsDailyConfig)
                .Include(u => u.UnitsDailyConfig.ProcessUnit)
                .Include(u => u.UnitsDailyConfig.DailyProductType)
                .Include(u => u.UnitsDailyConfig.MeasureUnit)
                .Include(u => u.UnitsDailyConfig.Product)
                .Include(u => u.UnitsManualDailyData);

            if (date.HasValue)
            {
                dbResult = dbResult.Where(u => u.RecordTimestamp == date.Value);
            }

            if (processUnitId.HasValue)
            {
                dbResult = dbResult.Where(u => u.UnitsDailyConfig.ProcessUnitId == processUnitId);
            }

            if (materialType.HasValue)
            {
                dbResult = dbResult.Where(u => u.UnitsDailyConfig.MaterialTypeId == materialType);
            }

            dbResult = dbResult.OrderBy(x => x.UnitsDailyConfig.Code);
            return dbResult;
        }

        public IEnumerable<UnitsDailyData> GetUnitsDailyApprovedDataForDateTime(DateTime? date, int? processUnitId, int? factoryId)
        {
            var result = new List<UnitsDailyData>();
            List<UnitsDailyData> unitsDailyDatas = this.data.UnitsDailyDatas.All()
                .Include(u => u.UnitsDailyConfig)
                .Include(u => u.UnitsDailyConfig.ProcessUnit)
                .Include(u => u.UnitsDailyConfig.DailyProductType)
                .Include(u => u.UnitsDailyConfig.MeasureUnit)
                .Include(u => u.UnitsDailyConfig.Product)
                .Include(u => u.UnitsManualDailyData)
                .Include(x => x.UnitsDailyConfig.ProcessUnit.Factory)
                .Where(x => x.RecordTimestamp == date)
                .ToList();

            List<UnitsApprovedDailyData> approvedDailyProcessUnits = this.data.UnitsApprovedDailyDatas.All().Where(d => d.RecordDate == date).ToList();

            IEnumerable<int> approvedDailyProcessUnitsIds = approvedDailyProcessUnits.Where(d => d.RecordDate == date).Select(x => x.ProcessUnitId);

            if (processUnitId.HasValue)
            {
                if (approvedDailyProcessUnitsIds.Contains(processUnitId.Value))
                {
                    IEnumerable<UnitsDailyData> subDbResult = unitsDailyDatas.Where(u => u.UnitsDailyConfig.ProcessUnitId == processUnitId);

                    UnitsApprovedDailyData dailyApprovedDataForProcessUnit = approvedDailyProcessUnits.Where(x => x.ProcessUnitId == processUnitId.Value).FirstOrDefault();
                    if (!dailyApprovedDataForProcessUnit.EnergyApproved)
                    {
                        subDbResult = subDbResult.Where(u => u.UnitsDailyConfig.MaterialTypeId != CommonConstants.EnergyType);
                    }

                    subDbResult = subDbResult.OrderBy(x => x.UnitsDailyConfig.Code);
                    result = subDbResult.ToList();
                }
            }
            else
            {
                var listOfProcessUnits = new List<int>();

                if (factoryId.HasValue)
                {

                   var processUnitIds = this.historicalService.GetActualFactories(date.Value, factoryId).SelectMany(x => x.ProcessUnits.Select(y => y.Id));
                   foreach (var selectedProcessUnitId in processUnitIds)
                    {
                        if (approvedDailyProcessUnitsIds.Contains(selectedProcessUnitId))
                        {
                            listOfProcessUnits.Add(selectedProcessUnitId);
                        }
                    }

                    foreach (var item in listOfProcessUnits)
                    {
                        IEnumerable<UnitsDailyData> subDbResult = unitsDailyDatas.Where(x => x.UnitsDailyConfig.ProcessUnitId == item);
                        UnitsApprovedDailyData dailyApprovedDataForProcessUnit = approvedDailyProcessUnits.Where(x => x.ProcessUnitId == item).FirstOrDefault();
                        if (!dailyApprovedDataForProcessUnit.EnergyApproved)
                        {
                            subDbResult = subDbResult.Where(u => u.UnitsDailyConfig.MaterialTypeId != CommonConstants.EnergyType);
                        }

                        result.AddRange(subDbResult.ToList());
                    }
                }
                else
                {
                    foreach (var processUnit in approvedDailyProcessUnitsIds)
                    {
                        listOfProcessUnits.Add(processUnit);
                    }

                    foreach (var item in listOfProcessUnits)
                    {
                        IEnumerable<UnitsDailyData> subDbResult = unitsDailyDatas.Where(x => x.UnitsDailyConfig.ProcessUnitId == item);
                        UnitsApprovedDailyData dailyApprovedDataForProcessUnit = approvedDailyProcessUnits.Where(x => x.ProcessUnitId == item).FirstOrDefault();
                        if (!dailyApprovedDataForProcessUnit.EnergyApproved)
                        {
                            subDbResult = subDbResult.Where(u => u.UnitsDailyConfig.MaterialTypeId != CommonConstants.EnergyType);
                        }

                        result.AddRange(subDbResult.ToList());
                    }
                }
            }

            this.historicalService.SetHistoricalProcessUnitParams(result, date.Value);
            return result;
        }

        //public IQueryable<UnitsDailyData> GetUnitsDailyApprovedDataForDateTimeIQueryable(DateTime? date, int? processUnitId, int? factoryId, int? materialType)
        //{
        //    List<UnitsApprovedDailyData> approvedDailyProcessUnits = this.data.UnitsApprovedDailyDatas.All().Where(d => d.RecordDate == date).ToList();

        //    IEnumerable<int> approvedDailyProcessUnitsIds = approvedDailyProcessUnits.Where(d => d.RecordDate == date).Select(x => x.ProcessUnitId);

        //    IQueryable<UnitsDailyData> dbResult = this.data.UnitsDailyDatas
        //        .All()
        //        .Include(u => u.UnitsDailyConfig)
        //        .Include(u => u.UnitsDailyConfig.ProcessUnit)
        //        .Include(u => u.UnitsDailyConfig.DailyProductType)
        //        .Include(u => u.UnitsDailyConfig.MeasureUnit)
        //        .Include(u => u.UnitsDailyConfig.Product)
        //        .Include(u => u.UnitsManualDailyData);

        //    if (date.HasValue)
        //    {
        //        dbResult = dbResult.Where(u => u.RecordTimestamp == date.Value);
        //    }

        //    if (processUnitId.HasValue && approvedDailyProcessUnitsIds.Contains(processUnitId.Value))
        //    {
        //        dbResult = dbResult.Where(u => u.UnitsDailyConfig.ProcessUnitId == processUnitId);

        //        UnitsApprovedDailyData dailyApprovedDataForProcessUnit = approvedDailyProcessUnits.Where(x => x.ProcessUnitId == processUnitId.Value).FirstOrDefault();
        //        if (!dailyApprovedDataForProcessUnit.EnergyApproved)
        //        {
        //            dbResult = dbResult.Where(u => u.UnitsDailyConfig.MaterialTypeId != CommonConstants.EnergyType);
        //        }
        //    }
        //    else
        //    {
        //        var listOfProcessUnits = new List<int>();

        //        if (factoryId.HasValue)
        //        {
        //            IQueryable<int> processUnits = data.ProcessUnits.All().Where(pu => pu.FactoryId == factoryId).Select(pu => pu.Id);
        //            foreach (var processUnit in processUnits)
        //            {
        //                if (approvedDailyProcessUnitsIds.Contains(processUnit))
        //                {
        //                    listOfProcessUnits.Add(processUnit);
        //                }
        //            }

        //            dbResult = dbResult.Where(x => listOfProcessUnits.Contains(x.UnitsDailyConfig.ProcessUnitId));
        //        }
        //        else
        //        {
        //            foreach (var processUnit in approvedDailyProcessUnitsIds)
        //            {
        //                listOfProcessUnits.Add(processUnit);
        //            }

        //            dbResult = dbResult.Where(x => listOfProcessUnits.Contains(x.UnitsDailyConfig.ProcessUnitId));
        //        }
        //    }

        //    if (materialType.HasValue)
        //    {
        //        dbResult = dbResult.Where(u => u.UnitsDailyConfig.MaterialTypeId == materialType);
        //    }

        //    dbResult = dbResult.OrderBy(x => x.UnitsDailyConfig.Code);
        //    return dbResult;
        //}

        private List<int> CheckStatusOfProcessUnitsDailyConfigs(DateTime? date)
        {
            var approvedDailyProcessUnitsIds = new List<int>();
            var approvedDailyProcessUnits = this.data.UnitsApprovedDailyDatas.All().Where(d => d.RecordDate == date).ToList();
            foreach (var item in approvedDailyProcessUnits)
            {
                bool addToCollection = true;
                bool exsistingMaterialRecords = this.data.UnitsDailyConfigs.All().Where(u => u.MaterialTypeId == CommonConstants.EnergyType && u.ProcessUnitId == item.ProcessUnitId).Any();
                if (exsistingMaterialRecords)
                {
                    if (item.EnergyApproved != true)
                    {
                        addToCollection = false;
                    }
                }

                if (addToCollection)
                {
                    approvedDailyProcessUnitsIds.Add(item.ProcessUnitId);
                }
            }

            return approvedDailyProcessUnitsIds;
        }

        public bool IsShitApproved(DateTime date, int processUnitId, int shiftId)
        {
            return this.data.UnitsApprovedDatas
                     .All()
                     .Where(u => u.RecordDate == date &&
                         u.ProcessUnitId == processUnitId &&
                         u.ShiftId == shiftId).Any();
        }

        /// <summary>
        /// Appends the total month quantity to daily records.
        /// </summary>
        /// <param name="resultDaily">The result daily.</param>
        /// <param name="processUnitId">The process unit id.</param>
        /// <param name="targetDay">The target day.</param>
        public Dictionary<string, double> GetTotalMonthQuantityToDayFromShiftData(DateTime targetDay, int processUnitId = 0)
        {
            var beginningOfMonth = new DateTime(targetDay.Year, targetDay.Month, 1);
            var endOfObservedPeriod = new DateTime(targetDay.Year, targetDay.Month, targetDay.Day);

            var totalMonthQuantities = data.UnitsData.All().Include(x => x.UnitConfig).Include(x => x.UnitsManualData)
               .Where(x => (processUnitId == 0 || x.UnitConfig.ProcessUnitId == processUnitId)
                        && x.UnitConfig.ShiftProductTypeId == CommonConstants.DailyInfoDailyInfoHydrocarbonsShiftTypeId // Тип на позициите за ежедневно сведение
                        && beginningOfMonth <= x.RecordTimestamp
                        && x.RecordTimestamp <= endOfObservedPeriod).ToList()
                       .GroupBy(x => x.UnitConfig.Code)
                       .Select(group => new { Code = group.Key, Value = group.Sum(x => x.RealValue) }).ToDictionary(x => x.Code, x => x.Value);

            return totalMonthQuantities;
        }

        public IEnumerable<MultiShift> GetConsolidatedShiftData(DateTime date, int? processUnitId, int? factoryId)
        {
            IEnumerable<UnitsData> dbResult = this.GetUnitsDataForDateTime(date, processUnitId, null)
                                                  .Where(x => x.UnitConfig.IsMemberOfShiftsReport).Include(x=>x.UnitConfig.ShiftProductType);
            if (processUnitId == null)
            {
                var processUnitIds = this.historicalService.GetActualFactories(date, factoryId).SelectMany(x => x.ProcessUnits.Select(y => y.Id));
                dbResult = dbResult.Where(x => processUnitIds.Contains(x.UnitConfig.ProcessUnit.Id))
                    .ToList();
            }

            this.historicalService.SetHistoricalProcessUnitParams(dbResult, date);
            //ToDo: On shifts changed to 2 must repair this code
            var result = dbResult.Select(x => new MultiShift
            {
                TimeStamp = x.RecordTimestamp,
                Factory = string.Format("{0:d2} {1}", x.UnitConfig.HistorycalProcessUnit.Factory.Id, x.UnitConfig.HistorycalProcessUnit.Factory.ShortName),
                ProcessUnit = string.Format("{0:d2} {1}", x.UnitConfig.HistorycalProcessUnit.Id, x.UnitConfig.HistorycalProcessUnit.ShortName),
                Code = x.UnitConfig.Code,
                Position = x.UnitConfig.Position,
                MeasureUnit = x.UnitConfig.MeasureUnit.Code,
                ShiftProductType = string.Format("{0:d2} {1}", x.UnitConfig.ShiftProductType.Id, x.UnitConfig.ShiftProductType.Name),
                UnitConfigId = x.UnitConfigId,
                UnitName = x.UnitConfig.Name,
                NotATotalizedPosition = x.UnitConfig.NotATotalizedPosition,
                Shift1 = dbResult.Where(y => y.RecordTimestamp == date && y.ShiftId == (int) ShiftType.First).FirstOrDefault(u => u.UnitConfigId == x.UnitConfigId),
                Shift2 = dbResult.Where(y => y.RecordTimestamp == date && y.ShiftId == (int) ShiftType.Second).FirstOrDefault(u => u.UnitConfigId == x.UnitConfigId),
                Shift3 = dbResult.Where(y => y.RecordTimestamp == date && y.ShiftId == (int) ShiftType.Third).FirstOrDefault(u => u.UnitConfigId == x.UnitConfigId),
            }).Distinct(new MultiShiftComparer()).ToList();

            return result;
        }
    }
}
