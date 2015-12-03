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

    public class ProductionPlanDataMap : EntityTypeConfiguration<ProductionPlanData>
    {
        public ProductionPlanDataMap() 
        {
            // Primary Key
            this.HasKey(u => u.Id);

            // Properties
            this.Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(u => u.RecordTimestamp)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_production_plan_data", 1)))
                .IsRequired();

            this.Property(u => u.FactoryId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_production_plan_data", 2)))
                .IsRequired();

            this.Property(u => u.ProcessUnitId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_production_plan_data", 3)))
                .IsRequired();

            this.Property(u => u.ProductionPlanConfigId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_production_plan_data", 4)))
                .IsRequired();

            this.Property(u => u.Name)
                .HasMaxLength(80)
                .IsRequired();


            // Table & Column Mappings
            this.ToTable("ProductionPlanDatas");

            // Relationships
            this.HasRequired(t => t.ProductionPlanConfig)
                .WithMany(t => t.ProductionPlanDatas)
                .HasForeignKey(d => d.ProductionPlanConfigId);
        }
    }
}
