namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Inventories;
    using System.ComponentModel.DataAnnotations.Schema;

    public class InProcessUnitDataMap : EntityTypeConfiguration<InProcessUnitData>
    {
        public InProcessUnitDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //index
            this.Property(t => t.RecordTimestamp)
              .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_timestamp_processUnit_productId", 1) { IsUnique = true }))
              .IsRequired();

            this.Property(t => t.ProcessUnitId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_timestamp_processUnit_productId", 2) { IsUnique = true }))
                .IsRequired();
            this.Property(t => t.ProductId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_timestamp_processUnit_productId", 3) { IsUnique = true }))
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("InProcessUnitDatas");

            // Relationships
            this.HasRequired(t => t.Product)
                .WithMany(t => t.InProcessUnitDatas)
                .HasForeignKey(d => d.ProductId)
                .WillCascadeOnDelete(false);
            this.HasRequired(t => t.ProcessUnit)
                .WithMany(t => t.InProcessUnitDatas)
                .HasForeignKey(d => d.ProcessUnitId)
                .WillCascadeOnDelete(false);
        }
    }
}
