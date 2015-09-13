namespace CollectingProductionDataSystem.Data.Mappings
{
    using CollectingProductionDataSystem.Models.Productions;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;

    public class UnitsApprovedDailyDataMap : EntityTypeConfiguration<UnitsApprovedDailyData>
    {
        public UnitsApprovedDailyDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.RecordDate)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_units_confirmed__daily_data", 1)))
                .IsRequired();

            this.Property(t => t.ProcessUnitId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_units_confirmed__daily_data", 2)))
                .IsRequired();

            this.Property(t => t.Approved)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("UnitsApprovedDailyDatas");
        }
    }
}
