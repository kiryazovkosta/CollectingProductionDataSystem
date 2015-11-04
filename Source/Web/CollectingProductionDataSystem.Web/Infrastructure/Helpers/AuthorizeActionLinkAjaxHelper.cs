using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace CollectingProductionDataSystem.Web.Infrastructure.Helpers
{
    public static class AuthorizeActionLinkAjaxHelper
    {
        private static bool ActionIsAccessibleToUser(this AjaxHelper ajaxHelper, string actionName, ControllerBase controllerBase)
        {
            ControllerContext controllerContext = new ControllerContext(ajaxHelper.ViewContext.RequestContext, controllerBase);
            ReflectedControllerDescriptor reflectedControllerDescriptor = new ReflectedControllerDescriptor(controllerBase.GetType());
            return AuthorizeActionLinkAjaxHelper.ActionIsAuthorized(reflectedControllerDescriptor.FindAction(controllerContext, actionName), controllerContext);
        }

        private static bool ActionIsAuthorized(ActionDescriptor actionDescriptor, ControllerContext controllerContext)
        {
            bool flag;
            if (actionDescriptor == null)
            {
                return false;
            }
            AuthorizationContext authorizationContext = new AuthorizationContext(controllerContext, actionDescriptor);
            IEnumerable<Filter> filters = FilterProviders.Providers.GetFilters(controllerContext, actionDescriptor);
            using (IEnumerator<IAuthorizationFilter> enumerator = (new FilterInfo(filters)).AuthorizationFilters.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    enumerator.Current.OnAuthorization(authorizationContext);
                    if (authorizationContext.Result == null)
                    {
                        continue;
                    }
                    flag = false;
                    return flag;
                }
                flag = true;
            }
            return flag;
        }

        public static bool ActionIsAccessibleToUser(this AjaxHelper ajaxHelper, string actionName, string controllerName, string areaName)
        {
            IController controller;

            if (areaName == null)
            {
                throw new ArgumentException("Argument areaName cannot be null. If root area is desired, specify parameter as an empty string.");
            }

            if (string.IsNullOrWhiteSpace(controllerName))
            {
                throw new ArgumentException("Argument controllerName must be specified.");
            }

            RequestContext requestContext = new RequestContext(ajaxHelper.ViewContext.RequestContext.HttpContext, new RouteData());
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
                string[] strArrays = new string[] { "Specified controller ", controllerName, " in area ", areaName, " does not exist." };
                throw new ArgumentException(string.Concat(strArrays));
            }
            if (controller == null)
            {
                string[] strArrays1 = new string[] { "Specified controller ", controllerName, " in area ", areaName, " does not exist." };
                throw new ArgumentException(string.Concat(strArrays1));
            }
            return ajaxHelper.ActionIsAccessibleToUser(actionName, (ControllerBase)controller);
        }

        public static bool ActionIsAccessibleToUser(this AjaxHelper ajaxHelper, string actionName, string controllerName)
        {
            ControllerBase controller;
            if (!string.IsNullOrWhiteSpace(controllerName))
            {
                IControllerFactory controllerFactory = ControllerBuilder.Current.GetControllerFactory();
                IController controller1 = null;
                try
                {
                    controller1 = controllerFactory.CreateController(ajaxHelper.ViewContext.RequestContext, controllerName);
                }
                catch (HttpException httpException)
                {
                    throw new ArgumentException(string.Concat("Specified controller ", controllerName, " does not exist."));
                }
                if (controller1 == null)
                {
                    throw new ArgumentException(string.Concat("Specified controller ", controllerName, " does not exist."));
                }
                controller = (ControllerBase)controller1;
            }
            else
            {
                controller = ajaxHelper.ViewContext.Controller;
            }
            return ajaxHelper.ActionIsAccessibleToUser(actionName, controller);
        }

        public static MvcHtmlString AuthorizedAjaxActionLinkOrText(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, AjaxOptions options, object routeValues=null, object htmlAttributes = null )
        {
            Type type = routeValues.GetType();
            if ((type.GetProperty("area") == null ? true : false))
            {
                if (!ajaxHelper.ActionIsAccessibleToUser(actionName, controllerName))
                {
                    return MvcHtmlString.Create(linkText);
                }
                return ajaxHelper.ActionLink(linkText, actionName, controllerName, routeValues, options, htmlAttributes);
            }
            if (!ajaxHelper.ActionIsAccessibleToUser(actionName, controllerName, (string)type.GetProperty("area").GetValue(routeValues, null)))
            {
                return MvcHtmlString.Create(linkText);
            }
            
           return ajaxHelper.ActionLink(linkText, actionName, controllerName, routeValues,options, htmlAttributes);
        }
    }
}