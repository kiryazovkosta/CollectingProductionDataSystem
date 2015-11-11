namespace CollectingProductionDataSystem.Web
{
    using System;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Web.AppStart;
    using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;

    public class MvcApplication : HttpApplication
    {

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.RegisterMappings();
            ViewEnginesConfig.RegisterViewEngines(ViewEngines.Engines);
            ModelBindersConfig.RegisterModelBinders(ModelBinders.Binders, DependencyResolver.Current);
            
        }
    }
}
