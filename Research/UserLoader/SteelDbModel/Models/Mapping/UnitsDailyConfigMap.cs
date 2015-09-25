using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SteelDbModel.Models.Mapping
{
    public class UnitsDailyConfigMap : EntityTypeConfiguration<UnitsDailyConfig>
    {
        public UnitsDailyConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.AggregationFormula)
                .IsRequired()
                .HasMaxLength(2048);

            this.Property(t => t.AggregationMembers)
                .IsRequired()
                .HasMaxLength(2048);

            // Table & Column Mappings
            this.ToTable("UnitsDailyConfigs");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.ProcessUnitId).HasColumnName("ProcessUnitId");
            this.Property(t => t.ProductTypeId).HasColumnName("ProductTypeId");
            this.Property(t => t.MeasureUnitId).HasColumnName("MeasureUnitId");
            this.Property(t => t.AggregationFormula).HasColumnName("AggregationFormula");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.DeletedOn).HasColumnName("DeletedOn");
            this.Property(t => t.DeletedFrom).HasColumnName("DeletedFrom");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.ModifiedOn).HasColumnName("ModifiedOn");
            this.Property(t => t.CreatedFrom).HasColumnName("CreatedFrom");
            this.Property(t => t.ModifiedFrom).HasColumnName("ModifiedFrom");
            this.Property(t => t.AggregationMembers).HasColumnName("AggregationMembers");
            this.Property(t => t.AggregationCurrentLevel).HasColumnName("AggregationCurrentLevel");

            // Relationships
            this.HasRequired(t => t.MeasureUnit)
                .WithMany(t => t.UnitsDailyConfigs)
                .HasForeignKey(d => d.MeasureUnitId);
            this.HasRequired(t => t.ProcessUnit)
                .WithMany(t => t.UnitsDailyConfigs)
                .HasForeignKey(d => d.ProcessUnitId);
            this.HasRequired(t => t.ProductType)
                .WithMany(t => t.UnitsDailyConfigs)
                .HasForeignKey(d => d.ProductTypeId);

        }
    }
}
