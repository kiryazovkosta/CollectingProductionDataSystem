namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics;
    using System.Text;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.CalculatorService;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Application.ProductionPlanDataServices;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Web.Infrastructure.Extentions;
    using CollectingProductionDataSystem.Web.ViewModels.Units;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using CollectingProductionDataSystem.Web.Infrastructure.Filters;
    using Newtonsoft.Json;
    using Resources = App_GlobalResources.Resources;
    using System.Net;
    using CollectingProductionDataSystem.Web.InputModels;

    public class UnitsDailyController : AreaBaseController
    {
        private readonly IUnitsDataService unitsData;
        private readonly IUnitDailyDataService dailyService;
        private readonly IProductionPlanDataService productionPlanData;

        public UnitsDailyController(IProductionData dataParam, IUnitsDataService unitsDataParam, IUnitDailyDataService dailyServiceParam,
            IProductionPlanDataService productionPlanDataParam)
            : base(dataParam)
        {
            this.unitsData = unitsDataParam;
            this.dailyService = dailyServiceParam;
            this.productionPlanData = productionPlanDataParam;
        }

        [HttpGet]
        public ActionResult UnitsDailyData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeFactory]
        public JsonResult ReadDailyUnitsData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? processUnitId)
        {
            ValidateModelState(date, processUnitId);
            if (ModelState.IsValid)
            {
                var kendoResult = new DataSourceResult();
                if (ModelState.IsValid)
                {
                    var dbResult = unitsData.GetUnitsDailyDataForDateTime(date, processUnitId);
                    var kendoPreparedResult = Mapper.Map<IEnumerable<UnitsDailyData>, IEnumerable<UnitDailyDataViewModel>>(dbResult);

                    try
                    {
                        if (date != null && processUnitId != null)
                        {
                            dailyService.CheckIfShiftsAreReady(date.Value, processUnitId.Value).ToModelStateErrors(this.ModelState);
                        }
     
                        kendoResult = kendoPreparedResult.ToDataSourceResult(request, ModelState);
                    }
                    catch (Exception ex1)
                    {
                        Debug.WriteLine(ex1.Message + "\n" + ex1.InnerException);
                    }
                }

                Session["reportParams"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(
                                                                   JsonConvert.SerializeObject(
                                                                       new ProcessUnitConfirmShiftInputModel()
                                                                       {
                                                                           date = date.Value,
                                                                           processUnitId = processUnitId.Value,
                                                                       }
                                                                   )
                                                               )
                                                           );

                return Json(kendoResult);
            }
            else
            {
                var kendoResult = new List<UnitDailyDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([DataSourceRequest]DataSourceRequest request, UnitDailyDataViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newManualRecord = new UnitsManualDailyData
                {
                    Id = model.Id,
                    Value = model.UnitsManualDailyData.Value,
                    EditReasonId = model.UnitsManualDailyData.EditReason.Id
                };

                var existManualRecord = this.data.UnitsManualDailyDatas.All().FirstOrDefault(x => x.Id == newManualRecord.Id);
                if (existManualRecord == null)
                {
                    this.data.UnitsManualDailyDatas.Add(newManualRecord);
                }
                else
                {
                    UpdateRecord(existManualRecord, model);
                }

                RecalculateData(model);

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

        private void RecalculateData(UnitDailyDataViewModel model)
        {
            // TODO: Need to get all related units data and recalculate only them

            var uc = this.data.UnitsDailyConfigs.All().Where(x => x.ProcessUnitId == model.UnitsDailyConfig.ProcessUnitId && x.AggregationCurrentLevel == true).ToList();
            var ud = this.data.UnitsDailyDatas.All().Where(x => x.RecordTimestamp == model.RecordTimestamp && x.UnitsDailyConfig.ProcessUnitId == model.UnitsDailyConfig.ProcessUnitId).ToList();
            var calculator = new CalculatorService();
            var splitter = new char[] { '@' };

            foreach (var c in uc)
            {
                var tokens = c.AggregationMembers.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                var inputParamsValues = new List<double>();
                foreach (var token in tokens)
                {
                    foreach (var d in ud)
                    {
                        if (d.UnitsDailyConfig.Code == token)
                        {
                            inputParamsValues.Add(d.RealValue);
                            continue;
                        }
                    }
                }

                var inputParams = new Dictionary<string, double>();
                for (int i = 0; i < inputParamsValues.Count(); i++)
                {
                    inputParams.Add(string.Format("p{0}", i), inputParamsValues[i]);
                }

                var value = calculator.Calculate(c.AggregationFormula, "p", inputParams.Count, inputParams);
                var id = ud.Where(x => x.UnitsDailyConfig.Code == c.Code).FirstOrDefault();
                if (id != null)
                {
                    var newNewManualRecord = new UnitsManualDailyData
                    {
                        Id = id.Id,
                        Value = (decimal)value,
                        EditReasonId = model.UnitsManualDailyData.EditReason.Id
                    };
                    var existNewManualRecord = this.data.UnitsManualDailyDatas.All().FirstOrDefault(x => x.Id == newNewManualRecord.Id);
                    if (existNewManualRecord == null)
                    {
                        existNewManualRecord.EditReasonId = 0;
                        this.data.UnitsManualDailyDatas.Add(newNewManualRecord);
                    }
                    else
                    {
                        existNewManualRecord.EditReasonId = model.UnitsManualDailyData.EditReason.Id;
                        existNewManualRecord.Value = newNewManualRecord.Value;
                        this.data.UnitsManualDailyDatas.Update(existNewManualRecord);
                    }
                }
            }
        }

        private void UpdateRecord(UnitsManualDailyData existManualRecord, UnitDailyDataViewModel model)
        {
            existManualRecord.Value = model.UnitsManualDailyData.Value;
            existManualRecord.EditReasonId = model.UnitsManualDailyData.EditReason.Id;
            this.data.UnitsManualDailyDatas.Update(existManualRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsConfirmed([DataSourceRequest]DataSourceRequest request, DateTime date, int processUnitId)
        {
            ValidateModelState(date, processUnitId);

            if (this.ModelState.IsValid)
            {
                var approvedShift = this.data.UnitsApprovedDailyDatas
                    .All()
                    .Where(u => u.RecordDate == date && u.ProcessUnitId == processUnitId)
                    .FirstOrDefault();
                if (approvedShift == null)
                {
                    if (this.data.UnitsDailyDatas.All().Where(x => x.RecordTimestamp == date && x.UnitsDailyConfig.ProcessUnitId == processUnitId).Any())
                    {
                        return Json(new { IsConfirmed = false });
                    }
                }
                return Json(new { IsConfirmed = true });
            }
            else
            {
                return Json(new { IsConfirmed = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ProcessUnitConfirmDailyInputModel model)
        {
            ValidateModelState(model.date, model.processUnitId);
            if (ModelState.IsValid)
            {
                var approvedShift = this.data.UnitsApprovedDailyDatas
                   .All()
                   .Where(u => u.RecordDate == model.date && u.ProcessUnitId == model.processUnitId)
                   .FirstOrDefault();
                if (approvedShift == null)
                {
                    this.data.UnitsApprovedDailyDatas.Add(
                        new UnitsApprovedDailyData
                        {
                            RecordDate = model.date,
                            ProcessUnitId = model.processUnitId,
                            Approved = true
                        });

                    // Get all process plan data and save it
                    IEnumerable<ProductionPlanData> dbResult = this.productionPlanData.ReadProductionPlanData(model.date, model.processUnitId);
                    if (dbResult.Count() > 0)
	                {
                        foreach (var item in dbResult)
	                    {
                            this.data.ProductionPlanDatas.Add(item);
	                    }
	                }

                    var result = this.data.SaveChanges(this.UserProfile.UserName);
                    return Json(new { IsConfirmed = result.IsValid }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("unitsapproveddata", "Дневните данни вече са потвърдени");
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
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

        private void ValidateModelState(DateTime? date, int? processUnitId)
        {
            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
            }
            if (processUnitId == null)
            {
                this.ModelState.AddModelError("processunits", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsProcessUnitSelector));
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