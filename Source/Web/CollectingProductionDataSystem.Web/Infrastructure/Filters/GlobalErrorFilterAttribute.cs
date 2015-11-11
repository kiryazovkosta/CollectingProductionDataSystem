namespace CollectingProductionDataSystem.Web.Infrastructure.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Web.ViewModels.Utility;


    public class GlobalErrorFilterAttribute : FilterAttribute, IExceptionFilter
    {
        //private readonly ILog logger;


        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalErrorFilterAttribute" /> class.
        /// </summary>
        //public GlobalErrorFilterAttribute(ILog loggerParam)
        //{
        //    this.logger = loggerParam;
        //}

        /// <summary>
        /// Called when an exception occurs.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnException(ExceptionContext filterContext)
        {
            var errorType = filterContext.Exception.GetType().Name;
            var errorArea = filterContext.RouteData.DataTokens["area"] ?? string.Empty;
            var errorController = filterContext.RouteData.Values["controller"] ?? string.Empty;
            var errorAction = filterContext.RouteData.Values["action"] ?? string.Empty;
            var errorMessage = filterContext.Exception.Message??string.Empty;
            var errorStackTrace = (filterContext.Exception.StackTrace??string.Empty).Replace("\r\n","\t\t\r\n");
            //logger.Error(string.Format("\t{0}\tArea: {1}\tController: {2}\tAction: {3}\r\n\tErrorMessage: {4}\r\n\tErrorStackTrace: {5}\r\n"
            //    , errorType
            //    , errorArea
            //    , errorController
            //    , errorAction
            //    , errorMessage
            //    , errorStackTrace));
            //filterContext.ExceptionHandled = true;

            filterContext.Result = new ViewResult
            {
                ViewName = "Error",
                //ViewData = new ViewDataDictionary<ErrorScreenViewModel>()
            };
        }
    }
}