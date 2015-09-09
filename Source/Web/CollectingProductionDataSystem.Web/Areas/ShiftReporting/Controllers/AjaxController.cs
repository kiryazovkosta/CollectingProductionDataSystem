using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data.Contracts;

namespace CollectingProductionDataSystem.Web.Areas.ShiftReporting.Controllers
{
    public class AjaxController : AreaBaseController
    {
        public AjaxController(IProductionData productionDataParam)
            :base(productionDataParam)
        { }
    }
}