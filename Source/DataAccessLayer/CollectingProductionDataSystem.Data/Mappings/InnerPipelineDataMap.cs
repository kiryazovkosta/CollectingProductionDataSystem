namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Inventories;

    public class InnerPipelineDataMap : EntityTypeConfiguration<InnerPipelineData>
    {
        public InnerPipelineDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            //Index
            this.Property(t => t.RecordTimestamp)
              .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_timestamp_productId", 1) { IsUnique = true }))
              .IsRequired();

            this.Property(t => t.ProductId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_timestamp_productId", 2) { IsUnique = true }))
                .IsRequired();

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("InnerPipelineDatas");

            // Relationships
            this.HasRequired(t => t.Product)
                .WithMany(t => t.InnerPipelineDatas)
                .HasForeignKey(d => d.ProductId)
                .WillCascadeOnDelete(false);
        }
    }
}
