namespace CollectingProductionDataSystem.Web.Infrastructure.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;

    public static class AuthorizedActionLinkHtmlHelper
    {
        public static bool ActionIsAccessibleToUser(this HtmlHelper htmlHelper, string actionName)
        {
            return htmlHelper.ActionIsAccessibleToUser(actionName, string.Empty);
        }

        public static bool ActionIsAccessibleToUser(this HtmlHelper htmlHelper, string actionName, string controllerName, string areaName)
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
                string[] strArrays = new string[] { "Specified controller ", controllerName, " in area ", areaName, " does not exist." };
                throw new ArgumentException(string.Concat(strArrays));
            }
            if (controller == null)
            {
                string[] strArrays1 = new string[] { "Specified controller ", controllerName, " in area ", areaName, " does not exist." };
                throw new ArgumentException(string.Concat(strArrays1));
            }
            return htmlHelper.ActionIsAccessibleToUser(actionName, (ControllerBase)controller);
        }

        public static bool ActionIsAccessibleToUser(this HtmlHelper htmlHelper, string actionName, string controllerName)
        {
            ControllerBase controller;
            if (!string.IsNullOrWhiteSpace(controllerName))
            {
                IControllerFactory controllerFactory = ControllerBuilder.Current.GetControllerFactory();
                IController controller1 = null;
                try
                {
                    controller1 = controllerFactory.CreateController(htmlHelper.ViewContext.RequestContext, controllerName);
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
                controller = htmlHelper.ViewContext.Controller;
            }
            return htmlHelper.ActionIsAccessibleToUser(actionName, controller);
        }

        private static bool ActionIsAccessibleToUser(this HtmlHelper htmlHelper, string actionName, ControllerBase controllerBase)
        {
            ControllerContext controllerContext = new ControllerContext(htmlHelper.ViewContext.RequestContext, controllerBase);
            ReflectedControllerDescriptor reflectedControllerDescriptor = new ReflectedControllerDescriptor(controllerBase.GetType());
            return AuthorizedActionLinkHtmlHelper.ActionIsAuthorized(reflectedControllerDescriptor.FindAction(controllerContext, actionName), controllerContext);
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
                return true;
            }
            return flag;
        }

        public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName)
        {
            if (!htmlHelper.ActionIsAccessibleToUser(actionName))
            {
                return MvcHtmlString.Empty;
            }
            return htmlHelper.ActionLink(linkText, actionName);
        }

        public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            if (!htmlHelper.ActionIsAccessibleToUser(actionName))
            {
                return MvcHtmlString.Empty;
            }
            return htmlHelper.ActionLink(linkText, actionName, routeValues);
        }

        public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName))
            {
                return MvcHtmlString.Empty;
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName);
        }

        public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            if (!htmlHelper.ActionIsAccessibleToUser(actionName))
            {
                return MvcHtmlString.Empty;
            }
            return htmlHelper.ActionLink(linkText, actionName, routeValues);
        }

        public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            if (!htmlHelper.ActionIsAccessibleToUser(actionName))
            {
                return MvcHtmlString.Empty;
            }
            return htmlHelper.ActionLink(linkText, actionName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (!htmlHelper.ActionIsAccessibleToUser(actionName))
            {
                return MvcHtmlString.Empty;
            }
            return htmlHelper.ActionLink(linkText, actionName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            Type type = routeValues.GetType();
            if ((type.GetProperty("area") == null ? true : false))
            {
                if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName))
                {
                    return MvcHtmlString.Empty;
                }
                return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
            }
            if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName, (string)type.GetProperty("area").GetValue(routeValues, null)))
            {
                return MvcHtmlString.Empty;
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if ((routeValues.ContainsKey("area") ? false : true))
            {
                if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName))
                {
                    return MvcHtmlString.Empty;
                }
                return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
            }
            if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName, (string)routeValues["area"]))
            {
                return MvcHtmlString.Empty;
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            Type type = routeValues.GetType();
            if ((type.GetProperty("area") == null ? true : false))
            {
                if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName))
                {
                    return MvcHtmlString.Empty;
                }
                return htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
            }
            if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName, (string)type.GetProperty("area").GetValue(routeValues, null)))
            {
                return MvcHtmlString.Empty;
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
        }

        public static MvcHtmlString AuthorizedActionLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if ((routeValues.ContainsKey("area") ? false : true))
            {
                if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName))
                {
                    return MvcHtmlString.Empty;
                }
                return htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
            }
            if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName, (string)routeValues["area"]))
            {
                return MvcHtmlString.Empty;
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
        }

        public static MvcHtmlString AuthorizedActionLinkOrText(this HtmlHelper htmlHelper, string linkText, string actionName)
        {
            if (!htmlHelper.ActionIsAccessibleToUser(actionName))
            {
                return MvcHtmlString.Create(linkText);
            }
            return htmlHelper.ActionLink(linkText, actionName);
        }

        public static MvcHtmlString AuthorizedActionLinkOrText(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues)
        {
            if (!htmlHelper.ActionIsAccessibleToUser(actionName))
            {
                return MvcHtmlString.Create(linkText);
            }
            return htmlHelper.ActionLink(linkText, actionName, routeValues);
        }

        public static MvcHtmlString AuthorizedActionLinkOrText(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName)
        {
            if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName))
            {
                return MvcHtmlString.Create(linkText);
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName);
        }

        public static MvcHtmlString AuthorizedActionLinkOrText(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues)
        {
            if (!htmlHelper.ActionIsAccessibleToUser(actionName))
            {
                return MvcHtmlString.Create(linkText);
            }
            return htmlHelper.ActionLink(linkText, actionName, routeValues);
        }

        public static MvcHtmlString AuthorizedActionLinkOrText(this HtmlHelper htmlHelper, string linkText, string actionName, object routeValues, object htmlAttributes)
        {
            if (!htmlHelper.ActionIsAccessibleToUser(actionName))
            {
                return MvcHtmlString.Create(linkText);
            }
            return htmlHelper.ActionLink(linkText, actionName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString AuthorizedActionLinkOrText(this HtmlHelper htmlHelper, string linkText, string actionName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if (!htmlHelper.ActionIsAccessibleToUser(actionName))
            {
                return MvcHtmlString.Create(linkText);
            }
            return htmlHelper.ActionLink(linkText, actionName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString AuthorizedActionLinkOrText(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, object routeValues, object htmlAttributes)
        {
            Type type = routeValues.GetType();
            if ((type.GetProperty("area") == null ? true : false))
            {
                if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName))
                {
                    return MvcHtmlString.Create(linkText);
                }
                return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
            }
            if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName, (string)type.GetProperty("area").GetValue(routeValues, null)))
            {
                return MvcHtmlString.Create(linkText);
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString AuthorizedActionLinkOrText(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if ((routeValues.ContainsKey("area") ? false : true))
            {
                if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName))
                {
                    return MvcHtmlString.Create(linkText);
                }
                return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
            }
            if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName, (string)routeValues["area"]))
            {
                return MvcHtmlString.Create(linkText);
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, routeValues, htmlAttributes);
        }

        public static MvcHtmlString AuthorizedActionLinkOrText(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, object routeValues, object htmlAttributes)
        {
            Type type = routeValues.GetType();
            if ((type.GetProperty("area") == null ? true : false))
            {
                if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName))
                {
                    return MvcHtmlString.Create(linkText);
                }
                return htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
            }
            if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName, (string)type.GetProperty("area").GetValue(routeValues, null)))
            {
                return MvcHtmlString.Create(linkText);
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
        }

        public static MvcHtmlString AuthorizedActionLinkOrText(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            if ((routeValues.ContainsKey("area") ? false : true))
            {
                if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName))
                {
                    return MvcHtmlString.Create(linkText);
                }
                return htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
            }
            if (!htmlHelper.ActionIsAccessibleToUser(actionName, controllerName, (string)routeValues["area"]))
            {
                return MvcHtmlString.Create(linkText);
            }
            return htmlHelper.ActionLink(linkText, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes);
        }
    }
}