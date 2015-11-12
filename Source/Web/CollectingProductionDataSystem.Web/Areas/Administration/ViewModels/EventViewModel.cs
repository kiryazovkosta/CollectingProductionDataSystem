using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.SystemLog;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Areas.Administration.ViewModels
{
    public class EventViewModel:IMapFrom<Event>,IHaveCustomMappings
    {
        [Display(Name = "EventId", ResourceType = typeof(Resources.Layout))]
        public string EventId { get; set; }

        [Display(Name = "EventTimeUtc", ResourceType = typeof(Resources.Layout))]
        public System.DateTime EventTimeUtc { get; set; }

        [Display(Name = "EventTime", ResourceType = typeof(Resources.Layout))]
        public System.DateTime EventTime { get; set; }

        [Display(Name = "EventType", ResourceType = typeof(Resources.Layout))]
        public string EventType { get; set; }

        [Display(Name = "EventSequence", ResourceType = typeof(Resources.Layout))]
        public decimal EventSequence { get; set; }

        [Display(Name = "EventOccurrence", ResourceType = typeof(Resources.Layout))]
        public decimal EventOccurrence { get; set; }

        [Display(Name = "EventCode", ResourceType = typeof(Resources.Layout))]
        public int EventCode { get; set; }

        [Display(Name = "EventDetailCode", ResourceType = typeof(Resources.Layout))]
        public int EventDetailCode { get; set; }

        [Display(Name = "Message", ResourceType = typeof(Resources.Layout))]
        public string Message { get; set; }

        [Display(Name = "ApplicationPath", ResourceType = typeof(Resources.Layout))]
        public string ApplicationPath { get; set; }

        [Display(Name = "ApplicationVirtualPath", ResourceType = typeof(Resources.Layout))]
        public string ApplicationVirtualPath { get; set; }

        [Display(Name = "MachineName", ResourceType = typeof(Resources.Layout))]
        public string MachineName { get; set; }

        [Display(Name = "RequestUrl", ResourceType = typeof(Resources.Layout))]
        public string RequestUrl { get; set; }

        [Display(Name = "ExceptionType", ResourceType = typeof(Resources.Layout))]
        public string ExceptionType { get; set; }

        [Display(Name = "Details", ResourceType = typeof(Resources.Layout))]
        public string Details { get; set; }
        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<EventViewModel, Event>()
                .ForMember(p => p.Id, opt => opt.Ignore());
        }
    }
}