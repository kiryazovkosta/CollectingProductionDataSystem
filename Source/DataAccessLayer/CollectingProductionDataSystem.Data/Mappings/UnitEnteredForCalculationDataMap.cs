namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Productions;

    public class UnitEnteredForCalculationDataMap : EntityTypeConfiguration<UnitEnteredForCalculationData>
    {
        public UnitEnteredForCalculationDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("UnitEnteredForCalculationDatas");

            // Relationships
            this.HasRequired(t => t.UnitsData)
                .WithMany(t => t.UnitEnteredForCalculationData)
                .HasForeignKey(d => d.UnitDataId);
        }
    }
}
