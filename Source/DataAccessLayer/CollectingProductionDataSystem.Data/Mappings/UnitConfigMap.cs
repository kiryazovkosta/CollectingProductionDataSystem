using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class UnitConfigMap : EntityTypeConfiguration<UnitConfig>
    {
        public UnitConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CollectingDataMechanism)
                .HasMaxLength(1);

            this.Property(t => t.PreviousShiftTag)
                .HasMaxLength(50);

            this.Property(t => t.CurrentInspectionDataTag)
                .HasMaxLength(50);

            this.Property(t => t.Notes)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("UnitsConfigs");

            // Relationships
            this.HasRequired(t => t.Direction)
                .WithMany(t => t.Units)
                .HasForeignKey(d => d.DirectionId);
            this.HasRequired(t => t.MaterialType)
                .WithMany(t => t.Units)
                .HasForeignKey(d => d.MaterialTypeId);
            this.HasRequired(t => t.MeasureUnit)
                .WithMany(t => t.Units)
                .HasForeignKey(d => d.MeasureUnitId);
            this.HasRequired(t => t.ProcessUnit)
                .WithMany(t => t.UnitsConfigs)
                .HasForeignKey(d => d.ProcessUnitId);
            this.HasOptional(t => t.Product)
                .WithMany(t => t.Units)
                .HasForeignKey(d => d.ProductId);
            this.HasOptional(t => t.ProductType)
                .WithMany(t => t.Units)
                .HasForeignKey(d => d.ProductTypeId);

        }
    }
}
