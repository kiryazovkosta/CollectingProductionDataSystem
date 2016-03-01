namespace CollectingProductionDataSystem.Web.Areas.Administration.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.UtilityEntities;
    using Resources = App_GlobalResources.Resources;

    public class MessageInputModel : IMapFrom<Message>
    {
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [UIHint("MessageTextInput")]
        [Display(Name = "MessageText", ResourceType = typeof(Resources.Layout))]
        public string MessageText { get; set; }

        [Required]
        [UIHint("MessageTypeInput")]
        [Display(Name = "MessageType", ResourceType = typeof(Resources.Layout))]
        public MessageType MessageType { get; set; }

        [Required]
        [UIHint("MessageValidUntillInput")]
        [Display(Name = "ValidUntill", ResourceType = typeof(Resources.Layout))]
        public DateTime ValidUntill { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsExpired { get { return this.ValidUntill < DateTime.Now; } set { } }

        public bool IsEditable { get { return true; } }

        
    }
}