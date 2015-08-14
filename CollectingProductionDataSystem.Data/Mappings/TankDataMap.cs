using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CollectingProductionDataSystem.Models.Inventories;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class TankDataMap : EntityTypeConfiguration<TankData>
    {
        public TankDataMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.RecordTimestamp });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TankDatas");

            // Relationships
            this.HasRequired(t => t.TankConfig)
                .WithMany(t => t.TankDatas)
                .HasForeignKey(d => d.Id);
            this.HasOptional(t => t.Product)
                .WithMany(t => t.TanksDatas)
                .HasForeignKey(d => d.ProductId);

        }
    }
}
