using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CollectingProductionDataSystem.Application.Contracts;
using CollectingProductionDataSystem.Constants;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Web.Areas.ManagementDashBoard.Controllers;
using CollectingProductionDataSystem.Web.Areas.ManagementDashBoard.Models;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Areas.ManagementDashBoard.Controllers
{
    public class DashboardController : AreaBaseController
    {
        private readonly IUnitDailyDataService dailyService;
        private const int HalfAnHour = 30 * 60;
        public DashboardController(IProductionData dataParam, IUnitDailyDataService dailyServiceParam)
            : base(dataParam)
        {
            this.dailyService = dailyServiceParam;
        }

        // GET: ManagemetDashBoard/Dashboard
        public ActionResult Index()
        {
            var factoriesForStatistics = this.data.Factories.All().Include(x => x.ProcessUnits).Where(x => x.ProcessUnits.Any(y => y.HasDailyStatistics && y.IsDeleted == false)).ToList();
            var factoriesForLoad = this.data.Factories.All().Include(x => x.ProcessUnits).Where(x => x.ProcessUnits.Any(y => y.HasLoadStatistics && y.IsDeleted == false)).ToList();
            var viewModel = new DashBoardViewModel()
            {
                ProcessUnitStatistics = new AllProcessUnitProductionPlanViewModel()
                    {
                        Factories = factoriesForStatistics,
                        ElementPrefix = "pu",
                        ElementRole = "pu-chart-holder"
                    },
                ProcessUnitLoadStatistics = new AllProcessUnitProductionPlanViewModel()
                    {
                        Factories = factoriesForLoad,
                        ElementPrefix = "pul",
                        ElementRole = "pu-load-chart-holder"
                    }

            };

            return View(viewModel);
        }

        [HttpGet]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.Server, VaryByParam = "*")]
        public ActionResult ReloadTabContent(int tabId)
        {
            var viewModel = new DashBoardViewModel();
            if (this.ModelState.IsValid && tabId > 0)
            {
                viewModel.TabId = tabId;
                switch (tabId)
                {
                    case 1:
                            var factoriesForLoad = this.data.Factories.All().Include(x => x.ProcessUnits).Where(x => x.ProcessUnits.Any(y => y.HasLoadStatistics && y.IsDeleted == false)).ToList();

                            viewModel.ProcessUnitLoadStatistics = new AllProcessUnitProductionPlanViewModel()
                                {
                                    Factories = factoriesForLoad,
                                    ElementPrefix = "pul",
                                    ElementRole = "pu-load-chart-holder"
                                };
                            break;
                    case 2:
                            var factoriesForStatistics = this.data.Factories.All().Include(x => x.ProcessUnits).Where(x => x.ProcessUnits.Any(y => y.HasDailyStatistics && y.IsDeleted == false)).ToList();
                            viewModel.ProcessUnitStatistics = new AllProcessUnitProductionPlanViewModel()
                            {
                                Factories = factoriesForStatistics,
                                ElementPrefix = "pu",
                                ElementRole = "pu-chart-holder"
                            };
                            break;
                    default:
                            break;
                }
            }

            return PartialView(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckDates(DateTime beginDate, DateTime endDate)
        {
            var difference = Math.Abs((beginDate - endDate).TotalDays);
            if (difference > CommonConstants.MaxDateDifference)
            {
                var message = string.Format(Resources.ErrorMessages.DateDifference, Resources.Layout.PeriodBegining, Resources.Layout.PeriodEnd, CommonConstants.MaxDateDifference);
                this.ModelState.AddModelError(string.Empty, message);
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var errors = GetErrorListFromModelState(ModelState);
                return Json(new { data = new { errors = errors } });
            }

            return Json(string.Empty, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.Server, VaryByParam = "*")]
        public async Task<ActionResult> DailyMaterialChart(int processUnitId, DateTime beginDate, DateTime endDate, int? height = null, bool shortTitle = false)
        {
            const int material = 1;

            try
            {
                var result = await this.dailyService.GetStatisticForProcessUnitAsync(processUnitId, beginDate, endDate, material);

                result.Title = string.Format(shortTitle ? Resources.Layout.DailyGraphicShortTitle : Resources.Layout.DailyGraphicTitle, this.data.ProcessUnits.GetById(processUnitId).ShortName);
                result.Height = height;
                return PartialView("ComplexDailyChart", result);
            }
            catch (ArgumentOutOfRangeException)
            {
                var message = string.Format(Resources.ErrorMessages.DateDifference, Resources.Layout.PeriodBegining, Resources.Layout.PeriodBegining, CommonConstants.MaxDateDifference);
                this.ModelState.AddModelError(string.Empty, message);
            }

            return PartialView("ComplexDailyChart", null);
        }

        [HttpGet]
        [OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.Server, VaryByParam = "*")]
        public async Task<ActionResult> DailyLoadChart(int processUnitId, DateTime beginDate, DateTime endDate, int? height = null, bool shortTitle = false)
        {
            const int material = 1;
            try
            {
                var result = await this.dailyService.GetStatisticForProcessUnitLoadAsync(processUnitId, beginDate, endDate, material);

                result.Title = string.Format(shortTitle ? Resources.Layout.DailyLoadGraphicShortTitle : Resources.Layout.DailyGraphicTitle, this.data.ProcessUnits.GetById(processUnitId).ShortName);
                result.Height = height;
                return PartialView("ComplexDailyChart", result);
            }
            catch (ArgumentOutOfRangeException)
            {
                var message = string.Format(Resources.ErrorMessages.DateDifference, Resources.Layout.PeriodBegining, Resources.Layout.PeriodBegining, CommonConstants.MaxDateDifference);
                this.ModelState.AddModelError(string.Empty, message);
            }

            return PartialView("ComplexDailyChart", null);
        }

        private List<string> GetErrorListFromModelState(ModelStateDictionary modelState)
        {
            var query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;

            var errorList = query.ToList();
            return errorList;
        }
    }

}