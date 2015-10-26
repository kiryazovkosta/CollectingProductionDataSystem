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
            this.HasKey(t => new { t.ProcessUnitId, t.RecordDate, t.ShiftId, });

            this.Property(t => t.Approved)
                .IsRequired();

            // Properties

            this.Property(p => p.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("UnitsApprovedDatas");
        }
    }
}
