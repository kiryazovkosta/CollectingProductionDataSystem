using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CollectingProductionDataSystem.Models.Inventories;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class TankConfigMap : EntityTypeConfiguration<TankConfig>
    {
        public TankConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("TankConfigs");

            // Relationships
            this.HasRequired(t => t.InventoryPark)
                .WithMany(t => t.InventoryTanks)
                .HasForeignKey(d => d.InventoryParkId);

        }
    }
}
