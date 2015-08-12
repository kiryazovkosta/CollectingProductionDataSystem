using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CollectingProductionDataSystem.Models.Inventories;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class ParkMap : EntityTypeConfiguration<Park>
    {
        public ParkMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("Parks");          

            // Relationships
            this.HasRequired(t => t.Area)
                .WithMany(t => t.InventoryParks)
                .HasForeignKey(d => d.AreaId);
            this.HasRequired(t => t.ExciseStore)
                .WithMany(t => t.InventoryParks)
                .HasForeignKey(d => d.ExciseStoreId);

        }
    }
}
