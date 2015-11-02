namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    using System;
    using System.Linq;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;

    public class ProductTypeController : GenericNomController<ProductType, ProductTypeViewModel>
    {
        public ProductTypeController(IProductionData dataParam)
            : base(dataParam)
        {
        }
    }
}