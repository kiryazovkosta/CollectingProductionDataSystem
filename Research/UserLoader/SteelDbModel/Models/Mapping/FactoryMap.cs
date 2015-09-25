using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SteelDbModel.Models.Mapping
{
    public class FactoryMap : EntityTypeConfiguration<Factory>
    {
        public FactoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ShortName)
                .IsRequired()
                .HasMaxLength(16);

            this.Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Factories");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.ShortName).HasColumnName("ShortName");
            this.Property(t => t.FullName).HasColumnName("FullName");
            this.Property(t => t.PlantId).HasColumnName("PlantId");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.DeletedOn).HasColumnName("DeletedOn");
            this.Property(t => t.DeletedFrom).HasColumnName("DeletedFrom");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.ModifiedOn).HasColumnName("ModifiedOn");
            this.Property(t => t.CreatedFrom).HasColumnName("CreatedFrom");
            this.Property(t => t.ModifiedFrom).HasColumnName("ModifiedFrom");

            // Relationships
            this.HasOptional(t => t.Plant)
                .WithMany(t => t.Factories)
                .HasForeignKey(d => d.PlantId);

        }
    }
}
