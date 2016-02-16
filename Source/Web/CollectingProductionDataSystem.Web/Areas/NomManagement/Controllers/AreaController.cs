namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;

    public class AreaController : GenericNomController<TankStatus, AreaViewModel>
    {
        public AreaController(IProductionData dataParam)
            : base(dataParam)
        {
        }
    }
}