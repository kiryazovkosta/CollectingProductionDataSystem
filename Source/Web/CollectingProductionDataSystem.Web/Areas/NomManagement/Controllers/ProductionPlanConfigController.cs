using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    public class ProductionPlanConfigController : GenericNomController<ProductionPlanConfig, ProductionPlanConfigViewModel>
    {
        public ProductionPlanConfigController(IProductionData dataParam)
            : base(dataParam)
        {

        }

        public override ActionResult Index()
        {
            this.ViewData["materialTypes"] = Mapper.Map<IEnumerable<MaterialTypeViewModel>>(this.data.MaterialTypes.All()).ToList();
            var materialDetailTypes = Mapper.Map<IEnumerable<MaterialDetailTypeViewModel>>(this.data.MaterialDetailTypes.All()).ToList();
            materialDetailTypes.Add(new MaterialDetailTypeViewModel{Id=0, Name=string.Empty});
            this.ViewData["materialDetailTypes"] = Mapper.Map<IEnumerable<MaterialDetailTypeViewModel>>(materialDetailTypes.OrderBy(x=>x.Id));
            this.ViewData["processUnits"] = data.ProcessUnits.All().ToList();
            return base.Index();
        }
    }
}