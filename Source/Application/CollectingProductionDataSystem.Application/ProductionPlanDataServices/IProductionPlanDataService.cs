namespace CollectingProductionDataSystem.Application.ProductionPlanDataServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;

    public interface IProductionPlanDataService
    {
        IEnumerable<ProductionPlanData> ReadProductionPlanData(DateTime? date, int? processUnitId);
    }
}