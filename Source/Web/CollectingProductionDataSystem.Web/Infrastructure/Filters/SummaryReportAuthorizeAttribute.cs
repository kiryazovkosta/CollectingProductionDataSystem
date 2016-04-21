using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Infrastructure.Filters
{
    public class SummaryReportAuthorizeAttribute : AuthorizeAttribute
    {


        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            var attributes = filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(true);
            AuthorizeAttribute filter = new AuthorizeAttribute();
            foreach (var attribute in attributes)
            {
                if (attribute is AuthorizeAttribute)
                {
                    filter = attribute as AuthorizeAttribute;
                    break;
                }
            }

            var rolesAllowed = filter.Roles.Split(",".ToArray<char>(), StringSplitOptions.RemoveEmptyEntries);

            foreach (var roleName in rolesAllowed)
            {
                roleName = roleName.Trim();
            }

            var user = filterContext.HttpContext.User;

            if (IsUserOnlySummaryReporter(rolesAllowed, user))
            {
                var actionAttribute = filterContext.ActionDescriptor.GetCustomAttributes(true).FirstOrDefault(x => x is SummaryReportFilterAttribute) as SummaryReportFilterAttribute;
                var strValue = (filterContext.HttpContext.Request.Params.Get("isReport") ?? string.Empty).Split(',')[0].Trim();
                bool valueOfIsReportParam = string.IsNullOrEmpty(strValue) ? false : Convert.ToBoolean(strValue);

                if ((actionAttribute == null) || valueOfIsReportParam)
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                }
            }
        }

        /// <summary>
        /// Determines whether [is user only summary reporter] [the specified roles allowed].
        /// </summary>
        /// <param name="rolesAllowed">The roles allowed.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        private bool IsUserOnlySummaryReporter(string[] rolesAllowed, IPrincipal user)
        {
            var result = user.IsInRole("SummaryReporter");

            foreach (var roleName in rolesAllowed)
            {
                if (user.IsInRole(roleName) && roleName != "SummaryReporter")
                {
                    result = false;
                }
            }

            return result;
        }
    }
}