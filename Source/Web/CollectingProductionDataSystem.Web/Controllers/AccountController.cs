namespace CollectingProductionDataSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Application.IdentityInfrastructure;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
    using log4net.Core;
    using Resources = App_GlobalResources.Resources;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using CollectingProductionDataSystem.Enumerations;

    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController(IProductionData dataParam)
            : base(dataParam)
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IProductionData dataParam)
            : this(dataParam)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.Title = Resources.Layout.Login;
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true

            var userProfile = data.Users.All().FirstOrDefault(x => x.UserName == model.UserName);

            if (null == userProfile)
            {
                ModelState.AddModelError("", Resources.ErrorMessages.InvalidLogin);
                return View(model);
            }
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    if (userProfile.IsChangePasswordRequired)
                    {
                        return RedirectToAction("ChangePassword");
                    }
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", Resources.ErrorMessages.InvalidLogin);
                    return View(model);
            }
        }


        // GET: /Account/ChangePassword
        public ActionResult ChangePassword()
        {
            ViewBag.Title = Resources.Layout.ChangePassword;
            return View();
        }

        // POST: /Account/ChangePassword
        // TODO: See what's wrong with the password change
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = ClearIsChangePasswordRequired();

            if (result.Succeeded)
            {
                result = await UserManager.ChangePasswordAsync(int.Parse(User.Identity.GetUserId()), model.OldPassword, model.NewPassword);

            }

            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(int.Parse(User.Identity.GetUserId()));
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", "Home", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [ChildActionOnly]
        public ActionResult GetGreetingMessage()
        {
            var user = this.UserProfile;
            return PartialView((object)user.FullName);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        /// <summary>
        /// Clears the is change password required.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        private IdentityResult ClearIsChangePasswordRequired()
        {
            try
            {
                var dbUser = data.Users.GetById(User.Identity.GetUserId<int>());
                dbUser.IsChangePasswordRequired = false;
                data.Users.Update(dbUser);
                var result = data.SaveChanges(this.UserProfile.UserName);
                if (result.EfErrors.Count > 0)
                {
                    var errors = new List<string>();
                    foreach (var err in result.EfErrors)
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
            catch (Exception ex)
            {
                var logger = Resolver.GetService<ILogger>();
                logger.Log(typeof(IdentityResult), Level.Error, ex.Message, ex);
                return new IdentityResult(new string[] { Resources.ErrorMessages.InvalidChangePassword });
            }
        }
        #endregion
    }
}