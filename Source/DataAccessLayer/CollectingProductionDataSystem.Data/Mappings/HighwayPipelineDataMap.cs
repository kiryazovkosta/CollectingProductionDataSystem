namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Productions.HighwayPipelines;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;

    public class HighwayPipelineDataMap : EntityTypeConfiguration<HighwayPipelineData>
    {
        public HighwayPipelineDataMap()
        {
            // Primary Key
            this.HasKey(u => u.Id);

            // Properties
            this.Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(u => u.RecordTimestamp)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_highway_pipe_data", 1)))
                .IsRequired();

            this.Property(u => u.HighwayPipelineConfigId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_highway_pipe_data", 2)))
                .IsRequired();


            // Table & Column Mappings
            this.ToTable("HighwayPipelineData");

            // Relationships
            this.HasRequired(u => u.HighwayPipelineConfig)
                .WithMany(u => u.HighwayPipelineDatas)
                .HasForeignKey(d => d.HighwayPipelineConfigId)
                .WillCascadeOnDelete(false);

            this.HasOptional(t => t.HighwayPipelineManualData)
                .WithRequired(t => t.HighwayPipelineData)
                .WillCascadeOnDelete(false);
        }
    }
}
