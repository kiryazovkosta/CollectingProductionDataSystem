namespace CollectingProductionDataSystem.Application.UnitsDataServices
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;

    public class UnitsDataService : IUnitsDataService
    {
        private readonly IProductionData data;
        
        public UnitsDataService(IProductionData dataParam) 
        {
            this.data = dataParam;
        }

        public IQueryable<UnitsData> GetUnitsDataForDateTime(DateTime? dateParam, int? processUnitIdParam, int? shiftIdParam)
        {
            var dbResult = this.data.UnitsData
                .All()
                .Include(x => x.UnitConfig)
                .Include(x => x.UnitConfig.ProductType)
                .Include(x => x.UnitConfig.ProcessUnit)
                .Include(x => x.UnitConfig.MeasureUnit)
                .Include(x=>x.UnitsManualData)
                .Include(x => x.UnitsManualData.EditReason);

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

        public IQueryable<UnitsDailyData> GetUnitsDailyDataForDateTime(DateTime? date, int? processUnitId)
        {
            var dbResult = this.data.UnitsDailyDatas
                .All()
                .Include(u => u.UnitsDailyConfig)
                .Include(u => u.UnitsDailyConfig.MeasureUnit)
                .Include(u => u.UnitsDailyConfig.ProductType);

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
    }
}
