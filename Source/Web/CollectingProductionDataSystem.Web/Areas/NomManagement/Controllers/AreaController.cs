using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Inventories;
using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    public class AreaController : GenericNomController<Area, AreaViewModel>
    {
        public AreaController(IProductionData dataParam)
            : base(dataParam)
        {
        }
    }
}