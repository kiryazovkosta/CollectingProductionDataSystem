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

        public IQueryable<UnitsData> GetUnitsDataForDateTime(DateTime? date, int? processUnit, int? offset)
        {
            var dbResult = this.data.UnitsData
                .All()
                .Include(x => x.Unit)
                .Include(x => x.Unit.ProductType)
                .Include(x => x.Unit.ProcessUnit)
                .Include(x => x.Unit.MeasureUnit)
                .Include(x=>x.UnitsManualData)
                .Include(x => x.UnitsManualData.EditReason);
            if (date != null && offset != null)
            {
                var beginTimestamp = date.Value.AddMinutes(offset.Value);
                var endTimestamp = beginTimestamp.AddMinutes(120);
                dbResult = dbResult.Where(x => x.RecordTimestamp > beginTimestamp && x.RecordTimestamp <= endTimestamp);
            }
            if (processUnit != null)
            {
                dbResult = dbResult.Include(x => x.Unit.ProcessUnit)
                    .Where(x => x.Unit.ProcessUnitId == processUnit);
            }

            return dbResult;
        }
    }
}
