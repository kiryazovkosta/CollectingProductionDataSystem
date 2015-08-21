/// <summary>
/// Summary description for UserNameValidationService
/// </summary>
namespace CollectingProductionDataSystem.Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class ValidationService
    {
        public static ValidationResult ValidateUserName(string userName, ValidationContext context) 
        {
            if (userName != "nikolay")
            {
                return new ValidationResult("Some Error");
            }
            else 
            {
                return ValidationResult.Success;
            }
        }
    }
}
