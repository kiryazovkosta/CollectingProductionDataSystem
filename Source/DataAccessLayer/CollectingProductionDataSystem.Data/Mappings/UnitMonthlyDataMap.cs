namespace CollectingProductionDataSystem.Data.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;

    public class UnitMonthlyDataMap : EntityTypeConfiguration<UnitMonthlyData>
    {
        public UnitMonthlyDataMap()
        {
            // Primary Key
            this.HasKey(u => u.Id);

            // Properties
            this.Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(u => u.RecordTimestamp)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_unit_monthly_data", 1)))
                .IsRequired();

            this.Property(u => u.UnitMonthlyConfigId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_unit_monthly_data", 2)))
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("UnitMonthlyDatas");

           // Relationships
            this.HasRequired(u => u.UnitMonthlyConfig)
                .WithMany(u => u.UnitMonthlyDatas)
                .HasForeignKey(d => d.UnitMonthlyConfigId)
                .WillCascadeOnDelete(false);

            this.HasOptional(t => t.UnitManualMonthlyData)
                .WithRequired(t => t.UnitMonthlyData)
                .WillCascadeOnDelete(false);

            this.HasOptional(t => t.UnitRecalculatedMonthlyData)
                .WithRequired(t => t.UnitMonthlyData)
                .WillCascadeOnDelete(false);
        }
    }
}