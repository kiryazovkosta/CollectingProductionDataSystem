namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Areas.DailyReporting.ViewModels;
    using CollectingProductionDataSystem.Web.Infrastructure.Extentions;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using CollectingProductionDataSystem.Application.Contracts;
    using Resources = App_GlobalResources.Resources;
    using AutoMapper;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Web.ViewModels.Tank;
    using CollectingProductionDataSystem.Models.Inventories;

    [Authorize(Roles = "TanksStatusesReporter")]
    public class TanksStatusesController : AreaBaseController
    {
        private readonly IInventoryTanksService tanks;

        public TanksStatusesController(IProductionData dataParam, IInventoryTanksService tanksParam)
            : base(dataParam)
        {
            this.tanks = tanksParam;
        }

        [HttpGet]
        public ActionResult TanksStatusesData()
        {
            this.ViewData["tankStatuses"] = Mapper.Map<IEnumerable<TankStatusViewModel>>(this.data.TankStatuses.All()).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReadTanksStatusesData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? areaId, int? parkId)
        {
            ValidateInputModel(date);

            if (this.ModelState.IsValid)
            {
                var dbResult = this.tanks.ReadDataForDay(date.Value, areaId, parkId).ToList();
                var kendoResult = dbResult.ToDataSourceResult(request, ModelState, Mapper.Map<TanksStatusDataViewModel>);
                return Json(kendoResult);
            }
            else
            {
                var kendoResult = new List<TanksStatusDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, TanksStatusDataViewModel inputViewModel)
        {
            if (ModelState.IsValid)
            {
                var dbEntity = this.data.TankStatusDatas.All().Where(x => x.TankConfigId == inputViewModel.TankConfigId && x.RecordTimestamp == inputViewModel.RecordTimestamp).FirstOrDefault();

                if (dbEntity != null)
                {
                    Mapper.Map(inputViewModel, dbEntity);
                    this.data.TankStatusDatas.Update(dbEntity);
                    var result = this.data.SaveChanges(this.UserProfile.UserName);
                    if (!result.IsValid)
                    {
                        result.ToModelStateErrors(ModelState);
                    }
                }
                else
                {
                    var entity = Mapper.Map<TankStatusData>(inputViewModel);
                    this.data.TankStatusDatas.Add(entity);
                    var result = this.data.SaveChanges(this.UserProfile.UserName);
                    if (!result.IsValid)
                    {
                        result.ToModelStateErrors(ModelState);
                    }
                    else 
                    {
                        inputViewModel.Id = entity.TankConfigId;
                    }
                }
            }

            return Json(new[] { inputViewModel }.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Destroy([DataSourceRequest]DataSourceRequest request, TanksStatusDataViewModel inputViewModel)
        {
            if (ModelState.IsValid)
            {
                var dbEntity = this.data.TankStatusDatas.All().Where(x => x.TankConfigId == inputViewModel.TankConfigId && x.RecordTimestamp == inputViewModel.RecordTimestamp).FirstOrDefault();
                if (dbEntity == null)
                {
                    ModelState.AddModelError("", string.Format(Resources.ErrorMessages.InvalidRecordUpdate, inputViewModel.Id));
                }

                this.data.TankStatusDatas.Delete(dbEntity);
                var result = data.SaveChanges(this.UserProfile.UserName);
                if (!result.IsValid)
                {
                    result.ToModelStateErrors(ModelState);
                }
            }

            return Json(new[] { inputViewModel }.ToDataSourceResult(request, ModelState));
        }

        private void ValidateInputModel(DateTime? date)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
            }
        }
    }
}