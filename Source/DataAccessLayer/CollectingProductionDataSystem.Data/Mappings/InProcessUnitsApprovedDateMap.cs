using CollectingProductionDataSystem.Models.Inventories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class InProcessUnitsApprovedDateMap: EntityTypeConfiguration<InProcessUnitsApprovedDate>
    {
        public InProcessUnitsApprovedDateMap()
        {
            // Primary Key
            this.HasKey(t => t.RecordDate);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("InProcessUnitsApprovedDates");
        }
    }
}
