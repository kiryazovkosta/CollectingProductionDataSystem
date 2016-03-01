namespace CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Controllers
{
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Models;
    using System.Web.Mvc;

    [Authorize(Roles = "Administrator, MonthlyHydroCarbonsReporter")]
    public class MonthlyHydroCarbonsController : GenericMonthlyController<MonthlyHydroCarbonViewModel>
    {
        private const string defaultView = "HydroCorbonsReport";
        public MonthlyHydroCarbonsController(IProductionData dataParam, IUnitMothlyDataService monthlyServiceParam) 
            :base(dataParam, monthlyServiceParam, CommonConstants.HydroCarbons, defaultView )
        {
            
        }
    }
}