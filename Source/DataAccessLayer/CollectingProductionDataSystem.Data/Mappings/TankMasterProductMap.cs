using CollectingProductionDataSystem.Models.Inventories;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class TankMasterProductMap : EntityTypeConfiguration<TankMasterProduct>
    {
        public TankMasterProductMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TankMasterProductCode)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_tankmaster_prod", 1)))
                .IsRequired();

            this.Property(t => t.Name)
                .IsOptional()
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("TankMasterProducts");
        }
    }
}
