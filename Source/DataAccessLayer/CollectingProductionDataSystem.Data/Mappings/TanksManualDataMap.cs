namespace CollectingProductionDataSystem.Data.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;

    using CollectingProductionDataSystem.Models.Inventories;

    public class TanksManualDataMap : EntityTypeConfiguration<TanksManualData>
    {
        public TanksManualDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TanksManualDatas");

            this.HasOptional(t => t.EditReason)
                .WithMany()
                .HasForeignKey(d => d.EditReasonId);
        }
    }
}
