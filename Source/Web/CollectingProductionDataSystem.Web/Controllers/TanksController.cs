using System.Data.Entity;
using AutoMapper;
using CollectingProductionDataSystem.Application.UnitsDataServices;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Inventories;
using CollectingProductionDataSystem.Web.ViewModels.Tank;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Resources = App_GlobalResources.Resources;

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
                new
                {
                    Id = a.Id,
                    Name = a.Name,
                    Minutes = a.BeginMinutes,
                    Offset = a.OffsetMinutes
                });
            return Json(shifts, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadTanksData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? parkId, int? shiftMinutesOffset)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.TanksDateSelector));
            }
            if (parkId == null)
            {
                this.ModelState.AddModelError("parks", string.Format(Resources.ErrorMessages.Required, Resources.Layout.TanksParkSelector));
            }

            if (this.ModelState.IsValid)
            {
                date = date.Value.AddMinutes(shiftMinutesOffset.Value);

                var dbResult = this.data.TanksData.All()
                    .Include(t => t.TankConfig)
                    .Include(t => t.TankConfig.Park)
                    .Where(t => t.RecordTimestamp == date)
                    .Where(t => t.ParkId == parkId);


                var kendoResult = dbResult.ToDataSourceResult(request, ModelState);
                kendoResult.Data = Mapper.Map<IEnumerable<TankData>, IEnumerable<TankDataViewModel>>((IEnumerable<TankData>)kendoResult.Data);
                return Json(kendoResult);
            }
            else
            {
                var kendoResult = new List<TankDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }
    }
}