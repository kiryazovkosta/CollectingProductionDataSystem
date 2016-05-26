using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
using CollectingProductionDataSystem.Web.Areas.PlanConfiguration.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Areas.PlanConfiguration.Controllers
{
    public class PlanValuesController : AreaBaseController
    {
        public PlanValuesController(IProductionData dataParam)
            :base(dataParam)
            {
                
            }

        // GET: PlanConfiguration/PlanValue
        public ActionResult Index()
        {
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
                    IEnumerable<PlanValue> records = GetPlanValue(date);
                    DataSourceResult result = records.ToDataSourceResult(request, ModelState, Mapper.Map<PlanValueViewModel>);
                    return Json(result);
                }
                catch (Exception ex)
                {
                    this.ModelState.AddModelError("", ex);
                    return Json(new List<PlanValue>().ToDataSourceResult(request, this.ModelState));
                }
            }
            else
            {
                return Json(new List<PlanValue>().ToDataSourceResult(request, this.ModelState));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([DataSourceRequest]DataSourceRequest request, PlanValueViewModel inputViewModel)
        {
            if (ModelState.IsValid)
            {
                if (IsNotMonthConfirmed(inputViewModel.Month))
                {

                    var dbEntry = this.data.PlanValues.GetById(inputViewModel.Id);

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
        /// Determines whether [is not month confirmed] [the specified month].
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
        private IEnumerable<PlanValue> GetPlanValue(DateTime date)
        {
            if (date.Month >= DateTime.Now.Month)
            {
                var resultFromProcessUnits = this.data.ProcessUnits.All()
                                .Include(x => x.PlanValues).ToList()
                                .Select(x => new
                                {
                                    ProcessUnit = x,
                                    PlanValue = x.PlanValues.FirstOrDefault(y => y.Month == date && y.IsDeleted == false)
                                }).ToList();

                List<PlanValue> planValues = new List<PlanValue>();

                foreach (var planConfig in resultFromProcessUnits)
                {
                    if (planConfig.PlanValue == null)
                    {
                        planValues.Add(new PlanValue() { Month = date, ProcessUnitId = planConfig.ProcessUnit.Id, Value = 0M });
                    }
                }

                this.data.PlanValues.BulkInsert(planValues, this.UserProfile.UserName);
                this.data.SaveChanges(this.UserProfile.UserName);
            }

            return this.data.PlanValues.All().Include(x=>x.ProcessUnit).Include(x=>x.ProcessUnit.Factory).Where(x => x.Month == date).ToList();
        }
    }
}