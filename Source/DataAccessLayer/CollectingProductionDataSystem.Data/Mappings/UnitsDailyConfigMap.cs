
namespace CollectingProductionDataSystem.Data.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using CollectingProductionDataSystem.Models.Productions;

    public class UnitsDailyConfigMap : EntityTypeConfiguration<UnitsDailyConfig>
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

            // Table & Column Mappings
            this.ToTable("UnitsDailyConfigs");

            // Relationships
            this.HasRequired(t => t.MeasureUnit)
                .WithMany(t => t.UnitsDailyConfigs)
                .HasForeignKey(d => d.MeasureUnitId);
            this.HasRequired(t => t.ProcessUnit)
                .WithMany(t => t.UnitsDailyConfigs)
                .HasForeignKey(d => d.ProcessUnitId);
            this.HasRequired(t => t.ProductType)
                .WithMany(t => t.UnitsDailyConfigs)
                .HasForeignKey(d => d.ProductTypeId);
        }
    }
}
