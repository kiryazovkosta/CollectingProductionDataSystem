using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.Models.Identity;
using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
using CollectingProductionDataSystem.Web.Infrastructure.IdentityInfrastructure;
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
            try
            {
                var db = (IdentityDbContext<ApplicationUser, ApplicationRole, int, UserLoginIntPk, UserRoleIntPk, UserClaimIntPk>)data.DbContext;
                var timer = new Stopwatch();
                timer.Start();
                var users = await (from u in db.Users
                                 select new EditUserViewModel
                                 {
                                     Id = u.Id,
                                     UserName = u.UserName,
                                     Email = u.Email,
                                     FirstName = u.FirstName ?? string.Empty,
                                     MiddleName = u.MiddleName ?? string.Empty,
                                     LastName = u.LastName ?? string.Empty,
                                     Occupation = u.Occupation ?? string.Empty,
                                     UserRoles = db.Roles.Where(rol => rol.Users.Any(x => x.UserId == u.Id)).Select(x => new AsignRoleViewModel { Id = x.Id, Name = x.Name, Description = x.Description }),
                                     ProcessUnits = u.ProcessUnits.Select(x => new ProcessUnitViewModel { Id = x.Id, Name = x.ShortName, FullName = x.FullName }),
                                     Parks = u.Parks.Select(x => new ParkViewModel { Id = x.Id, Name = x.Name }),
                                 }).ToListAsync();
                timer.Stop();
                Debug.WriteLine("Estimated time {0}", timer.Elapsed);
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