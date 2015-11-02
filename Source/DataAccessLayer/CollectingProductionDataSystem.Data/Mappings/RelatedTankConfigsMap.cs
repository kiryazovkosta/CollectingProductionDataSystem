using CollectingProductionDataSystem.Models.Inventories;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class RelatedTankConfigsMap : EntityTypeConfiguration<RelatedTankConfigs>
    {
        public RelatedTankConfigsMap()
        {
            this.HasKey(t => new { t.TankConfigId, t.RelatedTankConfigId });

            this.HasRequired(p => p.RelatedTankConfig).WithMany().WillCascadeOnDelete(false);
        }
    }
}
