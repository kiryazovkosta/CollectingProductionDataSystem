namespace CollectingProductionDataSystem.Web.Infrastructure.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Infrastructure.Contracts;
    using CollectingProductionDataSystem.Web.ViewModels.Utility;

    public class GlobalErrorFilterAttribute : FilterAttribute, IExceptionFilter
    {
        private readonly ILogger logger;
        public GlobalErrorFilterAttribute(ILogger loggerParam) 
        :base()
        {
            this.logger = loggerParam;
        }

        public void OnException(ExceptionContext filterContext)
        {
            var parameters = filterContext.HttpContext.Request.Params;
            var details = new List<string>();
            foreach (var key in parameters.AllKeys)
            {
                details.Add(string.Format("{0} = {1}", key, parameters[key]));
            }
            this.logger.Error(filterContext.Exception.Message, filterContext.Exception.Source, filterContext.Exception,details);

            filterContext.ExceptionHandled = true;

            filterContext.Result = new RedirectResult("/Content/ErrorPage/Error.html");
        }
    }
}