namespace CollectingProductionDataSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Diagnostics;
    using System.Linq;
    using System.Transactions;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
    using CollectingProductionDataSystem.Web.Infrastructure.IdentityInfrastructure;
    using CollectingProductionDataSystem.Web.ViewModels.Identity;
    using Kendo.Mvc.UI;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using Kendo.Mvc.Extensions;
    using System.Threading.Tasks;
    using Resources = App_GlobalResources.Resources;
    using CollectingProductionDataSystem.Contracts.Extensions;

    public class UserController : AreaBaseController
    {
        public UserController(IProductionData dataParam)
            : base(dataParam)
        {
        }
        // GET: Administration/User
        public ActionResult Index()
        {
            //var users = Mapper.Map<List<EditUserViewModel>>(UserManager.Users);
            //return View(users);
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
            var model = new EditUserViewModel();//{ Roles = Mapper.Map<IEnumerable<AsignRoleViewModel>>(data.Roles.All().ToList()) };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EditUserViewModel model)
        {
            if (model != null && ModelState.IsValid)
            {
                using (var transaction = new TransactionScope())
                {
                    var user = Mapper.Map<ApplicationUser>(model);
                    var rolesToAdd = model.UserRoles.Select( x=> new UserRoleIntPk(){UserId = user.Id, RoleId = x.Id}).ToList();
                    user.Roles.AddRange(rolesToAdd);
                    user.IsChangePasswordRequired = true;
                    user.CreatedFrom = HttpContext.User.Identity.GetUserName();
                    try
                    {
                        IdentityResult result = UserManager.Create(user, model.NewPassword);
                    }
                    catch ( System.Data.Entity.Validation.DbEntityValidationException ex) 
                    {
                        Debug.WriteLine(ex.Message);
                    }
                    //if (result.Succeeded)
                    //{
                    //   // SaveCustomDataToUser(inUserRoles, inParks, inProcessUnits, ref user, ref result);
                    //}

                    //if (result.Succeeded)
                    //{
                    //    this.TempData["success"] = string.Format(Resources.Layout.CreateUserSuccess, model.UserName);
                    //    transaction.Complete();
                    //    return RedirectToAction("Index", "User", new { aria = "Administration" });
                    //}
                    //else
                    //{
                    //    AddErrorsFromResult(result);
                    //    transaction.Dispose();
                    //}
                }
            }
            return View(model);
        }

        private void SaveCustomDataToUser(IEnumerable<int> inUserRoles, IEnumerable<int> inParks, IEnumerable<int> inProcessUnits, ref ApplicationUser user, ref IdentityResult result)
        {
            if (data.DbContext.Entry<ApplicationUser>(user).State == EntityState.Detached)
            {
                user = data.Users.GetById(user.Id);
            }
            AddAditionalDataToUser(inUserRoles, inParks, inProcessUnits, user);
            data.Users.Update(user);
            var status = data.SaveChanges(HttpContext.User.Identity.GetUserName());
            result = this.GetIdentityResult(status);
        }

        private void AddAditionalDataToUser(IEnumerable<int> inUserRoles, IEnumerable<int> inParks, IEnumerable<int> inProcessUnits, ApplicationUser user)
        {
            if (inUserRoles != null)
            {
                var rolesToAdd = inUserRoles.AsQueryable().Select(x => new UserRoleIntPk() { UserId = user.Id, RoleId = x }).ToList();
                user.Roles.AddRange(rolesToAdd);
                var rolesToRemove = data.Roles.All().Where(x => !inUserRoles.Any(y => y == x.Id)).ToList().Select(role => new UserRoleIntPk() { UserId = user.Id, RoleId = role.Id });
                user.Roles.RemoveRange(rolesToRemove);
            }
            if (inParks != null)
            {
                var parksToAdd = data.Parks.All().Where(x => inParks.Any(p => x.Id == p)).ToList();
                user.Parks.AddRange(parksToAdd);
                var parksToRemove = data.Parks.All().Where(x => !inParks.Any(p => x.Id == p)).ToList();
                user.Parks.RemoveRange(parksToRemove);
            }
            if (inProcessUnits != null)
            {
                var processUnitsToAdd = data.ProcessUnits.All().Where(x => inProcessUnits.Any(p => x.Id == p)).ToList();
                user.ProcessUnits.AddRange(processUnitsToAdd);
                var processUnitsToRenove = data.ProcessUnits.All().Where(x => !inProcessUnits.Any(p => x.Id == p)).ToList();
                user.ProcessUnits.RemoveRange(processUnitsToRenove);
            }
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
            try
            {
                data.Users.Delete(id);
                data.SaveChanges(this.UserProfile.User.UserName);
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