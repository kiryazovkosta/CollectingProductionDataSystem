using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CollectingProductionDataSystem.Web.Infrastructure.Filters
{
    public class SummaryReportAuthorizeAttribute:AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
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

            var roles = filter.Roles.Split(",".ToArray<char>(), StringSplitOptions.RemoveEmptyEntries);
            base.OnAuthorization(filterContext);
            
        }
    }
}