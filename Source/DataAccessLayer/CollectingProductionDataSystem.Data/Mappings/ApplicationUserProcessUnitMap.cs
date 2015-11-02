/// <summary>
/// Summary description for ApplicationUserProcessUnitMap
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

    class ApplicationUserProcessUnitMap:EntityTypeConfiguration<ApplicationUserProcessUnit>
    {
        public ApplicationUserProcessUnitMap()
        {
            // Key
            this.HasKey(t => (new { ApplicationUserId = t.ApplicationUserId, ProcessUnitId = t.ProcessUnitId }));

            //Foreign Keys
            this.HasRequired(p => p.ApplicationUser).WithMany().HasForeignKey(y => y.ApplicationUserId).WillCascadeOnDelete(false);
            this.HasRequired(p => p.ProcessUnit).WithMany().HasForeignKey(y=>y.ProcessUnitId).WillCascadeOnDelete(false);
        }
    }
}
