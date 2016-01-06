using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.Documentation
{
    public class DocumentationAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Documentation";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Documentation_default",
                "Documentation/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "CollectingProductionDataSystem.Web.Areas.Documentation.Controllers"}
            );
        }
    }
}