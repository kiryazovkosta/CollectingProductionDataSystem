using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Infrastructure.Contracts;
using CollectingProductionDataSystem.Web.Infrastructure.Filters;
using Ninject;

namespace CollectingProductionDataSystem.Web.AppStart
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //filters.Add(new HandleErrorAttribute());
            var logger = DependencyResolver.Current.GetService<ILogger>();
            filters.Add(new GlobalErrorFilterAttribute(logger));
            var handleAntiforgeryTokenError = DependencyResolver.Current.GetService<HandleAntiforgeryTokenErrorAttribute>();
            handleAntiforgeryTokenError.ExceptionType = typeof(HttpAntiForgeryException);
            filters.Add(handleAntiforgeryTokenError);
            filters.Add(new CheckUserSessionAttribute());
            //filters.Add(new CompressFilterAttribute());
        }
    }
}
