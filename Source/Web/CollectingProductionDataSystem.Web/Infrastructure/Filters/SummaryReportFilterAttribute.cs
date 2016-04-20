using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Infrastructure.Filters
{
    public class SummaryReportFilterAttribute : ActionFilterAttribute, IActionFilter
    {       
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.Controller.TempData.ContainsKey("isReport"))
            {
                if (filterContext.ActionParameters.ContainsKey("isReport"))
                {
                    filterContext.ActionParameters["isReport"] = filterContext.Controller.TempData["isReport"];
                }
            }
            else
            {
                if (filterContext.ActionParameters.ContainsKey("isReport"))
                {
                    filterContext.ActionParameters["isReport"] = false;
                }
            }
           
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Controller.TempData.ContainsKey("isReport"))
            {
                filterContext.Controller.TempData["isReport"] = filterContext.Controller.TempData["isReport"];
            }

            base.OnActionExecuted(filterContext);
        }

    }
}