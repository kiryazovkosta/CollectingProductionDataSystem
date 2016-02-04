namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;

    public class MonthlyReportTypeMap : EntityTypeConfiguration<MonthlyReportType>
    {
        public MonthlyReportTypeMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("MonthlyReportTypes");
        }
    }
}