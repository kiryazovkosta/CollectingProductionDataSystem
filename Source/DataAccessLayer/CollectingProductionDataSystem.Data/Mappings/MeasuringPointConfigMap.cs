namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Data.Entity.ModelConfiguration;
    using CollectingProductionDataSystem.Models.Transactions;
    using System.ComponentModel.DataAnnotations.Schema;

    public class MeasuringPointConfigMap : EntityTypeConfiguration<MeasuringPointConfig>
    {
        public MeasuringPointConfigMap()
        {
            // Primary key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.ControlPoint)
                .HasMaxLength(40);

            this.Property(t => t.MeasuringPointName)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.EngineeringUnitMass)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.EngineeringUnitVolume)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.EngineeringUnitDensity)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.EngineeringUnitTemperature)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("MeasuringPointConfigs");

            // Relationships
            this.HasRequired(t => t.Zone)
                .WithMany(t => t.MeasuringPointConfigs)
                .HasForeignKey(d => d.ZoneId);
            this.HasRequired(t => t.TransportType)
                .WithMany(t => t.MeasuringPointConfigs)
                .HasForeignKey(d => d.TransportTypeId);
        }
    }
}
