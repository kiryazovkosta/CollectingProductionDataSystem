using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Models.Transactions.HighwayPipelines;

namespace CollectingProductionDataSystem.Data.Mappings
{
    class HighwayPipelineApprovedDataMap : EntityTypeConfiguration<HighwayPipelineApprovedData>
    {
        public HighwayPipelineApprovedDataMap()
        {
            // Primary Key
            this.HasKey(t => t.RecordDate );

            // Properties
            this.Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Approved)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("HighwayPipelineApprovedDatas");
        }
    }
}
