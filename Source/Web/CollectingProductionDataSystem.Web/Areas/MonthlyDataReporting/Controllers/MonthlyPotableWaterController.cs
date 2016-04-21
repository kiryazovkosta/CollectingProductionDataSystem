namespace CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Net;
    using System.Security.Principal;
    using System.Text;
    using System.Transactions;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Models;
    using CollectingProductionDataSystem.Web.Infrastructure.Extentions;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Newtonsoft.Json;
    using Resources = App_GlobalResources.Resources;
    using CollectingProductionDataSystem.Data.Common;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.Infrastructure;
    using CollectingProductionDataSystem.Web.Infrastructure.Filters;

    [Authorize(Roles = "Administrator, MonthlyPotableWaterReporter, SummaryReporter")]
    public class MonthlyPotableWaterController : AreaBaseController
    {
        private readonly IUnitMothlyDataService monthlyService;
        private readonly TransactionOptions transantionOption;  

        public MonthlyPotableWaterController(IProductionData dataParam, IUnitMothlyDataService monthlyServiceParam)
            : base(dataParam)
        {
            this.monthlyService = monthlyServiceParam;
            this.transantionOption = DefaultTransactionOptions.Instance.TransactionOptions;
        }

        [HttpGet]
        public ActionResult MonthlyPotableWaterData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReadMonthlyPotableWaterData([DataSourceRequest]DataSourceRequest request, DateTime date)
        {


            if (!this.ModelState.IsValid)
            {
                var kendoResult = new List<MonthlyReportTableViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }

            IEfStatus status = this.monthlyService.CalculateMonthlyDataIfNotAvailable(date, CommonConstants.PotableWater, this.UserProfile.UserName);

            if (!status.IsValid)
            {
                status.ToModelStateErrors(this.ModelState);
            }

            if (ModelState.IsValid)
            {
                var kendoResult = new DataSourceResult();
                if (ModelState.IsValid)
                {
                    var dbResult = this.monthlyService.GetDataForMonth(date, CommonConstants.PotableWater).OrderBy(x => x.UnitMonthlyConfig.Code).ToList();
                    var vmResult = Mapper.Map<IEnumerable<MonthlyReportTableViewModel>>(dbResult);
                    kendoResult = vmResult.ToDataSourceResult(request, ModelState);
                }
                Session["reportParams"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(
                                                                   JsonConvert.SerializeObject(
                                                                       new ConfirmMonthlyInputModel()
                                                                       {
                                                                           date = date,
                                                                           monthlyReportTypeId = CommonConstants.PotableWater,
                                                                       }
                                                                   )
                                                               )
                                                           );
                return Json(kendoResult);
            }
            else
            {
                var kendoResult = new List<MonthlyReportTableViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }

        [HttpGet]
        [SummaryReportAllowedFilter]
        public ActionResult MonthlyPotableWaterReport(DateTime? reportDate,  bool? isReport)
        {
            return View(reportDate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SummaryReportAllowedFilter]
        [SummaryReportFilter]
        public JsonResult ReadMonthlyPotableWaterReport([DataSourceRequest]DataSourceRequest request, DateTime date, bool? isReport)
        {
            if (!this.ModelState.IsValid)
            {
                var kendoResult = new List<MonthlyReportTableViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }

            if (isReport ?? false)
            {
                if (!this.monthlyService.IsMonthlyReportConfirmed(date, CommonConstants.PotableWater))
                {
                    this.ModelState.AddModelError(string.Empty, string.Format(@Resources.ErrorMessages.MonthIsNotConfirmed, date.ToString("MMMM yyyy")));
                    var kendoResult = new List<MonthlyReportTableReportViewModel>().ToDataSourceResult(request, ModelState);
                    return Json(kendoResult);
                }
            }

            if (ModelState.IsValid)
            {
                var kendoResult = new DataSourceResult();
                var potableWaterData = this.monthlyService.GetDataForMonth(date, CommonConstants.PotableWater).ToList();
                var recalculatedPotableWater = potableWaterData.Where(x => x.UnitMonthlyConfig.IsTotalExternalOutputPosition == true).Sum(x => x.RealValue);
                var innerPotableWater = potableWaterData.Where(x => x.UnitMonthlyConfig.IsTotalInternalPosition == true && x.UnitMonthlyConfig.IsTotalPosition == true).Sum(x => x.RealValue);

                var dbResult = potableWaterData.OrderBy(x => x.UnitMonthlyConfig.Code).ToList();
                var vmResult = Mapper.Map<IEnumerable<MonthlyReportTableReportViewModel>>(dbResult);
                foreach (var item in vmResult)
                {
                    if (item.IsExternalOutputPosition == true || item.IsTotalInputPosition || item.IsTotalInternalPosition || item.IsTotalExternalOutputPosition)
                    {
                        item.RecalculationPercentage = 0;
                        item.TotalValue = item.RealValue + item.RecalculationPercentage;
                    }
                    else
                    {
                        if (item.RealValue == 0)
                        {
                            item.RecalculationPercentage = 0;
                            item.TotalValue = item.RealValue + item.RecalculationPercentage;
                        }
                        else
                        {
                            double percentages = item.RealValue / innerPotableWater;
                            double recalulated = (recalculatedPotableWater * percentages) / 100.0;
                            item.RecalculationPercentage = percentages;
                            item.TotalValue = recalulated + item.RealValue;
                        }
                    }
                }

                kendoResult = vmResult.ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
            else
            {
                var kendoResult = new List<MonthlyReportTableReportViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([DataSourceRequest]DataSourceRequest request, MonthlyReportTableViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newManualRecord = new UnitManualMonthlyData
                {
                    Id = model.Id,
                    Value = model.UnitManualMonthlyData.Value,
                };

                var existManualRecord = this.data.UnitManualMonthlyDatas.All().FirstOrDefault(x => x.Id == newManualRecord.Id);
                if (existManualRecord == null)
                {
                    this.data.UnitManualMonthlyDatas.Add(newManualRecord);
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

                        var updatedRecords = this.monthlyService.CalculateMonthlyDataForReportType(
                            inTargetMonth: model.RecordTimestamp,
                            isRecalculate: true,
                            reportTypeId: CommonConstants.PotableWater,
                            changedMonthlyConfigId: model.UnitMonthlyConfigId
                            );
                        var status = UpdateResultRecords(updatedRecords);

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
        private IEfStatus UpdateResultRecords(IEnumerable<UnitMonthlyData> updatedRecords)
        {
            foreach (var record in updatedRecords)
            {
                var manualRecord = this.data.UnitManualMonthlyDatas.GetById(record.Id);
                if (manualRecord != null)
                {
                    manualRecord.Value = (decimal)record.RealValue;
                    this.data.UnitManualMonthlyDatas.Update(manualRecord);
                }
                else
                {
                    this.data.UnitManualMonthlyDatas.Add(new UnitManualMonthlyData { Id = record.Id, Value = (decimal)record.RealValue });
                }
            }

            return this.data.SaveChanges(this.UserProfile.UserName);
        }

        private void UpdateRecord(UnitManualMonthlyData existManualRecord, MonthlyReportTableViewModel model)
        {
            existManualRecord.Value = model.UnitManualMonthlyData.Value;
            this.data.UnitManualMonthlyDatas.Update(existManualRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SummaryReportAllowedFilter]
        public ActionResult IsConfirmed(DateTime date, int monthlyReportTypeId)
        {
            if (this.ModelState.IsValid)
            {
                if (this.monthlyService.IsMonthlyReportConfirmed(date, CommonConstants.PotableWater) == false)
                {
                    if (this.monthlyService.GetDataForMonth(date, CommonConstants.PotableWater).Any())
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
        public ActionResult Confirm(ConfirmMonthlyInputModel model)
        {
            ValidateModelAgainstReportPatameters(this.ModelState, model, Session["reportParams"]);

            if (ModelState.IsValid)
            {
                var targetMonth = this.monthlyService.GetTargetMonth(model.date);
                var approvedMonth = this.data.UnitApprovedMonthlyDatas
                   .All()
                   .Where(u => u.RecordDate == targetMonth && u.MonthlyReportTypeId == model.monthlyReportTypeId)
                   .FirstOrDefault();
                if (approvedMonth == null)
                {
                    this.data.UnitApprovedMonthlyDatas.Add(
                        new UnitApprovedMonthlyData
                        {
                            RecordDate = targetMonth,
                            MonthlyReportTypeId = model.monthlyReportTypeId,
                            Approved = true
                        });

                    var result = this.data.SaveChanges(this.UserProfile.UserName);

                    return Json(new { IsConfirmed = result.IsValid }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("unitsapproveddata", "Месечните данни вече са потвърдени");
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Report(DateTime? date)
        {
            return RedirectToAction("MonthlyPotableWaterReport", new { reportDate = date, isReport = false });
        }

        /// <summary>
        /// Validates the model against report patameters.
        /// </summary>
        /// <param name="modelState">State of the model.</param>
        /// <param name="model">The model.</param>
        /// <param name="session">The session.</param>
        private void ValidateModelAgainstReportPatameters(ModelStateDictionary modelState, ConfirmMonthlyInputModel model, object inReportParams)
        {
            var inParamsString = (inReportParams ?? string.Empty).ToString();

            if (string.IsNullOrEmpty(inParamsString))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                modelState.AddModelError("", Resources.ErrorMessages.InvalidReportParams);
                return;
            }

            var decodedParamsString = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(inParamsString));
            var reportParams = JsonConvert.DeserializeObject<ConfirmMonthlyInputModel>(decodedParamsString);

            if (!model.Equals(reportParams))
            {
                var resultMessage = new StringBuilder();
                resultMessage.AppendLine(Resources.ErrorMessages.ParameterDifferencesHead);
                if (model.date != reportParams.date) { resultMessage.AppendLine(string.Format("\t\t -{0}", Resources.Layout.Date)); }
                if (model.monthlyReportTypeId != reportParams.monthlyReportTypeId) { resultMessage.AppendLine(string.Format("\t\t -{0}", Resources.Layout.MonthlyReportType)); }
                if (model.IsConfirmed != reportParams.IsConfirmed) { resultMessage.AppendLine(string.Format("\t\t -{0}", Resources.Layout.IsConfirmed)); }
                resultMessage.AppendLine(Resources.ErrorMessages.ParametersDifferencesTrail);

                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                modelState.AddModelError("", resultMessage.ToString());
                return;
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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var attributes = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(true);
            AuthorizeAttribute filter = new AuthorizeAttribute();
            foreach (var attribute in attributes)
            {
                if (attribute is AuthorizeAttribute)
                {
                    filter = attribute as AuthorizeAttribute;
                    break;
                }
            }
            var rolesAllowed = filter.Roles.Split(",".ToArray<char>(), StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < rolesAllowed.Count(); i++)
            {
                rolesAllowed[i] = rolesAllowed[i].Trim();
            }

            var user = filterContext.HttpContext.User;
            if (IsUserOnlySummaryReporter(rolesAllowed, user))
            {
                var actionAttribute = filterContext.ActionDescriptor.GetCustomAttributes(true).FirstOrDefault(x => x is SummaryReportAllowedFilterAttribute) as SummaryReportAllowedFilterAttribute;
                var strValue = (filterContext.HttpContext.Request.QueryString.Get("isReport") ?? string.Empty).Split(',')[0].Trim();
                var fromTempData = filterContext.Controller.TempData["isReport"] as bool? ?? false;
                bool valueOfIsReportParam = string.IsNullOrEmpty(strValue) ? false : Convert.ToBoolean(strValue);

                if ((actionAttribute == null) || ((valueOfIsReportParam || fromTempData) == false))
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
            base.OnActionExecuting(filterContext);
        }

        protected bool IsUserOnlySummaryReporter(string[] rolesAllowed, IPrincipal user)
        {
            var result = user.IsInRole("SummaryReporter");

            foreach (var roleName in rolesAllowed)
            {
                if (user.IsInRole(roleName) && roleName != "SummaryReporter")
                {
                    result = false;
                }
            }

            return result;
        }
    }
}