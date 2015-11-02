namespace CollectingProductionDataSystem.Web.Areas.RecordsHistory.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;
    using System.Web;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.UtilityEntities;
    using Resources = App_GlobalResources.Resources;

    public class AuditLogRecordViewModel : IMapFrom<AuditLogRecord>
    {
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Display(Name = "TimeStamp", ResourceType = typeof(Resources.Layout))]
        public DateTime TimeStamp { get; set; }

        [Display(Name = "FieldName", ResourceType = typeof(Resources.Layout))]
        public string FieldName { get; set; }

        [Display(Name = "OperationType", ResourceType = typeof(Resources.Layout))]
        public EntityState OperationType { get; set; }

        [Display(Name = "OldValue", ResourceType = typeof(Resources.Layout))]
        public string OldValue { get; set; }

        [Display(Name = "NewValue", ResourceType = typeof(Resources.Layout))]
        public string NewValue { get; set; }

        [Display(Name = "UserChanger", ResourceType = typeof(Resources.Layout))]
        public string UserName { get; set; }

        [Display(Name = "EntityName", ResourceType = typeof(Resources.Layout))]
        public string EntityName { get; set; }

        [Display(Name = "EntityId", ResourceType = typeof(Resources.Layout))]
        public int EntityId { get; set; }
    }
}