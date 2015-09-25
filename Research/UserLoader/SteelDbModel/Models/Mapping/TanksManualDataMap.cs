using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SteelDbModel.Models.Mapping
{
    public class TanksManualDataMap : EntityTypeConfiguration<TanksManualData>
    {
        public TanksManualDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TanksManualDatas");
            this.Property(t => t.Id).HasColumnName("Id");
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
            this.Property(t => t.EditReasonId).HasColumnName("EditReasonId");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.DeletedOn).HasColumnName("DeletedOn");
            this.Property(t => t.DeletedFrom).HasColumnName("DeletedFrom");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.ModifiedOn).HasColumnName("ModifiedOn");
            this.Property(t => t.CreatedFrom).HasColumnName("CreatedFrom");
            this.Property(t => t.ModifiedFrom).HasColumnName("ModifiedFrom");

            // Relationships
            this.HasRequired(t => t.EditReason)
                .WithMany(t => t.TanksManualDatas)
                .HasForeignKey(d => d.EditReasonId);
            this.HasRequired(t => t.TankData)
                .WithOptional(t => t.TanksManualData);

        }
    }
}
