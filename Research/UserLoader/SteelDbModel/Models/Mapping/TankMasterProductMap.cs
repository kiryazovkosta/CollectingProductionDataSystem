using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SteelDbModel.Models.Mapping
{
    public class TankMasterProductMap : EntityTypeConfiguration<TankMasterProduct>
    {
        public TankMasterProductMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("TankMasterProducts");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TankMasterProductId).HasColumnName("TankMasterProductId");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.DeletedOn).HasColumnName("DeletedOn");
            this.Property(t => t.DeletedFrom).HasColumnName("DeletedFrom");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.ModifiedOn).HasColumnName("ModifiedOn");
            this.Property(t => t.CreatedFrom).HasColumnName("CreatedFrom");
            this.Property(t => t.ModifiedFrom).HasColumnName("ModifiedFrom");
        }
    }
}