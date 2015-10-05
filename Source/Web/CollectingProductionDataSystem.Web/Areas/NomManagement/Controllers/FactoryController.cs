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


namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    public class FactoryController : GenericNomController<Factory, FactoryViewModel>
    {
        public FactoryController(IProductionData dataParam)
            : base(dataParam)
        {

        }
    }
}
