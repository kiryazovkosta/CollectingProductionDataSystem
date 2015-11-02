namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Productions;

    class RelatedUnitDailyConfigMap : EntityTypeConfiguration<RelatedUnitDailyConfigs>
    {
        public RelatedUnitDailyConfigMap()
        {
            this.HasKey(t => new { t.UnitsDailyConfigId, t.RelatedUnitsDailyConfigId });

            this.HasRequired(p => p.RelatedUnitsDailyConfig).WithMany().WillCascadeOnDelete(false);
        }
    }
}
