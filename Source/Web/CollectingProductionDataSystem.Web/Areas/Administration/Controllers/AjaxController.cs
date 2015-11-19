namespace CollectingProductionDataSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Application.IdentityInfrastructure;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
    using CollectingProductionDataSystem.Web.ViewModels.Identity;
    using CollectingProductionDataSystem.Web.ViewModels.Tank;
    using CollectingProductionDataSystem.Web.ViewModels.Units;
    using CollectingProductionDataSystem.Web.ViewModels.Utility;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Microsoft.AspNet.Identity.Owin;
    using Resources = App_GlobalResources.Resources;

    public class AjaxController : AreaBaseController
    {
        private readonly IUserService userService;
        public AjaxController(IProductionData dataParam, IUserService userServiceParam)
            : base(dataParam)
        {
            this.userService = userServiceParam;
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
            var parksStore = data.Parks.All();
            var processUnitsStore = data.ProcessUnits.All();
            var users = await (data.Users.All().Select(u => new EditUserViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                FirstName = u.FirstName,
                MiddleName = u.MiddleName,
                LastName = u.LastName,
                Occupation = u.Occupation,
                UserChangedPassword = !u.IsChangePasswordRequired,
                UserRoles = rolsStore.Where(rol => rol.Users.Any(x => x.UserId == u.Id)).Select(x => new RoleViewModel { Id = x.Id, Name = x.Name, Description = x.Description }),
                ProcessUnits = processUnitsStore.Where(proc => proc.ApplicationUserProcessUnits.Any(x => x.ApplicationUserId == u.Id)).Select(x => new ProcessUnitViewModel { Id = x.Id, Name = x.FullName }),
                Parks = parksStore.Where(park => park.ApplicationUserParks.Any(x => x.ApplicationUserId == u.Id)).Select(x => new ParkViewModel { Id = x.Id, Name = x.Name }),

            })).ToListAsync();
            return Json(users.ToDataSourceResult(request, ModelState));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetAllLogedUsers([DataSourceRequest]DataSourceRequest request)
        {
            var rolsStore = data.Roles.All();
            var parksStore = data.Parks.All();
            var processUnitsStore = data.ProcessUnits.All();
            var result = userService.GetAllLogedUsers().ToList();
            return Json(result.ToDataSourceResult(request, ModelState, Mapper.Map<LoggedUserViewModel>));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReadPieLogedInUsers([DataSourceRequest]DataSourceRequest request)
        {
            var usersIn = userService.GetAllLogedUsers().Count();
            var usersCount = data.Users.All().Count();
            var result = new List<PieViewModel> 
            {
                new PieViewModel(){ Category = Resources.Layout.LogedInUsers, Value = usersIn, Color="green", Explode= true},
                new PieViewModel(){ Category = Resources.Layout.NotLoggedInUsers, Value = usersCount - usersIn, Color="red"},
            };

            return Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ReadLogedStatistics(DateTime? beginTime = null, DateTime? endTime = null)
        {
            beginTime = beginTime ?? DateTime.Now.AddDays(-1);
            endTime = endTime ?? DateTime.Now;
            var result = data.LogedInUsers.All().Where(x => beginTime.Value <= x.TimeStamp && x.TimeStamp <= endTime.Value).ToList();
            return Json(result);
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