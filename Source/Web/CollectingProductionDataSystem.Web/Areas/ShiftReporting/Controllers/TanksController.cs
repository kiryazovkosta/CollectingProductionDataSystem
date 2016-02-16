namespace CollectingProductionDataSystem.Web.Areas.ShiftReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Web.ViewModels.Tank;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Resources = App_GlobalResources.Resources;

    [Authorize]
    public class TanksController : AreaBaseController
    {
        private readonly IInventoryTanksService tanksService;

        public TanksController(IProductionData dataParam, IInventoryTanksService tanksParam)
            : base(dataParam)
        {
            this.tanksService = tanksParam;
        }

        [HttpGet]
        public ActionResult TanksData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadTanksData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? parkId, int? areaId)
        {
            ValidateInputModel(date, parkId);

            if (this.ModelState.IsValid)
            {
                var statuses = this.tanksService.ReadDataForDay(date.Value, areaId, parkId).ToList();

                var dbResult = this.data.TanksData.All()
                    .Include(t => t.TankConfig)
                    .Include(t => t.TankConfig.Park)
                    .Where(t => t.RecordTimestamp == date
                        && t.TankConfig.Park.AreaId == (areaId ?? t.TankConfig.Park.AreaId)
                        && t.ParkId == (parkId ?? t.ParkId));

                var kendoResult = dbResult.ToDataSourceResult(request, ModelState);
                kendoResult.Data = Mapper.Map<IEnumerable<TankData>, IEnumerable<TankDataViewModel>>((IEnumerable<TankData>)kendoResult.Data);

                foreach (var item in kendoResult.Data)
                {
                    var model = (TankDataViewModel)item;
                    var status = statuses.Where(x => x.Tank.Id == model.TankId).FirstOrDefault();
                    if (status != null && status.Quantity.TankStatus != null)
                    {
                        model.StatusOfTank = status.Quantity.TankStatus.Id.ToString();   
                    }
                }

                return Json(kendoResult);
            }
            else
            {
                var kendoResult = new List<TankDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }
 
        private void ValidateInputModel(DateTime? date, int? parkId)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.TanksDateSelector));
            }
            if (!User.IsInRole("Administrator") && !User.IsInRole("AllParksReporter"))
            {
                if (parkId == null)
                {
                    this.ModelState.AddModelError("parks", string.Format(Resources.ErrorMessages.Required, Resources.Layout.TanksParkSelector));
                }
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