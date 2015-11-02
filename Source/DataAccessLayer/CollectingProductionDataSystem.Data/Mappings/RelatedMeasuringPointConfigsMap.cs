namespace CollectingProductionDataSystem.Data.Mappings
{
    using CollectingProductionDataSystem.Models.Transactions;
    using System;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;

    public class RelatedMeasuringPointConfigsMap : EntityTypeConfiguration<RelatedMeasuringPointConfigs>
    {
        public RelatedMeasuringPointConfigsMap()
        {
            this.HasKey(t => new { t.MeasuringPointConfigId, t.RelatedMeasuringPointConfigId });

            this.HasRequired(p => p.RelatedMeasuringPointConfig).WithMany().WillCascadeOnDelete(false);
        }
    }
}
