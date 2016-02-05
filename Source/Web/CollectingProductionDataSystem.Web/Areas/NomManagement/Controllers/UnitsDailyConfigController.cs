using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Nomenclatures;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    public class UnitsDailyConfigController : GenericNomController<UnitDailyConfig, UnitsDailyConfigViewModel>
    {
        public UnitsDailyConfigController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        public override ActionResult Index()
        {
            this.ViewData["processUnits"] = Mapper.Map<IEnumerable<ProcessUnitViewModel>>(this.data.ProcessUnits.All().ToList());
            this.ViewData["products"] = Mapper.Map<IEnumerable<ProductViewModel>>(this.data.Products.All().ToList());
            this.ViewData["measureUnits"] = Mapper.Map<IEnumerable<MeasureUnitViewModel>>(this.data.MeasureUnits.All().ToList());
            this.ViewData["dailyProductTypes"] = Mapper.Map<IEnumerable<DailyProductTypeViewModel>>(this.data.DailyProductTypes.All().ToList());
            this.ViewData["materialTypes"] = Mapper.Map<IEnumerable<MaterialTypeViewModel>>(this.data.MaterialTypes.All()).ToList();
            this.ViewData["materialDetailTypes"] = Mapper.Map<IEnumerable<MaterialDetailTypeViewModel>>(this.data.MaterialDetailTypes.All()).ToList();
            this.ViewData["relatedUnitConfigs"] = Mapper.Map<IEnumerable<UnitConfig>, IEnumerable<UnitConfigUnitDailyConfigViewModel>>(this.data.UnitConfigs.All()).ToList();
            this.ViewData["relatedDailyUnits"] = Mapper.Map<IEnumerable<UnitDailyConfig>, IEnumerable<RelatedUnitDailyConfigsViewModel>>(this.data.UnitsDailyConfigs.All()).ToList();
            return base.Index();
        }

         [HttpPost]
         [ValidateAntiForgeryToken]
         public override ActionResult Create([DataSourceRequest]DataSourceRequest request, UnitsDailyConfigViewModel inputViewModel) 
         {
             if (!ModelState.IsValid)
             {
                  return Json(new[] { inputViewModel }.ToDataSourceResult(request, ModelState));
             }

              if (inputViewModel.UnitConfigUnitDailyConfigs.Count == 1
                 && inputViewModel.UnitConfigUnitDailyConfigs.FirstOrDefault().UnitConfigId==0)
             {
                 inputViewModel.UnitConfigUnitDailyConfigs.Clear();
             }

             if (inputViewModel.RelatedUnitDailyConfigs.Count == 1
                 && inputViewModel.RelatedUnitDailyConfigs.FirstOrDefault().Id==0)
             {
                 inputViewModel.RelatedUnitDailyConfigs.Clear();
             }

             return base.Create(request, inputViewModel);
         }
    }
}