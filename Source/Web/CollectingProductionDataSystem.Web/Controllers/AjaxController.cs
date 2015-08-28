using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Web.InputModels;
using CollectingProductionDataSystem.Web.ViewModels.Tank;
using CollectingProductionDataSystem.Web.ViewModels.Nomenclatures;
using CollectingProductionDataSystem.Web.ViewModels.Units;

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

        public JsonResult GetAreas()
        {
            var areas = this.data.Areas.All().ToList();
            var areaView = Mapper.Map<IEnumerable<AreaViewModel>>(areas);
            return Json(areaView, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetParks(int? areaId, string parksFilter)
        {
            var parks = this.data.Parks.All();

            if (areaId != null)
            {
                parks = parks.Where(p => p.AreaId == areaId);
            }

            if (!string.IsNullOrEmpty(parksFilter))
            {
                parks = parks.Where(p => p.Name.Contains(parksFilter));
            }

            var parkView = Mapper.Map<IEnumerable<ParkViewModel>>(parks);
            return Json(parkView, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetShifts()
        {
            var shifts = this.data.ProductionShifts.All().ToList();
            var shiftView = Mapper.Map<IEnumerable<ProductionShiftViewModel>>(shifts); 
            return Json(shiftView, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetFactories()
        {
            var factories = this.data.Factories.All().ToList();
            var factoryView = Mapper.Map<IEnumerable<FactoryViewModel>>(factories);
            return Json(factoryView, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProcessUnits(int? factoryId)
        {
            var processUnits = this.data.ProcessUnits.All().ToList();
            var processUnitView = Mapper.Map<IEnumerable<ProcessUnitViewModel>>(processUnits);
            return Json(processUnitView, JsonRequestBehavior.AllowGet);
        }
    }
}