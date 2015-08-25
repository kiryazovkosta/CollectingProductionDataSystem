using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CollectingProductionDataSystem.Models.Nomenclatures;
using System.Data.Entity.Infrastructure.Annotations;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class ProductMap : EntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.Code)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_product", 1) ) )
                .IsRequired();

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("Products");

            // Relationships
            this.HasRequired(t => t.ProductType)
                .WithMany(t => t.Products)
                .HasForeignKey(d => d.ProductTypeId);

        }
    }
}
