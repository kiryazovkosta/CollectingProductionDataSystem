namespace CollectingProductionDataSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Web.Infrastructure.IdentityInfrastructure;
    using CollectingProductionDataSystem.Web.ViewModels.Identity;
    using Kendo.Mvc.UI;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Kendo.Mvc.Extensions;
    using System.Threading.Tasks;

    public class UserController : BaseController
    {
        private readonly IProductionData data;

        public UserController(IProductionData dataParam)
        {
            this.data = dataParam;
        }
        // GET: Administration/User
        public ActionResult Index()
        {
            //var users = Mapper.Map<List<EditUserViewModel>>(UserManager.Users);
            //return View(users);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Read([DataSourceRequest]DataSourceRequest request)
        {
            var users = Mapper.Map<List<EditUserViewModel>>(UserManager.Users);
            return Json(users.ToDataSourceResult(request, ModelState));
        }

        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public ActionResult Create()
        {
            var model = new CreateUserViewModel() { Roles = Mapper.Map<IEnumerable<AsignRoleViewModel>>(data.Roles.All().ToList()) };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateUserViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                var user = Mapper.Map<ApplicationUser>(model);
                IdentityResult result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    result = await AddUserInRolesAsync(user, model.Roles);                    
                }

                if (result.Succeeded)
                {
                    this.TempData["success"] = string.Format("Потребителя {0} беше създаден успешно.", model.UserName);
                    return RedirectToAction("Index", "User", new { aria = "Administration" });
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(model);
        }

        /// <summary>
        /// Adds the user in roles.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="roles">The roles.</param>
        private async Task<IdentityResult> AddUserInRolesAsync(ApplicationUser user, IEnumerable<AsignRoleViewModel> roles)
        {
            foreach (var role in roles.Where(role => role.IsUserInRole))
            {
                IdentityResult result = await UserManager.AddToRoleAsync(user.Id, role.Name);
                if (!result.Succeeded)
                {
                    AddErrorsFromResult(result);
                    return result;
                }
            }

            foreach (var role in roles.Where(role => !role.IsUserInRole))
            {
                if (await UserManager.IsInRoleAsync(user.Id, role.Name))
                {
                    IdentityResult result = await UserManager.RemoveFromRoleAsync(user.Id, role.Name);
                    if (!result.Succeeded)
                    {
                        AddErrorsFromResult(result);
                        return result;
                    }
                }
            }

            return IdentityResult.Success;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new string[] { "Потребителя не е намерен" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel user)
        {
            if (user != null && ModelState.IsValid)
            {
                ApplicationUser controlUser = await UserManager.FindByIdAsync(user.Id);
                IdentityResult validUserName = IdentityResult.Success;
                IdentityResult validNewPassword = IdentityResult.Success;
                IdentityResult validEmail = IdentityResult.Success;

                if (controlUser.UserName != user.UserName)
                {
                    validUserName = IdentityResult.Failed(new string[] { string.Format("Не е разрешено модифициране на потребителското име!!!") });
                    AddErrorsFromResult(validUserName);
                }
                else
                {
                    if (!string.IsNullOrEmpty(user.NewPassword))
                    {
                        validNewPassword = await UserManager.PasswordValidator.ValidateAsync(user.NewPassword);
                        if (!validNewPassword.Succeeded)
                        {
                            AddErrorsFromResult(validNewPassword);
                        }
                    }

                    validEmail = await UserManager.ValidateEmailAsync(user.Email);
                    if (!validEmail.Succeeded)
                    {
                        AddErrorsFromResult(validEmail);
                    }
                }
                if (validUserName.Succeeded && validEmail.Succeeded && validNewPassword.Succeeded)
                {
                    controlUser.Email = user.Email;
                    controlUser.PasswordHash =
                        (!string.IsNullOrEmpty(user.NewPassword) &&
                        (!string.IsNullOrWhiteSpace(user.NewPassword)))
                        ? UserManager.PasswordHasher.HashPassword(user.NewPassword)
                        : controlUser.PasswordHash;
                    IdentityResult result = await UserManager.UpdateAsync(controlUser);
                    if (result.Succeeded)
                    {
                        this.TempData["success"] = string.Format("Потребителя {0} беше променен успешно.", controlUser.UserName);
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
            }
            else
            {
                if (user == null)
                {
                    ModelState.AddModelError("", "Потребителя не е намерен");
                }
            }
            return View(user);
        }

        ///// <summary>
        ///// Validates the curent password async.
        ///// </summary>
        ///// <param name="user">The user.</param>
        ///// <param name="controlUser">The control user.</param>
        ///// <returns></returns>
        //private Task<IdentityResult> ValidateCurentPasswordAsync(EditUserViewModel user, AppUser controlUser)
        //{
        //    // TODO: Implement this method
        //    throw new NotImplementedException();
        //}

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
    }
}