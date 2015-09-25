using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SteelDbModel.Models.Mapping
{
    public class UnitsDataMap : EntityTypeConfiguration<UnitsData>
    {
        public UnitsDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("UnitsDatas");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RecordTimestamp).HasColumnName("RecordTimestamp");
            this.Property(t => t.UnitConfigId).HasColumnName("UnitConfigId");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.IsApproved).HasColumnName("IsApproved");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.ModifiedOn).HasColumnName("ModifiedOn");
            this.Property(t => t.CreatedFrom).HasColumnName("CreatedFrom");
            this.Property(t => t.ModifiedFrom).HasColumnName("ModifiedFrom");
            this.Property(t => t.ShiftId).HasColumnName("ShiftId");

            // Relationships
            this.HasRequired(t => t.UnitsConfig)
                .WithMany(t => t.UnitsDatas)
                .HasForeignKey(d => d.UnitConfigId);

        }
    }
}
