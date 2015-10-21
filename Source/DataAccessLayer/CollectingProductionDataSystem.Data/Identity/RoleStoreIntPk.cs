namespace CollectingProductionDataSystem.Data.Identity
{
    using CollectingProductionDataSystem.Models.Identity;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class RoleStoreIntPk : RoleStore<ApplicationRole, int, UserRoleIntPk>, IRoleStore<ApplicationRole,int>
    {
        public RoleStoreIntPk(CollectingDataSystemDbContext context)
            : base(context)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}