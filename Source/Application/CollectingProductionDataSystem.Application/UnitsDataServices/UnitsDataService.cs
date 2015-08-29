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
                .Include(x => x.Unit)
                .Include(x => x.Unit.ProductType)
                .Include(x => x.Unit.ProcessUnit)
                .Include(x => x.Unit.MeasureUnit)
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
                dbResult = dbResult.Include(x => x.Unit.ProcessUnit)
                    .Where(x => x.Unit.ProcessUnitId == processUnitId);
            }

            return dbResult;
        }
    }
}
