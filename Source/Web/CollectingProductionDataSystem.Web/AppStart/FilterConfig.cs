using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Web.Infrastructure.Filters;
using Ninject;

namespace CollectingProductionDataSystem.Web.AppStart
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new GlobalErrorFilterAttribute());
            filters.Add(new HandleAntiforgeryTokenErrorAttribute() { ExceptionType = typeof(HttpAntiForgeryException)});
        }
    }
}
