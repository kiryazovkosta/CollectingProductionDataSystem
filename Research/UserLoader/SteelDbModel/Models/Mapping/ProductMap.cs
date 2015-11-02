using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SteelDbModel.Models.Mapping
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Products");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.ProductTypeId).HasColumnName("ProductTypeId");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.DeletedOn).HasColumnName("DeletedOn");
            this.Property(t => t.DeletedFrom).HasColumnName("DeletedFrom");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.ModifiedOn).HasColumnName("ModifiedOn");
            this.Property(t => t.CreatedFrom).HasColumnName("CreatedFrom");
            this.Property(t => t.ModifiedFrom).HasColumnName("ModifiedFrom");

            // Relationships
            this.HasRequired(t => t.ProductType)
                .WithMany(t => t.Products)
                .HasForeignKey(d => d.ProductTypeId);

        }
    }
}
