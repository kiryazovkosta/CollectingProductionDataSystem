using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting
{
    public class MonthlyDataReportingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MonthlyDataReporting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MonthlyDataReporting_default",
                "MonthlyDataReporting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Controllers" }
            );
        }
    }
}