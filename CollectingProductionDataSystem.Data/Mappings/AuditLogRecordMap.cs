using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using CollectingProductionDataSystem.Models.UtilityEntities;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class AuditLogRecordMap : EntityTypeConfiguration<AuditLogRecord>
    {
        public AuditLogRecordMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Id, t.TimeStamp, t.EntityName, t.EntityId });

            // Properties
            this.Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            this.Property(t => t.EntityName)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.EntityId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FieldName)
                .IsRequired();

            this.Property(t => t.NewValue)
                .IsRequired();

            this.Property(t => t.UserName)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("AuditLogRecords");

        }
    }
}
