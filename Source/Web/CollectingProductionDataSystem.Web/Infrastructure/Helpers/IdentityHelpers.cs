namespace CollectingProductionDataSystem.Web.Infrastructure.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
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

        public static bool ActionIsAccessibleToCurrentRolls(this HtmlHelper html, IPrincipal user)
        {
            //ToDo: White this shit
            if (userId < 1)
            {
                return false;
            }
            var mgr = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roles = mgr.GetRoles(userId);
            return true;
        }

        /// <summary>
        /// Determines whether [is user only summary reporter] [the specified roles allowed].
        /// </summary>
        /// <param name="rolesAllowed">The roles allowed.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        private static bool IsUserOnlySummaryReporter(string[] rolesAllowed, IPrincipal user)
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