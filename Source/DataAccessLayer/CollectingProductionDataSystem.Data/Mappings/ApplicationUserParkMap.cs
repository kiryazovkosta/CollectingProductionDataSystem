/// <summary>
/// Summary description for ApplicationUserParks
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

    public class ApplicationUserParksMap:EntityTypeConfiguration<ApplicationUserParks>
    {
        public ApplicationUserParksMap()
        {
            // Key
            this.HasKey(t => (new { ApplicationUserId = t.ApplicationUserId, ParkId = t.ParkId }));

            //this.Property(t => t.UnitConfigId)
            //    .IsRequired()
            //    .HasColumnName("UnitConfigId");
            //this.Property(t => t.UnitDailyConfigId)
            //    .IsRequired()
            //    .HasColumnName("UnitDailyConfigId");
            

            //this.ToTable("UnitConfigUnitDailyConfigs");
            this.HasRequired(p => p.ApplicationUser).WithMany().HasForeignKey(y => y.ApplicationUserId).WillCascadeOnDelete(false);
            this.HasRequired(p => p.Park).WithMany().HasForeignKey(y=>y.ParkId).WillCascadeOnDelete(false);
        }
    }
}
