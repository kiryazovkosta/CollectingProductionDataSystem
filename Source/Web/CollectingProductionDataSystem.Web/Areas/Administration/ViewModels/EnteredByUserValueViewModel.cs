using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using CollectingProductionDataSystem.Models.Productions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Areas.Administration.ViewModels
{
    public class EnteredByUserValueViewModel : IMapFrom<UnitEnteredForCalculationData>//,IHaveCustomMappings
    {
        //[Display(Name = "Id", ResourceType = typeof(Resources.Layout))]
        //public int Id { get; set; }

        //[Display(Name = "OldValue", ResourceType = typeof(Resources.Layout))]
        public decimal OldValue { get; set; }

        //[Display(Name = "NewValue", ResourceType = typeof(Resources.Layout))]
        public decimal NewValue { get; set; }

        [Display(Name = "ProcessUnit", ResourceType = typeof(Resources.Layout))]
        public string ProcessUnitName { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Resources.Layout))]
        public string Code { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources.Layout))]
        public string Name { get; set; }

        //[Display(Name = "CreatedOn", ResourceType = typeof(Resources.Layout))]
        public DateTime CreatedOn { get; set; }

        //[Display(Name = "CreatedFrom", ResourceType = typeof(Resources.Layout))]
        public string CreatedFrom { get; set; }

        //public void CreateMappings(IConfiguration configuration)
        //{
        //    configuration.CreateMap<EnteredByUserValueViewModel, UnitEnteredForCalculationData>()
        //        //.ForMember(p => p.UnitsData.UnitConfig.ProcessUnit.ShortName, opt => opt.MapFrom(p => p.ProcessUnitName))
        //        .ForMember(p => p.UnitsData.UnitConfig.Code, opt => opt.MapFrom(p => p.Code));
        //}
    }
}