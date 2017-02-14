namespace HistoricalProductionStructure.DataAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using Domain.Models;

    public class FactoryMap : EntityTypeConfiguration<Factory>
    {
        public FactoryMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            Property(t => t.ShortName)
                .IsRequired()
                .HasMaxLength(16);

            Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(52);

            // Table & Column Mappings
            Ignore(t => t.FactorySortableName);
            ToTable("Factories");

            // Relationships
            HasOptional(t => t.Plant)
                .WithMany(t => t.Factories)
                .HasForeignKey(d => d.PlantId);

        }
    }
}
