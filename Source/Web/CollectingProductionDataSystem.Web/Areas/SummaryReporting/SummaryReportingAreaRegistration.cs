using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.SummaryReporting
{
    public class SummaryReportingAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SummaryReporting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SummaryReporting_default",
                "SummaryReporting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "CollectingProductionDataSystem.Web.Areas.SummaryReporting.Controllers" }
            );
        }
    }
}