/// <summary>
/// Summary description for Class1
/// </summary>
namespace CollectingProductionDataSystem.Data.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;

    public class UnitMonthlyConfigMap : EntityTypeConfiguration<UnitMonthlyConfig>
    {
        public UnitMonthlyConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            //Index
            this.Property(t => t.IsDeleted)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_units_monthly_config_data", 1)))
                .IsRequired();
            this.Property(t => t.MonthlyReportTypeId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_units_monthly_config_data", 2)))
                .IsRequired();

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Code)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_units_monthly_code_data", 1) { IsUnique = true }))
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.AggregationFormula)
                //.IsRequired()
                .HasMaxLength(2048);

            //this.Property(t => t.AggregationMembers)
            //    .IsRequired()
            //    .HasMaxLength(2048);

            //this.Property(t => t.MaterialTypeId)
            //    .IsOptional();

            //this.Property(t => t.MaterialDetailTypeId)
            //    .IsOptional();

            // Table & Column Mappings
            this.ToTable("UnitMonthlyConfigs");

            // Relationships
            this.HasRequired(t => t.MeasureUnit)
                .WithMany(t => t.UnitsMonthlyConfigs)
                .HasForeignKey(d => d.MeasureUnitId);

            this.HasRequired(t => t.ProcessUnit)
                .WithMany(t => t.UnitMonthlyConfigs)
                .HasForeignKey(d => d.ProcessUnitId);

            this.HasRequired(t => t.MonthlyReportType)
                .WithMany(t => t.UnitMonthlyConfigs)
                .HasForeignKey(d => d.MonthlyReportTypeId);

            this.HasRequired(t => t.ProductType)
                .WithMany(t => t.UnitsMonthlyConfigs)
                .HasForeignKey(d => d.ProductTypeId);

            this.HasMany(t => t.RelatedUnitMonthlyConfigs);

            this.HasMany(t => t.UnitDailyConfigUnitMonthlyConfigs)
                .WithRequired()
                .HasForeignKey(x => x.UnitMonthlyConfigId);

        }
    }
}
