using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Application.Contracts;
using CollectingProductionDataSystem.Constants;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Web.Areas.ManagementDashBoard.Controllers;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Areas.ManagementDashBoard.Controllers
{
    public class DashboardController : AreaBaseController
    {
        private readonly IUnitDailyDataService dailyService;
        public DashboardController(IProductionData dataParam, IUnitDailyDataService dailyServiceParam)
            : base(dataParam)
        {
            this.dailyService = dailyServiceParam;
        }

        // GET: ManagemetDashBoard/Dashboard
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult AllProcessUnitsProductionPlan()
        {
            var factories = this.data.Factories.All().Include(x => x.ProcessUnits).Where(x => x.ProcessUnits.Any(y => y.HasDailyStatistics && y.IsDeleted == false)).ToList();
            return PartialView(factories);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckDates(DateTime beginDate, DateTime endDate)
        {
            var difference = Math.Abs((beginDate-endDate).TotalDays);
            if ( difference > CommonConstants.MaxDateDifference)
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
        public async Task<ActionResult> DailyMaterialChart(int processUnitId, DateTime beginDate, DateTime endDate, int? height = null, bool shortTitle = false)
        {
            const int material = 1;
            //date = date ?? DateTime.Now.Date.AddDays(-2);
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