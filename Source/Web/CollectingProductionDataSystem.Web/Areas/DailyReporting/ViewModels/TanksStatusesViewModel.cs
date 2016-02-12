namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.ViewModels
{
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Web.ViewModels.Tank;
    using System.ComponentModel.DataAnnotations;
    using Resources = App_GlobalResources.Resources;

    public class TanksStatusesViewModel : IMapFrom<TankConfig>
    {
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Resources.ErrorMessages))]
        [Display(Name = "№")]
        public int Id { get; set; }

        public string TankName { get; set; }

        public int ParkId { get; set; }

        public ParkViewModel Park { get; set; }
    }
}