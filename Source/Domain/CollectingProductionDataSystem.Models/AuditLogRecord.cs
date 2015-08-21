using System;
using System.Collections.Generic;

namespace SteelingDataModels.Models
{
    public partial class AuditLogRecord
    {
        public int Id { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public string EntityName { get; set; }
        public int EntityId { get; set; }
        public string FieldName { get; set; }
        public int OperationType { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string UserName { get; set; }
    }
}
