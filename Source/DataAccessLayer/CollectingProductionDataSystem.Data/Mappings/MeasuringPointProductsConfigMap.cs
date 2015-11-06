namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Transactions;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MeasuringPointProductsConfigMap : EntityTypeConfiguration<MeasuringPointProductsConfig>
    {
        public MeasuringPointProductsConfigMap()
        {
            // Primary key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.PhdProductTotalizerTag)
                .IsRequired()
                .HasMaxLength(50);

             // Table & Column Mappings
            this.ToTable("MeasuringPointProductsConfigs");

            this.HasRequired(t => t.MeasuringPointConfig)
                .WithMany(t => t.MeasuringPointProductsConfigs)
                .HasForeignKey(d => d.MeasuringPointConfigId)
                .WillCascadeOnDelete(false);

            this.HasRequired(t => t.Direction)
                .WithMany(t => t.MeasuringPointProductsConfigs)
                .HasForeignKey(d => d.DirectionId);
            this.HasRequired(t => t.Product)
                .WithMany(t => t.MeasuringPointProductsConfigs)
                .HasForeignKey(d => d.ProductId);

        }
    }
}
