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
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
    using CollectingProductionDataSystem.Web.Infrastructure.IdentityInfrastructure;
    using Kendo.Mvc.Extensions;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Resources = App_GlobalResources.Resources;

    public class UserController : AreaBaseController
    {
        public UserController(IProductionData dataParam)
            : base(dataParam)
        {
        }
        // GET: Administration/User
        public ActionResult Index()
        {
            return View();
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
            var model = new EditUserViewModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditUserViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                model.NewPassword = string.IsNullOrEmpty(model.NewPassword) ? CommonConstants.StandartPassword : model.NewPassword;
                
                    var user = Mapper.Map<ApplicationUser>(model);
                    user.IsChangePasswordRequired = true;
                    user.CreatedFrom = HttpContext.User.Identity.GetUserName();

                    IdentityResult result = UserManager.Create(user, model.NewPassword);

                    if (result.Succeeded)
                    {
                        result = SaveCustomDataToUser(user, model);
                    }

                    if (result.Succeeded)
                    {
                        this.TempData["success"] = string.Format(Resources.Layout.CreateUserSuccess, model.UserName);
                        return RedirectToAction("Index", "User", new { aria = "Administration" });
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                        var duser = UserManager.FindById(user.Id);
                        UserManager.Delete(duser);
                    }
            }
            return View(model);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel user)
        {
            if (user != null && ModelState.IsValid)
            {
                ApplicationUser controlUser = data.Users.GetById(user.Id);
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
                    Mapper.Map(user,controlUser);
                    controlUser.PasswordHash =
                        (!string.IsNullOrEmpty(user.NewPassword) &&
                        (!string.IsNullOrWhiteSpace(user.NewPassword)))
                        ? UserManager.PasswordHasher.HashPassword(user.NewPassword)
                        : controlUser.PasswordHash;
                    IdentityResult result = SaveCustomDataToUser(controlUser, user);
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
            ViewBag.Mode = "Edit";
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

        private IdentityResult SaveCustomDataToUser(ApplicationUser user, EditUserViewModel model)
        {
            if (data.DbContext.Entry<ApplicationUser>(user).State == EntityState.Detached)
            {
                user = data.Users.GetById(user.Id);
            }
            AddAditionalDataToUser(user, model);
            data.Users.Update(user);
            var status = data.SaveChanges(HttpContext.User.Identity.GetUserName());
            return this.GetIdentityResult(status);
        }

        private void AddAditionalDataToUser(ApplicationUser user, EditUserViewModel model)
        {
            if (model.UserRoles != null)
            {
                var rolesToAdd = model.UserRoles.AsQueryable().Select(x => new UserRoleIntPk() { UserId = user.Id, RoleId = x.Id }).ToList();
                user.Roles.Clear();
                user.Roles.AddRange(rolesToAdd);
            }
            if (model.Parks != null)
            {
                List<int> parks = model.Parks.Select(x => x.Id).ToList();
                var parksToAdd = data.Parks.All().Where(x => parks.Any(p => x.Id == p)).ToList();
                user.Parks.Clear();
                user.Parks.AddRange(parksToAdd);
            }
            if (model.ProcessUnits != null)
            {
                List<int> processUnits = model.ProcessUnits.Select(x => x.Id).ToList();
                var processUnitsToAdd = data.ProcessUnits.All().Where(x => processUnits.Any(p => x.Id == p)).ToList();
                user.ProcessUnits.Clear();
                user.ProcessUnits.AddRange(processUnitsToAdd);
            }
        }

        private IdentityResult GetIdentityResult(IEfStatus status)
        {
            if (status.EfErrors.Count > 0)
            {
                var errors = new List<string>();
                foreach (var err in status.EfErrors)
                {
                    errors.Add(err.ErrorMessage);
                }
                return new IdentityResult(errors);
            }
            else
            {
                return IdentityResult.Success;
            }
        }


    }
}