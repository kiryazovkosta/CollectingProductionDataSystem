using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.ShiftReporting
{
    public class ShiftReportingAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ShiftReporting";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ShiftReporting_default",
                "ShiftReporting/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "CollectingProductionDataSystem.Web.Areas.ShiftReporting.Controllers" }
            );
        }
    }
}