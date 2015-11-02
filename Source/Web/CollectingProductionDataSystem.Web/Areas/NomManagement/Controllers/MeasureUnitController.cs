namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;

    public class MeasureUnitController : GenericNomController<MeasureUnit, MeasureUnitViewModel>
    {
        public MeasureUnitController(IProductionData dataParam)
            : base(dataParam)
        {
        }
    }
}