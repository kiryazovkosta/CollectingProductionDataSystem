namespace CollectingProductionDataSystem.Data.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using CollectingProductionDataSystem.Models.Productions;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;

    public class UnitsApprovedDataMap : EntityTypeConfiguration<UnitsApprovedData>
    {
        public UnitsApprovedDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.RecordDate)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_units_confirmed_data", 1)))
                .IsRequired();

            this.Property(t => t.ShiftId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_units_confirmed_data", 2)))
                .IsRequired();

            this.Property(t => t.ProcessUnitId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_units_confirmed_data", 3)))
                .IsRequired();

            this.Property(t => t.Approved)
                .IsRequired();

            // Properties
            // Table & Column Mappings
            this.ToTable("UnitsApprovedDatas");
        }
    }
}
