namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Transactions;

    public class MeasuringPointConfigsDataMap : EntityTypeConfiguration<MeasuringPointsConfigsData>
    {
        public MeasuringPointConfigsDataMap()
        {
            // Primary Key
            this.HasKey(t => new { t.MeasuringPointId, t.TransactionNumber, t.RowId, t.TransactionBeginTime, t.TransactionEndTime });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.ExciseStoreId)
                .HasMaxLength(50);

            this.Property(t => t.MeasuringPointConfigId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TransactionNumber)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.RowId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.BaseProductName)
                .HasMaxLength(150);

            this.Property(t => t.ProductName)
                .HasMaxLength(150);

            this.Property(t => t.EngineeringUnitMass)
                .HasMaxLength(50);

            this.Property(t => t.EngineeringUnitVolume)
                .HasMaxLength(50);

            this.Property(t => t.EngineeringUnitDensity)
                .HasMaxLength(50);

            this.Property(t => t.EngineeringUnitTemperature)
                .HasMaxLength(50);

            this.Property(t => t.TamasRecipeTransId)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("MeasuringPointsConfigsDatas");

            this.HasRequired(t => t.MeasuringPointConfig)
                .WithMany(t => t.MeasuringPointsConfigsDatas)
                .HasForeignKey(t => t.MeasuringPointConfigId);
            this.HasRequired(t => t.Product)
                .WithMany(t => t.MeasuringPointsConfigsDatas)
                .HasForeignKey(d => d.ProductId);
        }
    }
}
