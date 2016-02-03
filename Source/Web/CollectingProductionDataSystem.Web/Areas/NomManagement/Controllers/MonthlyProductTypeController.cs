using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Productions.Mounthly;
using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    public class MonthlyProductTypeController : GenericNomController<MonthlyProductType, MonthlyProductTypeViewModel>
    {
        public MonthlyProductTypeController(IProductionData dataParam) 
        :base(dataParam)
        { }
    }
}