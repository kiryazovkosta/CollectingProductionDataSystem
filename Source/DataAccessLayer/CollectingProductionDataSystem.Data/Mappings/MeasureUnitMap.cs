namespace CollectingProductionDataSystem.Data.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public class MeasureUnitMap : EntityTypeConfiguration<MeasureUnit>
    {
        public MeasureUnitMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(8);

            // Table & Column Mappings
            this.ToTable("MeasureUnits");
 
        }
    }
}
