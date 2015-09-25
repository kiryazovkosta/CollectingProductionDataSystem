using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SteelDbModel.Models.Mapping
{
    public class UnitsConfigMap : EntityTypeConfiguration<UnitsConfig>
    {
        public UnitsConfigMap()
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
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Position).HasColumnName("Position");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ProductTypeId).HasColumnName("ProductTypeId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.ProcessUnitId).HasColumnName("ProcessUnitId");
            this.Property(t => t.DirectionId).HasColumnName("DirectionId");
            this.Property(t => t.MeasureUnitId).HasColumnName("MeasureUnitId");
            this.Property(t => t.MaterialTypeId).HasColumnName("MaterialTypeId");
            this.Property(t => t.IsMaterial).HasColumnName("IsMaterial");
            this.Property(t => t.IsEnergy).HasColumnName("IsEnergy");
            this.Property(t => t.IsInspectionPoint).HasColumnName("IsInspectionPoint");
            this.Property(t => t.CollectingDataMechanism).HasColumnName("CollectingDataMechanism");
            this.Property(t => t.AggregateGroup).HasColumnName("AggregateGroup");
            this.Property(t => t.IsCalculated).HasColumnName("IsCalculated");
            this.Property(t => t.PreviousShiftTag).HasColumnName("PreviousShiftTag");
            this.Property(t => t.CurrentInspectionDataTag).HasColumnName("CurrentInspectionDataTag");
            this.Property(t => t.Notes).HasColumnName("Notes");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.DeletedOn).HasColumnName("DeletedOn");
            this.Property(t => t.DeletedFrom).HasColumnName("DeletedFrom");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.ModifiedOn).HasColumnName("ModifiedOn");
            this.Property(t => t.CreatedFrom).HasColumnName("CreatedFrom");
            this.Property(t => t.ModifiedFrom).HasColumnName("ModifiedFrom");
            this.Property(t => t.AggregateParameter).HasColumnName("AggregateParameter");
            this.Property(t => t.MaximumCost).HasColumnName("MaximumCost");
            this.Property(t => t.EstimatedDensity).HasColumnName("EstimatedDensity");
            this.Property(t => t.EstimatedPressure).HasColumnName("EstimatedPressure");
            this.Property(t => t.EstimatedTemperature).HasColumnName("EstimatedTemperature");
            this.Property(t => t.EstimatedCompressibilityFactor).HasColumnName("EstimatedCompressibilityFactor");

            // Relationships
            this.HasRequired(t => t.Direction)
                .WithMany(t => t.UnitsConfigs)
                .HasForeignKey(d => d.DirectionId);
            this.HasRequired(t => t.MaterialType)
                .WithMany(t => t.UnitsConfigs)
                .HasForeignKey(d => d.MaterialTypeId);
            this.HasRequired(t => t.MeasureUnit)
                .WithMany(t => t.UnitsConfigs)
                .HasForeignKey(d => d.MeasureUnitId);
            this.HasRequired(t => t.ProcessUnit)
                .WithMany(t => t.UnitsConfigs)
                .HasForeignKey(d => d.ProcessUnitId);
            this.HasOptional(t => t.Product)
                .WithMany(t => t.UnitsConfigs)
                .HasForeignKey(d => d.ProductId);
            this.HasRequired(t => t.ProductType)
                .WithMany(t => t.UnitsConfigs)
                .HasForeignKey(d => d.ProductTypeId);

        }
    }
}
