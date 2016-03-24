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

    [Authorize(Roles = "Administrator, MonthlyHeatEnetgyReporter")]
    public class MonthlyHeatEnergyController : BaseMonthlyController
    {
        public MonthlyHeatEnergyController(IProductionData dataParam, IUnitMothlyDataService monthlyServiceParam)
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
                    reportName: Resources.Layout.UnitMonthlyHEData,
                    controllerName: "MonthlyHeatEnergy",
                    monthlyReportTypeId: CommonConstants.HeatEnergy,
                    defaultViewName: "EnergyReport");
        }
    }
}