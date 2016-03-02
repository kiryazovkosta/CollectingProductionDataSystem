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

    [Authorize(Roles = "Administrator, MonthlyElectricalEnergyReporter")]
    public class MonthlyElectricalEnergyController : BaseMonthlyController
    {
        public MonthlyElectricalEnergyController(IProductionData dataParam, IUnitMothlyDataService monthlyServiceParam)
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
                    reportName: Resources.Layout.UnitMonthlyEEData,
                    controllerName: "MonthlyElectricalEnergy",
                    monthlyReportTypeId: CommonConstants.ElectricalEnergy,
                    defaultViewName:"EnergyReport");
        }
    }
}