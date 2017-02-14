namespace HistoricalProductionStructure.DataAccess.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using Domain.Models;

    public class ProcessUnitMap : EntityTypeConfiguration<ProcessUnit>
    {
        public ProcessUnitMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ShortName)
                .IsRequired()
                .HasMaxLength(16);

            this.Property(t => t.FullName)
                .IsRequired()
                .HasMaxLength(52);         

            // Table & Column Mappings
            this.Ignore(t => t.SortableName);
            ToTable("ProcessUnits");     

            // Relationships
            this.HasRequired(t => t.Factory)
                .WithMany(t => t.ProcessUnits)
                .HasForeignKey(d => d.FactoryId);
                
            //this.HasMany(t => t.FactoryHistories).WithRequired(x => x.ProcessUnit).HasForeignKey(fk => fk.ProcessUnitId);

        }
    }
}
