using CollectingProductionDataSystem.Models.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class MeasurementPointsProductsDataMap : EntityTypeConfiguration<MeasurementPointsProductsData>
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

            this.Property(t => t.MeasurementPointsProductsConfigId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_measurement_points_data", 2)))
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("MeasurementPointsProductsDatas");

            // Relationships
            this.HasRequired(t => t.MeasurementPointsProductsConfig)
                .WithMany(t => t.MeasurementPointsProductsDatas)
                .HasForeignKey(t => t.MeasurementPointsProductsConfigId);
        }
    }
}
