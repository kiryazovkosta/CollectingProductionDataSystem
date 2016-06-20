namespace CollectingProductionDataSystem.Web.Infrastructure.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.IdentityInfrastructure;
    using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;


    public static class IdentityHelpers
    {
        public static MvcHtmlString GetUserName(this HtmlHelper html, int id)
        {
            var mgr = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            return new MvcHtmlString(mgr.FindById(id).UserName);
        }

        public static MvcHtmlString ActionIsAccessibleToCurrentRolls(this HtmlHelper html, string linkText, string actionName, string controllerName, object routeValues = null)
        {
            var user = html.ViewContext.HttpContext.User;
            string areaName = string.Empty;
            bool isReport = false;
            if (routeValues != null)
            {
                var type = routeValues.GetType();
                PropertyInfo propertyArea = type.GetProperty("area");
                if (propertyArea != null)
                {
                    var areaFromRequest = propertyArea.GetValue(routeValues, null);
                    areaName = areaFromRequest == null ?string.Empty: (string)areaFromRequest;
                    
                }
                PropertyInfo propertyIsReport = type.GetProperty("isReport");
                if (propertyIsReport != null)
                {
                    var isReportFromRequest = propertyIsReport.GetValue(routeValues, null);
                    isReport = isReportFromRequest == null ? false : (bool)isReportFromRequest;
                }
                
            }

            var rolesAllowed = GetAllowedRolesForController(html, controllerName, areaName);

            if (!IsUserOnlySummaryReporter(rolesAllowed, user) || isReport)
            {
                return html.AuthorizedActionLink(linkText, actionName, controllerName, routeValues, null);
            }

            return MvcHtmlString.Empty; ;
        }

        /// <summary>
        /// Determines whether [is user only summary reporter] [the specified roles allowed].
        /// </summary>
        /// <param name="rolesAllowed">The roles allowed.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        private static bool IsUserOnlySummaryReporter(IEnumerable<string> rolesAllowed, IPrincipal user)
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

        private static IEnumerable<string> GetAllowedRolesForController(HtmlHelper htmlHelper, string controllerName, string areaName)
        {
            IController controller = GetControllerInstance(htmlHelper, areaName, controllerName);

            var attributes = controller.GetType().GetCustomAttributes(true);//filterContext.ActionDescriptor.ControllerDescriptor.GetCustomAttributes(true);
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

            for (int i = 0; i < rolesAllowed.Count(); i++)
            {
                rolesAllowed[i] = rolesAllowed[i].Trim();
            }

            return rolesAllowed;
        }

        private static IController GetControllerInstance(HtmlHelper htmlHelper, string areaName, string controllerName)
        {
            IController controller;
            IControllerFactory controllerFactory = ControllerBuilder.Current.GetControllerFactory();
            RequestContext requestContext = new RequestContext(htmlHelper.ViewContext.RequestContext.HttpContext, new RouteData());
            requestContext.RouteData.DataTokens["Area"] = areaName;
            requestContext.RouteData.DataTokens["Controller"] = controllerName;
            if (areaName != "")
            {
                var routeData = RouteTable.Routes.FirstOrDefault(x => (x as Route).Url.Contains(areaName)) as Route;
                requestContext.RouteData.DataTokens["Namespaces"] = routeData.DataTokens["Namespaces"];
            }

            try
            {
                controller = ControllerBuilder.Current.GetControllerFactory().CreateController(requestContext, controllerName);
            }
            catch (HttpException httpException)
            {
                string[] strArrays = new string[]
                {
                    "Specified controller ",
                    controllerName,
                    " in area ",
                    areaName,
                    " does not exist."
                };
                throw new ArgumentException(string.Concat(strArrays));
            }

            return controller;
        }
    }
}