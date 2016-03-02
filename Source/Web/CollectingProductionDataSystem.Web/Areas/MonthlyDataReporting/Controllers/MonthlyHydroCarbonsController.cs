namespace CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Controllers
{
    using System;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Models;
    using System.Web.Mvc;
    using Resources = App_GlobalResources.Resources;

    [Authorize(Roles = "Administrator, MonthlyHydroCarbonsReporter")]
    public class MonthlyHydroCarbonsController : BaseMonthlyController
    {
        public MonthlyHydroCarbonsController(IProductionData dataParam, IUnitMothlyDataService monthlyServiceParam) 
            :base(dataParam, monthlyServiceParam)
        {
            
        }
        /// <summary>
        /// Gets the report parameters.
        /// </summary>
        /// <returns></returns>
        protected override MonthlyReportParametersViewModel GetReportParameters()
        {
            return new MonthlyReportParametersViewModel(
                    reportName: Resources.Layout.UnitMonthlyHCData,
                    controllerName: "MonthlyHydroCarbons",
                    monthlyReportTypeId: CommonConstants.HydroCarbons,
                    defaultViewName: "HydroCorbonsReport");
        }
    }
}