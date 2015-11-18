namespace CollectingProductionDataSystem.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using System.Transactions;
    using System.Web;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Application.IdentityInfrastructure;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Contracts;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
    using Resources = App_GlobalResources.Resources;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin.Security;
    using CollectingProductionDataSystem.Enumerations;

    [Authorize]
    public class AccountController : BaseController
    {
        private ApplicationSignInManager signInManager;
        private ApplicationUserManager userManager;
        private ILogger logger;

        public AccountController(IProductionData dataParam,ILogger loggerParam)
            : base(dataParam)
        {
            this.logger = loggerParam;
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IProductionData dataParam, ILogger loggerParam)
            : this(dataParam,loggerParam)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
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
            if (result != SignInStatus.Success)
            {
                logger.AuthenticationError("Invalid Attempt to Login", this, model.UserName);
            }
            
            switch (result)
            {
                case SignInStatus.Success:
                    DocumentUserLogIn(model.UserName, true);
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
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            DocumentUserLogIn(this.UserProfile.UserName, false);
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
                if (userManager != null)
                {
                    userManager.Dispose();
                    userManager = null;
                }

                if (signInManager != null)
                {
                    signInManager.Dispose();
                    signInManager = null;
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

        /// <summary>
        /// Documents the user log in.
        /// </summary>
        /// <param name="model">The model.</param>
        private void DocumentUserLogIn(string userName, bool isOperationLogIn)
        {
            var user = this.data.Users.All().FirstOrDefault(x => x.UserName == userName);
            if (user != null)
            {
                user.IsUserLoggedIn = isOperationLogIn;
                
                if (isOperationLogIn)
                {
                    user.LastLogedIn = DateTime.Now;
                }

                this.data.Users.Update(user);
                var result = this.data.SaveChanges(userName);
                if (!result.IsValid)
                {
                    logger.Error("Cannot persist user LogIn", this, new AccessViolationException(), result.EfErrors.Select(x => x.ErrorMessage));
                }

                using (var transaction = new TransactionScope(TransactionScopeOption.Required))
                {
                    try
                    {
                        var numberLogedInUsers = this.data.Users.All().Count(x => x.IsUserLoggedIn);
                        var logedInUsers = new LogedInUser() { LogedUsersCount = numberLogedInUsers };
                        this.data.LogedInUsers.Add(logedInUsers);
                        result = this.data.SaveChanges("System");

                        if (!result.IsValid)
                        {
                            logger.Error("Cannot persist user LogIn", this, new AccessViolationException(), result.EfErrors.Select(x => x.ErrorMessage));
                        }

                        transaction.Complete();
                    }
                    catch (Exception ex) {
                        transaction.Dispose();
                        logger.Error(ex.Message, this, ex);
                    }
                }
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
                //TODO: Log this error !!!
                //var logger = Resolver.GetService<ILogger>();
                //logger.Log(typeof(IdentityResult), Level.Error, ex.Message, ex);
                return new IdentityResult(new string[] { Resources.ErrorMessages.InvalidChangePassword });
            }
        }
        #endregion
    }
}