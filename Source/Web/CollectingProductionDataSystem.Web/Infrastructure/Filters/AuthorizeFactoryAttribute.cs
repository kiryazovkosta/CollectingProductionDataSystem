namespace CollectingProductionDataSystem.Web.Infrastructure.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Models.UtilityEntities;
    using CollectingProductionDataSystem.Web.Controllers;
    using Resources = App_GlobalResources.Resources;

    public class AuthorizeFactoryAttribute : FilterAttribute, IActionFilter
    {
        /// <summary>
        /// Called before an action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var controller = filterContext.Controller as BaseController;
            if (controller != null)
            {
                int requestProcessUnit;
                int.TryParse(filterContext.ActionParameters["processUnitId"].ToString(), out requestProcessUnit);
                if (controller.UserProfile.ProcessUnits.Any(x => x.Id == requestProcessUnit) || this.IsPowerUser(controller.UserProfile))
                {
                    return;
                }
            }

            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                ((BaseController)filterContext.Controller).ModelState.AddModelError(string.Empty,Resources.ErrorMessages.UnAuthorized);
            }
            else
            {
                controller.ViewBag.Title = Resources.ErrorMessages.Error;
                controller.ViewBag.Message = Resources.ErrorMessages.UnAuthorized;
                filterContext.Result = new ViewResult() { ViewName = "Error" };
            }

        }

        /// <summary>
        /// Called after the action method executes.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // Do nothig
        }

        private bool IsPowerUser(UserProfile userProfile)
        {
            return userProfile.UserRoles.Where(x => CommonConstants.PowerUsers.Any(y => y == x.Name)).Any();
        }
    }
}