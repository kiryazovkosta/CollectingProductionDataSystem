namespace CollectingProductionDataSystem.Data.Mappings
{
    using CollectingProductionDataSystem.Models.Inventories;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TankStatusMap : EntityTypeConfiguration<TankStatus>
    {
        public TankStatusMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.FlagValue)
                .IsRequired();

            this.Property(t => t.Name)
                .HasMaxLength(50)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TankStatuses");
        }
    }
}
