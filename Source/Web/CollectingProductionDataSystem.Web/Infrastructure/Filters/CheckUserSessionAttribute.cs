namespace CollectingProductionDataSystem.Web.Infrastructure.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using Microsoft.AspNet.Identity;

    public class CheckUserSessionAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Called before an action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Trim();
            var actionName = filterContext.ActionDescriptor.ActionName.Trim();
            var uri = string.Format("{0}/{1}", controllerName.ToLower(), actionName.ToLower());
            var returnUrl = ConstructReturnUrl(filterContext);

            if (uri != "account/login"
                && uri != "account/getgreetingmessage"
                && uri != "home/index"
                && filterContext.HttpContext.Session["user"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new
                    {
                        action = "Login",
                        controller = "Account",
                        area = string.Empty,
                        returnUrl = returnUrl
                    }));
                return;
            }

            if (uri == "home/index"
                && filterContext.HttpContext.User.Identity.IsAuthenticated
                && filterContext.HttpContext.Session["user"] == null)
            {
                filterContext.HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                ClearCookies(filterContext);
            }

            base.OnActionExecuting(filterContext);
        }

        private void ClearCookies(ActionExecutingContext filterContext)
        {
            //clear client-site cookies to prevents unauthorized access

            var requestCookyKeys = filterContext.HttpContext.Request.Cookies.AllKeys;

            filterContext.HttpContext.Response.Cookies.Clear();
            foreach (var key in requestCookyKeys)
            {
                filterContext.HttpContext.Response.Cookies.Add(new HttpCookie(key) { Expires = DateTime.Now.AddDays(-1) });
            }
        }

        /// <summary>
        /// Constructs the return URL.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        /// <returns></returns>
        private string ConstructReturnUrl(ActionExecutingContext filterContext)
        {
            var areaName = (filterContext.RequestContext.RouteData.DataTokens["area"] ?? string.Empty).ToString();
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName.Trim();
            var actionName = filterContext.ActionDescriptor.ActionName.Trim();

            StringBuilder returnUrl = new StringBuilder(1000);
            if (!string.IsNullOrEmpty(areaName))
            {
                returnUrl.Append("/" + areaName);
            }
            if (!string.IsNullOrEmpty(controllerName))
            {
                returnUrl.Append("/" + controllerName);
            }
            if (!string.IsNullOrEmpty(actionName))
            {
                returnUrl.Append("/" + actionName);
            }

            return returnUrl.ToString();
        }
    }
}