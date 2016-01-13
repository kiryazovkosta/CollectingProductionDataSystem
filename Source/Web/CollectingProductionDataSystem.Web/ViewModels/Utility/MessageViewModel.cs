namespace CollectingProductionDataSystem.Web.ViewModels.Utility
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.UtilityEntities;
    using Resources = App_GlobalResources.Resources;

    public class MessageViewModel : IMapFrom<Message>
    {
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Display(Name = "MessageText", ResourceType = typeof(Resources.Layout))]
        public string MessageText { get; set; }

        [UIHint("MessageTypeView")]
        [Display(Name = "MessageType", ResourceType = typeof(Resources.Layout))]
        public MessageType MessageType { get; set; }
    }
}