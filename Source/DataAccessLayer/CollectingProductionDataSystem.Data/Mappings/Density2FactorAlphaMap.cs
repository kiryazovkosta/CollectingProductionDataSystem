using CollectingProductionDataSystem.Models.Productions.Qpt;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class Density2FactorAlphaMap : EntityTypeConfiguration<Density2FactorAlpha>
    {
        public Density2FactorAlphaMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Density)
                .IsRequired();

            this.Property(t => t.FactorAlpha)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("Density2FactorAlphas");
        }
    }
}
