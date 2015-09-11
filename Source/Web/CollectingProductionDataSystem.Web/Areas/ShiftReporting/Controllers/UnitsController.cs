namespace CollectingProductionDataSystem.Web.Areas.ShiftReporting.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;
    using System.Net;
    using System.Net.Mime;
    using System.Transactions;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Application.CalculateServices;
    using CollectingProductionDataSystem.Application.UnitsDataServices;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Web.Controllers;
    using CollectingProductionDataSystem.Web.Infrastructure.Filters;
    using CollectingProductionDataSystem.Web.InputModels;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using AutoMapper;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Web.ViewModels.Units;
    using Resources = App_GlobalResources.Resources;
    using System.Collections;
    using System.Text;
    using MathExpressions.Application;

    [Authorize]
    public class UnitsController : AreaBaseController
    {
        private readonly IUnitsDataService unitsData;

        public UnitsController(IProductionData dataParam, IUnitsDataService unitsDataParam)
            : base(dataParam)
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
            //if (this.ModelState.IsValid)
            //{
            //    var dbResult = this.unitsData.GetUnitsDataForDateTime(date, processUnitId, shiftId);
            //    var kendoResult = dbResult.ToDataSourceResult(request, ModelState);
            //    kendoResult.Data = Mapper.Map<IEnumerable<UnitsData>, IEnumerable<UnitDataViewModel>>((IEnumerable<UnitsData>)kendoResult.Data);
            //    return Json(kendoResult);
            //}
            //else
            //{
            //    var kendoResult = new List<UnitDataViewModel>().ToDataSourceResult(request, ModelState);
            //    return Json(kendoResult);
            //}
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([DataSourceRequest]DataSourceRequest request, UnitDataViewModel model)
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
                                        .Where(u => u.RecordDate == model.date && u.ProcessUnitId == model.processUnitId && u.ShiftId == model.shiftId)
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

                    data.SaveChanges(this.UserProfile.UserName);
                    // TODO: need to refactoring code to get max shift not to hardcode this number
                    if (model.shiftId.Value == 3)
                    {
                        var calculator = new Calculator();

                        //Todo: check there are already first and second productions data for the day

                        // last shift for the day. Need to calculate daily units data at level 2
                        var shift = this.data.ProductionShifts.All().Where(s => s.Id == model.shiftId.Value).FirstOrDefault();
                        // It will be verry strang if there is not a shift with provided id but who knows
                        if (shift != null)
                        {
                            var endRecordTimespan = model.date.Value.AddMinutes(shift.BeginMinutes + shift.OffsetMinutes);
                            var ud = this.data.UnitsData.All().Include(u => u.UnitConfig)
                                .Where(u => u.RecordTimestamp > model.date.Value && 
                                    u.RecordTimestamp < endRecordTimespan && 
                                    u.UnitConfig.ProcessUnitId == model.processUnitId.Value)
                                .Select(u => new
                                {
                                    Id = u.Id,
                                    UnitConfigId = u.UnitConfigId,
                                    Code = u.UnitConfig.Code,
                                    Value = u.UnitsManualData.Value == null ? u.Value : u.UnitsManualData.Value
                                });

                            // Todo: Refactoring, refactoring, refactoring
                            var ht = new Hashtable();
                            try
                            {
                                foreach (var item in ud)
                                {
                                    if (ht.ContainsKey(item.Code))
                                    {
                                        decimal? newValue = item.Value.HasValue ? (decimal)ht[item.Code] + item.Value : (decimal)ht[item.Code] + default(decimal);
                                        ht[item.Code] = newValue.Value;
                                    }
                                    else
                                    {
                                        if (item.Value.HasValue)
                                        {
                                            ht.Add(item.Code, item.Value); 
                                        }
                                        else
                                        {
                                            ht.Add(item.Code, default(decimal)); 
                                        }
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }

                            var unitsDailyData = this.data.UnitsDailyConfigs
                                .All()
                                .Include(u => u.ProcessUnit)
                                .Where(u => u.ProcessUnitId == model.processUnitId.Value)
                                .Select(u => new CalculatedField
                                {
                                    Id = u.Id,
                                    Members = u.AggregationMembers,
                                    Formula = u.AggregationFormula
                                });

                            foreach (var item in unitsDailyData)
                            {
                               var chars = new char[] { ';' };
                                var tokens = item.Members.Split(chars, StringSplitOptions.RemoveEmptyEntries);
                                var inputParamsValues = new List<decimal>();
                                foreach (var token in tokens)
                                {
                                    var v = ht[token] == null ? default(decimal) : (decimal)ht[token];
                                    inputParamsValues.Add(v);
                                }

                                var inputParams = new Dictionary<string, decimal>();
                                for (int i = 0; i < inputParamsValues.Count(); i++)
                                {
                                    inputParams.Add(string.Format("p{0}", i), inputParamsValues[i]);  
                                }

                                decimal value = calculator.Calculate(item.Formula, "p", inputParams.Count, inputParams);
                                try
                                {
                                    value = calculator.Calculate(item.Formula, "p", inputParams.Count, inputParams);
                                }
                                catch (Exception ex)
                                {
                                    ModelState.AddModelError("", "Възникна грешка при обработка на данните");
                                }

                                if (ModelState.IsValid)
                                {
                                    this.data.UnitsDailyDatas.Add(
                                                                new UnitsDailyData
                                                                {
                                                                    RecordTimestamp = model.date.Value,
                                                                    Value = value,
                                                                    UnitsDailyConfigId = item.Id
                                                                });

                                }
                            }
                            if (ModelState.IsValid)
                            {
                                this.data.SaveChanges(this.UserProfile.UserName);        
                            }
                        }
                    }
                    // не така
                    // just for example
                    // we want to throw some error
                    //сеха го рънни така че да стигне до тук ако не стане  моля те вкарай ги в един инпут модел и долу вкарай модела - 
                    //ModelState.AddModelError("", "Не ме кефиш!!!");
                    //model.IsConfirmed = false;
                    //return Json(new[] { model }.ToDataSourceResult(request, ModelState));
                    //return this.Json(new DataSourceResult
                    //{
                    //    Errors = new { errors = new []{ "Не ме кафиш"}, },
                    //});
                    var resultContent = new List<UnitDataViewModel>() { null};
                    var result = new { resultContent  };
                    ModelState.AddModelError("", "Не ме кефиш!!!");
                    ModelState.AddModelError("", "Ама много не ме кефиш!!!");
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    var errors = GetErrorListFromModelState(ModelState);
                    return Json(new { data = new { errors=errors} });
                }

                
                return new HttpStatusCodeResult(200,"Ok");
            }
            else
            {
               // var kendoResult = new List<UnitDataViewModel>().ToDataSourceResult(request, ModelState);
               //tuka ok li e? какво трябва да върна. Тук модела не е валиден. Т.е. не е анаизбр
                // после ще си го оправиш някъде имах метод дето обхождаше колекцията за грешки..Да
                return Json(400,"Не ме кефиш!!!");
            }
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
        public ActionResult UnitsDataIsConfirmed([DataSourceRequest]
                                                 DataSourceRequest request, DateTime? date, int? processUnitId, int? shiftId)
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

            if (this.ModelState.IsValid)
            {
                var approvedShift = this.data.UnitsApprovedDatas
                                        .All()
                                        .Where(u => u.RecordDate == date && u.ProcessUnitId == processUnitId && u.ShiftId == shiftId)
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

        private List<string> GetErrorListFromModelState
                                            (ModelStateDictionary modelState)
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
    }


}