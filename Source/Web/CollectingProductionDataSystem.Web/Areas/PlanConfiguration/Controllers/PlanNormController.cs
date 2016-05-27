namespace CollectingProductionDataSystem.Web.Areas.PlanConfiguration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Data.Entity;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Web.Areas.PlanConfiguration.Models;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Resources = App_GlobalResources.Resources;

    public class PlanNormController : AreaBaseController
    {
        public PlanNormController(IProductionData dataParam)
            : base(dataParam)
        {

        }

        // GET: PlanConfiguration/PlanNorm
        public ActionResult Index()
        {
            this.ViewData["planConfigs"] = Mapper.Map<IEnumerable<ProductionPlanConfigViewModel>>(this.data.ProductionPlanConfigs.All().ToList());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Read([DataSourceRequest]DataSourceRequest request, DateTime date)
        {
            if (this.ModelState.IsValid)
            {
                try
                {
                    IEnumerable<PlanNorm> records = GetPlanNorm(date);
                    DataSourceResult result = records.ToDataSourceResult(request, ModelState, Mapper.Map<PlanNormViewModel>);
                    return Json(result);
                }
                catch (Exception ex)
                {
                    this.ModelState.AddModelError("", ex);
                    return Json(new List<PlanNorm>().ToDataSourceResult(request, this.ModelState));
                }
            }
            else
            {
                return Json(new List<PlanNorm>().ToDataSourceResult(request, this.ModelState));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, PlanNormViewModel inputViewModel)
        {
            if (ModelState.IsValid)
            {
                if (IsNotMonthConfirmed(inputViewModel.Month))
                {

                    var dbEntry = this.data.PlanNorms.GetById(inputViewModel.Id);

                    dbEntry.Value = inputViewModel.Value;

                    try
                    {
                        this.data.SaveChanges(this.UserProfile.UserName);
                    }
                    catch (Exception ex)
                    {
                        this.ModelState.AddModelError("", ex);
                    }
                }
                else
                {
                    this.ModelState.AddModelError("", @Resources.ErrorMessages.MonthIsConfirmed);
                }
            }

            return Json(new[] { inputViewModel }.ToDataSourceResult(request, ModelState));
        }

        /// <summary>
        /// Determines whether the specified month is not confirmed.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <returns></returns>
        private bool IsNotMonthConfirmed(DateTime month)
        {
            DateTime targetDate = new DateTime(month.Year, month.Month, DateTime.DaysInMonth(month.Year, month.Month));
            var reports = this.data.MonthlyReportTypes.All().Include(x => x.UnitApprovedMonthlyDatas)
                            .Select(x => new
                            {
                                ReportType = x,
                                Confirmation = x.UnitApprovedMonthlyDatas.FirstOrDefault(y => y.RecordDate == targetDate && y.IsDeleted == false)
                            }).ToList();

            bool result = false;

            foreach (var report in reports)
            {
                if (report.Confirmation == null)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// Gets the plan norm.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        private IEnumerable<PlanNorm> GetPlanNorm(DateTime date)
        {
            if (date.Month >= DateTime.Now.Month)
            {

                var resultFromPlan = this.data.ProductionPlanConfigs.All()
                                .Include(x => x.PlanNorms).Where(x => x.MaterialTypeId == CommonConstants.MaterialType).ToList()
                                .Select(x => new
                                    {
                                        PlanConfig = x,
                                        PlanNorm = x.PlanNorms.FirstOrDefault(y => y.Month == date && y.IsDeleted == false)
                                    }).ToList();

                List<PlanNorm> planNorms = new List<PlanNorm>();

                foreach (var planConfig in resultFromPlan)
                {
                    if (planConfig.PlanNorm == null)
                    {
                        planNorms.Add(new PlanNorm()
                        {
                            Month = date,
                            ProductionPlanConfigId = planConfig.PlanConfig.Id,
                            Value = planConfig.PlanConfig.IsSummaryOfProcessing ? 100M : 0M
                        });
                    }
                }

                this.data.PlanNorms.BulkInsert(planNorms, this.UserProfile.UserName);
                this.data.SaveChanges(this.UserProfile.UserName);
            }

            return this.data.PlanNorms.All()
                .Include(x=>x.ProductionPlanConfig)
                .Include(x => x.ProductionPlanConfig.ProcessUnit)
                .Include(x => x.ProductionPlanConfig.ProcessUnit.Factory)
                .Where(x => x.Month == date).ToList();
        }


    }
}