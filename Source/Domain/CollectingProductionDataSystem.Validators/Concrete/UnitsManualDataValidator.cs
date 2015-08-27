/// <summary>
/// Summary description for UnitsManualDataValidator
/// </summary>
namespace CollectingProductionDataSystem.Validators.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Validators.Abstract;
    using Resources = CollectingProductionDataSystem.Models.Resources;

    class UnitsManualDataValidator : IValidator<UnitsManualData>
    {
        /// <summary>
        /// Validates the specified validation context.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext, UnitsManualData model)
        {
            if (model.Id == 0)
            {
                yield return new ValidationResult(Resources.ModelErrors.UnitsManualData_Id, new string[] { "Id" });
            }

            if (model.Value <= 0 && model.Value > decimal.MaxValue)
            {
                yield return new ValidationResult(string.Format(Resources.ModelErrors.UnitsManualData_Value_Range, 0, decimal.MaxValue), new string[] { "Value" });
            }

            if (model.EditReasonId < 1)
            {
                yield return new ValidationResult(Resources.ModelErrors.InvalidEditReason, new string[] { "Value" });
            }
        }
    }
}
