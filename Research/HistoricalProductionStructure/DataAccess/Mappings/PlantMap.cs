namespace HistoricalProductionStructure.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Models;

    public class PlantMap : EntityTypeConfiguration<Plant>
    {
        public PlantMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ShortName)
                .IsRequired()
                .HasMaxLength(16);

            this.Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(50);


            // Table & Column Mappings
            ToTable("Plants");
        }
    }
}
