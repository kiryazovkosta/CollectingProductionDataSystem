using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Models.Transactions;
using System.Data.Entity.ModelConfiguration;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class ZoneMap : EntityTypeConfiguration<Zone>
    {
        public ZoneMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Zones");

            // Relationships
            this.HasRequired(t => t.Ikunk)
                .WithMany(t => t.Zones)
                .HasForeignKey(d => d.IkunkId);

        }
    }
}
