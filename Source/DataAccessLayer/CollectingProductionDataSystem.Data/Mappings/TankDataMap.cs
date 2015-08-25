using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CollectingProductionDataSystem.Models.Inventories;
using System.Data.Entity.Infrastructure.Annotations;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class TankDataMap : EntityTypeConfiguration<TankData>
    {
        public TankDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.RecordTimestamp)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_tankdata_log", 1)))
                .IsRequired();

            this.Property(t => t.TankConfigId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_tankdata_log", 2)))
                .IsRequired();

            this.Property(t => t.ParkId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_tankdata_log", 3)))
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TankDatas");

            // Relationships
            this.HasRequired(t => t.TankConfig)
                .WithMany(t => t.TankDatas)
                .HasForeignKey(t => t.TankConfigId);
            this.HasOptional(t => t.Product)
                .WithMany(t => t.TanksDatas)
                .HasForeignKey(d => d.ProductId);
            this.HasOptional(t => t.EditReason)
                .WithMany()
                .HasForeignKey(d => d.EditReasonId);
            
            this.HasOptional(t => t.TanksManualData)
                .WithRequired(t => t.TankData);
        }
    }
}
