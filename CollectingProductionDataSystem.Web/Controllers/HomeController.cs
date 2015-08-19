namespace CollectingProductionDataSystem.Web.Controllers
{
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Enumerations;

    public class HomeController : Controller
    {
        public ActionResult Index(ManageMessageId? message )
        {
            if (message != null)
            {
                ViewBag.StatusMessage =
                    message == ManageMessageId.ChangePasswordSuccess ? App_GlobalResources.Layout.ChanhePasswordSuccess
                    : message == ManageMessageId.Error ? App_GlobalResources.ErrorMessages.Error
                    : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                    : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                    : string.Empty;
            }
            ViewBag.MessageType = message == ManageMessageId.Error ? "Error" : "Success";
            return View();
        }

        public ActionResult About() 
        {
            return View();
        }
    }
}