namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Web.Infrastructure.MadelBinders;
    using Resources = App_GlobalResources.Resources;

    [Bind(Exclude = "BeginTime, ReadOffset, ReadPollTimeSlot")]
    public class ShiftViewModel : IMapFrom<Shift>, IHaveCustomMappings, IEntity
    {

        public ShiftViewModel()
        {
            this.BeginTime = new TimeSpan();
            this.ReadOffset = new TimeSpan();
            this.ReadPollTimeSlot = new TimeSpan();
        }

        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [Required]
        [TimeSpanComponent]
        [Display(Name = "BeginTime", ResourceType = typeof(Resources.Layout))]
        public TimeSpan BeginTime { get; set; }

        [Required]
        [TimeSpanComponent]
        [Display(Name = "ReadOffset", ResourceType = typeof(Resources.Layout))]
        public TimeSpan ReadOffset { get; set; }

        [Required]
        [TimeSpanComponent]
        [Display(Name = "ReadPollTimeSlot", ResourceType = typeof(Resources.Layout))]
        public TimeSpan ReadPollTimeSlot { get; set; }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ShiftViewModel, Shift>()
                .ForMember(p => p.BeginTicks, opt => opt.Ignore())
                .ForMember(p => p.ReadOffsetTicks, opt => opt.Ignore())
                .ForMember(p => p.ReadPollTimeSlotTicks, opt => opt.Ignore())
                .ForMember(p => p.IsDeleted, opt => opt.Ignore())
                .ForMember(p => p.DeletedOn, opt => opt.Ignore())
                .ForMember(p => p.DeletedFrom, opt => opt.Ignore())
                .ForMember(p => p.CreatedOn, opt => opt.Ignore())
                .ForMember(p => p.PreserveCreatedOn, opt => opt.Ignore())
                .ForMember(p => p.ModifiedOn, opt => opt.Ignore())
                .ForMember(p => p.CreatedFrom, opt => opt.Ignore())
                .ForMember(p => p.ModifiedFrom, opt => opt.Ignore());
        }
    }
}