namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Inventories;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;

    public class TankStatusDataMap : EntityTypeConfiguration<TankStatusData>
    {
        public TankStatusDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.RecordTimestamp)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_tankstatusdata", 1)))
                .IsRequired();

            this.Property(t => t.TankConfigId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_tankstatusdata", 2)))
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TankStatusDatas");

            // Relationships
            this.HasRequired(t => t.TankConfig)
                .WithMany(t => t.TankStatusDatas)
                .HasForeignKey(t => t.TankConfigId);
        }
    }
}
