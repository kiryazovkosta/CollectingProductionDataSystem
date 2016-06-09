namespace CollectingProductionDataSystem.Web.Areas.MonthlyTechReporting
{
    using System.Web.Mvc;

    public class MonthlyTechReportingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MonthlyTechReporting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MonthlyTechReporting_default",
                "MonthlyTechReporting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "CollectingProductionDataSystem.Web.Areas.MonthlyTechReporting.Controllers" }
            );
        }
    }
}