/// <summary>
/// Summary description for UserServices
/// </summary>
namespace CollectingProductionDataSystem.Application.UserServices
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper;
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Application.IdentityInfrastructure;
    using CollectingProductionDataSystem.Application.UserServices;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Contracts;
    using CollectingProductionDataSystem.Data.Contracts;
    using CollectingProductionDataSystem.Infrastructure.Extentions;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Models.UtilityEntities;
    using Microsoft.AspNet.Identity;
    using Ninject;

    public class UserService : IUserService
    {
        private readonly IProductionData data;
        private readonly IKernel kernel;

        public UserService(IKernel kernelParam, IProductionData dataParam)
        {
            this.data = dataParam;
            this.kernel = kernelParam;
        }

        /// <summary>
        /// Gets all loged users.
        /// </summary>
        /// <returns></returns>
        public IQueryable<ApplicationUser> GetAllLogedUsers()
        {
            return this.data.Users.All().Where(x => x.IsUserLoggedIn > 0);
        }

        public async Task<IEfStatus> CreateUserAsync(ApplicationUser user, UserProfile creator, ApplicationUserManager manager)
        {
            user.PasswordHash = new PasswordHasher().HashPassword(CommonConstants.StandartPassword);
            user.IsChangePasswordRequired = true;
            user.SecurityStamp = Guid.NewGuid().ToString();

            var userValidationResult = await ValidateUserAsync(user, manager, Operation.Create);

            if (userValidationResult.Count() > 0)
            {
                var validationResult = this.kernel.Get<IEfStatus>();
                validationResult = validationResult.SetErrors(userValidationResult);
                return validationResult;
            }

            data.Users.Add(user);
            var result = data.SaveChanges(creator.UserName);
            return result;
        }

        public async Task<IEfStatus> UpdateUserAsync(ApplicationUser user, UserProfile creator, ApplicationUserManager manager)
        {
            var controlUser = data.Users.GetById(user.Id);
            if (controlUser == null)
            {
                var validationResult = this.kernel.Get<IEfStatus>();
                validationResult = validationResult.SetErrors(new List<ValidationResult>() { new ValidationResult(App_Resources.ErrorMessages.InvalidUser) });
                return validationResult;
            }

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                user.PasswordHash = controlUser.PasswordHash;
                user.SecurityStamp = controlUser.SecurityStamp;
                user.IsChangePasswordRequired = controlUser.IsChangePasswordRequired;
            }
            else
            {
                user.IsChangePasswordRequired = true;
                user.SecurityStamp = Guid.NewGuid().ToString();
            }

            var userValidationResult = await ValidateUserAsync(user, manager, Operation.Update, controlUser);

            if (userValidationResult.Count() > 0)
            {
                var validationResult = this.kernel.Get<IEfStatus>();
                validationResult = validationResult.SetErrors(userValidationResult);
                return validationResult;
            }
            this.CopyProperties(user, controlUser);
            data.Users.Update(controlUser);
            var result = data.SaveChanges(creator.UserName);
            return result;
        }

        /// <summary>
        /// Copies the properties.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="controlUser">The control user.</param>
        private void CopyProperties(ApplicationUser user, ApplicationUser dbUser)
        {
            dbUser.Email = user.Email;
            dbUser.FirstName = user.FirstName;
            dbUser.MiddleName = user.MiddleName;
            dbUser.LastName = user.LastName;
            dbUser.Occupation = user.Occupation;
            dbUser.PasswordHash = user.PasswordHash;
            dbUser.SecurityStamp = user.SecurityStamp;
            dbUser.IsChangePasswordRequired = user.IsChangePasswordRequired;
            dbUser.Roles.Clear();
            dbUser.Roles.AddRange(user.Roles);
            dbUser.ApplicationUserParks.Clear();
            dbUser.ApplicationUserParks.AddRange(user.ApplicationUserParks);
            dbUser.ApplicationUserProcessUnits.Clear();
            dbUser.ApplicationUserProcessUnits.AddRange(user.ApplicationUserProcessUnits);
        }

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        private async Task<IEnumerable<ValidationResult>> ValidateUserAsync(ApplicationUser user, ApplicationUserManager manager, Operation operation, ApplicationUser controlUser = null)
        {
            List<ValidationResult> result = new List<ValidationResult>();

            // If the Id of new user is different by 0 an operation is create the Id is invalid
            if (operation == Operation.Create)
            {

                if (user.Id != 0)
                {
                    result.Add(new ValidationResult(string.Format(App_Resources.ErrorMessages.InvalidUserId, user.Id)));
                }

                var validateUserName = data.Users.All().FirstOrDefault(x => x.UserName == user.UserName);

                if (validateUserName != null)
                {
                    result.Add(new ValidationResult(string.Format(App_Resources.ErrorMessages.UserAllreadyExists, user.UserName)));
                }
            }

            if (operation == Operation.Update)
            {
                if (controlUser.UserName != user.UserName)
                {
                    result.Add(new ValidationResult(App_Resources.ErrorMessages.InvalidUserNameModification));
                }
            }

            if (operation == Operation.Update || operation == Operation.Delete)
            {
                if (controlUser == null)
                {
                    result.Add(new ValidationResult(App_Resources.ErrorMessages.InvalidUser));
                }
                else
                {
                    if (user.Id == 0)
                    {
                        result.Add(new ValidationResult(App_Resources.ErrorMessages.InvalidUser));
                    }
                }
            }

            if (operation != Operation.Delete)
            {

                var validateEmail = await manager.ValidateEmailAsync(user.Email);

                if (!validateEmail.Succeeded)
                {
                    foreach (var error in validateEmail.Errors)
                    {
                        result.Add(new ValidationResult(error));
                    }
                }

                if (string.IsNullOrEmpty(user.PasswordHash) && operation == Operation.Update)
                {
                    user.PasswordHash = controlUser.PasswordHash;
                }

                if (!string.IsNullOrEmpty(user.PasswordHash))
                {

                    var passwordValidator = await manager.PasswordValidator.ValidateAsync(user.PasswordHash);

                    if (!passwordValidator.Succeeded)
                    {
                        foreach (var error in validateEmail.Errors)
                        {
                            result.Add(new ValidationResult(error));
                        }
                    }
                }
                else
                {
                    result.Add(new ValidationResult(App_Resources.ErrorMessages.InvalidPassword));
                }
            }

            return result;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public enum Operation
    {
        Create,
        Update,
        Delete,
    }
}
