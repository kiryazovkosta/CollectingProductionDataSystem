namespace CollectingProductionDataSystem.Web.Areas.MonthlyTechnicalReporting.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.MonthlyTechnologicalDataServices;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Areas.MonthlyTechnicalReporting.ViewModels;
    using CollectingProductionDataSystem.Web.Infrastructure.Extentions;
    using CollectingProductionDataSystem.Web.Infrastructure.Filters;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using System.Threading.Tasks;
    public class MonthlyTechnicalController : AreaBaseController
    {
        private readonly IMonthlyTechnicalDataService monthlyService;

        public MonthlyTechnicalController(IProductionData dataParam, IMonthlyTechnicalDataService monthlyServiceParam)
            : base(dataParam)
        {
            this.monthlyService = monthlyServiceParam;
        }

        [HttpGet]
        public ActionResult MonthlyTechnicalData()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ReadMonthlyTechnicalData([DataSourceRequest]DataSourceRequest request, DateTime? date)
        {
            if (!this.ModelState.IsValid)
            {
                var kendoResult = new List<MonthlyTechnicalViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }

            var status = this.monthlyService.CheckIfAllMonthReportAreApproved(date.Value);
            if (!status.IsValid)
            {
                status.ToModelStateErrors(this.ModelState);
            }

            if (this.ModelState.IsValid)
            {
                try
                {
                    IEnumerable<int> processUnits = new HashSet<int>();
                    if (IsPowerUser())
                    {
                        processUnits = this.data.ProcessUnits.All().Where(x => x.FactoryId <= 5).Select(x => x.Id);
                    }
                    else
                    {
                        processUnits = this.UserProfile.ProcessUnits.ToList().Select(x => x.Id);
                    }

                    var kendoResult = new DataSourceResult();
                    var dbResult = await this.monthlyService.ReadMonthlyTechnologicalDataAsync(date.Value, processUnits.ToArray());
                    var vmResult = Mapper.Map<IEnumerable<MonthlyTechnicalViewModel>>(dbResult);
                    kendoResult = vmResult.ToDataSourceResult(request, ModelState);
                    return Json(kendoResult);
                }
                catch (Exception ex)
                {
                    this.ModelState.AddModelError("", ex.Message);
                    status.ToModelStateErrors(this.ModelState);
                    var kendoResult = new List<MonthlyTechnicalViewModel>().ToDataSourceResult(request, ModelState);
                    return Json(kendoResult);
                }
            }
            else
            {
                var kendoResult = new List<MonthlyTechnicalViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IsConfirmed(DateTime date)
        {
            return Json(new { IsConfirmed = true });
        }

        private bool IsPowerUser()
        {
            return UserProfile.UserRoles.Where(x => CommonConstants.PowerUsers.Any(y => y == x.Name)).Any();
        }
    }
}