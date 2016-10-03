namespace CollectingProductionDataSystem.Data.Mappings
{
    using CollectingProductionDataSystem.Models.Productions.Technological;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    class MonthlyTechnologicalReportsDataMap : EntityTypeConfiguration<MonthlyTechnologicalReportsData>
    {
        public MonthlyTechnologicalReportsDataMap()
        {
            // Primary Key
            this.HasKey(u => u.Id);

            // Properties
            this.Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(u => u.Month)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_tech_monthly_report_data", 1)))
                .IsRequired();

            this.Property(u => u.FactoryId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_tech_monthly_report_data", 2)))
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("MonthlyTechnologicalReportsDatas");
        }
    }
}
