using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SteelDbModel.Models.Mapping
{
    public class TankDataMap : EntityTypeConfiguration<TankData>
    {
        public TankDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("TankDatas");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RecordTimestamp).HasColumnName("RecordTimestamp");
            this.Property(t => t.TankConfigId).HasColumnName("TankConfigId");
            this.Property(t => t.ParkId).HasColumnName("ParkId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.LiquidLevel).HasColumnName("LiquidLevel");
            this.Property(t => t.TotalObservableVolume).HasColumnName("TotalObservableVolume");
            this.Property(t => t.ProductLevel).HasColumnName("ProductLevel");
            this.Property(t => t.GrossObservableVolume).HasColumnName("GrossObservableVolume");
            this.Property(t => t.ObservableDensity).HasColumnName("ObservableDensity");
            this.Property(t => t.GrossStandardVolume).HasColumnName("GrossStandardVolume");
            this.Property(t => t.AverageTemperature).HasColumnName("AverageTemperature");
            this.Property(t => t.NetStandardVolume).HasColumnName("NetStandardVolume");
            this.Property(t => t.ReferenceDensity).HasColumnName("ReferenceDensity");
            this.Property(t => t.WeightInAir).HasColumnName("WeightInAir");
            this.Property(t => t.WeightInVacuum).HasColumnName("WeightInVacuum");
            this.Property(t => t.FreeWaterLevel).HasColumnName("FreeWaterLevel");
            this.Property(t => t.FreeWaterVolume).HasColumnName("FreeWaterVolume");
            this.Property(t => t.MaxVolume).HasColumnName("MaxVolume");
            this.Property(t => t.AvailableRoom).HasColumnName("AvailableRoom");
            this.Property(t => t.UnusableResidueLevel).HasColumnName("UnusableResidueLevel");
            this.Property(t => t.IsApproved).HasColumnName("IsApproved");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.ModifiedOn).HasColumnName("ModifiedOn");
            this.Property(t => t.CreatedFrom).HasColumnName("CreatedFrom");
            this.Property(t => t.ModifiedFrom).HasColumnName("ModifiedFrom");

            // Relationships
            this.HasOptional(t => t.Product)
                .WithMany(t => t.TankDatas)
                .HasForeignKey(d => d.ProductId);
            this.HasRequired(t => t.TankConfig)
                .WithMany(t => t.TankDatas)
                .HasForeignKey(d => d.TankConfigId);

        }
    }
}
