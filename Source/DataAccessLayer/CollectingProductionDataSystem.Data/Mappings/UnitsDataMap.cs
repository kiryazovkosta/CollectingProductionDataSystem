using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using CollectingProductionDataSystem.Models.Productions;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class UnitsDataMap : EntityTypeConfiguration<UnitsData>
    {
        public UnitsDataMap()
        {
            // Primary Key
            this.HasKey(u => u.Id);

            // Properties
            this.Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(u => u.RecordTimestamp)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_unitdata_log", 1)))
                .IsRequired();

            this.Property(u => u.ShiftId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_unitdata_log", 2)))
                .IsRequired();

            this.Property(u => u.UnitConfigId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_unitdata_log", 3)))
                .IsRequired();


            // Table & Column Mappings
            this.ToTable("UnitsDatas");

            // Relationships
            this.HasRequired(u => u.UnitConfig)
                .WithMany(u => u.UnitsDatas)
                .HasForeignKey(d => d.UnitConfigId).WillCascadeOnDelete(false);

            this.HasOptional(t => t.UnitsManualData)
                .WithRequired(t => t.UnitsData).WillCascadeOnDelete(false);
        }
    }
}
