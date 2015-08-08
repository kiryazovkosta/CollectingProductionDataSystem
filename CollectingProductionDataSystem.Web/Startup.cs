using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CollectingProductionDataSystem.Web.Startup))]
namespace CollectingProductionDataSystem.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
