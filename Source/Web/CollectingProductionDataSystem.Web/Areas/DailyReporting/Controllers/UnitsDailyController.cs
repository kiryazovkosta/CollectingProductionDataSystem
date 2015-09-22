﻿namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.Infrastructure;
    using System.Diagnostics;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.UnitsDataServices;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Web.ViewModels.Units;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using CollectingProductionDataSystem.Web.Infrastructure.Filters;
    using Resources = App_GlobalResources.Resources;
    using System.Net;
    using MathExpressions.Application;

    [Authorize]
    public class UnitsDailyController : AreaBaseController
    {
        private readonly IUnitsDataService unitsData;

        public UnitsDailyController(IProductionData dataParam, IUnitsDataService unitsDataParam) : base(dataParam)
        {
            this.unitsData = unitsDataParam;
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
            var dbResult = unitsData.GetUnitsDailyDataForDateTime(date, processUnitId);
            var kendoPreparedResult = Mapper.Map<IEnumerable<UnitsDailyData>, IEnumerable<UnitDailyDataViewModel>>(dbResult);
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
        [AuthorizeFactory]
        public JsonResult ReadProductionPlanData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? processUnitId)
        {
            ValidateModelState(date, processUnitId);

            var dailyData = unitsData.GetUnitsDailyDataForDateTime(date, processUnitId).ToList();

            var dbResult = this.data.ProductionPlanConfigs.All();
            if (processUnitId != null)
            {
                dbResult = dbResult.Where(x => x.ProcessUnitId == processUnitId.Value);
            }

            var productionPlans = dbResult.Select(p => new
            {
                Id = p.Id,
                Name = p.Name,
                Percentages = p.Percentages,
                QuantityPlanFormula = p.QuantityPlanFormula,
                QuantityPlanMembers = p.QuantityPlanMembers,
                QuantityFactFormula = p.QuantityFactFormula,
                QuantityFactMembers = p.QuantityFactMembers
            }).ToList();

            var calculator = new Calculator();
            var splitter = new char[] { ';' };
            var result = new HashSet<ProductionPlanViewModel>();

            foreach (var productionPlan in productionPlans)
            {
                var planTokens = productionPlan.QuantityPlanMembers.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                var planInputParamsValues = new List<double>();
                foreach (var token in planTokens)
                {
                    foreach (var item in dailyData)
                    {
                        if (item.UnitsDailyConfig.Code == token)
                        {
                            planInputParamsValues.Add((double)item.Value);
                        }  
                    }
                }

                var planInputParams = new Dictionary<string, double>();
                for (int i = 0; i < planInputParamsValues.Count(); i++)
                {
                    planInputParams.Add(string.Format("p{0}", i), planInputParamsValues[i]);  
                }

                var factTokens = productionPlan.QuantityFactMembers.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                var factInputParamsValues = new List<double>();
                foreach (var token in factTokens)
                {
                    foreach (var item in dailyData)
                    {
                        if (item.UnitsDailyConfig.Code == token)
                        {
                            factInputParamsValues.Add((double)item.Value);
                        }  
                    }
                }

                var factInputParams = new Dictionary<string, double>();
                for (int i = 0; i < factInputParamsValues.Count(); i++)
                {
                    factInputParams.Add(string.Format("p{0}", i), factInputParamsValues[i]);  
                }

                var planValue = calculator.Calculate(productionPlan.QuantityPlanFormula, "p", planInputParams.Count, planInputParams);
                var factValue = calculator.Calculate(productionPlan.QuantityFactFormula, "p", factInputParams.Count, factInputParams);

                result.Add(new ProductionPlanViewModel
                {
                    Id = productionPlan.Id,
                    Name = productionPlan.Name,
                    Percentages = productionPlan.Percentages,
                    QuantityPlan = (decimal)planValue,
                    QuantityFact = (decimal)factValue
                });
            }

            var kendoResult = new DataSourceResult();
            try
            {
                kendoResult = result.ToDataSourceResult(request, ModelState);
            }
            catch (Exception ex1)
            {
                Debug.WriteLine(ex1.Message + "\n" + ex1.InnerException);
            }

            return Json(kendoResult);
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
                    return Json(new { IsConfirmed = false });
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
        public ActionResult Confirm(DateTime? date, int? processUnitId)
        {
            ValidateModelState(date, processUnitId);
            if (ModelState.IsValid)
            {
                 var approvedShift = this.data.UnitsApprovedDailyDatas
                    .All()
                    .Where(u => u.RecordDate == date && u.ProcessUnitId == processUnitId)
                    .FirstOrDefault();
                if (approvedShift == null)
                {
                    this.data.UnitsApprovedDailyDatas.Add(
                        new UnitsApprovedDailyData { 
                            RecordDate = date.Value, 
                            ProcessUnitId = processUnitId.Value, 
                            Approved = true 
                        });
                    var result = this.data.SaveChanges(this.UserProfile.UserName);
                    return Json(new { IsConfirmed = result.IsValid}, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError("unitsapproveddata", "Дневните данни вече са потвърдени");
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var errors = GetErrorListFromModelState(ModelState);
                    return Json(new { data = new { errors=errors} });
                }
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = GetErrorListFromModelState(ModelState);
                return Json(new { data = new { errors=errors} });
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