using CollectingProductionDataSystem.Application.UnitsDataServices;
using CollectingProductionDataSystem.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Controllers
{
    public class TanksController : BaseController
    {
        public TanksController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        public ActionResult TanksData()
        {
            return View();
        }

        public JsonResult GetAreas()
        {
            var areas = this.data.Areas.All().Select(a => new { AreaId = a.Id, AreaName = a.Name });
            return Json(areas, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetParks(int? areaId, string parksFilter)
        {
            var parks = this.data.Parks.All().AsQueryable();

            if (areaId != null)
            {
                parks = parks.Where(p => p.AreaId == areaId);
            }

            if (!string.IsNullOrEmpty(parksFilter))
            {
                parks = parks.Where(p => p.Name.Contains(parksFilter));
            }

            return Json(parks.Select(p => new { ParkId = p.Id, ParkName = p.Name }), JsonRequestBehavior.AllowGet);
        }

    }
}