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
using Kendo.Mvc.Extensions;
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
            this.ViewData["relatedDailyUnits"] = Mapper.Map<IEnumerable<UnitDailyConfig>, IEnumerable<UnitDailyConfigUnitMonthlyConfigViewModel>>(this.data.UnitsDailyConfigs.All()).ToList();
            this.ViewData["relatedMonthlyConfigs"] = Mapper.Map<IEnumerable<UnitMonthlyConfig>, IEnumerable<RelatedUnitMonthlyConfigsViewModel>>(this.data.UnitMonthlyConfigs.All()).ToList();
            List<ProductionPlanConfigDropDownViewModel> productionPlanData = Mapper.Map<List<ProductionPlanConfig>, List<ProductionPlanConfigDropDownViewModel>>(this.data.ProductionPlanConfigs.All().ToList());
            productionPlanData.Add(new ProductionPlanConfigDropDownViewModel() { Id = 0, DisplayText = string.Empty });
            this.ViewData["productionPlanConfigs"] = productionPlanData.OrderBy(x=>x.Id);//Mapper.Map<IEnumerable<ProductionPlanConfig>, IEnumerable<ProductionPlanConfigDropDownViewModel>>(this.data.ProductionPlanConfigs.All().ToList());

            return base.Index();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Create([DataSourceRequest]DataSourceRequest request, UnitMonthlyConfigViewModel inputViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new[] { inputViewModel }.ToDataSourceResult(request, ModelState));
            }

            if (inputViewModel.UnitDailyConfigUnitMonthlyConfigs.Count == 1
               && inputViewModel.UnitDailyConfigUnitMonthlyConfigs.FirstOrDefault().UnitDailyConfigId == 0)
            {
                inputViewModel.UnitDailyConfigUnitMonthlyConfigs.Clear();
            }

            if (inputViewModel.RelatedUnitMonthlyConfigs.Count == 1
                && inputViewModel.RelatedUnitMonthlyConfigs.FirstOrDefault().Id == 0)
            {
                inputViewModel.RelatedUnitMonthlyConfigs.Clear();
            }

            return base.Create(request, inputViewModel);
        }

        public JsonResult ReadProductionPlans()//[DataSourceRequest]DataSourceRequest request)
        {
            List<ProductionPlanConfigDropDownViewModel> productionPlanData = Mapper.Map<List<ProductionPlanConfig>, List<ProductionPlanConfigDropDownViewModel>>(this.data.ProductionPlanConfigs.All().ToList());
            productionPlanData.Add(new ProductionPlanConfigDropDownViewModel() { Id = 0, DisplayText = string.Empty });
            this.ViewData["productionPlanConfigs"] = productionPlanData;
            return Json(productionPlanData.OrderBy(x=>x.Id));
        }

        public JsonResult GetRelatedMonthly()
        {
            var result = Mapper.Map<IEnumerable<UnitMonthlyConfig>, IEnumerable<RelatedUnitMonthlyConfigsViewModel>>(this.data.UnitMonthlyConfigs.All()).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}