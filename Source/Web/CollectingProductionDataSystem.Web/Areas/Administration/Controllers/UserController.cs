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
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
    using CollectingProductionDataSystem.Infrastructure.Extentions;
    using CollectingProductionDataSystem.Web.Infrastructure.Extentions;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Resources = App_GlobalResources.Resources;

    public class UserController : AreaBaseController
    {
        private readonly IUserService userService;
        public UserController(IProductionData dataParam, IUserService userServiceParam)
            : base(dataParam)
        {
            this.userService = userServiceParam;
        }
        // GET: Administration/User
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            var model = new EditUserViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(EditUserViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var user = Mapper.Map<ApplicationUser>(model);
                user = await AddAditionalDataToUserAsync(user, model);

                var result = await this.userService.CreateUserAsync(user, this.UserProfile, this.UserManager);

                if (result.IsValid)
                {
                    this.TempData["success"] = string.Format(Resources.Layout.CreateUserSuccess, model.UserName);
                    return RedirectToAction("Index", "User", new { aria = "Administration" });
                }
                else
                {
                    result.ToModelStateErrors(ModelState);
                }
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel model, [DataSourceRequest] DataSourceRequest dataSourceRequest)
        {
            try
            {
                if (model != null && ModelState.IsValid)
                {
                    var user = await this.ConstructUserForUpdateAsync(model);

                    var result = await this.userService.UpdateUserAsync(user, this.UserProfile, this.UserManager);

                    if (result.IsValid)
                    {
                        this.TempData["success"] = string.Format(Resources.Layout.CreateUserSuccess, model.UserName);
                        return RedirectToAction("Index", "User", new { aria = "Administration" });
                    }
                    else
                    {
                        result.ToModelStateErrors(ModelState);
                    }

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            var resultData = new[] { model };
            return Json(resultData.AsQueryable().ToDataSourceResult(dataSourceRequest, ModelState));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                data.Users.Delete(id);
                data.SaveChanges(this.UserProfile.UserName);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Error", ex.Message);
            }
        }

        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        /// <summary>
        /// Adds the errors from result.
        /// </summary>
        /// <param name="result">The result.</param>
        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                this.ModelState.AddModelError("", error);
            }
        }

        /// <summary>
        /// Constructs the user for update.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        private async Task<ApplicationUser> ConstructUserForUpdateAsync(EditUserViewModel model)
        {
            var user = Mapper.Map<ApplicationUser>(model);
            user = await AddAditionalDataToUserAsync(user, model);
            return user;
        }

        private async Task<ApplicationUser> AddAditionalDataToUserAsync(ApplicationUser user, EditUserViewModel model)
        {
            if (model.Parks != null)
            {
                List<int> parks = model.Parks.Select(x => x.Id).ToList();
                ICollection<Park> parksToAdd = await data.Parks.All().Where(x => parks.Any(p => x.Id == p)).ToListAsync();
                user.Parks.Clear();
                user.Parks.AddRange(parksToAdd);
            }
            if (model.ProcessUnits != null)
            {
                List<int> processUnits = model.ProcessUnits.Select(x => x.Id).ToList();
                var processUnitsToAdd = await data.ProcessUnits.All().Where(x => processUnits.Any(p => x.Id == p)).ToListAsync();
                user.ProcessUnits.Clear();
                user.ProcessUnits.AddRange(processUnitsToAdd);
            }
            return user;
        }
    }
}