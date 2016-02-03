using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Models.Inventories;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class TankReportConfigMap : EntityTypeConfiguration<TankReportConfig>
    {
        public TankReportConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.TankConfigId);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("TankReportConfigs");
        }
    }
}
