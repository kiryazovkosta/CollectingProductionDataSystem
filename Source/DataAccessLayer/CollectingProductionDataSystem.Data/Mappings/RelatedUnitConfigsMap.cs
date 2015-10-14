namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Productions;

    public class RelatedUnitConfigsMap : EntityTypeConfiguration<RelatedUnitConfigs>
    {
        public RelatedUnitConfigsMap()
        {
            this.HasKey(t => new { t.UnitConfigId, t.RelatedUnitConfigId });

            this.HasRequired(p => p.RelatedUnitConfig).WithMany().WillCascadeOnDelete(false);
        }
    }
}
