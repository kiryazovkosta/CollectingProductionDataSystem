namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Transactions;

    public class MeasurementPointsProductsDataMap : EntityTypeConfiguration<MeasuringPointProductsData>
    {
        public MeasurementPointsProductsDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.RecordTimestamp)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_measurement_points_data", 1)))
                .IsRequired();

            this.Property(t => t.MeasuringPointConfigId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_measurement_points_data", 2)))
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("MeasurementPointsProductsDatas");

            // Relationships
            this.HasRequired(t => t.MeasuringPointConfig)
                .WithMany(t => t.MeasuringPointProductsDatas)
                .HasForeignKey(t => t.MeasuringPointConfigId);
        }
    }
}
