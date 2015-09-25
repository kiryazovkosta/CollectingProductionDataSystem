using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SteelDbModel.Models.Mapping
{
    public class TankConfigMap : EntityTypeConfiguration<TankConfig>
    {
        public TankConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("TankConfigs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ParkId).HasColumnName("ParkId");
            this.Property(t => t.ControlPoint).HasColumnName("ControlPoint");
            this.Property(t => t.TankName).HasColumnName("TankName");
            this.Property(t => t.PhdTagProductId).HasColumnName("PhdTagProductId");
            this.Property(t => t.PhdTagProductName).HasColumnName("PhdTagProductName");
            this.Property(t => t.PhdTagLiquidLevel).HasColumnName("PhdTagLiquidLevel");
            this.Property(t => t.LiquidLevelLowExtreme).HasColumnName("LiquidLevelLowExtreme");
            this.Property(t => t.LiquidLevelHighExtreme).HasColumnName("LiquidLevelHighExtreme");
            this.Property(t => t.PhdTagProductLevel).HasColumnName("PhdTagProductLevel");
            this.Property(t => t.ProductLevelLowExtreme).HasColumnName("ProductLevelLowExtreme");
            this.Property(t => t.ProductLevelHighExtreme).HasColumnName("ProductLevelHighExtreme");
            this.Property(t => t.PhdTagFreeWaterLevel).HasColumnName("PhdTagFreeWaterLevel");
            this.Property(t => t.FreeWaterLevelLowExtreme).HasColumnName("FreeWaterLevelLowExtreme");
            this.Property(t => t.FreeWaterLevelHighExtreme).HasColumnName("FreeWaterLevelHighExtreme");
            this.Property(t => t.PhdTagFreeWaterVolume).HasColumnName("PhdTagFreeWaterVolume");
            this.Property(t => t.FreeWaterVolumeLowExtreme).HasColumnName("FreeWaterVolumeLowExtreme");
            this.Property(t => t.FreeWaterVolumeHighExtreme).HasColumnName("FreeWaterVolumeHighExtreme");
            this.Property(t => t.PhdTagObservableDensity).HasColumnName("PhdTagObservableDensity");
            this.Property(t => t.ObservableDensityLowExtreme).HasColumnName("ObservableDensityLowExtreme");
            this.Property(t => t.ObservableDensityHighExtreme).HasColumnName("ObservableDensityHighExtreme");
            this.Property(t => t.PhdTagReferenceDensity).HasColumnName("PhdTagReferenceDensity");
            this.Property(t => t.ReferenceDensityLowExtreme).HasColumnName("ReferenceDensityLowExtreme");
            this.Property(t => t.ReferenceDensityHighExtreme).HasColumnName("ReferenceDensityHighExtreme");
            this.Property(t => t.PhdTagGrossObservableVolume).HasColumnName("PhdTagGrossObservableVolume");
            this.Property(t => t.GrossObservableVolumeLowExtreme).HasColumnName("GrossObservableVolumeLowExtreme");
            this.Property(t => t.GrossObservableVolumeHighExtreme).HasColumnName("GrossObservableVolumeHighExtreme");
            this.Property(t => t.PhdTagGrossStandardVolume).HasColumnName("PhdTagGrossStandardVolume");
            this.Property(t => t.GrossStandardVolumeLowExtreme).HasColumnName("GrossStandardVolumeLowExtreme");
            this.Property(t => t.GrossStandardVolumeHighExtreme).HasColumnName("GrossStandardVolumeHighExtreme");
            this.Property(t => t.PhdTagNetStandardVolume).HasColumnName("PhdTagNetStandardVolume");
            this.Property(t => t.NetStandardVolumeLowExtreme).HasColumnName("NetStandardVolumeLowExtreme");
            this.Property(t => t.NetStandardVolumeHighExtreme).HasColumnName("NetStandardVolumeHighExtreme");
            this.Property(t => t.PhdTagWeightInAir).HasColumnName("PhdTagWeightInAir");
            this.Property(t => t.WeightInAirLowExtreme).HasColumnName("WeightInAirLowExtreme");
            this.Property(t => t.WeightInAirHighExtreme).HasColumnName("WeightInAirHighExtreme");
            this.Property(t => t.PhdTagAverageTemperature).HasColumnName("PhdTagAverageTemperature");
            this.Property(t => t.AverageTemperatureLowExtreme).HasColumnName("AverageTemperatureLowExtreme");
            this.Property(t => t.AverageTemperatureHighExtreme).HasColumnName("AverageTemperatureHighExtreme");
            this.Property(t => t.PhdTagTotalObservableVolume).HasColumnName("PhdTagTotalObservableVolume");
            this.Property(t => t.TotalObservableVolumeLowExtreme).HasColumnName("TotalObservableVolumeLowExtreme");
            this.Property(t => t.TotalObservableVolumeHighExtreme).HasColumnName("TotalObservableVolumeHighExtreme");
            this.Property(t => t.PhdTagWeightInVacuum).HasColumnName("PhdTagWeightInVacuum");
            this.Property(t => t.WeightInVacuumLowExtreme).HasColumnName("WeightInVacuumLowExtreme");
            this.Property(t => t.WeightInVacuumHighExtreme).HasColumnName("WeightInVacuumHighExtreme");
            this.Property(t => t.PhdTagMaxVolume).HasColumnName("PhdTagMaxVolume");
            this.Property(t => t.MaxVolumeLowExtreme).HasColumnName("MaxVolumeLowExtreme");
            this.Property(t => t.MaxVolumeHighExtreme).HasColumnName("MaxVolumeHighExtreme");
            this.Property(t => t.PhdTagAvailableRoom).HasColumnName("PhdTagAvailableRoom");
            this.Property(t => t.AvailableRoomLowExtreme).HasColumnName("AvailableRoomLowExtreme");
            this.Property(t => t.AvailableRoomHighExtreme).HasColumnName("AvailableRoomHighExtreme");
            this.Property(t => t.UnusableResidueLevel).HasColumnName("UnusableResidueLevel");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.DeletedOn).HasColumnName("DeletedOn");
            this.Property(t => t.DeletedFrom).HasColumnName("DeletedFrom");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.ModifiedOn).HasColumnName("ModifiedOn");
            this.Property(t => t.CreatedFrom).HasColumnName("CreatedFrom");
            this.Property(t => t.ModifiedFrom).HasColumnName("ModifiedFrom");

            // Relationships
            this.HasRequired(t => t.Park)
                .WithMany(t => t.TankConfigs)
                .HasForeignKey(d => d.ParkId);

        }
    }
}
