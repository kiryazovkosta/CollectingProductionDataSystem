namespace CollectingProductionDataSystem.Web.Areas.ShiftReporting.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Transactions;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Web.Areas.ShiftReporting.ViewModels;
    using CollectingProductionDataSystem.Web.Infrastructure.Filters;
    using CollectingProductionDataSystem.Web.InputModels;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using MathExpressions.Application;
    using Resources = App_GlobalResources.Resources;

    [Authorize]
    public class UnitsController : AreaBaseController
    {
        private readonly IUnitsDataService shiftServices;
        private readonly IUnitDailyDataService dailyServices;

        public UnitsController(IProductionData dataParam, IUnitsDataService shiftServicesParam, IUnitDailyDataService dailyServicesParam)
            : base(dataParam)
        {
            this.shiftServices = shiftServicesParam;
            this.dailyServices = dailyServicesParam;
        }

        [HttpGet]
        public ActionResult UnitsData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeFactory]
        public JsonResult ReadUnitsData([DataSourceRequest]
                                        DataSourceRequest request, DateTime? date, int? processUnitId, int? shiftId)
        {
            ValidateModelState(date, processUnitId, shiftId);
            var dbResult = this.shiftServices.GetUnitsDataForDateTime(date, processUnitId, shiftId);
            var kendoPreparedResult = Mapper.Map<IEnumerable<UnitsData>, IEnumerable<UnitDataViewModel>>(dbResult);
            var kendoResult = new DataSourceResult();
            try
            {
                kendoResult = kendoPreparedResult.ToDataSourceResult(request, ModelState);
            }
            catch (Exception ex1)
            {
                Debug.WriteLine(ex1.Message + "\n" + ex1.InnerException);
            }

            return Json(kendoResult);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([DataSourceRequest]
                                 DataSourceRequest request, UnitDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.UnitConfig.IsCalculated)
                {
                    model.UnitsManualData.Value = (decimal)CalculateValueByProductionDataFormula(model);
                }

                var newManualRecord = new UnitsManualData
                {
                    Id = model.Id,
                    Value = model.UnitsManualData.Value,
                    EditReasonId = model.UnitsManualData.EditReason.Id
                };
                var existManualRecord = this.data.UnitsManualData.All().FirstOrDefault(x => x.Id == newManualRecord.Id);
                if (existManualRecord == null)
                {
                    this.data.UnitsManualData.Add(newManualRecord);
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

        private double CalculateValueByProductionDataFormula(UnitDataViewModel model)
        {
            var formulaCode = model.UnitConfig.CalculatedFormula ?? string.Empty;
            var arguments = new FormulaArguments();
            arguments.InputValue = (double)model.UnitsManualData.Value;
            arguments.MaximumFlow = (double?)model.UnitConfig.MaximumFlow;
            arguments.EstimatedDensity = (double?)model.UnitConfig.EstimatedDensity;
            arguments.EstimatedPressure = (double?)model.UnitConfig.EstimatedPressure;
            arguments.EstimatedTemperature = (double?)model.UnitConfig.EstimatedTemperature;
            arguments.EstimatedCompressibilityFactor = (double?)model.UnitConfig.EstimatedCompressibilityFactor;
            // Todo: need to add arguments here. Need to create a good mechanism for parameters!
            return ProductionDataCalculator.Calculate(formulaCode, arguments);
        }

        private void UpdateRecord(UnitsManualData existManualRecord, UnitDataViewModel model)
        {
            existManualRecord.Value = model.UnitsManualData.Value;
            existManualRecord.EditReasonId = model.UnitsManualData.EditReason.Id;
            this.data.UnitsManualData.Update(existManualRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Confirm(ProcessUnitConfirmShiftInputModel model)
        {
            ValidateModelState(model.date, model.processUnitId, model.shiftId);

            if (this.ModelState.IsValid)
            {
                if (!await this.shiftServices.IsShitApproved(model.date, model.processUnitId, model.shiftId))
                {
                    this.data.UnitsApprovedDatas.Add(new UnitsApprovedData
                    {
                        RecordDate = model.date,
                        ProcessUnitId = model.processUnitId,
                        ShiftId = model.shiftId,
                        Approved = true
                    });

                    IEfStatus status;

                    IEnumerable<UnitsDailyData> dailyResult = new List<UnitsDailyData>();

                    using (TransactionScope transaction = new TransactionScope())
                    {
                        status = data.SaveChanges(this.UserProfile.UserName);

                        if (status.IsValid)
                        {
                            if (dailyServices.CheckIfAllShiftsAreReady(model.date, model.processUnitId))
                            {
                                if (!dailyServices.CheckIfDayIsApproved(model.date, model.processUnitId))
                                {
                                    status = dailyServices.ClearUnitDailyDatas(model.date, model.processUnitId, this.UserProfile.UserName);
                                    if (status.IsValid)
                                    {
                                        dailyResult = dailyServices.CalculateDailyDataForProcessUnit(model.processUnitId, model.date);
                                    }
                                }
                            }
                        }

                        if (dailyResult.Count() > 0)
                        {
                            this.data.UnitsDailyDatas.BulkInsert(dailyResult, this.UserProfile.UserName);
                            status = this.data.SaveChanges(this.UserProfile.UserName);
                        }

                        transaction.Complete();
                    }

                    return Json(new { IsConfirmed = status.IsValid });
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    ModelState.AddModelError("shiftdata", "Данните за смяната вече са потвърдени!!!");
                    var errors = GetErrorListFromModelState(ModelState);
                    return Json(new { data = new { errors = errors } });
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = GetErrorListFromModelState(ModelState);
                return Json(new { data = new { errors = errors } });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> IsConfirmed([DataSourceRequest]
                                        DataSourceRequest request, DateTime date, int processUnitId, int shiftId)
        {
            ValidateModelState(date, processUnitId, shiftId);

            if (this.ModelState.IsValid)
            {
                return Json(new { IsConfirmed = await shiftServices.IsShitApproved(date, processUnitId, shiftId)});
            }

            return Json(new { IsConfirmed = false });
        }

        private List<string> GetErrorListFromModelState(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            return errorList;
        }

        private void ValidateModelState(DateTime? date, int? processUnitId, int? shiftId)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
            }
            if (processUnitId == null)
            {
                this.ModelState.AddModelError("processunits", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsProcessUnitSelector));
            }
            if (shiftId == null)
            {
                this.ModelState.AddModelError("shifts", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsProcessUnitShiftSelector));
            }
        }
    }
}