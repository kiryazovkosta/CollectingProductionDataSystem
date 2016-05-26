namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Productions;

    public class RelatedProductionPlanConfigsMap : EntityTypeConfiguration<RelatedProductionPlanConfigs>
    {
        public RelatedProductionPlanConfigsMap()
        {
            this.HasKey(t => new { t.ProductionPlanConfigId, t.RelatedProductionPlanConfigId });

            this.HasRequired(p => p.RelatedProductionPlanConfig).WithMany().WillCascadeOnDelete(false);

            //this.Property(x => x.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
        }
    }
}
