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

        public ActionResult About() 
        {
            return View();
        }
    }
}