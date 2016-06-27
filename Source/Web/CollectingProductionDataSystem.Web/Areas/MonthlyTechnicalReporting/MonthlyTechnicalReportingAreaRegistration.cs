using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.MonthlyTechnicalReporting
{
    public class MonthlyTechnicalReportingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MonthlyTechnicalReporting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MonthlyTechnicalReporting_default",
                "MonthlyTechnicalReporting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "CollectingProductionDataSystem.Web.Areas.MonthlyTechnicalReporting.Controllers" }
            );
        }
    }
}