using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Application.Contracts;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Web.Areas.ManagementDashBoard.Controllers;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Areas.ManagementDashBoard.Controllers
{
    public class DashboardController : AreaBaseController
    {
        private readonly IUnitDailyDataService dailyService;
        public DashboardController(IProductionData dataParam, IUnitDailyDataService dailyServiceParam)
            :base(dataParam)
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
            var factories = this.data.Factories.All().Include(x => x.ProcessUnits).ToList();
            return PartialView(factories);
        }

        [HttpGet]
        public async Task<ActionResult> DailyMaterialChart(int processUnitId, DateTime? date, int? height = null, bool shortTitle = false)
        {
            const int material = 1;
            date = date ?? DateTime.Now.Date.AddDays(-2);
            var result = await this.dailyService.GetStatisticForProcessUnitAsync(processUnitId, date.Value, material);
            result.Title = string.Format(shortTitle ? Resources.Layout.DailyGraphicShortTitle : Resources.Layout.DailyGraphicTitle, this.data.ProcessUnits.GetById(processUnitId).ShortName);
            result.Height = height;
            return PartialView("ComplexDailyChart", result);
        }
    }
}