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

            // Relationships
            this.HasMany(t => t.ProcessUnits)
                .WithMany(t => t.Users);

            this.HasMany(t => t.Parks)
                .WithMany(t => t.Users);

        }
    }
}
