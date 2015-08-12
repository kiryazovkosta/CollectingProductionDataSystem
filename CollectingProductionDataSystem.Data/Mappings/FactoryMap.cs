using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class FactoryMap : EntityTypeConfiguration<Factory>
    {
        public FactoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ShortName)
                .IsRequired()
                .HasMaxLength(16);

            this.Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Factories");

            // Relationships
            this.HasOptional(t => t.Plant)
                .WithMany(t => t.Factories)
                .HasForeignKey(d => d.PlantId);

        }
    }
}
