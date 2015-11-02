namespace CollectingProductionDataSystem.Application.IdentityInfrastructure
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Identity;
    using CollectingProductionDataSystem.Models.Identity;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;

    public class ApplicationRoleManager : RoleManager<ApplicationRole,int>, IDisposable
    {
        public ApplicationRoleManager(RoleStoreIntPk store)
            :base(store)
        {
            
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options,
            IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStoreIntPk(context.Get<CollectingDataSystemDbContext>()));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}