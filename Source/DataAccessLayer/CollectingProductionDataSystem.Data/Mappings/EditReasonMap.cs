namespace CollectingProductionDataSystem.Data.Mappings
{
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using System.Data.Entity.ModelConfiguration;

    public class EditReasonMap : EntityTypeConfiguration<EditReason>
    {
        public EditReasonMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(80);

            // Table & Column Mappings
            this.ToTable("EditReasons");
        }
    }
}
