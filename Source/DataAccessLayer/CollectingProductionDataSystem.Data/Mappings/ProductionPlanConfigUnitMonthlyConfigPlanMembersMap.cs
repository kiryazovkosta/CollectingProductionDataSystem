namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;

    public class ProductionPlanConfigUnitMonthlyConfigPlanMembersMap:EntityTypeConfiguration<ProductionPlanConfigUnitMonthlyConfigPlanMembers>
    {
        public ProductionPlanConfigUnitMonthlyConfigPlanMembersMap()
        {
            // Key
            this.HasKey(t => (new { ProductionPlanConfigId = t.ProductionPlanConfigId, UnitMonthlyConfigId = t.UnitMonthlyConfigId }));

            // Relationships
            this.HasRequired(p => p.ProductionPlanConfig).WithMany().HasForeignKey(y => y.ProductionPlanConfigId).WillCascadeOnDelete(false);
            this.HasRequired(p => p.UnitMonthlyConfig).WithMany().HasForeignKey(y => y.UnitMonthlyConfigId).WillCascadeOnDelete(false);
        }
    }
}
