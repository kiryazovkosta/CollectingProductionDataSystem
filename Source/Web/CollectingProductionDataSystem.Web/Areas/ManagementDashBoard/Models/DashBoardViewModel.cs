using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollectingProductionDataSystem.Web.Areas.ManagementDashBoard.Models
{
    public class DashBoardViewModel
    {
        public AllProcessUnitProductionPlanViewModel ProcessUnitLoadStatistics { get; set; }
        public AllProcessUnitProductionPlanViewModel ProcessUnitStatistics { get; set; }
    }
}