using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.DailyReporting
{
    public class DailyReportingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "DailyReporting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "DailyReporting_default",
                "DailyReporting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers" }
            );
        }
    }
}