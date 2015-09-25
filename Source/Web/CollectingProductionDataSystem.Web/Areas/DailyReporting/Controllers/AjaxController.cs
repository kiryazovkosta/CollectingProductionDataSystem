using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Application.UnitsDataServices;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Web.ViewModels.Units;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MathExpressions.Application;
using CollectingProductionDataSystem.Web.Infrastructure.Filters;

namespace CollectingProductionDataSystem.Web.Areas.DailyReporting.Controllers
{
    public class AjaxController : AreaBaseController
    {
        private readonly IUnitsDataService unitsData;

        public AjaxController(IProductionData dataParam, IUnitsDataService unitsDataParam)
            : base(dataParam)
        {
            this.unitsData = unitsDataParam;
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
        public JsonResult ReadProductionPlanData([DataSourceRequest]DataSourceRequest request, DateTime? date, int? processUnitId)
        {
            var dailyData = unitsData.GetUnitsDailyDataForDateTime(date, processUnitId).ToList();

            var dbResult = this.data.ProductionPlanConfigs.All();
            if (processUnitId != null)
            {
                dbResult = dbResult.Where(x => x.ProcessUnitId == processUnitId.Value);
            }

            var productionPlans = dbResult.ToList();
            var calculator = new Calculator();
            var result = new HashSet<ProductionPlanViewModel>();
            foreach (ProductionPlanConfig productionPlan in productionPlans)
            {
                var planValue = CalculatePlanValue(productionPlan, dailyData, calculator);
                var factValue = CalculateFactValue(productionPlan, dailyData, calculator);

                result.Add(new ProductionPlanViewModel
                {
                    Id = productionPlan.Id,
                    Name = productionPlan.Name,
                    Percentages = productionPlan.Percentages,
                    QuantityPlan = (decimal)planValue,
                    QuantityFact = (decimal)factValue
                });
            }

            var kendoResult = new DataSourceResult();
            try
            {
                kendoResult = result.ToDataSourceResult(request, ModelState);
            }
            catch (Exception ex1)
            {
                Debug.WriteLine(ex1.Message + "\n" + ex1.InnerException);
            }

            return Json(kendoResult);
        }

 
        private double CalculateFactValue(ProductionPlanConfig productionPlan, List<UnitsDailyData> dailyData, Calculator calculator)
        {
            var splitter = new char[] { ';' };

            var factTokens = productionPlan.QuantityFactMembers.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            var factInputParamsValues = new List<double>();
            foreach (var token in factTokens)
            {
                foreach (var item in dailyData)
                {
                    if (item.UnitsDailyConfig.Code == token)
                    {
                        factInputParamsValues.Add((double)item.RealValue);
                    }
                }
            }

            var factInputParams = new Dictionary<string, double>();
            for (int i = 0; i < factInputParamsValues.Count(); i++)
            {
                factInputParams.Add(string.Format("p{0}", i), factInputParamsValues[i]);  
            }
                
            var factValue = calculator.Calculate(productionPlan.QuantityFactFormula, "p", factInputParams.Count, factInputParams);
            return factValue;
        }
 
        private double CalculatePlanValue(ProductionPlanConfig productionPlan, List<UnitsDailyData> dailyData, Calculator calculator)
        {
            var splitter = new char[] { ';' };

            var planTokens = productionPlan.QuantityPlanMembers.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            var planInputParamsValues = new List<double>();
            foreach (var token in planTokens)
            {
                foreach (var item in dailyData)
                {
                    if (item.UnitsDailyConfig.Code == token)
                    {
                        planInputParamsValues.Add((double)item.RealValue);
                    }
                }
            }

            var planInputParams = new Dictionary<string, double>();
            for (int i = 0; i < planInputParamsValues.Count(); i++)
            {
                planInputParams.Add(string.Format("p{0}", i), planInputParamsValues[i]);  
            }

            var planValue = calculator.Calculate(productionPlan.QuantityPlanFormula, "p", planInputParams.Count, planInputParams);
            return planValue;
        }

    }
}