namespace CollectingProductionDataSystem.Data.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;

    public class UnitApprovedMonthlyDataMap : EntityTypeConfiguration<UnitApprovedMonthlyData>
    {                                                                 
        public UnitApprovedMonthlyDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.RecordDate)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_units_confirmed__monthly_data", 1)))
                .IsRequired();

            this.Property(t => t.MonthlyProductTypeId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_units_confirmed__monthly_data", 2)))
                .IsRequired();

            this.Property(t => t.Approved)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("UnitApprovedMonthlyDatas");

            //Relations

            this.HasRequired(p => p.MonthlyProductType)
                .WithMany(p => p.UnitApprovedMonthlyDatas)
                .HasForeignKey(p => p.MonthlyProductTypeId);
        }
    }
}