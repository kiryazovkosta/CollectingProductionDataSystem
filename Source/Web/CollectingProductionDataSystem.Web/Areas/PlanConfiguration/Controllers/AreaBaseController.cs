using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Web.Controllers;

namespace CollectingProductionDataSystem.Web.Areas.PlanConfiguration.Controllers
{
    [Authorize(Roles = "Administrator,ProductionPlanManager")]
    public class AreaBaseController : BaseController
    {
        public AreaBaseController(IProductionData dataParam)
            : base(dataParam)
        {

        }
    }
}