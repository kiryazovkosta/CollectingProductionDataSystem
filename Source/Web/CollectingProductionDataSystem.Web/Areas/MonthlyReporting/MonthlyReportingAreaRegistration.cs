using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.MonthlyReporting
{
    public class MonthlyReportingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MonthlyReporting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MonthlyReporting_default",
                "MonthlyReporting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "CollectingProductionDataSystem.Web.Areas.MonthlyReporting.Controllers" }
            );
        }
    }
}