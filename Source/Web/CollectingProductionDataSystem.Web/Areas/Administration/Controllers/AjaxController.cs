using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Application.IdentityInfrastructure;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Identity;
using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
using CollectingProductionDataSystem.Web.ViewModels.Identity;
using CollectingProductionDataSystem.Web.ViewModels.Tank;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using CollectingProductionDataSystem.Web.ViewModels.Units;

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
        public async Task<JsonResult> GetAllUsers([DataSourceRequest]DataSourceRequest request)
        {
            var rolsStore = data.Roles.All();
            var users = await (data.Users.All().Select(u => new EditUserViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                FirstName = u.FirstName,
                MiddleName = u.MiddleName,
                LastName = u.LastName,
                Occupation = u.Occupation,
                UserRoles = rolsStore.Where(rol => rol.Users.Any(x => x.UserId == u.Id)).Select(x => new RoleViewModel { Id = x.Id, Name = x.Name, Description = x.Description }),
                ProcessUnits = u.ProcessUnits.Select(x => new ProcessUnitViewModel { Id = x.Id, Name = x.ShortName, FullName = x.FullName }),
                Parks = u.Parks.Select(x => new ParkViewModel { Id = x.Id, Name = x.Name }),
            })).ToListAsync();
            return Json(users.ToDataSourceResult(request, ModelState));

        }


        public async Task<JsonResult> GetAllProcessUnits()
        {
            var prosessUnits = Mapper.Map<IEnumerable<ProcessUnitViewModel>>(await data.ProcessUnits.All().ToListAsync());
            return Json(prosessUnits, JsonRequestBehavior.AllowGet);
        }

        public async Task<JsonResult> GetAllParks()
        {
            var parks = Mapper.Map<IEnumerable<ParkViewModel>>(await data.Parks.All().ToListAsync());
            return Json(parks, JsonRequestBehavior.AllowGet);
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