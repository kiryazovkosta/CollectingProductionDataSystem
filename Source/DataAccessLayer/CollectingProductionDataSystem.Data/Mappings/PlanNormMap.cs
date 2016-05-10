namespace CollectingProductionDataSystem.Data.Mappings
{
    using CollectingProductionDataSystem.Models.Productions;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PlanNormMap : EntityTypeConfiguration<PlanNorm>
    {
        public PlanNormMap()
        {
            // Primary Key
            this.HasKey(u => u.Id);

            // Properties
            this.Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(u => u.Month)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_production_plan", 1){IsUnique = true}))
                .IsRequired();

            this.Property(u => u.ProductionPlanConfigId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_production_plan", 2) { IsUnique = true }))
                .IsRequired();


            // Table & Column Mappings
            this.ToTable("PlanNorms");

            // Relationships
            this.HasRequired(t => t.ProductionPlanConfig)
                .WithMany(t => t.PlanNorms)
                .HasForeignKey(d => d.ProductionPlanConfigId);
        }
    }
}

