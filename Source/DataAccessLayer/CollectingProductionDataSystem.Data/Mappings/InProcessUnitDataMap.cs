namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Inventories;
    using System.ComponentModel.DataAnnotations.Schema;

    public class InProcessUnitDataMap : EntityTypeConfiguration<InProcessUnitData>
    {
        public InProcessUnitDataMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Recordimestamp, t.ProcessUnitId, t.ProductId });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            // Table & Column Mappings
            this.ToTable("InProcessUnitDatas");

            // Relationships
            this.HasRequired(t => t.Product)
                .WithMany(t => t.InProcessUnitDatas)
                .HasForeignKey(d => d.ProductId)
                .WillCascadeOnDelete(false);
            this.HasRequired(t => t.ProcessUnit)
                .WithMany(t => t.InProcessUnitDatas)
                .HasForeignKey(d => d.ProcessUnitId)
                .WillCascadeOnDelete(false);
        }
    }
}
