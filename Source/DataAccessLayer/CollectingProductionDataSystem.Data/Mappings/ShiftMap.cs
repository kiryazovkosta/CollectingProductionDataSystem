/// <summary>
/// Summary description for ShiftsMapping
/// </summary>
namespace CollectingProductionDataSystem.Data.Mappings
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    class ShiftMap:EntityTypeConfiguration<Shift>
    {
        public ShiftMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(80);

            this.Property(t => t.BeginTime)
                .IsRequired();

            this.Property(t => t.ReadOffset)
                .IsRequired();

            this.Property(t => t.ReadPollTimeSlot)
                .IsRequired();


            // Table & Column Mappings
            this.ToTable("Shifts");
        }
    }
}
