namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;

    public class ProductController :GenericNomController<Product, ProductViewModel>
    {
        public ProductController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        public override ActionResult Index()
        {
            this.ViewData["productTypes"] = Mapper.Map<IEnumerable<ProductTypeViewModel>>(this.data.ProductTypes.All().ToList());
            this.ViewData["shiftProductTypes"] = Mapper.Map<IEnumerable<ShiftProductTypeViewModel>>(this.data.ShiftProductTypes.All().ToList());
            this.ViewData["dailyProductTypes"] = Mapper.Map<IEnumerable<DailyProductTypeViewModel>>(this.data.DailyProductTypes.All().ToList());
            return base.Index();
        }
    }
}