using System;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CollectingProductionDataSystem.Web.AppStart.Startup))]

namespace CollectingProductionDataSystem.Web.AppStart
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
