namespace CollectingProductionDataSystem.Data.Mappings
{
    using CollectingProductionDataSystem.Models.Productions;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;

    public class UnitsManualDataMap : EntityTypeConfiguration<UnitsManualData>
    {
        public UnitsManualDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.HasRequired(u => u.EditReason)
                .WithMany()
                .HasForeignKey(d => d.EditReasonId);

            // Table & Column Mappings
            this.ToTable("UnitsManualDatas");
        }

    }
}
