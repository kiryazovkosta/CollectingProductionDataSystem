using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using CollectingProductionDataSystem.Models.Nomenclatures;

namespace CollectingProductionDataSystem.Data.Mappings
{
    class PhdConfgMap : EntityTypeConfiguration<PhdConfig>
    {
        public PhdConfgMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(t => t.HostName).HasColumnType("nvarchar").HasMaxLength(50)
               .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_phdconfig", 1) { IsUnique = true }))
               .IsRequired();

            this.Property(u => u.HostIpAddress).HasColumnType("nvarchar").HasMaxLength(15)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_phdconfig", 2) { IsUnique = true }))
                .IsRequired();

            this.Property(u => u.Position).IsRequired();

            // Table & Column Mappings
            this.ToTable("PhdConfigurations");

        }
    }
}
