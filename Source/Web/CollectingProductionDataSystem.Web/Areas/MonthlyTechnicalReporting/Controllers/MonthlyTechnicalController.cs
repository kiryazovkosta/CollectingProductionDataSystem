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
    using System.Web.UI;
    using Resources = App_GlobalResources.Resources;
    using Models.Productions;

    public class MonthlyTechnicalController : AreaBaseController
    {
        private const int HalfAnHour = 60 * 30;
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
        //[OutputCache(Duration = HalfAnHour, Location = OutputCacheLocation.Server, VaryByParam = "*")]
        //public async Task<JsonResult> ReadMonthlyTechnicalData([DataSourceRequest]DataSourceRequest request, DateTime? date)
        public JsonResult ReadMonthlyTechnicalData([DataSourceRequest]DataSourceRequest request, int? factoryId, DateTime? date)
        {
            ValidateModelState(factoryId, date);

            if (!this.ModelState.IsValid)
            {
                DataSourceResult kendoResult = new List<MonthlyTechnicalViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }

            IEfStatus status = this.monthlyService.CheckIfAllMonthReportAreApproved(date.Value);
            if (!status.IsValid)
            {
                status.ToModelStateErrors(this.ModelState);
            }

            if (this.ModelState.IsValid)
            {
                //try
                {
                    IEnumerable<int> processUnits = new HashSet<int>();
                    if (IsPowerUser())
                    {
                        processUnits = this.data.ProcessUnits.All().Where(x => x.FactoryId == factoryId).Select(x => x.Id);
                    }
                    else
                    {
                        List<int> processUnitsForFactory = this.data.ProcessUnits.All().Where(x => x.FactoryId == factoryId).Select(x => x.Id).ToList();
                        processUnits = this.UserProfile.ProcessUnits.ToList().Where(p => processUnitsForFactory.Contains(p.Id)).Select(x => x.Id);
                    }
                    IEnumerable<MonthlyTechnicalReportDataDto> dbResult = this.monthlyService.ReadMonthlyTechnologicalDataAsync(date.Value, processUnits.ToArray());
                    IEnumerable<MonthlyTechnicalViewModel> vmResult = Mapper.Map<IEnumerable<MonthlyTechnicalViewModel>>(dbResult);
                    DataSourceResult kendoResult = vmResult.ToDataSourceResult(request, ModelState);
                    //return Json(kendoResult);
                    JsonResult output = Json(kendoResult, JsonRequestBehavior.AllowGet);
                    output.MaxJsonLength = int.MaxValue;
                    return output;
                }
                //catch (Exception ex)
                //{
                //    this.ModelState.AddModelError("", ex.Message);
                //    status.ToModelStateErrors(this.ModelState);
                //    DataSourceResult kendoResult = new List<MonthlyTechnicalViewModel>().ToDataSourceResult(request, ModelState);
                //    return Json(kendoResult);
                //}
            }
            else
            {
                DataSourceResult kendoResult = new List<MonthlyTechnicalViewModel>().ToDataSourceResult(request, ModelState);
                return Json(kendoResult);
            }
        }

        private void ValidateModelState(int? factoryId, DateTime? date)
        {
            if (factoryId == null)
            {
                this.ModelState.AddModelError("factoryId", string.Format(Resources.ErrorMessages.Required, Resources.Layout.ChooseFactory));
            }

            if (date == null)
            {
                this.ModelState.AddModelError("date", string.Format(Resources.ErrorMessages.Required, Resources.Layout.UnitsDateSelector));
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetFactoryName([DataSourceRequest]DataSourceRequest request, int? factoryId)
        {
            if (!factoryId.HasValue)
            {
                this.ModelState.AddModelError("", "factory id");
            }

            if (this.ModelState.IsValid)
            {
                Factory factory = this.data.Factories.All().Where(x => x.Id == factoryId).FirstOrDefault();
                if (factory != null)
                {
                    return Json(new { factoryName = factory.FullName });
                }

                return Json(new { factoryName = string.Empty });
            }
            else
            {
                return Json(new { factoryName = string.Empty });
            }
        }
    }
}