namespace CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Models;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Constants;
    using Resources = App_GlobalResources.Resources;

    [Authorize(Roles = "Administrator, MonthlyFuelsReporter, SummaryReporter")]
    public class MonthlyFuelsController : BaseMonthlyController
    {
        public MonthlyFuelsController(IProductionData dataParam, IUnitMothlyDataService monthlyServiceParam)
            : base(dataParam, monthlyServiceParam)
        {
        }

        protected override MonthlyReportParametersViewModel GetReportParameters()
        {
            return new MonthlyReportParametersViewModel(
                reportName: Resources.Layout.UnitMonthlyFuelsData,
                controllerName: "MonthlyFuels",
                monthlyReportTypeId: CommonConstants.Fuels,
                defaultViewName: "EnergyReport");
        }
    }
}