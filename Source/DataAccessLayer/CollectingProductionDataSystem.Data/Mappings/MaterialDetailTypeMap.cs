/// <summary>
/// Summary description for MaterialDetailTypeMap
/// </summary>
namespace CollectingProductionDataSystem.Data.Mappings
{
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class MaterialDetailTypeMap : EntityTypeConfiguration<MaterialDetailType>
    {
        public MaterialDetailTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            //Relations
            this.HasMany(x => x.UnitDailyConfigs)
                .WithOptional(x=>x.MaterialDetailType)
                .HasForeignKey(y=>y.MaterialDetailTypeId);

            // Properties
            // Table & Column Mappings
            this.ToTable("MaterialDetailTypes");
            this.Property(t => t.Id).HasColumnName("Id"); 
        }
    }
}
