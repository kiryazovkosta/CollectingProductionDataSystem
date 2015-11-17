namespace CollectingProductionDataSystem.Web.Infrastructure.Filters
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
            filterContext.ExceptionHandled = true;
            logger.AuthenticationError("Handled AntiforgeryTokenError", this, filterContext.HttpContext.User.Identity.Name);
            filterContext.Result = new RedirectToRouteResult(
                new RouteValueDictionary(new { action = "Login", controller = "Account" }));
        }
    }
}