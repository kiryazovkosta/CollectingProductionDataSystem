using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace ManyToManyWithNavigationTable.Models.Mapping
{
    public class LeftTableMap : EntityTypeConfiguration<LeftTable>
    {
        public LeftTableMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Data)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("LeftTable");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Data).HasColumnName("Data");

            // Relationships
            this.HasMany(t => t.RightTables)
                .WithMany(t => t.LeftTables)
                .Map(m =>
                    {
                        m.ToTable("LeftRightTable");
                        m.MapLeftKey("LeftId");
                        m.MapRightKey("RightId");
                    });


        }
    }
}
