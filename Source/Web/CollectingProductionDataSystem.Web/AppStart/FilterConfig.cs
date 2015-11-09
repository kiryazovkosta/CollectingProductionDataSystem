using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Web.Infrastructure.Filters;
using Ninject;

namespace CollectingProductionDataSystem.Web.AppStart
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters,IDependencyResolver kernel)
        {
            //filters.Add(new HandleErrorAttribute());
            filters.Add(kernel.GetService<GlobalErrorFilterAttribute>());
        }
    }
}
