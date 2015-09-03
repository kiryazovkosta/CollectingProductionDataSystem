using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Identity;
using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
using CollectingProductionDataSystem.Web.Infrastructure.IdentityInfrastructure;
using CollectingProductionDataSystem.Web.ViewModels.Identity;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace CollectingProductionDataSystem.Web.Areas.Administration.Controllers
{
    public class AjaxController : AreaBaseController
    {
        public AjaxController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        public JsonResult GetAllRoles()
        {
            var roles = data.Roles.All();
            var rolesViewModel = Mapper.Map<IEnumerable<AsignRoleViewModel>>(roles.ToList());
            return Json(rolesViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetAllUsers([DataSourceRequest]DataSourceRequest request)
        {
            try
            {
                var users = Mapper.Map<IEnumerable<EditUserViewModel>>(data.Users.All().ToList());
                foreach (var user in users)
                {
                    user.UserRoles = Mapper.Map<IEnumerable<AsignRoleViewModel>>(data.Roles.All().Where(x=>x.Users.Any(y=>y.UserId == user.Id)));
                }
                return Json(users.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Json(null);

        }

        //public JsonResult GetRolesByUser(int userId)
        //{
        //}

        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }
    }
}