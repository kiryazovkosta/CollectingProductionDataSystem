using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.ManagementDashBoard
{
    public class ManagementDashBoardAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ManagementDashBoard";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ManagementDashBoard_default",
                "ManagementDashBoard/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "CollectingProductionDataSystem.Web.Areas.ManagementDashBoard.Controllers" }
            );
        }
    }
}