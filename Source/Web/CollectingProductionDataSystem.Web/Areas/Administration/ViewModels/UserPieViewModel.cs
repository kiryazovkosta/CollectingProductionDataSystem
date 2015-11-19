namespace CollectingProductionDataSystem.Web.Areas.Administration.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;
    using Resources = App_GlobalResources.Resources;
    public class UserPieViewModel
    {
        public UserPieViewModel(int logedInUsers, int allUsersCount) 
        {
            this.LogedInUsers = logedInUsers;
            this.AllUsersCount = allUsersCount;
        }

        [Display(Name = "LogedInUsers", ResourceType = typeof(Resources.Layout))]
        public int LogedInUsers { get; set; }

        [Display(Name = "NotLoggedInUsers", ResourceType = typeof(Resources.Layout))]
        public int NotLoggedInUsers 
        { 
            get { return this.AllUsersCount - this.LogedInUsers; } 
        }

        [Display(Name = "AllUsersCount", ResourceType = typeof(Resources.Layout))]
        public int AllUsersCount { get; set; }
    }
}