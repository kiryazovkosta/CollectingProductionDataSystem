namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;

    public class TankStatusController : GenericNomController<TankStatus, TankStatusViewModel>
    {
        public TankStatusController(IProductionData dataParam)
            : base(dataParam)
        {
        }
    }
}