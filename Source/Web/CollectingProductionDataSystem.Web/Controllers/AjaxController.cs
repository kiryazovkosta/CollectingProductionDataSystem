using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Web.InputModels;

namespace CollectingProductionDataSystem.Web.Controllers
{
    [Authorize]
    public class AjaxController : Controller
    {
        private readonly IProductionData data;

        public AjaxController(IProductionData dataParam) 
        {
            this.data = dataParam;
        }

        public JsonResult GetReasons() 
        {
            var reasons = this.data.EditReasons.All().ToList();
            var reasonView = Mapper.Map<IEnumerable<EditReasonInputModel>>(reasons);
            return Json(reasonView, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFactories()
        {
            var factories = this.data.Factories.All().Select(f => new
            {
                Id = f.Id,
                Name = f.ShortName
            });
            return Json(factories, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProcessUnits(int? factoryId)
        {
            var processUnits = this.data.ProcessUnits.All();

            if (factoryId != null)
            {
                processUnits = processUnits.Where(p => p.FactoryId == factoryId);
            }

            return Json(processUnits.Select(p => new { Id = p.Id, Name = p.ShortName }), JsonRequestBehavior.AllowGet);
        }
    }
}