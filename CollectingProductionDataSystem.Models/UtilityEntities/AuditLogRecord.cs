using System;
using System.Data.Entity;
using System.Linq;
using CollectingProductionDataSystem.Models.Contracts;

namespace CollectingProductionDataSystem.Models.UtilityEntities
{
    public class AuditLogRecord: IEntity
    {
        public int Id { get; set; }

        public DateTime TimeStamp { get; set; }

        public string EntityName { get; set; }

        public int EntityId { get; set; }

        public string FieldName{ get; set; }

        public EntityState OperationType { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public string UserName { get; set; }
    }
 
}
