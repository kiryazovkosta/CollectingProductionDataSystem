using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CollectingProductionDataSystem.Models.Productions;
using System.Data.Entity.Infrastructure.Annotations;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class UnitsInspectionDataMap : EntityTypeConfiguration<UnitsInspectionData>
    {
        public UnitsInspectionDataMap()
        {
            // Primary Key
            this.HasKey(u => u.Id);

            // Properties
            this.Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(u => u.RecordTimestamp)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_unitpoint_log", 1)))
                .IsRequired();

            this.Property(u => u.UnitConfigId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_unitpoint_log", 2)))
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("UnitsInspectionDatas");

            // Relationships
            this.HasRequired(u => u.Unit)
                .WithMany(u => u.UnitsInspectionDatas)
                .HasForeignKey(u => u.UnitConfigId);

        }
    }
}
