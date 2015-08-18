using CollectingProductionDataSystem.Models.Productions;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class AggregationsFormulaMap : EntityTypeConfiguration<AggregationsFormula>
    {
        public AggregationsFormulaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Formula)
                .HasMaxLength(250);

            this.Property(t => t.Description)
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("AggregationsFormulas");
        }
    }
}
