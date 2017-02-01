namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Transactions;
    using System.Web.Mvc;
    using AutoMapper;
    using Application.Contracts;
    using Application.ProductionPlanDataServices;
    using Application.UnitDailyDataServices;
    using App_Resources;
    using Constants;
    using Data.Common;
    using Data.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Contracts;
    using Models.Productions;
    using Infrastructure.Extentions;
    using Infrastructure.Filters;
    using InputModels;
    using Web.ViewModels.Units;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Newtonsoft.Json;
    using Resources = App_GlobalResources.Resources;
    using ErrorMessages = App_GlobalResources.Resources.ErrorMessages;

    public class EnergyDailyController : AreaBaseController
    {
        private readonly IUnitsDataService unitsData;
        private readonly IUnitDailyDataService dailyService;
        private readonly IProductionPlanDataService productionPlanData;
        private readonly TransactionOptions transantionOption;
        private readonly ITestUnitDailyCalculationService testUnitDailyCalculationService;

        public EnergyDailyController(IProductionData dataParam,
            IUnitsDataService unitsDataParam,
            IUnitDailyDataService dailyServiceParam,
            IProductionPlanDataService productionPlanDataParam,
            ILogger loggerParam,
            ITestUnitDailyCalculationService testUnitDailyCalculationServiceParam)
            : base(dataParam)
        {
            this.unitsData = unitsDataParam;
            this.dailyService = dailyServiceParam;
            this.productionPlanData = productionPlanDataParam;
            this.transantionOption = DefaultTransactionOptions.Instance.TransactionOptions;
            this.testUnitDailyCalculationService = testUnitDailyCalculationServiceParam;
        }

        [HttpGet]
        public ActionResult EnergyDailyData()
        {
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeFactory]
        public JsonResult ReadEnergyDailyData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? processUnitId, int? materialTypeId)
        {
            this.ValidateModelState(date, processUnitId);
            if (!this.ModelState.IsValid)
            {
                var kendoResult = new List<UnitDailyDataViewModel>().ToDataSourceResult(request, this.ModelState);
                return this.Json(kendoResult);
            }

            var status = this.CalculateDailyDataIfNotAvailable(date.Value, processUnitId.Value);

            if (!status.IsValid)
            {
                status.ToModelStateErrors(this.ModelState);
            }

            if (this.ModelState.IsValid)
            {
                this.ValidateProductionPlanRelatedData(processUnitId, materialTypeId, date);
            }

            if (this.ModelState.IsValid)
            {
                var kendoResult = new DataSourceResult();
                if (this.ModelState.IsValid)
                {
                    var dbResult = this.unitsData.GetUnitsDailyDataForDateTime(date, processUnitId, CommonConstants.EnergyType);
                    var kendoPreparedResult = Mapper.Map<IEnumerable<UnitsDailyData>, IEnumerable<UnitDailyDataViewModel>>(dbResult);
                    kendoResult = kendoPreparedResult.ToDataSourceResult(request, this.ModelState);
                }

                this.Session["reportParams"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(
                                                                   JsonConvert.SerializeObject(
                                                                       new ProcessUnitConfirmDailyInputModel()
                                                                       {
                                                                           date = date.Value,
                                                                           processUnitId = processUnitId.Value,
                                                                       }
                                                                   )
                                                               )
                                                           );

                return this.Json(kendoResult);
            }
            else
            {
                var kendoResult = new List<UnitDailyDataViewModel>().ToDataSourceResult(request, this.ModelState);
                return this.Json(kendoResult);
            }
        }
 
        private void ValidateProductionPlanRelatedData(int? processUnitId, int? materialTypeId, DateTime? date)
        {
            var splitPattern = new char[] { '@' };

            var productionPalnsData = this.data.ProductionPlanConfigs.All().Where(x => x.ProcessUnitId == processUnitId && x.MaterialTypeId == materialTypeId).ToList();
            var dailyPositionsCodeList = new List<string>();
            foreach (var item in productionPalnsData)
            {
                var list = item.QuantityPlanMembers.Split(splitPattern, StringSplitOptions.RemoveEmptyEntries );
                foreach (var subItem in list)
                {
                    if (!dailyPositionsCodeList.Contains(subItem))
                    {
                        dailyPositionsCodeList.Add(subItem);    
                    }
                }

                list = item.QuantityFactMembers.Split(splitPattern, StringSplitOptions.RemoveEmptyEntries );
                foreach (var subItem in list)
                {
                    if (!dailyPositionsCodeList.Contains(subItem))
                    {
                        dailyPositionsCodeList.Add(subItem);    
                    }
                }

                list = item.UsageRateMembers.Split(splitPattern, StringSplitOptions.RemoveEmptyEntries );
                foreach (var subItem in list)
                {
                    if (!dailyPositionsCodeList.Contains(subItem))
                    {
                        dailyPositionsCodeList.Add(subItem);    
                    }
                }
            }

            foreach (var item in dailyPositionsCodeList)
            {
                var unitDailyDataExists = this.data.UnitsDailyDatas.All()
                                              .Include(x => x.UnitsDailyConfig)
                                              .FirstOrDefault(x => x.RecordTimestamp == date && x.UnitsDailyConfig.Code == item);
                if (unitDailyDataExists == null)
                {
                    var unitConfig = this.data.UnitsDailyConfigs.All().Include(y => y.ProcessUnit).First(y => y.Code == item);
                    this.ModelState.AddModelError("",
                        string.Format(ErrorMessages.NoDataForEnergyDailyReport, unitConfig.ProcessUnit.ShortName, unitConfig.Code, unitConfig.Name));  
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([DataSourceRequest]DataSourceRequest request, UnitDailyDataViewModel model)
        {
            if (this.ModelState.IsValid)
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
                    this.UpdateRecord(existManualRecord, model);
                }
                try
                {
                    using (var transaction = new TransactionScope(TransactionScopeOption.Required, this.transantionOption))
                    {
                        var result = this.data.SaveChanges(this.UserProfile.UserName);
                        if (!result.IsValid)
                        {
                            foreach (ValidationResult error in result.EfErrors)
                            {
                                this.ModelState.AddModelError(error.MemberNames.ToList()[0], error.ErrorMessage);
                            }
                        }

                        var updatedRecords = this.dailyService.CalculateDailyDataForProcessUnit(model.UnitsDailyConfig.ProcessUnitId, model.RecordTimestamp, isRecalculate: true, editReasonId: model.UnitsManualDailyData.EditReason.Id, materialTypeId:CommonConstants.EnergyType);
                        var status = this.UpdateResultRecords(updatedRecords, model.UnitsManualDailyData.EditReason.Id);

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
                    this.ModelState.AddModelError("ManualValue", Resources.Layout.CannotMakeRecord);
                }
            }

            return this.Json(new[] { model }.ToDataSourceResult(request, this.ModelState));
        }

        /// <summary>
        /// Updates the result records.
        /// </summary>
        /// <param name="updatedRecords">The updated records.</param>
        /// <param name="editReasonId">The edit reason Id</param>
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

        /// <summary>
        /// Update a single record
        /// </summary>
        /// <param name="existManualRecord">UnitsManualDailyData record</param>
        /// <param name="model">input data</param>
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
            this.ValidateModelState(date, processUnitId);

            if (this.ModelState.IsValid)
            {
                var approvedShift = this.data.UnitsApprovedDailyDatas
                    .All()
                    .FirstOrDefault(u => u.RecordDate == date 
                                        && u.ProcessUnitId == processUnitId 
                                        && u.EnergyApproved == true);
                if (approvedShift == null)
                {
                    if (this.data.UnitsDailyDatas.All().Any(x => x.RecordTimestamp == date 
                                                              && x.UnitsDailyConfig.ProcessUnitId == processUnitId 
                                                              && x.UnitsDailyConfig.MaterialTypeId == 2))
                    {
                        var approvedShiftMaterial = this.data.UnitsApprovedDailyDatas
                            .All()
                            .FirstOrDefault(u => u.RecordDate == date 
                                              && u.ProcessUnitId == processUnitId 
                                              && u.EnergyApproved == false);
                        if (approvedShiftMaterial != null)
                        {
                            return this.Json(new { IsConfirmed = false });
                        }
                    }
                }

                return this.Json(new { IsConfirmed = true });
            }
            else
            {
                return this.Json(new { IsConfirmed = true });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ProcessUnitConfirmDailyInputModel model)
        {
            this.ValidateModelAgainstReportPatameters(this.ModelState, model, this.Session["reportParams"]);

            if (this.ModelState.IsValid)
            {
                var approvedDay = this.data.UnitsApprovedDailyDatas
                   .All()
                   .FirstOrDefault(u => u.RecordDate == model.date 
                                     && u.ProcessUnitId == model.processUnitId 
                                     && u.EnergyApproved == false);
                if (approvedDay != null)
                {
                    approvedDay.EnergyApproved = true;
                    this.data.UnitsApprovedDailyDatas.Update(approvedDay);

                    // Get all process plan data and save it
                    var dbResult = this.productionPlanData.ReadProductionPlanData(model.date, model.processUnitId, CommonConstants.EnergyType);
                    if (dbResult.Any())
                    {
                        this.data.ProductionPlanDatas.BulkInsert(dbResult, this.UserProfile.UserName);
                    }

                    var result = this.data.SaveChanges(this.UserProfile.UserName);
                    return this.Json(new { IsConfirmed = result.IsValid }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    this.ModelState.AddModelError("unitsapproveddata", "Дневните данни вече са потвърдени");
                    this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var errors = this.GetErrorListFromModelState(this.ModelState);
                    return this.Json(new { data = new { errors = errors } });
                }
            }
            else
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = this.GetErrorListFromModelState(this.ModelState);
                return this.Json(new { data = new { errors = errors } });
            }
        }

        [HttpGet]
        public ActionResult DailyEnergyChart(int processUnitId, DateTime? date)
        {
            const int energy = 2;
            date = date ?? DateTime.Now.Date.AddDays(-2);
            var result = this.dailyService.GetStatisticForProcessUnit(processUnitId, date.Value, energy);
            result.Title = string.Format(Resources.Layout.DailyGraphicTitle, this.data.ProcessUnits.GetById(processUnitId).ShortName);
            return this.PartialView("DailyChart", result);
        }

        /// <summary>
        /// Validates the model against report patameters.
        /// </summary>
        /// <param name="modelState">State of the model.</param>
        /// <param name="model">The model.</param>
        /// <param name="inReportParams">The session.</param>
        private void ValidateModelAgainstReportPatameters(ModelStateDictionary modelState, ProcessUnitConfirmDailyInputModel model, object inReportParams)
        {
            var inParamsString = (inReportParams ?? string.Empty).ToString();

            if (string.IsNullOrEmpty(inParamsString))
            {
                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = this.GetErrorListFromModelState(this.ModelState);
                modelState.AddModelError("", @Resources.ErrorMessages.InvalidReportParams);
                return;
            }

            var decodedParamsString = Encoding.UTF8.GetString(Convert.FromBase64String(inParamsString));
            var reportParams = JsonConvert.DeserializeObject<ProcessUnitConfirmDailyInputModel>(decodedParamsString);

            if (!model.Equals(reportParams))
            {
                var resultMessage = new StringBuilder();
                resultMessage.AppendLine(@Resources.ErrorMessages.ParameterDifferencesHead);
                if (model.date != reportParams.date) { resultMessage.AppendLine(string.Format("\t\t -{0}", @Resources.Layout.Date)); }
                if (model.processUnitId != reportParams.processUnitId) { resultMessage.AppendLine(string.Format("\t\t -{0}", @Resources.Layout.ProcessUnit)); }
                if (model.IsConfirmed != reportParams.IsConfirmed) { resultMessage.AppendLine(string.Format("\t\t -{0}", @Resources.Layout.IsConfirmed)); }
                resultMessage.AppendLine(@Resources.ErrorMessages.ParametersDifferencesTrail);

                this.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = this.GetErrorListFromModelState(this.ModelState);
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
                var relatedData = this.data.UnitsDailyDatas
                                            .All()
                                            .Any(u => u.RecordTimestamp == date
                                                    && u.UnitsDailyConfigId == item.RelatedUnitsDailyConfigId);

                if (!relatedData)
                {
                    validationResult.Add(new ValidationResult(
                        $"Не са налични дневни данни за позиция: {item.UnitsDailyConfig.Name}"));
                }
            }

            var status = DependencyResolver.Current.GetService<IEfStatus>();

            if (validationResult.Any())
            {
                status.SetErrors(validationResult);
            }

            return status;
        }

        /// <summary>
        /// Calculates the daily data if not available.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <param name="processUnitId">The process unit id.</param>
        private IEfStatus CalculateDailyDataIfNotAvailable(DateTime date, int processUnitId)
        {
            bool readyForCalculation;

            IEfStatus status = this.dailyService.CheckIfDayIsApprovedButEnergyNot(date, processUnitId, out readyForCalculation);

            if (status.IsValid
                && readyForCalculation
                && !this.dailyService.CheckExistsUnitDailyDatas(date, processUnitId, CommonConstants.EnergyType)
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
                            status = this.dailyService.CheckIfPreviousDaysAreReady(processUnitId, date, CommonConstants.EnergyType);
                        }

                        if (status.IsValid)
                        {
                            status = this.IsRelatedDataExists(date, processUnitId);

                            if (status.IsValid)
                            {
                                dailyResult = this.dailyService.CalculateDailyDataForProcessUnit(processUnitId, date, materialTypeId: CommonConstants.EnergyType);

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
                        string message =
                            $"Cannot clear record for begun Process Unit Calculation For ProcessUnitId {processUnitId} and Date {date}";
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
    }
}