namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using Resources = App_GlobalResources.Resources;

    /// <summary>
    /// 
    /// </summary>
    public class ProcessUnitViewModel : IMapFrom<ProcessUnit>,IEntity
    {
        [Required]
        [UIHint("Hidden")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "ShortName", ResourceType = typeof(Resources.Layout))]
        public string ShortName { get; set; }

        [Required]
        [Display(Name = "FullName", ResourceType = typeof(Resources.Layout))]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Factory", ResourceType = typeof(Resources.Layout))]
        public int FactoryId { get; set; }

        [Required]
        [Display(Name = "HasDailyStatistics", ResourceType = typeof(Resources.Layout))]
        public bool HasDailyStatistics { get; set; }

        [Required]
        [Display(Name = "HasLoadStatistics", ResourceType = typeof(Resources.Layout))]
        public bool HasLoadStatistics { get; set; }

        [UIHint("Hidden")]
        [Editable(false)]
        public bool IsDeleted { get; set; }
    }
}