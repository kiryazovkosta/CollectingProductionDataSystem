namespace CollectingProductionDataSystem.Data.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.UtilityEntities;

    public class MessageMap : EntityTypeConfiguration<Message>
    {
        public MessageMap() 
        {
            // Primary Key
            this.HasKey(u => u.Id);

            // Properties
            this.Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(u => u.Id)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_message", 1)))
                .IsRequired();

            this.Property(u => u.ValidUntill)
                .HasColumnAnnotation("ValidUntill", new IndexAnnotation(new IndexAttribute("ix_message", 2)))
                .IsRequired();
        }
    }
}