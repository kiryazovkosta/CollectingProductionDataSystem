
namespace CollectingProductionDataSystem.Data.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using CollectingProductionDataSystem.Models.Productions;

    public class UnitsAggregateDailyConfigMap : EntityTypeConfiguration<UnitsAggregateDailyConfig>
    {
        public UnitsAggregateDailyConfigMap()
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
                .HasMaxLength(512);

            // Table & Column Mappings
            this.ToTable("UnitsAggregateDailyConfigs");

            // Relationships
            this.HasRequired(t => t.ProcessUnit)
                .WithMany(t => t.UnitsAggregateDailyConfigs)
                .HasForeignKey(d => d.ProcessUnitId);
        }
    }
}
