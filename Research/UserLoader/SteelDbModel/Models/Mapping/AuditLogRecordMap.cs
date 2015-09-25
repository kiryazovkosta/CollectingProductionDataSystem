using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace SteelDbModel.Models.Mapping
{
    public class AuditLogRecordMap : EntityTypeConfiguration<AuditLogRecord>
    {
        public AuditLogRecordMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.EntityName)
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.FieldName)
                .IsRequired();

            this.Property(t => t.NewValue)
                .IsRequired();

            this.Property(t => t.UserName)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("AuditLogRecords");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TimeStamp).HasColumnName("TimeStamp");
            this.Property(t => t.EntityName).HasColumnName("EntityName");
            this.Property(t => t.EntityId).HasColumnName("EntityId");
            this.Property(t => t.FieldName).HasColumnName("FieldName");
            this.Property(t => t.OperationType).HasColumnName("OperationType");
            this.Property(t => t.OldValue).HasColumnName("OldValue");
            this.Property(t => t.NewValue).HasColumnName("NewValue");
            this.Property(t => t.UserName).HasColumnName("UserName");
        }
    }
}
