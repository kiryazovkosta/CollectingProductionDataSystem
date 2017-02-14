namespace HistoricalProductionStructure.DataAccess.Mappings
{
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.ModelConfiguration;
    using Domain.Models;

    public class AuditLogRecordMap : EntityTypeConfiguration<AuditLogRecord>
    {
        public AuditLogRecordMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            Property(t => t.TimeStamp)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_audit_log", 1)))
                .IsRequired();

            Property(t => t.EntityName)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_audit_log", 2)))
                .IsRequired()
                .HasMaxLength(250);

            Property(t => t.EntityId)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("ix_audit_log", 3)))
                .IsRequired();

            Property(t => t.FieldName)
                .IsRequired();

            Property(t => t.NewValue)
                .IsRequired();

            Property(t => t.UserName)
                .IsRequired();

            // Table & Column Mappings
            ToTable("AuditLogRecords");
        }
    }
}