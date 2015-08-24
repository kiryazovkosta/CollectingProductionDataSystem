using CollectingProductionDataSystem.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class MeasurementPointsProductsConfigMap : EntityTypeConfiguration<MeasurementPointsProductsConfig>
    {
        public MeasurementPointsProductsConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PhdTotalCounterTag)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("MeasurementPointsProductsConfigs");

            // Relationships
            this.HasRequired(t => t.Direction)
                .WithMany(t => t.MeasurementPointsProductsConfigs)
                .HasForeignKey(d => d.DirectionId);
            this.HasRequired(t => t.MeasurementPoint)
                .WithMany(t => t.MeasurementPointsProductsConfigs)
                .HasForeignKey(d => d.MeasurementPointId);
            this.HasRequired(t => t.Product)
                .WithMany(t => t.MeasurementPointsProductsConfigs)
                .HasForeignKey(d => d.Code);

        }
    }
}
