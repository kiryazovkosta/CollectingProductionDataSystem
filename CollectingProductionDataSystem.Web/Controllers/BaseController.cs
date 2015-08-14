namespace CollectingProductionDataSystem.Web.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Models;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Web.AppStart;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;

    public abstract class BaseController : Controller
    {
        protected readonly IProductionData data;

        protected UserProfile UserProfile { get; private set; }

        public BaseController(IProductionData dataParam)
        {
            this.data = dataParam;
        }

        protected override IAsyncResult BeginExecute(System.Web.Routing.RequestContext requestContext, AsyncCallback callback, object state)
        {
            InitializeUserProfileAsync(requestContext);
            return base.BeginExecute(requestContext, callback, state);
        }

        private void InitializeUserProfileAsync(System.Web.Routing.RequestContext requestContext)
        {
            if (requestContext.HttpContext.User.Identity.IsAuthenticated)
            {

                var user = ((IdentityDbContext<ApplicationUser>)this.data.DbContext)
                    .Users.Include(x => x.UserRoles)
                    .FirstOrDefault(x => x.UserName == requestContext.HttpContext.User.Identity.Name);
                var roleIds = user.Roles.Select(x => x.RoleId).ToArray();
                var roleManager = requestContext.HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
                var roles = roleManager.Roles.Where(x => roleIds.Any(y => y == x.Id));
                this.UserProfile = new UserProfile()
                {
                    User = user,
                    Roles = roles
                };
            }
            else
            {
                this.UserProfile = null;
            }

        }

    }
}
