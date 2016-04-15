using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Infrastructure.Extentions;
using CollectingProductionDataSystem.Models.Nomenclatures;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.PhdApplication.Contracts;
using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
using CollectingProductionDataSystem.Web.Hubs;
using Resources = App_GlobalResources.Resources;

namespace CollectingProductionDataSystem.Web.Areas.Administration.Controllers
{
    public class ForceLoadingController : AreaBaseController
    {
        private readonly IPhdPrimaryDataService phdService;
        private static int max;
        public ForceLoadingController(IProductionData dataParam, IPhdPrimaryDataService phdServiceParam)
            : base(dataParam)
        {
            this.phdService = phdServiceParam;
        }

        // GET: Administration/ForceLoading
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Index(ForceLoadingViewModel model)
        {
            var phdServers = this.data.PhdConfigs.All().OrderBy(x => x.Position);
            var shift = this.data.Shifts.GetById(model.ShiftId);

            IEnumerable<UnitDatasTemp> result = new List<UnitDatasTemp>();

            foreach (var server in phdServers)
            {
                result = this.phdService.GetPrimaryProductionData((PrimaryDataSourceType)server.Position, server.HostIpAddress, model.BeginDate, shift, result);
            }

            this.data.UnitDatasTemps.BulkInsert(result, this.UserProfile.UserName);

            return Json(model);
        }

        public async Task<ActionResult> GetProgressBar(ForceLoadingViewModel model)
        {
            if (ModelState.IsValid)
            {
                IEnumerable<KeyValuePair<string, string>> validationResult = ValidateModelState(model);
                validationResult.ForEach(x => ModelState.AddModelError(x.Key, x.Value));

                if (ModelState.IsValid)
                {

                    var result = CalculateOperationsCount(model);
                    if (ModelState.IsValid)
                    {
                        max = result.MaxValue;
                        return PartialView(result);
                    }
                }
            }

            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var errors = GetErrorListFromModelState(ModelState);
            return Json(new { data = new { errors = errors } }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Validates the state of the model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        private IEnumerable<KeyValuePair<string, string>> ValidateModelState(ForceLoadingViewModel model)
        {
            if (model.BeginDate != model.EndDate && model.ShiftId != 0)
            {
                yield return new KeyValuePair<string, string>(Resources.Layout.Shift, Resources.ErrorMessages.ShiftCannotBeSet);
            }

            if (model.BeginDate > model.EndDate)
            {
                yield return new KeyValuePair<string, string>(string.Empty, string.Format(Resources.ErrorMessages.MustBeGreaterThan, Resources.Layout.EndDate, Resources.Layout.BeginDate));
            }

            var includedProcessUnitIds = this.data.Factories.All().Include(x=>x.ProcessUnits)
                .Where(x => (model.FactoryId == 0 || x.Id == model.FactoryId))
                .SelectMany(x => x.ProcessUnits.Where(y => y.IsDeleted == false).Select(y => y.Id))
                    .ToList();

            IEnumerable<UnitsApprovedDailyData> approvedRecords = this.data.UnitsApprovedDailyDatas.All()
                .Where(x => x.RecordDate >= model.BeginDate
                         && x.RecordDate <= model.EndDate
                         && (model.ProcessUnitId == 0 || x.ProcessUnitId == model.ProcessUnitId)).ToList();
              approvedRecords = approvedRecords.Where(x=>(model.FactoryId == 0) || includedProcessUnitIds.Any(pu => pu == x.ProcessUnitId)).Distinct(new ApprovedDailyDataComparer());

            if (approvedRecords.Count() > 0)
            {
                var processUnits = this.data.ProcessUnits.All().ToDictionary(x => x.Id);

                foreach (var record in approvedRecords)
                {
                    yield return new KeyValuePair<string, string>(string.Empty,
                                                     string.Format("Дневните данни за инсталация {0} са потвърдени.",
                                                     processUnits[record.ProcessUnitId].FullName));
                }
            }

            // Todo: check if end date is not after last available shift 
            var lastAvailableShiftTime = GetLastAvailableTime();
        }
 
        /// <summary>
        /// Gets the last available time.
        /// </summary>
        /// <returns></returns>
        private Shift GetLastAvailableTime()
        {
            var result = this.data.Shifts.All().ToList().LastOrDefault(x => x.ReadPollTimeSlot <= DateTime.Now.TimeOfDay);
            return result;
        }

        /// <summary>
        /// Calculates the operations count.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        private ProgressBarViewModel CalculateOperationsCount(ForceLoadingViewModel model)
        {
            int result = 0;
            var shiftDataCount = this.data.UnitConfigs.All().Include(x => x.ProcessUnit)
                                    .Where(x => (model.ProcessUnitId == 0 || x.ProcessUnitId == model.ProcessUnitId)
                                        && (model.FactoryId == 0 || x.ProcessUnit.FactoryId == model.FactoryId)
                                    ).Count();
            result = shiftDataCount;
            if (model.ShiftId == 0)
            {
                try
                {
                    int days = Convert.ToInt32(((model.EndDate.Date - model.BeginDate.Date).TotalDays)) + 1;
                    int shiftsCount = Convert.ToInt32((this.data.Shifts.All().Count()));
                    result = shiftDataCount * days * shiftsCount;
                }
                catch (Exception ex)
                {
                    this.ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return new ProgressBarViewModel(result);
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
 
    /// <summary>
    /// 
    /// </summary>
    public class ApprovedDailyDataComparer:IEqualityComparer<UnitsApprovedDailyData>
    {
        /// <summary>
        /// Equalses the specified x.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns></returns>
        public bool Equals(UnitsApprovedDailyData x, UnitsApprovedDailyData y)
        {
            return this.GetHashCode(x) == this.GetHashCode(x);
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        public int GetHashCode(UnitsApprovedDailyData obj)
        {
            return obj.ProcessUnitId;
        }
    }
}