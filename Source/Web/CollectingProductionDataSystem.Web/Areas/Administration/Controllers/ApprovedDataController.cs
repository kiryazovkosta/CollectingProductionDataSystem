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

    public class ApprovedDataController : AreaBaseController
    {
        public ApprovedDataController(IProductionData dataParam)
            : base(dataParam)
        {

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

        public ActionResult UnlockShiftReport(int? monthlyReportTypeId, DateTime? month)
        {
            if (!monthlyReportTypeId.HasValue)
            {
                this.ModelState.AddModelError("", "Не е избран тип на отчета");
            }

            if (!month.HasValue)
            {
                this.ModelState.AddModelError("", "Не е избран месец");
            }

            if (this.ModelState.IsValid)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                List<string> errors = GetErrorListFromModelState(ModelState);
                return Json(new { data = new { errors = errors } }, JsonRequestBehavior.AllowGet);

                return Json(new { IsUnlocked = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                List<string> errors = GetErrorListFromModelState(ModelState);
                return Json(new { data = new { errors = errors } }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UnlockDailyReport(int? monthlyReportTypeId, DateTime? month)
        {
            if (!monthlyReportTypeId.HasValue)
            {
                this.ModelState.AddModelError("", "Не е избран тип на отчета");
            }

            if (!month.HasValue)
            {
                this.ModelState.AddModelError("", "Не е избран месец");
            }

            if (this.ModelState.IsValid)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                List<string> errors = GetErrorListFromModelState(ModelState);
                return Json(new { data = new { errors = errors } }, JsonRequestBehavior.AllowGet);

                return Json(new { IsUnlocked = false }, JsonRequestBehavior.AllowGet);
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
                this.ModelState.AddModelError("", "Не е избран тип на отчета");
            }

            if (!month.HasValue)
            {
                this.ModelState.AddModelError("", "Не е избран месец");
            }

            if (this.ModelState.IsValid)
            {
                Response.StatusCode = (int) HttpStatusCode.BadRequest;
                List<string> errors = GetErrorListFromModelState(ModelState);
                return Json(new { data = new { errors = errors } }, JsonRequestBehavior.AllowGet);

                return Json(new { IsUnlocked = false }, JsonRequestBehavior.AllowGet);
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