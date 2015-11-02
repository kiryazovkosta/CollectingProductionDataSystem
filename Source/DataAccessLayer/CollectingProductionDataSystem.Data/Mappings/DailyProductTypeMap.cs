namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public class DailyProductTypeMap : EntityTypeConfiguration<DailyProductType>
    {
        public DailyProductTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("DailyProductTypes");
        }
    }
}
