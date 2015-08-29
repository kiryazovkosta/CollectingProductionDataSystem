namespace CollectingProductionDataSystem.Data.Mappings
{
    using CollectingProductionDataSystem.Models.Productions;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;

    public class UnitsDailyDataMap  : EntityTypeConfiguration<UnitsDailyData>
    {
        public UnitsDailyDataMap()
        {
            // Primary Key
            this.HasKey(u => u.Id);

            // Properties
            this.Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(u => u.RecordTimestamp)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_unit_daily_data", 1)))
                .IsRequired();

            this.Property(u => u.UnitsDailyConfigId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_unit_daily_data", 2)))
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("UnitsDailyDatas");

            // Relationships
            this.HasRequired(u => u.UnitsDailyConfig)
                .WithMany(u => u.UnitsDailyDatas)
                .HasForeignKey(d => d.UnitsDailyConfigId);

            this.HasOptional(t => t.UnitsManualDailyData)
                .WithRequired(t => t.UnitsDailyData);
        }
    }
}
