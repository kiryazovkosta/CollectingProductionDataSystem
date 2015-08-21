namespace CollectingProductionDataSystem.Data.Identity
{
    using System;
    using CollectingProductionDataSystem.Models.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class UserStoreIntPk : UserStore<ApplicationUser, ApplicationRole, int,
        UserLoginIntPk, UserRoleIntPk, UserClaimIntPk>
    {
        public UserStoreIntPk(CollectingDataSystemDbContext context)
            : base(context)
        {
        }
    }
}
