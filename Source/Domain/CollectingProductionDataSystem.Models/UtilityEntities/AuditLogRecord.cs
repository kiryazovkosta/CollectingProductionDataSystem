using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using CollectingProductionDataSystem.Models.Contracts;

namespace CollectingProductionDataSystem.Models.UtilityEntities
{
    public class AuditLogRecord: IEntity
    {
        [Key]
        public int Id { get; set; }

        //[Index("ix_audit_log", 1)]
        public DateTime TimeStamp { get; set; }

        //[Index("ix_audit_log", 2)]
        public string EntityName { get; set; }

       // [Index("ix_audit_log", 3)]
        public int EntityId { get; set; }

        public string FieldName{ get; set; }

        public EntityState OperationType { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string UserName { get; set; }
    }
 
}
