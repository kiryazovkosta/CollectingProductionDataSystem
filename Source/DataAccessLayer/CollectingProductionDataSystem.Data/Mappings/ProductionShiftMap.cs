namespace CollectingProductionDataSystem.Data.Mappings
{
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ProductionShiftMap : EntityTypeConfiguration<ProductionShift>
    {
        public ProductionShiftMap()
        {
            // Primary Key
            this.HasKey(x => x.Id);

            // Properties
            this.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(16);

            this.Property(x => x.BeginTime)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(x => x.EndTime)
                .IsRequired()
                .HasMaxLength(5);

            this.Property(x => x.OffsetMinutes)
                .IsRequired();

            this.Property(x => x.EndTime)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("ProductionShifts");
        }
    }
}
