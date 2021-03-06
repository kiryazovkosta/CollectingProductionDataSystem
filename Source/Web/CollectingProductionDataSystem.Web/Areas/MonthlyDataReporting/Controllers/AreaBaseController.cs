﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Web.Controllers;

namespace CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Controllers
{
    public abstract class AreaBaseController : BaseController
    {
        public AreaBaseController(IProductionData dataParam)
            : base(dataParam)
        { }
    }
}