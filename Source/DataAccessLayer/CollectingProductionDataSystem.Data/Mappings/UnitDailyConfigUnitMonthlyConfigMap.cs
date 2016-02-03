namespace CollectingProductionDataSystem.Data.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;

    public class UnitDailyConfigUnitMonthlyConfigMap : EntityTypeConfiguration<UnitDailyConfigUnitMonthlyConfig>
    {
        public UnitDailyConfigUnitMonthlyConfigMap()
        {
            // Key
            this.HasKey(t => (new { UnitDailyConfigId = t.UnitDailyConfigId, UnitMonthlyConfigId = t.UnitMonthlyConfigId }));

            // Relationships
            this.HasRequired(p => p.UnitDailyConfig).WithMany().HasForeignKey(y => y.UnitDailyConfigId).WillCascadeOnDelete(false);
            this.HasRequired(p => p.UnitMonthlyConfig).WithMany().HasForeignKey(y => y.UnitMonthlyConfigId).WillCascadeOnDelete(false);
        }
    }
}