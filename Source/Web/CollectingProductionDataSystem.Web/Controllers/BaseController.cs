namespace CollectingProductionDataSystem.Web.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core.Objects;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Principal;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;
    using AutoMapper;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Models.UtilityEntities;
    using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
    using CollectingProductionDataSystem.Web.ViewModels.Tank;
    using CollectingProductionDataSystem.Web.ViewModels.Units;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity.Owin;
    using System.Web.Caching;

    public abstract class BaseController : Controller
    {
        protected readonly IProductionData data;

        public UserProfile UserProfile { get; private set; }

        public BaseController(IProductionData dataParam)
        {
            this.data = dataParam;
            Mapper.CreateMap<ApplicationUser, ApplicationUser>();
        }

        protected override IAsyncResult BeginExecute(RequestContext requestContext, AsyncCallback callback, object state)
        {
            InitializeUserProfileAsync(requestContext);
            return base.BeginExecute(requestContext, callback, state);
        }

        private void InitializeUserProfileAsync(System.Web.Routing.RequestContext requestContext)
        {
            var userIdentity = requestContext.HttpContext.User.Identity;
            if (userIdentity.IsAuthenticated)
            {
                var cached = requestContext.HttpContext.Cache.Get(userIdentity.Name + "_profile");
                if (cached != null)
                {
                    this.UserProfile = (UserProfile)cached;
                }
                else
                {
                    RenewApplicationUser(userIdentity.Name, requestContext);
                }
                
            }
            else
            {
                this.UserProfile = null;
            }
        }
 
        /// <summary>
        /// Renews the application user.
        /// </summary>
        private void RenewApplicationUser(string userName , RequestContext context)
        {
            var rolsStore = data.Roles.All();
                var user = this.data.Users.All()
                    .Include(x => x.Roles)
                    .Include(x => x.ApplicationUserProcessUnits.Select(y=>y.ProcessUnit))
                    .Include(x => x.ApplicationUserParks.Select(y=>y.Park))
                    .FirstOrDefault(x => x.UserName == userName);
                var roles = user.Roles.Select(x=>x.RoleId).ToList();
                user.UserRoles = rolsStore.Where(rol => roles.Any(x => rol.Id == x)).ToList();
                this.UserProfile= new UserProfile();
                Mapper.Map(user, this.UserProfile);
                context.HttpContext.Cache.Add(userName + "_profile", this.UserProfile,null,DateTime.Now.AddMinutes(2), Cache.NoSlidingExpiration, CacheItemPriority.High, RemovedCallback);
        }
 
        /// <summary>
        /// Removeds the callback.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="reason">The reason.</param>
        private void RemovedCallback(string key, object value, CacheItemRemovedReason reason)
        {
            Debug.WriteLine(string.Format("-------------------------------------------- Cache expired {0} because {1} ------------------------------------------------------", key,reason));
        }

        protected override void Dispose(bool disposing)
        {
            data.Dispose();
            base.Dispose(disposing);
        }
    }
}
