using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.OverseersReporting
{
    public class OverseersReportingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "OverseersReporting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "OverseersReporting_default",
                "OverseersReporting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "CollectingProductionDataSystem.Web.Areas.OverseersReporting.Controllers" }
            );
        }
    }
}