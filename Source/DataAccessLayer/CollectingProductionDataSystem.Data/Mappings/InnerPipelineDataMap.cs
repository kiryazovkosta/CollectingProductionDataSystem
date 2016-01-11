namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Inventories;

    public class InnerPipelineDataMap : EntityTypeConfiguration<InnerPipelineData>
    {
        public InnerPipelineDataMap()
        {
            // Primary Key
            this.HasKey(t => new { t.RecordTimestamp, t.ProductId });

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
