using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SteelDbModel.Models.Mapping
{
    public class ProductionPlanConfigMap : EntityTypeConfiguration<ProductionPlanConfig>
    {
        public ProductionPlanConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("ProductionPlanConfigs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Percentages).HasColumnName("Percentages");
            this.Property(t => t.ProcessUnitId).HasColumnName("ProcessUnitId");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.DeletedOn).HasColumnName("DeletedOn");
            this.Property(t => t.DeletedFrom).HasColumnName("DeletedFrom");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.ModifiedOn).HasColumnName("ModifiedOn");
            this.Property(t => t.CreatedFrom).HasColumnName("CreatedFrom");
            this.Property(t => t.ModifiedFrom).HasColumnName("ModifiedFrom");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.QuantityPlanFormula).HasColumnName("QuantityPlanFormula");
            this.Property(t => t.QuantityPlanMembers).HasColumnName("QuantityPlanMembers");
            this.Property(t => t.QuantityFactFormula).HasColumnName("QuantityFactFormula");
            this.Property(t => t.QuantityFactMembers).HasColumnName("QuantityFactMembers");

            // Relationships
            this.HasRequired(t => t.ProcessUnit)
                .WithMany(t => t.ProductionPlanConfigs)
                .HasForeignKey(d => d.ProcessUnitId);

        }
    }
}
