namespace CollectingProductionDataSystem.Web.Controllers
{
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Enumerations;
    using Resources = App_GlobalResources.Resources;

    public class HomeController : Controller
    {
        public ActionResult Index(ManageMessageId? message )
        {
            if (message != null)
            {
                ViewBag.StatusMessage =
                    message == ManageMessageId.ChangePasswordSuccess ? Resources.Layout.ChanhePasswordSuccess
                    : message == ManageMessageId.Error ? Resources.ErrorMessages.Error
                    : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                    : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                    : string.Empty;
            }
            ViewBag.MessageType = message == ManageMessageId.Error ? "Error" : "Success";
            return View();
        }

        public ActionResult ShowMessagesGrid() 
        {
            return View();
        }


        //Define Authorization for Base Menus

        [Authorize(Roles="Administrator, ShiftReporter, HighwayPipelinesDataReporter")]
        public ActionResult AuthorizeShiftMenu(){return Content("");}

        [Authorize(Roles="Administrator, DailyReporter, TanksStatusesReporter")]
        public ActionResult AuthorizeDailyMenu(){return Content("");}

        [Authorize(Roles= "Administrator, MonthlyReporter, MonthlyHydroCarbonsReporter,MonthlyChemicalReporter,MonthlyFreshWaterReporter, MonthlyCirculatingWaterReporter,MonthlyChemicalClearedWaterReporter, MonthlyPotableWaterReporter, MonthlyHeatEnergyReporter, MonthlyElectricalEnergyReporter, MonthlyAirReporter, MonthlyNitrogenReporter")]
        public ActionResult AuthorizeMonthlyMenu(){return Content("");}

        [Authorize(Roles="Administrator,SummaryReporter")]
        public ActionResult AuthorizeSummaryMenu(){return Content("");}




    }
}