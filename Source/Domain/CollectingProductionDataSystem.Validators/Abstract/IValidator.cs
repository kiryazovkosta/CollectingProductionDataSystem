/// <summary>
/// Summary description for IValidator
/// </summary>
namespace CollectingProductionDataSystem.Validators.Abstract
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IValidator<T> where T: class
    {
        IEnumerable<ValidationResult> Validate(ValidationContext validationContext, T model);
    }
}
