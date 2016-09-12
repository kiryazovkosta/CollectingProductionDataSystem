using CollectingProductionDataSystem.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollectingProductionDataSystem.Web.AppStart
{
    public class StartUpUsersConfiguration
    {
        internal static void RegisterUsersStateChange(IProductionData productionData)
        {
            var users = productionData.Users.All();
            foreach (var user in users)
            {
                user.IsUserLoggedIn = 0;
            }

            productionData.SaveChanges("Reset Sessions from system restart");
        }
    }
}