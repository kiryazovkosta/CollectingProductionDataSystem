using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CollectingProductionDataSystem.Models.Inventories;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class TankConfigMap : EntityTypeConfiguration<TankConfig>
    {
        public TankConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            //this.Property(t => t.DataSource)
            //    .HasColumnAnnotation(name: "DefaultValue", value: PrimaryDataSourceType.SrvVmMesPhdA);

            // Properties
            // Table & Column Mappings
            this.ToTable("TankConfigs");

            // Relationships
            this.HasRequired(t => t.Park)
                .WithMany(t => t.TankConfigs)
                .HasForeignKey(d => d.ParkId);

            this.HasMany(t => t.RelatedTankConfigs);
        }
    }
}
