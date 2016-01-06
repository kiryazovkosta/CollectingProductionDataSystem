
namespace CollectingProductionDataSystem.Data.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using CollectingProductionDataSystem.Models.Productions;

    public class UnitsDailyConfigMap : EntityTypeConfiguration<UnitDailyConfig>
    {
        public UnitsDailyConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.AggregationFormula)
                .IsRequired()
                .HasMaxLength(2048);

            this.Property(t => t.AggregationMembers)
                .IsRequired()
                .HasMaxLength(2048);

            this.Property(t => t.MaterialTypeId)
                .IsOptional();

            this.Property(t => t.MaterialDetailTypeId)
                .IsOptional();

            // Table & Column Mappings
            this.ToTable("UnitDailyConfigs");

            //this.HasMany(t => t.UnitsConfigs)
            //    .WithMany(t => t.UnitsDailyConfigs);

            // Relationships
            this.HasRequired(t => t.MeasureUnit)
                .WithMany(t => t.UnitsDailyConfigs)
                .HasForeignKey(d => d.MeasureUnitId);

            this.HasRequired(t => t.ProcessUnit)
                .WithMany(t => t.UnitsDailyConfigs)
                .HasForeignKey(d => d.ProcessUnitId);

            this.HasRequired(t => t.DailyProductType)
                .WithMany(t => t.UnitsDailyConfigs)
                .HasForeignKey(d => d.DailyProductTypeId);

            this.HasMany(t => t.RelatedUnitDailyConfigs);

            this.HasMany(t => t.UnitConfigUnitDailyConfigs)
                .WithRequired()
                .HasForeignKey(x=>x.UnitDailyConfigId);

        }
    }
}