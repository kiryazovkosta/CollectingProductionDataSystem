namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    using System.Diagnostics;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Web.Infrastructure.Filters;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Web.Areas.DailyReporting.ViewModels;
    using Resources = App_GlobalResources.Resources;

    [Authorize]
    public class UnitsReportsController : AreaBaseController
    {
        private readonly IUnitsDataService unitsData;

        public UnitsReportsController(IProductionData dataParam, IUnitsDataService unitsDataParam)
            : base(dataParam)
        {
            this.unitsData = unitsDataParam;
        }

        [HttpGet]
        public ActionResult UnitsReportsData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeFactory]
        public JsonResult ReadUnitsReportsData([DataSourceRequest]
                                        DataSourceRequest request, DateTime? date, int? processUnitId, int? shiftId)
        {
            ValidateModelState(date);
            if (ModelState.IsValid)
            {
                var dbResult = this.unitsData.GetUnitsDataForDateTime(date, processUnitId, null)
                    .Where(x => x.UnitConfig.IsMemberOfShiftsReport)
                    .ToList();
                //ToDo: On shifts changed to 2 must repair this code 
                var result = dbResult.Select(x => new MultiShift
                    {
                        TimeStamp = x.RecordTimestamp,
                        Factory = x.UnitConfig.ProcessUnit.Factory.ShortName,
                        ProcessUnit = x.UnitConfig.ProcessUnit.ShortName,
                        Code = x.UnitConfig.Code,
                        Position = x.UnitConfig.Position,
                        MeasureUnit = x.UnitConfig.MeasureUnit.Code,
                        ShiftProductType = string.Format("{0:d2} {1}", x.UnitConfig.ShiftProductType.Id, x.UnitConfig.ShiftProductType.Name),
                        UnitConfigId = x.UnitConfigId,
                        UnitName = x.UnitConfig.Name,
                        NotATotalizedPosition = x.UnitConfig.NotATotalizedPosition,
                        Shift1 = dbResult.Where(y => y.RecordTimestamp == date && y.ShiftId == (int)ShiftType.First).Where(u => u.UnitConfigId == x.UnitConfigId).FirstOrDefault(),
                        Shift2 = dbResult.Where(y => y.RecordTimestamp == date && y.ShiftId == (int)ShiftType.Second).Where(u => u.UnitConfigId == x.UnitConfigId).FirstOrDefault(),
                        Shift3 = dbResult.Where(y => y.RecordTimestamp == date && y.ShiftId == (int)ShiftType.Third).Where(u => u.UnitConfigId == x.UnitConfigId).FirstOrDefault(),
                    }).Distinct(new MultiShiftComparer()).ToList();

                var kendoPreparedResult = Mapper.Map<IEnumerable<MultiShift>, IEnumerable<UnitsReportsDataViewModel>>(result);
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
            else 
            {
                return Json(new[] { new object() }.ToDataSourceResult(request, ModelState));
            }
        }

        /// <summary>
        /// Validates the state of the model.
        /// </summary>
        /// <param name="date">The date.</param>
        private void ValidateModelState(DateTime? date)
        {

            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
            }

        }
    }
}