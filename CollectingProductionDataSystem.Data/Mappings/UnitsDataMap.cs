using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class UnitsDataMap : EntityTypeConfiguration<UnitsData>
    {
        public UnitsDataMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.RecordTimestamp });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("UnitsData");

            // Relationships
            this.HasRequired(t => t.Unit)
                .WithMany(t => t.UnitsDatas)
                .HasForeignKey(d => d.Id);

        }
    }
}
