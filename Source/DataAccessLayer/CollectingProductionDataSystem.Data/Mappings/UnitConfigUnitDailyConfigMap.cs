/// <summary>
/// Summary description for UnitConfigUnitDailyConfigMap
/// </summary>
namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Productions;

    public class UnitConfigUnitDailyConfigMap : EntityTypeConfiguration<UnitConfigUnitDailyConfig>
    {
        public UnitConfigUnitDailyConfigMap()
        {
            // Key
            this.HasKey(t => (new { UnitConfigId = t.UnitConfigId, UnitDailyConfigId = t.UnitDailyConfigId }));

            // Relationships
            this.HasRequired(p => p.UnitConfig).WithMany().HasForeignKey(y => y.UnitConfigId).WillCascadeOnDelete(false);
            this.HasRequired(p => p.UnitDailyConfig).WithMany().HasForeignKey(y=>y.UnitDailyConfigId).WillCascadeOnDelete(false);
        }
    }
}
