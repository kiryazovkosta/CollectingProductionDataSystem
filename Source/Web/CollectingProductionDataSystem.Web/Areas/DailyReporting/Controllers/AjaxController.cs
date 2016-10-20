namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.CalculatorService;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Web.Areas.ShiftReporting.ViewModels;
    using CollectingProductionDataSystem.Web.ViewModels.Units;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using CollectingProductionDataSystem.Web.Infrastructure.Filters;
    using Resources = App_GlobalResources.Resources;
    using CollectingProductionDataSystem.Application.ProductionPlanDataServices;
    using Constants;

    public class AjaxController : AreaBaseController
    {
        private readonly IUnitsDataService unitsData;
        private readonly IProductionPlanDataService productionPlanData;

        public AjaxController(IProductionData dataParam, IUnitsDataService unitsDataParam, IProductionPlanDataService productionPlanDataParam)
            : base(dataParam)
        {
            this.unitsData = unitsDataParam;
            this.productionPlanData = productionPlanDataParam;
        }

        public ActionResult ReadDetails([DataSourceRequest]
                                        DataSourceRequest request, int recordId)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<UnitsData> dbResult = this.unitsData.GetUnitsDataForDailyRecord(recordId);
                var kendoPreparedResult = Mapper.Map<IEnumerable<UnitsData>, IEnumerable<UnitDataViewModel>>(dbResult);
                var kendoResult = new DataSourceResult();
                kendoResult = kendoPreparedResult.ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
            else
            {
                return Json(new[] { recordId }.ToDataSourceResult(request, ModelState));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AuthorizeFactory]
        public JsonResult ReadProductionPlanData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? processUnitId, int? materialTypeId)
        {
            if (date.HasValue == false || processUnitId.HasValue == false || materialTypeId.HasValue == false)
            {
                return Json(string.Empty);    
            }

            IEnumerable<ProductionPlanData> dbResult = this.productionPlanData.ReadProductionPlanData(date, processUnitId, materialTypeId);
            if (dbResult.Count() == 0)
	        {
		         return Json(string.Empty);
	        }

            IEnumerable<ProductionPlanData> visibleDbData = dbResult.Where(x => x.ProductionPlanConfig.IsPropductionPlan == true);
            if (materialTypeId == CommonConstants.MaterialType)
            {
                visibleDbData = visibleDbData.Where(x => CommonConstants.MaterialTypeChemicalType.Contains(x.ProductionPlanConfig.MaterialTypeId));
            }
            else
            {
                visibleDbData = visibleDbData.Where(x => x.ProductionPlanConfig.MaterialTypeId == materialTypeId.Value);
            }
                
            List<ProductionPlanData> visibleData = visibleDbData.ToList();
            var kendoResult = new DataSourceResult();
            IEnumerable<ProductionPlanViewModel> kendoPreparedResult = Mapper.Map<IEnumerable<ProductionPlanData>, IEnumerable<ProductionPlanViewModel>>(visibleData);
            try
            {
                kendoResult = kendoPreparedResult.ToDataSourceResult(request, ModelState);
            }
            catch (Exception ex1)
            {
                Debug.WriteLine(ex1.Message + "\n" + ex1.InnerException);
            }

            return Json(kendoResult);
        }
    }
}