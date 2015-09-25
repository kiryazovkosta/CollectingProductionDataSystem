using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SteelDbModel.Models.Mapping
{
    public class UnitsApprovedDataMap : EntityTypeConfiguration<UnitsApprovedData>
    {
        public UnitsApprovedDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("UnitsApprovedDatas");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RecordDate).HasColumnName("RecordDate");
            this.Property(t => t.ShiftId).HasColumnName("ShiftId");
            this.Property(t => t.ProcessUnitId).HasColumnName("ProcessUnitId");
            this.Property(t => t.Approved).HasColumnName("Approved");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.DeletedOn).HasColumnName("DeletedOn");
            this.Property(t => t.DeletedFrom).HasColumnName("DeletedFrom");
            this.Property(t => t.CreatedOn).HasColumnName("CreatedOn");
            this.Property(t => t.ModifiedOn).HasColumnName("ModifiedOn");
            this.Property(t => t.CreatedFrom).HasColumnName("CreatedFrom");
            this.Property(t => t.ModifiedFrom).HasColumnName("ModifiedFrom");
        }
    }
}
