using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class UnitsInspectionDataMap : EntityTypeConfiguration<UnitsInspectionData>
    {
        public UnitsInspectionDataMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.RecordTimestamp });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("UnitsInspectionData");

            // Relationships
            this.HasRequired(t => t.Unit)
                .WithMany(t => t.UnitsInspectionDatas)
                .HasForeignKey(d => d.Id);

        }
    }
}
