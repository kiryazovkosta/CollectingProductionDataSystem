namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Productions;
    using System.ComponentModel.DataAnnotations.Schema;

    public class RelatedUnitDailyConfigMap : EntityTypeConfiguration<RelatedUnitDailyConfigs>
    {
        public RelatedUnitDailyConfigMap()
        {
            this.HasKey(t => new { t.UnitsDailyConfigId, t.RelatedUnitsDailyConfigId });

            this.HasRequired(p => p.RelatedUnitsDailyConfig).WithMany().WillCascadeOnDelete(false);

            //this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
