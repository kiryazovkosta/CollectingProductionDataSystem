using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;

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
            this.ViewData["materialDetailTypes"] = Mapper.Map<IEnumerable<MaterialDetailTypeViewModel>>(this.data.MaterialDetailTypes.All()).ToList();
            this.ViewData["processUnits"] = data.ProcessUnits.All().ToList();
            this.ViewData["measureUnits"] = data.MeasureUnits.All().ToList();
            this.ViewData["relatedMonthlyUnits"] = Mapper.Map<IEnumerable<ProductionPlanConfigUnitMonthlyConfigPlanMembersViewModel>>(data.UnitMonthlyConfigs.All()).ToList();
            this.ViewData["relatedMonthlyFractionUnits"] = Mapper.Map<IEnumerable<ProductionPlanConfigUnitMonthlyConfigFactFractionMembersViewModel>>(data.UnitMonthlyConfigs.All()).ToList();

            return base.Index();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override ActionResult Create([DataSourceRequest]DataSourceRequest request, ProductionPlanConfigViewModel inputViewModel)
        {
            if (!ModelState.IsValid)
            {
                return Json(new[] { inputViewModel }.ToDataSourceResult(request, ModelState));
            }

            if (inputViewModel.ProductionPlanConfigUnitMonthlyConfigPlanMembers.Count == 1
               && inputViewModel.ProductionPlanConfigUnitMonthlyConfigPlanMembers.FirstOrDefault().UnitMonthlyConfigId == 0)
            {
                inputViewModel.ProductionPlanConfigUnitMonthlyConfigPlanMembers.Clear();
            }

            if (inputViewModel.ProductionPlanConfigUnitMonthlyConfigFactFractionMembers.Count == 1
                && inputViewModel.ProductionPlanConfigUnitMonthlyConfigFactFractionMembers.FirstOrDefault().UnitMonthlyConfigId == 0)
            {
                inputViewModel.ProductionPlanConfigUnitMonthlyConfigFactFractionMembers.Clear();
            }

            return base.Create(request, inputViewModel);
        }
    }
}