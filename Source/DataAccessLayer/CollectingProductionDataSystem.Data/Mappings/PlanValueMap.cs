namespace CollectingProductionDataSystem.Data.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using CollectingProductionDataSystem.Models.Productions;

    public class PlanValueMap : EntityTypeConfiguration<PlanValue>
    {
        public PlanValueMap()
        {
            // Primary Key
            this.HasKey(u => u.Id);

            // Properties
            this.Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(u => u.Month)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_plan_value", 1) { IsUnique = true }))
                .IsRequired();

            this.Property(u => u.ProcessUnitId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_plan_value", 2) { IsUnique = true }))
                .IsRequired();


            // Table & Column Mappings
            this.ToTable("PlanValues");

            // Relationships
            this.HasRequired(t => t.ProcessUnit)
                .WithMany(t => t.PlanValues)
                .HasForeignKey(d => d.ProcessUnitId);
        }
    }
}
