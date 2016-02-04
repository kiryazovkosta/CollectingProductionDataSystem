using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Models.Productions.Mounthly;
using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
using Kendo.Mvc.UI;

namespace CollectingProductionDataSystem.Web.Areas.NomManagement.Controllers
{
    public class UnitMonthlyConfigController : GenericNomController<UnitMonthlyConfig, UnitMonthlyConfigViewModel>
    {
        public UnitMonthlyConfigController(IProductionData dataParam)
            : base(dataParam)
        {
        }

         public override ActionResult Index()
        {
            this.ViewData["processUnits"] = Mapper.Map<IEnumerable<ProcessUnitViewModel>>(this.data.ProcessUnits.All().ToList());
            this.ViewData["products"] = Mapper.Map<IEnumerable<ProductViewModel>>(this.data.Products.All().ToList());
            this.ViewData["measureUnits"] = Mapper.Map<IEnumerable<MeasureUnitViewModel>>(this.data.MeasureUnits.All().ToList());
            this.ViewData["monthlyReportTypes"] = Mapper.Map<IEnumerable<MonthlyReportTypeViewModel>>(this.data.MonthlyReportTypes.All().ToList());
            this.ViewData["dailyProductTypes"] = Mapper.Map<IEnumerable<DailyProductTypeViewModel>>(this.data.DailyProductTypes.All().ToList());
            this.ViewData["materialTypes"] = Mapper.Map<IEnumerable<MaterialTypeViewModel>>(this.data.MaterialTypes.All()).ToList();
            this.ViewData["materialDetailTypes"] = Mapper.Map<IEnumerable<MaterialDetailTypeViewModel>>(this.data.MaterialDetailTypes.All()).ToList();
            this.ViewData["relatedDailyUnits"]  = Mapper.Map<IEnumerable<UnitDailyConfig>, IEnumerable<UnitDailyConfigUnitMonthlyConfigViewModel>>(this.data.UnitsDailyConfigs.All()).ToList();
            this.ViewData["relatedMonthlyConfigs"] = Mapper.Map<IEnumerable<UnitMonthlyConfig>, IEnumerable<RelatedUnitMonthlyConfigsViewModel>>(this.data.UnitMonthlyConfigs.All()).ToList();

            return base.Index();
        }

         [HttpPost]
         [ValidateAntiForgeryToken]
         public override ActionResult Create([DataSourceRequest]DataSourceRequest request, UnitMonthlyConfigViewModel inputViewModel) 
         {
             if (inputViewModel.RelatedUnitMonthlyConfigs.Count == 1)
             {
                 
             }
             return base.Create(request, inputViewModel);
         }
    }
}