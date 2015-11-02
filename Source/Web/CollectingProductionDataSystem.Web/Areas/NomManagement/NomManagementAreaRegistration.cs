using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement
{
    public class NomManagementAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "NomManagement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "NomManagement_default",
                "NomManagement/{controller}/{action}/{id}",
                 new { action = "Index", id = UrlParameter.Optional },
                 namespaces: new string[] { "CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers" }
            );
        }
    }
}