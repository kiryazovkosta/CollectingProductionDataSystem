/// <summary>
/// Summary description for ApplicationUserPark
/// </summary>
namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Identity;

    public class ApplicationUserParkMap:EntityTypeConfiguration<ApplicationUserPark>
    {
        public ApplicationUserParkMap()
        {
            // Key
            this.HasKey(t => (new { ApplicationUserId = t.ApplicationUserId, ParkId = t.ParkId }));

            //Foreign Keys
            this.HasRequired(p => p.ApplicationUser).WithMany().HasForeignKey(y => y.ApplicationUserId).WillCascadeOnDelete(false);
            this.HasRequired(p => p.Park).WithMany().HasForeignKey(y=>y.ParkId).WillCascadeOnDelete(false);
        }
    }
}
