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

        public IQueryable<UnitsData> GetUnitsDataForDateTime(DateTime? date, int? processUnitId, int? shiftId)
        {
            var dbResult = this.data.UnitsData
                .All()
                .Include(x => x.UnitConfig)
                .Include(x => x.UnitConfig.ProductType)
                .Include(x => x.UnitConfig.ProcessUnit)
                .Include(x => x.UnitConfig.MeasureUnit)
                .Include(x=>x.UnitsManualData)
                .Include(x => x.UnitsManualData.EditReason);
            if (date != null && shiftId != null)
            {
                var shiftData = this.data.ProductionShifts.All().Where(s => s.Id == shiftId).FirstOrDefault();

                if (shiftData != null)
                {
                    var beginTimestamp = date.Value.AddMinutes(shiftData.BeginMinutes);
                    var endTimestamp = beginTimestamp.AddMinutes(shiftData.OffsetMinutes);
                    dbResult = dbResult.Where(x => x.RecordTimestamp > beginTimestamp && x.RecordTimestamp <= endTimestamp);
                }
            }

            if (processUnitId != null)
            {
                dbResult = dbResult.Include(x => x.UnitConfig.ProcessUnit)
                    .Where(x => x.UnitConfig.ProcessUnitId == processUnitId);
            }

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
		        dbResult = dbResult.Include(u => u.UnitsDailyConfig.ProcessUnit)
                    .Where(u => u.UnitsDailyConfig.ProcessUnitId == processUnitId);
	        }

            return dbResult;
        }
    }
}
