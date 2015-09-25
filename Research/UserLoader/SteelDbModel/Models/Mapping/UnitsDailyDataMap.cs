using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SteelDbModel.Models.Mapping
{
    public class UnitsDailyDataMap : EntityTypeConfiguration<UnitsDailyData>
    {
        public UnitsDailyDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("UnitsDailyDatas");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RecordTimestamp).HasColumnName("RecordTimestamp");
            this.Property(t => t.UnitsDailyConfigId).HasColumnName("UnitsDailyConfigId");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.IsApproved).HasColumnName("IsApproved");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.ModifiedOn).HasColumnName("ModifiedOn");
            this.Property(t => t.CreatedFrom).HasColumnName("CreatedFrom");
            this.Property(t => t.ModifiedFrom).HasColumnName("ModifiedFrom");
            this.Property(t => t.HasManualData).HasColumnName("HasManualData");

            // Relationships
            this.HasMany(t => t.UnitsDatas)
                .WithMany(t => t.UnitsDailyDatas)
                .Map(m =>
                    {
                        m.ToTable("UnitsDailyDataUnitsDatas");
                        m.MapLeftKey("UnitsDailyData_Id");
                        m.MapRightKey("UnitsData_Id");
                    });

            this.HasRequired(t => t.UnitsDailyConfig)
                .WithMany(t => t.UnitsDailyDatas)
                .HasForeignKey(d => d.UnitsDailyConfigId);

        }
    }
}
