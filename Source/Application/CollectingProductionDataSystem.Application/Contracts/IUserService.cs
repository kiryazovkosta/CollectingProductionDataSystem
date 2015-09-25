namespace CollectingProductionDataSystem.Application.Contracts
{
    using System;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Application.IdentityInfrastructure;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Models.UtilityEntities;

    public interface IUserService
    {
        Task<IEfStatus> CreateUserAsync( ApplicationUser user, UserProfile creator, ApplicationUserManager manager);
        Task<IEfStatus> UpdateUserAsync( ApplicationUser user, UserProfile creator, ApplicationUserManager manager);
    }
}
