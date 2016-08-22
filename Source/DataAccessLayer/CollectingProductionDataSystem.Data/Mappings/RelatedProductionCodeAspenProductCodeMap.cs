namespace CollectingProductionDataSystem.Data.Mappings
{
    using CollectingProductionDataSystem.Models.PetroleumScheduler;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class RelatedProductionCodeAspenProductCodeMap : EntityTypeConfiguration<RelatedProductionCodeAspenProductCode>
    {
        public RelatedProductionCodeAspenProductCodeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(u => u.Id)
               .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(u => u.ProductCode)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("Ix_product_code_aspen_code", 1)))
                .IsRequired();

            this.Property(u => u.AspenCode)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("Ix_product_code_aspen_code", 2)))
                .IsRequired();

            // Properties
            this.Property(t => t.AspenCode)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("ProductsCodes2AspenCodes");
        }
    }
}
