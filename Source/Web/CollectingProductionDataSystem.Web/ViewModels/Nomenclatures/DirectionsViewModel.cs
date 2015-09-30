namespace CollectingProductionDataSystem.Web.ViewModels.Nomenclatures
{
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using System;
    using System.Linq;

    public class DirectionsViewModel : IMapFrom<Direction>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}