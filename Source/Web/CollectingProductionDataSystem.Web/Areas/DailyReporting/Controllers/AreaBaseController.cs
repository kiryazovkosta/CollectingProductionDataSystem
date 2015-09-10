using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Web.Controllers;

namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    [Authorize(Roles = "Administrator, DailyReporter")]
    public abstract class AreaBaseController : BaseController
    {
        public AreaBaseController(IProductionData dataParam)
            : base(dataParam)
        { }
    }
}