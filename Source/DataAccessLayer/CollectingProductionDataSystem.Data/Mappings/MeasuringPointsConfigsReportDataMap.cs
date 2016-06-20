namespace CollectingProductionDataSystem.Data.Mappings
{
    using System.Data.Entity.Infrastructure.Annotations;
    using CollectingProductionDataSystem.Models.Transactions;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    public class MeasuringPointsConfigsReportDataMap : EntityTypeConfiguration<MeasuringPointsConfigsReportData>
    {
        public MeasuringPointsConfigsReportDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

             this.Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(u => u.ProductId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("Ix_Measuring_Points_Configs_Report_Data", 1)))
                .IsRequired();

            this.Property(u => u.Direction)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("Ix_Measuring_Points_Configs_Report_Data", 2)))
                .IsRequired();

            this.Property(u => u.RecordTimestamp)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("Ix_Measuring_Points_Configs_Report_Data", 3)))
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("MeasuringPointsConfigsReportDatas");
        }
    }
}
