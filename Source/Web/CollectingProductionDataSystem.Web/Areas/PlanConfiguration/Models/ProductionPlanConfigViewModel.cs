using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Web.Areas.PlanConfiguration.Models
{
    public class ProductionPlanConfigViewModel : IMapFrom<ProductionPlanConfig>
    {
        public int Id { get; set; }
        public string Code { get; set; }

    }
}