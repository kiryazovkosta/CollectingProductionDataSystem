using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Inventories;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Web.InputModels;
using CollectingProductionDataSystem.Web.ViewModels.Tank;
using CollectingProductionDataSystem.Web.ViewModels.Nomenclatures;
using CollectingProductionDataSystem.Web.ViewModels.Units;

namespace CollectingProductionDataSystem.Web.Controllers
{
    [Authorize]
    public class AjaxController : BaseController
    {
        public AjaxController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        public JsonResult GetReasons()
        {
            var reasons = this.data.EditReasons.All().ToList();
            var reasonView = Mapper.Map<IEnumerable<EditReasonInputModel>>(reasons);
            return Json(reasonView, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAreas()
        {
            IEnumerable<Area> areas = new HashSet<Area>();
            if (UserProfile.UserRoles.FirstOrDefault(x => x.Name == "Administrator") != null)
            {
                areas = this.data.Areas.All().ToList();
            }
            else
            {
                areas = Mapper.Map<IEnumerable<Area>>(this.UserProfile.Parks.Select(x=>x.Area).Distinct());
            }
            var areaView = Mapper.Map<IEnumerable<AreaViewModel>>(areas);
            return Json(areaView, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetParks(int? areaId, string parksFilter)
        {
            IEnumerable<Park> parks = new HashSet<Park>();
            if (UserProfile.UserRoles.FirstOrDefault(x => x.Name == "Administrator") != null)
            {
                parks = this.data.Parks.All();
            }
            else
            {
                parks = Mapper.Map<IEnumerable<Park>>(this.UserProfile.Parks);
            }

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
            IEnumerable<Factory> factories = new HashSet<Factory>();
            if (UserProfile.UserRoles.FirstOrDefault(x => x.Name == "Administrator") != null)
            {
                factories = this.data.Factories.All().ToList();
            }
            else
            {
                factories = Mapper.Map<IEnumerable<Factory>>(this.UserProfile.ProcessUnits.Select(x => x.Factory).Distinct().ToList());
            }
            var factoryView = Mapper.Map<IEnumerable<FactoryViewModel>>(factories);
            return Json(factoryView, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProcessUnits(int? factoryId)
        {
            IEnumerable<ProcessUnit> processUnits = new HashSet<ProcessUnit>();
            if (UserProfile.UserRoles.FirstOrDefault(x => x.Name == "Administrator") != null)
            {
                processUnits = this.data.ProcessUnits.All();
            }
            else
            {
                processUnits = Mapper.Map<IEnumerable<ProcessUnit>>(this.UserProfile.ProcessUnits);
            }

            if (factoryId.HasValue)
            {
                processUnits = processUnits.Where(p => p.FactoryId == factoryId);
            }
            var processUnitView = Mapper.Map<IEnumerable<ProcessUnitViewModel>>(processUnits.ToList());
            return Json(processUnitView, JsonRequestBehavior.AllowGet);
        }
    }
}