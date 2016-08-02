namespace CollectingProductionDataSystem.Web.Areas.Administration.Controllers
{
    using CollectingProductionDataSystem.Data.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Web;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Infrastructure.Extentions;
    using System.Data.Entity;
    using Models.Productions;
    using System.Transactions;
    using Data.Common;
    using System.Data.Entity.Infrastructure;
    using Constants;

    public class ApprovedDataController : AreaBaseController
    {
        private readonly TransactionOptions transantionOption;

        public ApprovedDataController(IProductionData dataParam)
            : base(dataParam)
        {
            this.transantionOption = DefaultTransactionOptions.Instance.TransactionOptions;
        }

        // GET: Administration/ApprovedData
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DailyIndex()
        {
            return Json(DateTime.Now);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult MonthlyIndex()
        {
            return Json(DateTime.Now);
        }

        public ActionResult UnlockShiftReport(DateTime? day, int? processUnitId, int? shiftId)
        {
            if (!day.HasValue)
            {
                this.ModelState.AddModelError("", "Не е избрана дата");
            }

            if (!processUnitId.HasValue || processUnitId.Value <= 0)
            {
                this.ModelState.AddModelError("", "Не е избрана инсталация");
            }

            if (!shiftId.HasValue || shiftId.Value <= 0)
            {
                this.ModelState.AddModelError("", "Не е избрана смяна");
            }

            if (this.ModelState.IsValid)
            {
                bool exsistDailyData = this.data.UnitsDailyDatas.All().Where(x => x.RecordTimestamp == day).Any();
                if (exsistDailyData)
                {
                    this.ModelState.AddModelError("", "Вече има генерирани данни за избраният ден.");
                }

                if (this.ModelState.IsValid)
                {
                    UnitsApprovedData approvedShift = this.data.UnitsApprovedDatas.All()
                        .Where(x => x.RecordDate == day && x.ProcessUnitId == processUnitId && x.ShiftId == shiftId)
                        .FirstOrDefault();
                    if (approvedShift != null)
                    {
                        this.data.UnitsApprovedDatas.Delete(approvedShift);
                        IEfStatus status = this.data.SaveChanges(this.UserProfile.UserName);
                        return Json(new { IsUnlocked = status.IsValid }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        this.ModelState.AddModelError("", "Смените данни по избраните параметри не са потвърдени!");
                        Response.StatusCode = (int) HttpStatusCode.BadRequest;
                        List<string> errors = GetErrorListFromModelState(ModelState);
                        return Json(new { data = new { errors = errors } }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    List<string> errors = GetErrorListFromModelState(ModelState);
                    return Json(new { data = new { errors = errors } }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                List<string> errors = GetErrorListFromModelState(ModelState);
                return Json(new { data = new { errors = errors } }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UnlockDailyReport(DateTime? day, int? processUnitId, int? materialTypeId)
        {
            if (!day.HasValue)
            {
                this.ModelState.AddModelError("", "Не е избрана дата");
            }

            if (!processUnitId.HasValue || processUnitId.Value <= 0)
            {
                this.ModelState.AddModelError("", "Не е избрана инсталация");
            }

            if (!materialTypeId.HasValue || materialTypeId.Value <= 0 || materialTypeId.Value > 2)
            {
                this.ModelState.AddModelError("", "Не е избрана тип на материала");
            }

            if (this.ModelState.IsValid)
            {
                // Check whether there are daily data for recent day
                var exsistingDailyDataForRecentDay = this.data.UnitsDailyDatas.All()
                    .Where(x => x.RecordTimestamp > day)
                    .Any();
                if (exsistingDailyDataForRecentDay)
                {
                    this.ModelState.AddModelError("", "Съществуват дневни данни от по-скоро!");
                }

                // Check whether there are monthly data for selected month
                var month = new DateTime(day.Value.Year, day.Value.Month, DateTime.DaysInMonth(day.Value.Year, day.Value.Month));
                var exsistingMonthlyDataForSelectedMonth = this.data.UnitMonthlyDatas.All()
                    .Where(x => x.RecordTimestamp == month)
                    .Any();
                if (exsistingMonthlyDataForSelectedMonth)
                {
                    this.ModelState.AddModelError("", "Съществуват месечни данни!");
                }

                if (this.ModelState.IsValid)
                {
                    var approvedDayQuery = this.data.UnitsApprovedDailyDatas.All()
                        .Where(x => x.RecordDate == day && x.ProcessUnitId == processUnitId);
                    if (materialTypeId == CommonConstants.EnergyType)
                    {
                        approvedDayQuery = approvedDayQuery.Where(x => x.EnergyApproved == true);
                    }

                    var approvedDay = approvedDayQuery.FirstOrDefault();
                    if (approvedDay != null)
                    {
                        try
                        {
                            using (var transaction = new TransactionScope(TransactionScopeOption.Required, this.transantionOption))
                            {
                                // remove unit approved daily record or change energy flag depending of materialTypeId
                                if (materialTypeId == CommonConstants.EnergyType)
                                {
                                    approvedDay.EnergyApproved = false;
                                    this.data.UnitsApprovedDailyDatas.Update(approvedDay);
                                }
                                else
                                {
                                    this.data.UnitsApprovedDailyDatas.Delete(approvedDay);
                                }

                                var status = this.data.SaveChanges(this.UserProfile.UserName);

                                // remove production plan data depending of materialTypeId
                                var productionPlanDataQuery = this.data.ProductionPlanDatas.All()
                                    .Include(x => x.ProductionPlanConfig)
                                    .Include(x => x.ProductionPlanConfig.MaterialType)
                                    .Where(x => x.RecordTimestamp == day && x.ProcessUnitId == processUnitId);
                                if (materialTypeId == CommonConstants.EnergyType)
                                {
                                    productionPlanDataQuery = productionPlanDataQuery.Where(x => x.ProductionPlanConfig.MaterialTypeId == CommonConstants.EnergyType);
                                }

                                var productionPlanDatas = productionPlanDataQuery.ToList();
                                foreach (var productionPlanData in productionPlanDatas)
                                {
                                    this.data.ProductionPlanDatas.Delete(productionPlanData);
                                }

                                status = this.data.SaveChanges(this.UserProfile.UserName);

                                transaction.Complete();
                                return Json(new { IsUnlocked = status.IsValid }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        catch (DbUpdateException)
                        {
                            this.ModelState.AddModelError("", "Отключването не можа да бъде осъществено. Моля опитайте на ново!");
                            Response.StatusCode = (int) HttpStatusCode.BadRequest;
                            List<string> errors = GetErrorListFromModelState(ModelState);
                            return Json(new { data = new { errors = errors } }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        this.ModelState.AddModelError("", "Дневните данни по избраните параметри не са потвърдени!");
                        Response.StatusCode = (int) HttpStatusCode.BadRequest;
                        List<string> errors = GetErrorListFromModelState(ModelState);
                        return Json(new { data = new { errors = errors } }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    List<string> errors = GetErrorListFromModelState(ModelState);
                    return Json(new { data = new { errors = errors } }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                List<string> errors = GetErrorListFromModelState(ModelState);
                return Json(new { data = new { errors = errors } }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UnlockMonthlyReport(int? monthlyReportTypeId, DateTime? month)
        {
            if (!monthlyReportTypeId.HasValue)
            {
                this.ModelState.AddModelError("", "Не е избран тип на месечен отчет");
            }

            if (!month.HasValue)
            {
                this.ModelState.AddModelError("", "Не е избран месец");
            }

            if (this.ModelState.IsValid)
            {
                var lastDayInMonth = new DateTime(month.Value.Year,
                    month.Value.Month,
                    DateTime.DaysInMonth(month.Value.Year, month.Value.Month));

                var approvedMonthTypes = this.data.UnitApprovedMonthlyDatas.All()
                    .Include(x => x.MonthlyReportType)
                    .Where(x => x.RecordDate == lastDayInMonth)
                    .ToDictionary(x => x.MonthlyReportTypeId, y => y.RecordDate);
                var monthlyReportTypes = this.data.MonthlyReportTypes.All().ToList();

                bool allMonthlyReportsAreApproved = true;
                foreach (var monthlyReportType in monthlyReportTypes)
                {
                    if(!approvedMonthTypes.ContainsKey(monthlyReportType.Id))
                    {
                        allMonthlyReportsAreApproved = false;
                    }
                }

                if (allMonthlyReportsAreApproved)
                {
                    this.ModelState.AddModelError("", "Всчики типове отчети за избраният месец са потвърдени!");
                }

                if (this.ModelState.IsValid)
                {
                    var approvedMonth = this.data.UnitApprovedMonthlyDatas.All()
                        .Where(x => x.MonthlyReportTypeId == monthlyReportTypeId
                        && x.RecordDate == lastDayInMonth).FirstOrDefault();
                    if (approvedMonth != null)
                    {
                        this.data.UnitApprovedMonthlyDatas.Delete(approvedMonth);
                        var status = data.SaveChanges(this.UserProfile.UserName);
                        return Json(new { IsUnlocked = status.IsValid }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        this.ModelState.AddModelError("", "Месечните данни по избраните параметри не са потвърдени!");
                        Response.StatusCode = (int) HttpStatusCode.BadRequest;
                        List<string> errors = GetErrorListFromModelState(ModelState);
                        return Json(new { data = new { errors = errors } }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    List<string> errors = GetErrorListFromModelState(ModelState);
                    return Json(new { data = new { errors = errors } }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                List<string> errors = GetErrorListFromModelState(ModelState);
                return Json(new { data = new { errors = errors } }, JsonRequestBehavior.AllowGet);
            }
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