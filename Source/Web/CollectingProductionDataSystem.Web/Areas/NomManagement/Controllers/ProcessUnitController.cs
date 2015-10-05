namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    ﻿using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
    using CollectingProductionDataSystem.Web.Infrastructure.Extentions;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using CollectingProductionDataSystem.Models.Productions;
    using Resources = App_GlobalResources.Resources;

    public class ProcessUnitController : GenericNomController<ProcessUnit, ProcessUnitViewModel>
    {
        public ProcessUnitController(IProductionData dataParam)
            : base(dataParam)
        {

        }
    }
}
