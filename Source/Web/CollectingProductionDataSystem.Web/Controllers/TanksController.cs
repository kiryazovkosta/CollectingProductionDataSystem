using CollectingProductionDataSystem.Application.UnitsDataServices;
using CollectingProductionDataSystem.Data.Contracts;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Controllers
{
    [Authorize]
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

        public JsonResult GetShifts()
        {
            var shifts = this.data.ProductionShifts.All().Select(a => 
                new { 
                    Id = a.Id, 
                    Name = a.Name, 
                    Minutes = a.BeginMinutes, 
                    Offset = a.OffsetMinutes 
                });
            return Json(shifts, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReadTanksData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? parkId)
        {
            var tanksData = this.data.TanksData.All().Where(t => t.ParkId == parkId && t.RecordTimestamp == date);
            return Json(tanksData);
            //var kendoResult = new DataSourceResult();
            //try
            //{
            //    kendoResult = dbResult.ToDataSourceResult(request, ModelState);
            //    kendoResult.Data = Mapper.Map<IEnumerable<UnitsData>, IEnumerable<UnitDataViewModel>>((IEnumerable<UnitsData>)kendoResult.Data);
            //}
            //catch (ArgumentException ex)
            //{
            //    // Dirty hack
            //    kendoResult.Data = Mapper.Map<IEnumerable<UnitsData>, IEnumerable<UnitDataViewModel>>((IEnumerable<UnitsData>)dbResult);
            //    kendoResult = kendoResult.Data.ToDataSourceResult(request, ModelState);
            //}

            //return Json(kendoResult);
        }

    }
}