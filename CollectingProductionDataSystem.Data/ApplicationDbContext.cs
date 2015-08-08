namespace CollectingProductionDataSystem.Data
{
    using Microsoft.AspNet.Identity.EntityFramework;

    using CollectingProductionDataSystem.Models;

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("CollectingPrimaryDataSystemConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
