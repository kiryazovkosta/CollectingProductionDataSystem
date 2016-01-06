namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    using AutoMapper;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    public class MaterialDetailTypeController : GenericNomController<MaterialDetailType, MaterialDetailTypeViewModel>
    {
        public MaterialDetailTypeController(IProductionData dataParam) 
            :base(dataParam)
        { 
        }

        public override ActionResult Index() 
        {
            ViewData["materialTipes"] = data.MaterialTypes.All().ToList();
            return base.Index();
        }

         [HttpPost]
        [ValidateAntiForgeryToken]
        public virtual ActionResult ReadMaterialDetailTypeWithUndefined([DataSourceRequest]DataSourceRequest request)
        {
            var records = new List<MaterialDetailType>();

            try
            {
                records = this.data.MaterialDetailTypes.All().ToList();
                records.Add(new MaterialDetailType() { Id = 0, Name = "Недефиниран" });
                DataSourceResult result = records.OrderBy(x=>x.Id).ToDataSourceResult(request, ModelState, Mapper.Map<MaterialDetailTypeViewModel>);

                return Json(result);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                DataSourceResult result = new object[] { null }.ToDataSourceResult(request, ModelState);
                return Json(result);
            }
        }
    }
}
