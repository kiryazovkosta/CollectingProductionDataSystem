﻿namespace CollectingProductionDataSystem.Web.Infrastructure.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using CollectingProductionDataSystem.Infrastructure.Contracts;

    public class HandleAntiforgeryTokenErrorAttribute : HandleErrorAttribute
    {
        ILogger logger;
        public HandleAntiforgeryTokenErrorAttribute(ILogger loggerParam)
            :base()
        {
            this.logger = loggerParam;
        }
        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception.GetType()==this.ExceptionType)
            {
                filterContext.ExceptionHandled = true;
                logger.AuthenticationError("Handled AntiforgeryTokenError", this, filterContext.HttpContext.User.Identity.Name);
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { action = "Login", controller = "Account"}));

                //clear client-site cookies to prevents unauthorized access

                var requestCookyKeys = filterContext.HttpContext.Request.Cookies.AllKeys;

                filterContext.HttpContext.Response.Cookies.Clear();
                foreach (var key in requestCookyKeys)
                {
                    filterContext.HttpContext.Response.Cookies.Add(new HttpCookie(key) { Expires = DateTime.Now.AddDays(-1) });
                } 
            }
        }
    }
}