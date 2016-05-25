using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.PlanConfiguration
{
    public class PlanConfigurationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "PlanConfiguration";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "PlanConfiguration_default",
                "PlanConfiguration/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "CollectingProductionDataSystem.Web.Areas.PlanConfiguration.Controllers"}
            );
        }
    }
}