/// <summary>
/// Summary description for UserServices
/// </summary>
namespace CollectingProductionDataSystem.Application.UserServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Models.UtilityEntities;

    public class UserServices
    {
        private readonly IProductionData data;
        public UserServices(IProductionData dataParam) 
        {
            this.data = dataParam;
        }
        //public IEfStatus CreateUser(ApplicationUser user, UserProfile creator, ApplicationUserManager manager) 
        //{
        //    data.Users.Add(user);
        //    var result = data.SaveChanges(creator.UserName);
        //    return result;
        //}
    }
}
