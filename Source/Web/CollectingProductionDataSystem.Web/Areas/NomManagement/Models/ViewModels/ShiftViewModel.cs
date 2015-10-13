namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using Resources = App_GlobalResources.Resources;

    public class ShiftViewModel : IMapFrom<Shift>, IHaveCustomMappings, IEntity
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        [Required]
        [Display(Name = "BeginTime", ResourceType = typeof(Resources.Layout))]
        public TimeSpan BeginTime { get; set; }

        [Required]
        [Display(Name = "ReadOffset", ResourceType = typeof(Resources.Layout))]
        public TimeSpan ReadOffset { get; set; }

        [Required]
        [Display(Name = "ReadPollTimeSlot", ResourceType = typeof(Resources.Layout))]
        public TimeSpan ReadPollTimeSlot { get; set; }
        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ShiftViewModel, Shift>()
                //.ForMember(p => p.BeginTime, opt => opt.MapFrom(p => new TimeSpan(p.BeginTime.Hour, p.BeginTime.Minute, p.BeginTime.Second)))
                //.ForMember(p => p.ReadOffset, opt => opt.MapFrom(p => new TimeSpan(p.ReadOffset.Hour, p.ReadOffset.Minute, p.ReadOffset.Second)))
                //.ForMember(p => p.ReadPollTimeSlot, opt => opt.MapFrom(p => new TimeSpan(p.ReadPollTimeSlot.Hour, p.ReadPollTimeSlot.Minute, p.ReadPollTimeSlot.Second)))
                .ForMember(p => p.IsDeleted, opt => opt.Ignore())
                .ForMember(p => p.DeletedOn, opt => opt.Ignore())
                .ForMember(p => p.DeletedFrom, opt => opt.Ignore())
                .ForMember(p => p.CreatedOn, opt => opt.Ignore())
                .ForMember(p => p.PreserveCreatedOn, opt => opt.Ignore())
                .ForMember(p => p.ModifiedOn, opt => opt.Ignore())
                .ForMember(p => p.CreatedFrom, opt => opt.Ignore())
                .ForMember(p => p.ModifiedFrom, opt => opt.Ignore());

            //var date = DateTime.Now;
            //Mapper.CreateMap<Shift, ShiftViewModel>()
            //    .ForMember(p => p.BeginTime, opt => opt.MapFrom(p => new DateTime(date.Year, date.Month, date.Day, p.BeginTime.Hours, p.BeginTime.Minutes, p.BeginTime.Seconds)))
            //    .ForMember(p => p.ReadOffset, opt => opt.MapFrom(p => new DateTime(date.Year, date.Month, date.Day, p.ReadOffset.Hours, p.ReadOffset.Minutes, p.ReadOffset.Seconds)))
            //    .ForMember(p => p.ReadPollTimeSlot, opt => opt.MapFrom(p => new DateTime(date.Year, date.Month, date.Day, p.ReadPollTimeSlot.Hours, p.ReadPollTimeSlot.Minutes, p.ReadPollTimeSlot.Seconds)));
        }
    }
}