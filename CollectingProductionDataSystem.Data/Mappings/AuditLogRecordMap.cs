using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using CollectingProductionDataSystem.Models.UtilityEntities;

namespace CollectingProductionDataSystem.Data.Mappings
{
    public class AuditLogRecordMap : EntityTypeConfiguration<AuditLogRecord>
    {
        public AuditLogRecordMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.TimeStamp)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_audit_log", 1)))
                .IsRequired();

            this.Property(t => t.EntityName)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_audit_log", 2)))
                .IsRequired()
                .HasMaxLength(250);

            this.Property(t => t.EntityId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_audit_log", 3)))
                .IsRequired();

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