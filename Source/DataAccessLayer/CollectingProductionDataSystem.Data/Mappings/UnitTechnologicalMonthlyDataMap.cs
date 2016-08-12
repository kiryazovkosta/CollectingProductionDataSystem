using CollectingProductionDataSystem.Models.Productions.Technological;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class UnitTechnologicalMonthlyDataMap : EntityTypeConfiguration<UnitTechnologicalMonthlyData>
    {
        public UnitTechnologicalMonthlyDataMap()
        {
            // Primary Key
            this.HasKey(u => u.Id);

            // Properties
            this.Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(u => u.RecordTimestamp)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_unit_tech_monthly_data", 1)))
                .IsRequired();

            this.Property(u => u.UnitMonthlyConfigId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_unit__tech_monthly_data", 2)))
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("UnitTechnologicalMonthlyDatas");
        }
    }
}
