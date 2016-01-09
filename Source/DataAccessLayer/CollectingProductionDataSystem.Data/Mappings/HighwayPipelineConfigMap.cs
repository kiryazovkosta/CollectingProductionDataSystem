namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.ComponentModel.DataAnnotations.Schema;
    using CollectingProductionDataSystem.Models.Transactions.HighwayPipelines;

    public class HighwayPipelineConfigMap : EntityTypeConfiguration<HighwayPipelineConfig>
    {
        public HighwayPipelineConfigMap()
        {
            // Primary key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(u => u.PipeNumber)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_highway_pipes", 1)))
                .IsRequired();

            this.Property(u => u.ProductId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_highway_pipes", 2)))
                .IsRequired();

            this.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("HighwayPipelineConfigs");

            this.HasRequired(t => t.Product)
                .WithMany(t => t.HighwayPipelineConfigs)
                .HasForeignKey(d => d.ProductId);
        }

    }
}
