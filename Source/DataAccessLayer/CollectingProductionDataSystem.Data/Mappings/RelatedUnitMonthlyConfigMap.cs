namespace CollectingProductionDataSystem.Data.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;

    public class RelatedUnitMonthlyConfigMap : EntityTypeConfiguration<RelatedUnitMonthlyConfigs>
    {
        public RelatedUnitMonthlyConfigMap()
        {
            this.HasKey(t => new { t.UnitMonthlyConfigId, t.RelatedUnitMonthlyConfigId });

            this.HasRequired(p => p.RelatedUnitMonthlyConfig).WithMany().WillCascadeOnDelete(false);
        }
    }
}