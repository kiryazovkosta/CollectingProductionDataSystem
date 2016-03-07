

namespace CollectingProductionDataSystem.Web.Areas.SummaryReporting.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public class WeightInVacuumDto
    {
        public int Id { get; set; }

        public DateTime RecordTimestamp { get; set; }

        public Product Product { get; set; }

        public decimal WeightInVaccum { get; set; }
    }
}