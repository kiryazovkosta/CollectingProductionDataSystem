using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Areas.RecordsHistory
{
    public class RecordsHistoryAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "RecordsHistory";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
            "RecordsHistory_history",
            "RecordsHistory/History/{entityName}/{id}",
            new { controller = "History", action = "Index"},
            namespaces: new string[] { "CollectingProductionDataSystem.Web.Areas.RecordsHistory.Controllers" }
            );

            context.MapRoute(
                "RecordsHistory_default",
                "RecordsHistory/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "CollectingProductionDataSystem.Web.Areas.RecordsHistory.Controllers" }
            );
        }
    }
}