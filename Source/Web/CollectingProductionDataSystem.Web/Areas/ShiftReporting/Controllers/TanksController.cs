namespace CollectingProductionDataSystem.Web.Areas.ShiftReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Web.ViewModels.Tank;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Resources = App_GlobalResources.Resources;

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
        public ActionResult ReadTanksData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? parkId, long? shiftMinutesOffset)
        {
            ValidateInputModel(date, parkId, shiftMinutesOffset);

            if (this.ModelState.IsValid)
            {
                date = date.Value.AddTicks(shiftMinutesOffset.Value);

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
 
        private void ValidateInputModel(DateTime? date, int? parkId, long? shiftMinutesOffset)
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
        }

        private List<string> GetErrorListFromModelState(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            return errorList;
        }
    }
}