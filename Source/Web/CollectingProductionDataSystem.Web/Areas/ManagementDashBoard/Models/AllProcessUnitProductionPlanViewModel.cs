using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Web.Areas.ManagementDashBoard.Models
{
    public class AllProcessUnitProductionPlanViewModel
    {
        public IEnumerable<Factory> Factories{ get; set; }

        public string ElementPrefix { get; set; }

        public string ElementRole { get; set; }
    }
}