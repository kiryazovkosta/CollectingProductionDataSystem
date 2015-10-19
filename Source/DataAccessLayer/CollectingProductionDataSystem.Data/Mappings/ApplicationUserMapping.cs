/// <summary>
/// Summary description for AppplicationUserMapping
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
    using CollectingProductionDataSystem.Models.Productions;

    class ApplicationUserMapping : EntityTypeConfiguration<ApplicationUser>
    {
        public ApplicationUserMapping()
        {
            // Primary Key
            this.HasKey(t => t.Id);

   
            

            //Foreign Keys

            this.HasMany(t => t.ApplicationUserParks)
                .WithRequired()
                .HasForeignKey(x => x.ApplicationUserId).WillCascadeOnDelete(false);

            this.HasMany(t => t.ApplicationUserProcessUnits)
                .WithRequired()
                .HasForeignKey(x => x.ApplicationUserId).WillCascadeOnDelete(false);
        }
    }
}
