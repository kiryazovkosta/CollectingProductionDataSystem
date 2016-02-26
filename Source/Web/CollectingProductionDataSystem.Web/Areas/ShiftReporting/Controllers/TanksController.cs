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
                        && (areaId == null || t.TankConfig.Park.AreaId == areaId)
                        && (parkId == null || t.ParkId == parkId))
                    .ToList();

                //var kendoResult = dbResult.ToDataSourceResult(request, ModelState);
                //kendoResult.Data = Mapper.Map<IEnumerable<TankData>, IEnumerable<TankDataViewModel>>((IEnumerable<TankData>)kendoResult.Data);
                var vmResult = Mapper.Map<IEnumerable<TankDataViewModel>>(dbResult);


                foreach (var tank in vmResult)
                {
                    var status = statuses.Where(x => x.Tank.Id == tank.TankId).FirstOrDefault();
                    if (status != null && status.Quantity.TankStatus != null)
                    {
                        tank.StatusOfTank = status.Quantity.TankStatus.Id.ToString();
                    }
                }

                return Json(vmResult.ToDataSourceResult(request, this.ModelState));
            }
            else
            {
                var kendoResult = new List<TankDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }

        [HttpGet]
        public ActionResult TanksDataNew()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReadTanksDataNew([DataSourceRequest]DataSourceRequest request, DateTime? date, int? parkId, int? areaId)
        {
            ValidateInputModel(date, parkId);

            if (this.ModelState.IsValid)
            {
                var statuses = this.tanksService.ReadDataForDay(date.Value, areaId, parkId).ToList();

                var dbResult = this.data.TanksData.All()
                    .Include(t => t.TankConfig)
                    .Include(t => t.TankConfig.Park)
                    .Where(t => t.RecordTimestamp == date
                        && (areaId == null || t.TankConfig.Park.AreaId == areaId)
                        && (parkId == null || t.ParkId == parkId))
                    .ToList();

                //var kendoResult = dbResult.ToDataSourceResult(request, ModelState);
                //kendoResult.Data = Mapper.Map<IEnumerable<TankData>, IEnumerable<TankDataViewModel>>((IEnumerable<TankData>)kendoResult.Data);
                var vmResult = Mapper.Map<IEnumerable<TankDataViewModel>>(dbResult);


                foreach (var tank in vmResult)
                {
                    var status = statuses.Where(x => x.Tank.Id == tank.TankId).FirstOrDefault();
                    if (status != null && status.Quantity.TankStatus != null)
                    {
                        tank.StatusOfTank = status.Quantity.TankStatus.Id.ToString();
                    }
                }

                return Json(vmResult.OrderByDescending(x=>x.TankName[0]).ThenBy(x=>x.TankNumber).ToDataSourceResult(request, this.ModelState));
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