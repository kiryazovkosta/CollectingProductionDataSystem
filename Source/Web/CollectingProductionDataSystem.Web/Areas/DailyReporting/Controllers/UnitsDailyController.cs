namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Transactions;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.CalculatorService;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Application.ProductionPlanDataServices;
    using CollectingProductionDataSystem.Application.UnitDailyDataServices;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Common;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Web.Infrastructure.Extentions;
    using CollectingProductionDataSystem.Web.Infrastructure.Filters;
    using CollectingProductionDataSystem.Web.InputModels;
    using CollectingProductionDataSystem.Web.ViewModels.Units;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Newtonsoft.Json;
    using Resources = App_GlobalResources.Resources;

    public class UnitsDailyController : AreaBaseController
    {
        private readonly IUnitsDataService unitsData;
        private readonly IUnitDailyDataService dailyService;
        private readonly IProductionPlanDataService productionPlanData;
        private readonly TransactionOptions transantionOption;
        private readonly ILogger logger;
        private readonly ITestUnitDailyCalculationService testUnitDailyCalculationService;
        private readonly IHistoricalService historicalService;

        public UnitsDailyController(IProductionData dataParam, IUnitsDataService unitsDataParam, IUnitDailyDataService dailyServiceParam,
            IProductionPlanDataService productionPlanDataParam, ILogger loggerParam, ITestUnitDailyCalculationService testUnitDailyCalculationServiceParam,
            IHistoricalService historicalServiceParam)
            : base(dataParam)
        {
            this.unitsData = unitsDataParam;
            this.dailyService = dailyServiceParam;
            this.productionPlanData = productionPlanDataParam;
            this.transantionOption = DefaultTransactionOptions.Instance.TransactionOptions;
            this.logger = loggerParam;
            this.testUnitDailyCalculationService = testUnitDailyCalculationServiceParam;
            this.historicalService = historicalServiceParam;
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
            if (!this.ModelState.IsValid)
            {
                var kendoResult = new List<UnitDailyDataViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }

            var status = CalculateDailyDataIfNotAvailable(date.Value, processUnitId.Value);

            if (!status.IsValid)
            {
                status.ToModelStateErrors(this.ModelState);
            }

            if (ModelState.IsValid)
            {
                var kendoResult = new DataSourceResult();
                if (ModelState.IsValid)
                {
                    var dbResult = unitsData.GetUnitsDailyDataForDateTime(date, processUnitId, CommonConstants.MaterialType).ToList();
                    this.historicalService.SetHistoricalProcessUnitParams(dbResult, date.Value);
                    var kendoPreparedResult = Mapper.Map<IEnumerable<UnitsDailyData>, IEnumerable<UnitDailyDataViewModel>>(dbResult);
                    kendoResult = kendoPreparedResult.ToDataSourceResult(request, ModelState);
                }

                Session["reportParams"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(
                                                                   JsonConvert.SerializeObject(
                                                                       new ProcessUnitConfirmDailyInputModel()
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

        /// <summary>
        /// Calculates the daily data if not available.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="processUnitId">The process unit id.</param>
        private IEfStatus CalculateDailyDataIfNotAvailable(DateTime date, int processUnitId)
        {
            IEfStatus status = dailyService.CheckIfShiftsAreReady(date, processUnitId);

            if (status.IsValid 
                &&(!this.dailyService.CheckIfDayIsApproved(date, processUnitId))
                && (!this.dailyService.CheckExistsUnitDailyDatas(date, processUnitId,CommonConstants.MaterialType))
                && this.testUnitDailyCalculationService.TryBeginCalculation(new UnitDailyCalculationIndicator(date, processUnitId)))
            {
                Exception exc = null;
                try
                {
                    IEnumerable<UnitsDailyData> dailyResult = new List<UnitsDailyData>();

                    if (status.IsValid)
                    {
                        if (!Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ComplitionCheckDeactivared"]))
                        {
                            status = this.dailyService.CheckIfPreviousDaysAreReady(processUnitId, date, CommonConstants.MaterialType);
                        }

                        if (status.IsValid)
                        {
                            status = IsRelatedDataExists(date, processUnitId);

                            if (status.IsValid)
                            {
                                dailyResult = this.dailyService.CalculateDailyDataForProcessUnit(processUnitId, date, materialTypeId: CommonConstants.MaterialType);

                                if (dailyResult.Count() > 0)
                                {
                                    this.data.UnitsDailyDatas.BulkInsert(dailyResult, this.UserProfile.UserName);
                                    status = this.data.SaveChanges(this.UserProfile.UserName);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    exc = ex;
                }
                finally
                {
                    int ix = 0;
                    while (!(this.testUnitDailyCalculationService.EndCalculation(new UnitDailyCalculationIndicator(date, processUnitId)) || ix == 10))
                    {
                        ix++;
                    }

                    if (ix >= 10)
                    {
                        string message = string.Format("Cannot clear record for begun Process Unit Calculation For ProcessUnitId {0} and Date {1}", processUnitId, date);
                        exc = new InvalidOperationException();
                    }

                    if (exc != null)
                    {
                        throw exc;
                    }
                }
            }

            return status;
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
                try
                {
                    using (var transaction = new TransactionScope(TransactionScopeOption.Required, this.transantionOption))
                    {
                        var result = this.data.SaveChanges(UserProfile.UserName);
                        if (!result.IsValid)
                        {
                            foreach (ValidationResult error in result.EfErrors)
                            {
                                this.ModelState.AddModelError(error.MemberNames.ToList()[0], error.ErrorMessage);
                            }
                        }

                        var updatedRecords = this.dailyService.CalculateDailyDataForProcessUnit(model.UnitsDailyConfig.ProcessUnitId, model.RecordTimestamp, isRecalculate: true, editReasonId: model.UnitsManualDailyData.EditReason.Id, materialTypeId: CommonConstants.MaterialType);
                        var status = UpdateResultRecords(updatedRecords, model.UnitsManualDailyData.EditReason.Id);

                        if (!status.IsValid)
                        {
                            status.ToModelStateErrors(this.ModelState);
                        }
                        else
                        {
                            transaction.Complete();
                        }
                    }
                }
                catch (DbUpdateException)
                {
                    this.ModelState.AddModelError("ManualValue", "Записът не можа да бъде осъществен. Моля опитайте на ново!");
                }
            }

            return Json(new[] { model }.ToDataSourceResult(request, ModelState));
        }

        /// <summary>
        /// Updates the result records.
        /// </summary>
        /// <param name="updatedRecords">The updated records.</param>
        private IEfStatus UpdateResultRecords(IEnumerable<UnitsDailyData> updatedRecords, int editReasonId)
        {
            foreach (var record in updatedRecords)
            {
                var manualRecord = this.data.UnitsManualDailyDatas.GetById(record.Id);
                if (manualRecord != null)
                {
                    manualRecord.Value = (decimal)record.RealValue;
                    this.data.UnitsManualDailyDatas.Update(manualRecord);
                }
                else
                {
                    this.data.UnitsManualDailyDatas.Add(new UnitsManualDailyData { Id = record.Id, Value = (decimal)record.RealValue, EditReasonId = editReasonId });
                }
            }

            return this.data.SaveChanges(this.UserProfile.UserName);
        }

        private void UpdateRecord(UnitsManualDailyData existManualRecord, UnitDailyDataViewModel model)
        {
            existManualRecord.Value = model.UnitsManualDailyData.Value;
            existManualRecord.EditReasonId = model.UnitsManualDailyData.EditReason.Id;
            this.data.UnitsManualDailyDatas.Update(existManualRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsConfirmed([DataSourceRequest]DataSourceRequest request, DateTime? date, int? processUnitId)
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
            ValidateModelAgainstReportPatameters(this.ModelState, model, Session["reportParams"]);

            if (!Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["ComplitionCheckDeactivared"]))
            {
                var status = this.dailyService.CheckIfPreviousDaysAreReady(model.processUnitId, model.date, CommonConstants.MaterialType);
                if (!status.IsValid)
                {
                    status.ToModelStateErrors(this.ModelState);
                }
            }

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
                    
                    var productionPlansDatas = this.data.ProductionPlanDatas
                        .All()
                        .Where(x => x.RecordTimestamp == model.date && x.ProcessUnitId == model.processUnitId)
                        .ToList();
                    if (productionPlansDatas.Count > 0)
                    {
                        foreach (var productionPlansData in productionPlansDatas)
                        {
                            this.data.ProductionPlanDatas.Delete(productionPlansData);
                        }
                    }
                    IEnumerable<ProductionPlanData> dbResult = this.productionPlanData.ReadProductionPlanData(model.date, model.processUnitId, 1);
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

        [HttpGet]
        public ActionResult DailyMaterialChart(int processUnitId, DateTime? date)
        {
            const int material = 1;
            date = date ?? DateTime.Now.Date.AddDays(-2);
            var result = this.dailyService.GetStatisticForProcessUnit(processUnitId, date.Value, material);
            result.Title = string.Format(Resources.Layout.DailyGraphicTitle, this.data.ProcessUnits.GetById(processUnitId).ShortName);
            return PartialView("DailyChart",result);
        }

        /// <summary>
        /// Validates the model against report patameters.
        /// </summary>
        /// <param name="modelState">State of the model.</param>
        /// <param name="model">The model.</param>
        /// <param name="session">The session.</param>
        private void ValidateModelAgainstReportPatameters(ModelStateDictionary modelState, ProcessUnitConfirmDailyInputModel model, object inReportParams)
        {
            var inParamsString = (inReportParams ?? string.Empty).ToString();

            if (string.IsNullOrEmpty(inParamsString))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = GetErrorListFromModelState(ModelState);
                modelState.AddModelError("", @Resources.ErrorMessages.InvalidReportParams);
                return;
            }

            var decodedParamsString = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(inParamsString));
            var reportParams = JsonConvert.DeserializeObject<ProcessUnitConfirmDailyInputModel>(decodedParamsString);

            if (!model.Equals(reportParams))
            {
                var resultMessage = new StringBuilder();
                resultMessage.AppendLine(@Resources.ErrorMessages.ParameterDifferencesHead);
                if (model.date != reportParams.date) { resultMessage.AppendLine(string.Format("\t\t -{0}", @Resources.Layout.Date)); }
                if (model.processUnitId != reportParams.processUnitId) { resultMessage.AppendLine(string.Format("\t\t -{0}", @Resources.Layout.ProcessUnit)); }
                if (model.IsConfirmed != reportParams.IsConfirmed) { resultMessage.AppendLine(string.Format("\t\t -{0}", @Resources.Layout.IsConfirmed)); }
                resultMessage.AppendLine(@Resources.ErrorMessages.ParametersDifferencesTrail);

                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = GetErrorListFromModelState(ModelState);
                modelState.AddModelError("", resultMessage.ToString());
                return;
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

        private IEfStatus IsRelatedDataExists(DateTime date, int processUnitId)
        {
            var validationResult = new List<ValidationResult>();

            var relatedDailyDatasFromOtherProcessUnits = this.data.UnitsDailyConfigs
                                                             .All()
                                                             .Include(x => x.ProcessUnit)
                                                             .Include(x => x.RelatedUnitDailyConfigs)
                                                             .Where(x => x.ProcessUnitId == processUnitId && x.AggregationCurrentLevel == true)
                                                             .SelectMany(y => y.RelatedUnitDailyConfigs)
                                                             .Where(z => z.RelatedUnitsDailyConfig.ProcessUnitId != processUnitId)
                                                             .ToList();

            foreach (var item in relatedDailyDatasFromOtherProcessUnits)
            {
                var relatedData = this.data.UnitsDailyDatas.All()
                                            .Where(u => u.RecordTimestamp == date
                                                    && u.UnitsDailyConfigId == item.RelatedUnitsDailyConfigId)
                                            .Any();

                if (!relatedData)
                {
                    validationResult.Add(new ValidationResult(
                        string.Format("Не са налични дневни данни за позиция: {0}", item.UnitsDailyConfig.Name)));
                }
            }

            var status = DependencyResolver.Current.GetService<IEfStatus>();

            if (validationResult.Count() > 0)
            {
                status.SetErrors(validationResult);
            }

            return status;
        }
    }
}