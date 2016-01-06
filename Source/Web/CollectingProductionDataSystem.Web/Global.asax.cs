namespace CollectingProductionDataSystem.Web
{
    using System;
    using System.Web;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Web.AppStart;
    using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;

    public class MvcApplication : HttpApplication
    {
        private const int An_Hour = 60;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutoMapperConfig.RegisterMappings();
            ViewEnginesConfig.RegisterViewEngines(ViewEngines.Engines);
            ModelBindersConfig.RegisterModelBinders(ModelBinders.Binders, DependencyResolver.Current);
            StartUpUsersConfiguration.RegisterUsersStateChange(DependencyResolver.Current.GetService<IProductionData>());
        }

        protected void Session_Start() 
        {
            this.Session.Timeout = An_Hour/2;
        }
    }
}
