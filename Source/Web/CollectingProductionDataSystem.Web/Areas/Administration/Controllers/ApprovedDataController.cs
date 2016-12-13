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
    using System.Data.SqlClient;
    using Models.Productions.Mounthly;
    using Models.Productions.Technological;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult TechMonthIndex()
        {
            return Json(DateTime.Now);
        }

        public ActionResult UnlockShiftReport(DateTime? day, int? processUnitId, int? shiftId)
        {
            if (!day.HasValue)
            {
                this.ModelState.AddModelError(key: "", errorMessage: "Не е избрана дата");
            }

            if (!processUnitId.HasValue || processUnitId.Value <= 0)
            {
                this.ModelState.AddModelError(key: "", errorMessage: "Не е избрана инсталация");
            }

            if (!shiftId.HasValue || shiftId.Value <= 0)
            {
                this.ModelState.AddModelError(key: "", errorMessage: "Не е избрана смяна");
            }

            if (this.ModelState.IsValid)
            {
                bool exsistDailyData = this.data.UnitsDailyDatas.All().Where(x => x.RecordTimestamp == day).Any();
                if (exsistDailyData)
                {
                    this.ModelState.AddModelError(key: "", errorMessage: "Вече има генерирани данни за избраният ден.");
                }

                if (this.ModelState.IsValid)
                {
                    UnitsApprovedData approvedShift = this.data.UnitsApprovedDatas.All().Where(x => x.RecordDate == day && x.ProcessUnitId == processUnitId && x.ShiftId == shiftId).FirstOrDefault();
                    if (approvedShift != null)
                    {
                        string deleteSqlQueryText = @"DELETE FROM [dbo].[UnitsApprovedDatas] WHERE [RecordDate] = @RecordDate AND [ShiftId] = @ShiftId AND [ProcessUnitId] = @ProcessUnitId";
                        int deleteResult = this.data.Context.DbContext.Database.ExecuteSqlCommand(
                            deleteSqlQueryText,
                            new SqlParameter(parameterName: "@RecordDate", value: day.Value),
                            new SqlParameter(parameterName: "@ShiftId", value: shiftId.Value),
                            new SqlParameter(parameterName: "@ProcessUnitId", value: processUnitId.Value));

                        return Json(new { IsUnlocked = (deleteResult == 1) }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        this.ModelState.AddModelError(key: "", errorMessage: "Смените данни по избраните параметри не са потвърдени!");
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
                this.ModelState.AddModelError(key: "", errorMessage: "Не е избрана дата");
            }

            if (!processUnitId.HasValue || processUnitId.Value <= 0)
            {
                this.ModelState.AddModelError(key: "", errorMessage: "Не е избрана инсталация");
            }

            if (!materialTypeId.HasValue || materialTypeId.Value <= 0 || materialTypeId.Value > 2)
            {
                this.ModelState.AddModelError(key: "", errorMessage: "Не е избрана тип на материала");
            }

            if (this.ModelState.IsValid)
            {
                bool exsistingDailyDataForRecentDay = this.data.UnitsDailyDatas.All().Where(x => x.RecordTimestamp > day).Any();
                if (exsistingDailyDataForRecentDay)
                {
                    this.ModelState.AddModelError(key: "", errorMessage: "Съществуват дневни данни от по-скоро!");
                }

                var month = new DateTime(day.Value.Year, day.Value.Month, DateTime.DaysInMonth(day.Value.Year, day.Value.Month));
                bool exsistingMonthlyDataForSelectedMonth = this.data.UnitMonthlyDatas.All().Where(x => x.RecordTimestamp == month).Any();
                if (exsistingMonthlyDataForSelectedMonth)
                {
                    this.ModelState.AddModelError(key: "", errorMessage: "Съществуват месечни данни!");
                }

                if (this.ModelState.IsValid)
                {
                    var approvedDayQuery = this.data.UnitsApprovedDailyDatas.All()
                        .Where(x => x.RecordDate == day && x.ProcessUnitId == processUnitId);
                    if (materialTypeId == CommonConstants.EnergyType)
                    {
                        approvedDayQuery = approvedDayQuery.Where(x => x.EnergyApproved == true);
                    }

                    UnitsApprovedDailyData approvedDay = approvedDayQuery.FirstOrDefault();
                    if (approvedDay != null)
                    {
                        try
                        {
                            using (var transaction = new TransactionScope(TransactionScopeOption.Required, this.transantionOption))
                            {
                                if (materialTypeId == CommonConstants.EnergyType)
                                {
                                    approvedDay.EnergyApproved = false;
                                    this.data.UnitsApprovedDailyDatas.Update(approvedDay);
                                }
                                else
                                {
                                    this.data.UnitsApprovedDailyDatas.Delete(approvedDay);
                                }

                                IEfStatus status = this.data.SaveChanges(this.UserProfile.UserName);

                                IQueryable<ProductionPlanData> productionPlanDataQuery = this.data.ProductionPlanDatas.All()
                                    .Include(x => x.ProductionPlanConfig)
                                    .Include(x => x.ProductionPlanConfig.MaterialType)
                                    .Where(x => x.RecordTimestamp == day && x.ProcessUnitId == processUnitId);
                                if (materialTypeId == CommonConstants.EnergyType)
                                {
                                    productionPlanDataQuery = productionPlanDataQuery.Where(x => x.ProductionPlanConfig.MaterialTypeId == CommonConstants.EnergyType);
                                }

                                List<ProductionPlanData> productionPlanDatas = productionPlanDataQuery.ToList();
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
                            this.ModelState.AddModelError(key: "", errorMessage: "Отключването не можа да бъде осъществено. Моля опитайте на ново!");
                            Response.StatusCode = (int) HttpStatusCode.BadRequest;
                            List<string> errors = GetErrorListFromModelState(ModelState);
                            return Json(new { data = new { errors = errors } }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        this.ModelState.AddModelError(key: "", errorMessage: "Дневните данни по избраните параметри не са потвърдени!");
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
                this.ModelState.AddModelError(key: "", errorMessage: "Не е избран тип на месечен отчет");
            }

            if (!month.HasValue)
            {
                this.ModelState.AddModelError(key: "", errorMessage: "Не е избран месец");
            }

            if (this.ModelState.IsValid)
            {
                var lastDayInMonth = new DateTime(month.Value.Year,
                    month.Value.Month,
                    DateTime.DaysInMonth(month.Value.Year, month.Value.Month));

                Dictionary<int, DateTime> approvedMonthTypes = this.data.UnitApprovedMonthlyDatas.All()
                    .Include(x => x.MonthlyReportType)
                    .Where(x => x.RecordDate == lastDayInMonth)
                    .ToDictionary(x => x.MonthlyReportTypeId, y => y.RecordDate);
                List<Models.Productions.Mounthly.MonthlyReportType> monthlyReportTypes = this.data.MonthlyReportTypes.All().ToList();

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
                    this.ModelState.AddModelError(key: "", errorMessage: "Всчики типове отчети за избраният месец са потвърдени!");
                }

                if (this.ModelState.IsValid)
                {
                    UnitApprovedMonthlyData approvedMonth = this.data.UnitApprovedMonthlyDatas.All()
                        .Where(x => x.MonthlyReportTypeId == monthlyReportTypeId && x.RecordDate == lastDayInMonth).FirstOrDefault();
                    if (approvedMonth != null)
                    {
                        this.data.UnitApprovedMonthlyDatas.Delete(approvedMonth);
                        var status = data.SaveChanges(this.UserProfile.UserName);
                        return Json(new { IsUnlocked = status.IsValid }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        this.ModelState.AddModelError(key: "", errorMessage: "Месечните данни по избраните параметри не са потвърдени!");
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

        public ActionResult UnlockMonthlyTechnologicalReport(int? factoryId, DateTime? month)
        {
            if (!factoryId.HasValue)
            {
                this.ModelState.AddModelError(key: "", errorMessage: "Не е избрано производство");
            }

            if (!month.HasValue)
            {
                this.ModelState.AddModelError(key: "", errorMessage: "Не е избран месец");
            }

            if (this.ModelState.IsValid)
            {
                //var lastDayInMonth = new DateTime(month.Value.Year, month.Value.Month, DateTime.DaysInMonth(month.Value.Year, month.Value.Month));

                MonthlyTechnologicalReportsData approvedMonthlyTechReport = this.data.MonthlyTechnologicalReportsDatas
                    .All()
                    .FirstOrDefault(x => x.FactoryId == factoryId
                    && x.Month == month
                    && x.IsComposed == true
                    );

                if (approvedMonthlyTechReport != null)
                {
                    approvedMonthlyTechReport.IsComposed = false;
                    approvedMonthlyTechReport.ComposedBy = null;
                    approvedMonthlyTechReport.ComposedOn = null;
                    approvedMonthlyTechReport.IsApproved = false;
                    approvedMonthlyTechReport.ApprovedBy = null;
                    approvedMonthlyTechReport.ApprovedOn = null;

                    this.data.MonthlyTechnologicalReportsDatas.Update(approvedMonthlyTechReport);
                    IEfStatus status = data.SaveChanges(this.UserProfile.UserName);
                    return Json(new { IsUnlocked = status.IsValid }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    this.ModelState.AddModelError(key: "", errorMessage: "Технологичният отчет по избраните параметри не е съставен или утвърден!");
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
            IEnumerable<string> query = from state in modelState.Values
                        from error in state.Errors
                        select error.ErrorMessage;
            return query.ToList();
        }
    }
}