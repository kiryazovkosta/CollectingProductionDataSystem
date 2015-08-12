using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CollectingProductionDataSystem.Models.Nomenclatures;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class MeasureUnitMap : EntityTypeConfiguration<MeasureUnit>
    {
        public MeasureUnitMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("MeasureUnits");
 
        }
    }
}
