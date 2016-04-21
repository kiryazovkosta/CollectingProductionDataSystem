namespace CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Models;
    using Resources = App_GlobalResources.Resources;

    [Authorize(Roles = "Administrator, MonthlyFreshWaterReporter, SummaryReporter")]
    public class MonthlyFreshWaterController :BaseMonthlyController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MonthlyFreshWaterController" /> class.
        /// </summary>
        /// <param name="dataParam">The data param.</param>
        /// <param name="monthlyServiceParam">The monthly service param.</param>
        public MonthlyFreshWaterController(IProductionData dataParam, IUnitMothlyDataService monthlyServiceParam) 
            : base(dataParam, monthlyServiceParam)
        {
        }

        /// <summary>
        /// Gets the report parameters.
        /// </summary>
        /// <returns></returns>
        protected override MonthlyReportParametersViewModel GetReportParameters()
        {
            return new MonthlyReportParametersViewModel(
                    reportName: Resources.Layout.UnitMonthlyFWData,
                    controllerName: "MonthlyFreshWater",
                    monthlyReportTypeId: CommonConstants.FreshWater,
                    defaultViewName: "EnergyReport");
        }
    }
}