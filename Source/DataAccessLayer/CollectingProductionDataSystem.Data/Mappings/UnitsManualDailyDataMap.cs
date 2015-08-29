namespace CollectingProductionDataSystem.Data.Mappings
{
    using CollectingProductionDataSystem.Models.Productions;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public partial class UnitsManualDailyDataMap: EntityTypeConfiguration<UnitsManualDailyData>
    {
        public UnitsManualDailyDataMap()
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
            this.ToTable("UnitsManualDailyDatas");
        }

    }
}
