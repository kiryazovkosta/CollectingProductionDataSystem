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

            var dbResult = this.data.UnitsDailyDatas.GetById(unitDailyDataId).UnitsDailyConfig.UnitConfigUnitDailyConfigs
                            .SelectMany(x => x.UnitConfig.UnitsDatas)
                            .Where(y => y.RecordTimestamp == unitDailyData.RecordTimestamp);

            return dbResult;
        }

        public IQueryable<UnitsData> GetUnitsDataForDateTime(DateTime? dateParam, int? processUnitIdParam, int? shiftIdParam)
        {
            var dbResult = GetAllUnitDataIncludeRelations();

            if (dateParam != null)
            {
                dbResult = dbResult.Where(u => u.RecordTimestamp == dateParam.Value);
            }

            if (shiftIdParam != null)
            {
                dbResult = dbResult.Where(u => u.ShiftId == (ShiftType)shiftIdParam.Value);
            }

            if (processUnitIdParam != null)
            {
                dbResult = dbResult.Where(x => x.UnitConfig.ProcessUnitId == processUnitIdParam.Value);
            }

            dbResult = dbResult.OrderBy(x => x.UnitConfig.Code);
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
                               .Include(x => x.UnitsManualData.EditReason)
                               .Include(x => x.UnitConfig.UnitConfigUnitDailyConfigs.Select(y=>y.UnitDailyConfig).Select(z=>z.UnitsDailyDatas));
            return dbResult;
        }

        public IQueryable<UnitsDailyData> GetUnitsDailyDataForDateTime(DateTime? date, int? processUnitId)
        {
            var dbResult = this.data.UnitsDailyDatas
                .All()
                .Include(u => u.UnitsDailyConfig)
                .Include(u => u.UnitsDailyConfig.MeasureUnit)
                .Include(u => u.UnitsDailyConfig.Product);

            if (date.HasValue)
            {
                dbResult = dbResult.Where(u => u.RecordTimestamp == date.Value);
            }

            if (processUnitId.HasValue)
            {
                dbResult = dbResult.Where(u => u.UnitsDailyConfig.ProcessUnitId == processUnitId);
            }

            dbResult = dbResult.OrderBy(x => x.UnitsDailyConfig.Code);
            return dbResult;
        }

        public bool IsShitApproved(DateTime date, int processUnitId, int shiftId ) 
        {
           return this.data.UnitsApprovedDatas
                    .All()
                    .Where(u => u.RecordDate == date &&
                        u.ProcessUnitId == processUnitId &&
                        u.ShiftId == shiftId).Any();
        }
    }
}
