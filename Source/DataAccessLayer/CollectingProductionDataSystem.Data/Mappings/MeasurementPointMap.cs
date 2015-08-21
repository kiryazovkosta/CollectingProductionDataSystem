using CollectingProductionDataSystem.Models.Transactions;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class MeasurementPointMap : EntityTypeConfiguration<MeasurementPoint>
    {
        public MeasurementPointMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ControlPoint)
                .IsRequired()
                .HasMaxLength(8);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("MeasurementPoints");

            // Relationships
            this.HasRequired(t => t.TransportType)
                .WithMany(t => t.MeasurementPoints)
                .HasForeignKey(d => d.TransportTypeId);
            this.HasRequired(t => t.Zone)
                .WithMany(t => t.MeasurementPoints)
                .HasForeignKey(d => d.ZoneId);

        }
    }
}
