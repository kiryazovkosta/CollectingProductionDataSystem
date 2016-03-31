namespace CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
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

    [Authorize(Roles = "Administrator, MonthlyPotableWaterReporter")]
    public class MonthlyPotableWaterController : BaseMonthlyController
    {
        public MonthlyPotableWaterController(IProductionData dataParam, IUnitMothlyDataService monthlyServiceParam)
            : base(dataParam, monthlyServiceParam)
        {
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

            IEfStatus status = this.MonthlyService.CalculateMonthlyDataIfNotAvailable(date, this.ModelParams.MonthlyReportTypeId, this.UserProfile.UserName);

            if (!status.IsValid)
            {
                status.ToModelStateErrors(this.ModelState);
            }

            if (ModelState.IsValid)
            {
                var kendoResult = new DataSourceResult();
                if (ModelState.IsValid)
                {
                    var dbResult = this.MonthlyService.GetDataForMonth(date, this.ModelParams.MonthlyReportTypeId).OrderBy(x => x.UnitMonthlyConfig.Code).ToList();
                    var vmResult = Mapper.Map<IEnumerable<MonthlyReportTableViewModel>>(dbResult);
                    kendoResult = vmResult.ToDataSourceResult(request, ModelState);
                }
                Session["reportParams"] = Convert.ToBase64String(Encoding.UTF8.GetBytes(
                                                                   JsonConvert.SerializeObject(
                                                                       new ConfirmMonthlyInputModel()
                                                                       {
                                                                           date = date,
                                                                           monthlyReportTypeId = this.ModelParams.MonthlyReportTypeId,
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

        /// <summary>
        /// Gets the report parameters.
        /// </summary>
        /// <returns></returns>
        protected override MonthlyReportParametersViewModel GetReportParameters()
        {
            return
                new MonthlyReportParametersViewModel(
                    reportName: Resources.Layout.UnitMonthlyPWData,
                    controllerName: "MonthlyPotableWater",
                    monthlyReportTypeId: CommonConstants.PotableWater,
                    defaultViewName: "EnergyReport");
        }
    }
}