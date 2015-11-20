using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CollectingProductionDataSystem.Web.Infrastructure.Filters
{
    public class CheckUserSessionAttribute : ActionFilterAttribute
    {

        /// <summary>
        /// Called before an action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var uri = string.Format("{0}/{1}", filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.ToLower().Trim(),
           filterContext.ActionDescriptor.ActionName.ToLower().Trim());

            if (uri != "account/login"
                && uri != "account/getgreetingmessage"
                && uri != "home/index"
                && filterContext.HttpContext.Session["user"] == null)
            {

                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { action = "Login", controller = "Account", area = string.Empty }));

                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}