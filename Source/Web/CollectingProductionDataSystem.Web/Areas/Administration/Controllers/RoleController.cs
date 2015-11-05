namespace CollectingProductionDataSystem.Web.Areas.Administration.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.IdentityInfrastructure;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Web.ViewModels.Identity;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;

    public class RoleController : AreaBaseController
    {
        public RoleController(IProductionData dataParam) 
        :base(dataParam)
        {
        }
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = Mapper.Map<ApplicationRole>(model);
                IdentityResult result = await RoleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await RoleManager.DeleteAsync(role);
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
                return View("Error", new string[] { "Ролята не е открита!" });
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



        public async Task<ActionResult> Edit(int? id)
        {
            id = id ?? 0;
            ApplicationRole role = await RoleManager.FindByIdAsync((int)id);
            int[] memberIds = role.Users.Select(x => x.UserId).ToArray();
            var auMembers = data.Users.All().ToList().Where(x => memberIds.Any(y => y == x.Id)).ToList();
            IEnumerable<ApplicationUserModel> members = Mapper.Map<IEnumerable<ApplicationUserModel>>(auMembers);
            IEnumerable<ApplicationUserModel> nonMembes =  Mapper.Map<IEnumerable<ApplicationUserModel>>(data.Users.All().ToList().Except(auMembers));
            return View(new RoleEditViewModel
            {
                Role = role,
                Name = role.Name,
                Description = role.Description,
                Members = members,
                NonMembers = nonMembes
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleModificationViewModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                var role = await RoleManager.FindByNameAsync(model.RoleName);
                if (role != null)
                {
                    role.Name = model.Name;
                    role.Description = model.Description;
                    result = await RoleManager.UpdateAsync(role);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                else 
                {
                    return View("Error", new string[] { "Ролята не е намерена!" });
                }

                foreach (int userId in model.IdsToAdd ?? new int[] { })
                {
                    result = await UserManager.AddToRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }

                foreach (int userId in model.IdsToDelete ?? new int[] { })
                {
                    result = await UserManager.RemoveFromRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                return RedirectToAction("Index");
            }
            return View("Error", new string[] { "Ролята не е намерена!" });
        }

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