using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.MonthlyHydroCarbons
{
    public class MonthlyHydroCarbonsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "MonthlyHydroCarbons";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "MonthlyHydroCarbons_default",
                "MonthlyHydroCarbons/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "CollectingProductionDataSystem.Web.Areas.MonthlyHydroCarbons.Controllers"}
            );
        }
    }
}