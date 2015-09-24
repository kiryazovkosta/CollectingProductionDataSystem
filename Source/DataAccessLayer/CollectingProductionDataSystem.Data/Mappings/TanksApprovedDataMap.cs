namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using CollectingProductionDataSystem.Models.Inventories;
    using System.Data.Entity.Infrastructure.Annotations;

    public class TanksApprovedDataMap : EntityTypeConfiguration<TanksApprovedData>
    {
        public TanksApprovedDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.RecordDate)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_tanks_confirmed_data", 1)))
                .IsRequired();

            this.Property(t => t.ShiftId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_tanks_confirmed_data", 2)))
                .IsRequired();

            this.Property(t => t.ParkId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_tanks_confirmed_data", 3)))
                .IsRequired();

            this.Property(t => t.Approved)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TanksApprovedDatas");
        }
    }
}
