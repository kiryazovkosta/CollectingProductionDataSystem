namespace CollectingProductionDataSystem.Web.Areas.ShiftReporting.Controllers
{
    using System.Data.Entity;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Infrastructure.Filters;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Resources = App_GlobalResources.Resources;
    using CollectingProductionDataSystem.Web.ViewModels.Tank;
    using CollectingProductionDataSystem.Models.Inventories;
    using AutoMapper;

    [Authorize]
    public class TanksController : AreaBaseController
    {
        public TanksController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        [HttpGet]
        public ActionResult TanksData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Read([DataSourceRequest]DataSourceRequest request, DateTime? date, int? parkId, int? shiftMinutesOffset)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.TanksDateSelector));
            }
            if (parkId == null)
            {
                this.ModelState.AddModelError("parks", string.Format(Resources.ErrorMessages.Required, Resources.Layout.TanksParkSelector));
            }
            if (shiftMinutesOffset == null)
            {
                this.ModelState.AddModelError("shifts", string.Format(Resources.ErrorMessages.Required, Resources.Layout.TanksShiftMinutesOffsetSelector));
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