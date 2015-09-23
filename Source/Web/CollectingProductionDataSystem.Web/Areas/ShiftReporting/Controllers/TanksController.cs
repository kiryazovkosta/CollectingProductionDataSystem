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
    using System.Data.Entity.Infrastructure;
    using System.ComponentModel.DataAnnotations;

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

                [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([DataSourceRequest]
                                 DataSourceRequest request, TankDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newManualRecord = new TanksManualData
                {
                    Id = model.Id,
                    LiquidLevel = model.LiquidLevel.Value,
                    ProductLevel = model.ProductLevel.Value,
                    NetStandardVolume = model.NetStandardVolume.Value,
                    ReferenceDensity = model.ReferenceDensity.Value,
                    WeightInAir = model.WeightInAir.Value,
                    WeightInVacuum = model.WeightInVacuum.Value,
                    FreeWaterLevel = model.FreeWaterLevel.Value,
                    EditReasonId = model.TanksManualData.EditReason.Id
                };

                var existManualRecord = this.data.TanksManualData.All().FirstOrDefault(x => x.Id == newManualRecord.Id);
                if (existManualRecord == null)
                {
                    this.data.TanksManualData.Add(newManualRecord);
                }
                else
                {
                    UpdateRecord(existManualRecord, model);
                }
                try
                {
                    var result = this.data.SaveChanges(UserProfile.UserName);
                    if (!result.IsValid)
                    {
                        foreach (ValidationResult error in result.EfErrors)
                        {
                            this.ModelState.AddModelError(error.MemberNames.ToList()[0], error.ErrorMessage);
                        }
                    }
                }
                catch (DbUpdateException)
                {
                    this.ModelState.AddModelError("ManualValue", "Записът не можа да бъде осъществен. Моля опитайте на ново!");
                }
                finally
                {
                }
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        private void UpdateRecord(TanksManualData existManualRecord, TankDataViewModel model)
        {
            existManualRecord.LiquidLevel = model.LiquidLevel.Value;
            existManualRecord.ProductLevel = model.ProductLevel.Value;
            existManualRecord.NetStandardVolume = model.NetStandardVolume.Value;
            existManualRecord.ReferenceDensity = model.ReferenceDensity.Value;
            existManualRecord.WeightInAir = model.WeightInAir.Value;
            existManualRecord.WeightInVacuum = model.WeightInVacuum.Value;
            existManualRecord.FreeWaterLevel = model.FreeWaterLevel.Value;
            existManualRecord.EditReasonId = model.TanksManualData.EditReason.Id;
            this.data.TanksManualData.Update(existManualRecord);
        }
    }
}