using System;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(CollectingProductionDataSystem.Web.AppStart.Startup))]
[assembly: log4net.Config.XmlConfigurator(ConfigFile = "Web.config", Watch = true)]

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
