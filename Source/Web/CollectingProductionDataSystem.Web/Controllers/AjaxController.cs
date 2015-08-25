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
    }
}