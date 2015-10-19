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
    using System.Transactions;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.UnitsDataServices;
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
        private readonly IUnitsDataService unitsData;

        public UnitsController(IProductionData dataParam, IUnitsDataService unitsDataParam) : base(dataParam)
        {
            this.unitsData = unitsDataParam;
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
            var dbResult = this.unitsData.GetUnitsDataForDateTime(date, processUnitId, shiftId);
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

        private void UpdateRecord(UnitsManualData existManualRecord, UnitDataViewModel model)
        {
            existManualRecord.Value = model.UnitsManualData.Value;
            existManualRecord.EditReasonId = model.UnitsManualData.EditReason.Id;
            this.data.UnitsManualData.Update(existManualRecord);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Confirm(ProcessUnitConfirmShiftInputModel model)
        {
            ValidateModelState(model.date, model.processUnitId, model.shiftId);

            if (this.ModelState.IsValid)
            {
                var approvedShift = this.data.UnitsApprovedDatas
                                        .All()
                                        .Where(u => u.RecordDate == model.date &&
                                                    u.ProcessUnitId == model.processUnitId &&
                                                    u.ShiftId == model.shiftId)
                                        .FirstOrDefault();
                if (approvedShift == null)
                {
                    this.data.UnitsApprovedDatas.Add(new UnitsApprovedData
                    {
                        RecordDate = model.date.Value,
                        ProcessUnitId = model.processUnitId.Value,
                        ShiftId = model.shiftId.Value,
                        Approved = true
                    });

                    var lastShiftId = this.data.ProductionShifts.All().OrderByDescending(x => x.Id).First().Id;
                    if (model.shiftId.Value == lastShiftId)
                    {
                        var confirmedShifts = this.data.UnitsApprovedDatas.All()
                            .Where(x => x.RecordDate == model.date.Value)
                            .Where(x => x.ProcessUnitId == model.processUnitId.Value)
                            .Where(x => x.ShiftId == (int)ShiftType.First || x.ShiftId == (int)ShiftType.Second)
                            .Count();
                        if (confirmedShifts != lastShiftId - 1)
                        {
                            Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            ModelState.AddModelError("shifts", "Не са потвърдени данните за предходните смени!!!");
                            var errors = GetErrorListFromModelState(ModelState);
                            return Json(new { data = new { errors = errors } });   
                        }

                        var lastShift = this.data.ProductionShifts.All().Where(s => s.Id == model.shiftId.Value).FirstOrDefault();
                        if (lastShift != null)
                        {
                            using (TransactionScope transaction = new TransactionScope())
                            {
                                data.SaveChanges(this.UserProfile.UserName);
                                var ud = GetUnitsDataForDay(model);
                                var ht = CalculateDailyDataByCodes(ud);
                                var unitsDailyData = GetUnitsDailyDataConfig(model);
                                CalculateUnitsDailyData(unitsDailyData, ht, model);
                                this.data.SaveChanges(this.UserProfile.UserName);
                                transaction.Complete();
                            }
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
                        data.SaveChanges(this.UserProfile.UserName);
                    }

                    return Json(new { IsConfirmed = true }, JsonRequestBehavior.AllowGet);
                }
                return new HttpStatusCodeResult(200,"Ok");
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = GetErrorListFromModelState(ModelState);
                return Json(new { data = new { errors = errors } });
            }
        }
 
        private void CalculateUnitsDailyData(IQueryable<CalculatedField> unitsDailyData, 
            Dictionary<string, HashSet<UnitsData>> unitsDatasParam, 
            ProcessUnitConfirmShiftInputModel model)
        {
            var calculator = new Calculator();
            var splitter = new char[] { '@' };

            Dictionary<string, double> level2Values = new Dictionary<string, double>();

            foreach (var item in unitsDailyData)
            {
                if (item.IsCurrentLevelCalculation)
                {
                    var tokens = item.Members.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                    var inputParamsValues = new List<double>();
                    foreach (var token in tokens)
                    {
                        double val;
                        if(level2Values.TryGetValue(token, out val))
                        {
                            inputParamsValues.Add(val);
                        }
                    }

                    var inputParams = new Dictionary<string, double>();
                    for (int i = 0; i < inputParamsValues.Count(); i++)
                    {
                        inputParams.Add(string.Format("p{0}", i), inputParamsValues[i]);  
                    }

                    var value = calculator.Calculate(item.Formula, "p", inputParams.Count, inputParams);
                    level2Values.Add(item.Code, value);
                    this.data.UnitsDailyDatas.Add(new UnitsDailyData
                    {
                        RecordTimestamp = model.date.Value,
                        Value = (decimal)value,
                        UnitsDailyConfigId = item.Id,
                    });
                }
                else
                {
                    var unitsDataList = new HashSet<UnitsData>();
                    var hasManualData = false;   
                
                    var tokens = item.Members.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                    var inputParamsValues = new List<double>();
                    foreach (var token in tokens)
                    {
                        var hs = unitsDatasParam[token];
                        var sum = 0.0;
                        foreach (var unitsData in hs)
                        {
                            if (unitsData.IsManual)
                            {
                                hasManualData = true;
                            }
   
                            sum += unitsData.RealValue;
                        }

                        inputParamsValues.Add(sum);
                        unitsDataList.AddRange(hs);
                    }

                    var inputParams = new Dictionary<string, double>();
                    for (int i = 0; i < inputParamsValues.Count(); i++)
                    {
                        inputParams.Add(string.Format("p{0}", i), inputParamsValues[i]);  
                    }

                    var value = calculator.Calculate(item.Formula, "p", inputParams.Count, inputParams);
                    level2Values.Add(item.Code, value);

                    this.data.UnitsDailyDatas.Add(new UnitsDailyData
                    {
                        RecordTimestamp = model.date.Value,
                        Value = (decimal)value,
                        UnitsDailyConfigId = item.Id,
                        HasManualData = hasManualData,
                        //UnitsDatas = unitsDataList
                    });
                }
            }
        }
 
        private IQueryable<CalculatedField> GetUnitsDailyDataConfig(ProcessUnitConfirmShiftInputModel model)
        {
            var unitsDailyData = this.data.UnitsDailyConfigs
                                     .All()
                                     .Include(u => u.ProcessUnit)
                                     .Where(u => u.ProcessUnitId == model.processUnitId.Value)
                                     .Select(u => new CalculatedField
                                            {
                                                Id = u.Id,
                                                Members = u.AggregationMembers,
                                                Formula = u.AggregationFormula,
                                                IsCurrentLevelCalculation = u.AggregationCurrentLevel,
                                                Code = u.Code
                                            });
            return unitsDailyData;
        }
 
        private IQueryable<UnitsData> GetUnitsDataForDay(ProcessUnitConfirmShiftInputModel model)
        {
            var records = this.data.UnitsData.All()
                              .Include(u => u.UnitConfig)
                              .Where(u => u.RecordTimestamp == model.date &&
                                          u.UnitConfig.ProcessUnitId == model.processUnitId.Value);
            return records;
        }
 
        private Dictionary<string, HashSet<UnitsData>> CalculateDailyDataByCodes(IQueryable<UnitsData> unitsDataParam)
        {
            var result = new Dictionary<string, HashSet<UnitsData>>();
            foreach (var unitsData in unitsDataParam)
            {
                if (result.ContainsKey(unitsData.UnitConfig.Code))
                {
                    result[unitsData.UnitConfig.Code].Add(unitsData);
                }
                else
                {
                    var unitsDataList = new HashSet<UnitsData>();
                    unitsDataList.Add(unitsData);
                    result.Add(unitsData.UnitConfig.Code, unitsDataList);
                }
            }

            return result;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsConfirmed([DataSourceRequest]
                                        DataSourceRequest request, DateTime? date, int? processUnitId, int? shiftId)
        {
            ValidateModelState(date, processUnitId, shiftId);

            if (this.ModelState.IsValid)
            {
                var approvedShift = this.data.UnitsApprovedDatas
                    .All()
                    .Where(u => u.RecordDate == date &&
                                u.ProcessUnitId == processUnitId &&
                                u.ShiftId == shiftId)
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

        private List<string> GetErrorListFromModelState(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            return errorList;
        }
    }

    public class CalculatedField
    {
        public int Id { get; set; }
        public string Members { get; set; }
        public string Formula { get; set; }
        public bool IsCurrentLevelCalculation { get; set; }
        public string Code { get; set; }
    }
}