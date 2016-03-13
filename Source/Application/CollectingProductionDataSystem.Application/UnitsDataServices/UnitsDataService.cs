namespace CollectingProductionDataSystem.Application.UnitsDataServices
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Data.Common;

    public class UnitsDataService : IUnitsDataService
    {
        private readonly IProductionData data;

        public UnitsDataService(IProductionData dataParam)
        {
            this.data = dataParam;
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
                .Where(x=>
                (dateParam == null || x.RecordTimestamp == dateParam.Value)
                &&(processUnitIdParam == null || x.UnitConfig.ProcessUnitId == processUnitIdParam.Value)
                &&(shiftIdParam == null || x.ShiftId == shiftIdParam.Value)
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

        public bool IsShitApproved(DateTime date, int processUnitId, int shiftId)
        {
            return this.data.UnitsApprovedDatas
                     .All()
                     .Where(u => u.RecordDate == date &&
                         u.ProcessUnitId == processUnitId &&
                         u.ShiftId == shiftId).Any();
        }
    }
}
