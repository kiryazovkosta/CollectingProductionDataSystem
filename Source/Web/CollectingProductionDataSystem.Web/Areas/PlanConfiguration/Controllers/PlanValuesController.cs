using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;

namespace CollectingProductionDataSystem.Web.Areas.PlanConfiguration.Controllers
{
    public class PlanValuesController : AreaBaseController
    {
        public PlanValuesController(IProductionData dataParam)
            :base(dataParam)
            {
                
            }

        // GET: PlanConfiguration/PlanValues
        public ActionResult Index()
        {
            this.ViewData["planConfigs"] = Mapper.Map<IEnumerable<ProductionPlanConfigViewModel>>(this.data.ProductionPlanConfigs.All().ToList());
            return View();
        }


    }
}