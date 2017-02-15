namespace CollectingProductionDataSystem.Data.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using CollectingProductionDataSystem.Models.Productions;

    public class UnitConfigMap : EntityTypeConfiguration<UnitConfig>
    {
        public UnitConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CollectingDataMechanism)
                .HasMaxLength(2);

            this.Property(t => t.PreviousShiftTag)
                .HasMaxLength(50);

            this.Property(t => t.Notes)
                .HasMaxLength(200);

            this.Ignore(t => t.HistorycalProcessUnit);

            // Table & Column Mappings
            this.ToTable("UnitConfigs");

            // Relationships
            //this.HasMany(t => t.UnitsDailyConfigs)
            //    .WithMany(t => t.UnitConfigs);
            //this.HasRequired(t => t.UnitsDailyConfigs).WithMany().WillCascadeOnDelete(false);

            this.HasRequired(t => t.Direction)
                .WithMany(t => t.UnitConfigs)
                .HasForeignKey(d => d.DirectionId);

            this.HasRequired(t => t.MaterialType)
                .WithMany(t => t.Units)
                .HasForeignKey(d => d.MaterialTypeId);

            this.HasRequired(t => t.MeasureUnit)
                .WithMany(t => t.UnitsConfigs)
                .HasForeignKey(d => d.MeasureUnitId);

            this.HasRequired(t => t.ProcessUnit)
                .WithMany(t => t.UnitsConfigs)
                .HasForeignKey(d => d.ProcessUnitId);

            this.HasRequired(t => t.Product)
                .WithMany(t => t.UnitConfigs)
                .HasForeignKey(d => d.ProductId);

            this.HasOptional(t => t.ShiftProductType)
                .WithMany(t => t.UnitConfigs)
                .HasForeignKey(d => d.ShiftProductTypeId);

            this.HasMany(t => t.RelatedUnitConfigs);

            this.HasMany(t => t.UnitConfigUnitDailyConfigs)
                .WithRequired()
                .HasForeignKey(x=>x.UnitConfigId);

            this.HasOptional(t => t.EnteredMeasureUnit)
                .WithMany(t => t.EnteredUnitConfigs)
                .HasForeignKey(d => d.EnteredMeasureUnitId);
        }
    }
}
