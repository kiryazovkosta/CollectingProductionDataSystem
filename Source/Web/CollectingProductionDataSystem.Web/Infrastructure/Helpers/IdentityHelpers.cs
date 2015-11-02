namespace CollectingProductionDataSystem.Web.Infrastructure.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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
    }
}