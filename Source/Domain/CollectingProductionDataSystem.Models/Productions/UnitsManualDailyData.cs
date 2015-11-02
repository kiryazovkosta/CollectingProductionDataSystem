namespace CollectingProductionDataSystem.Models.Productions
{
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public partial class UnitsManualDailyData : DeletableEntity, IEntity, IValidatableObject
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public virtual UnitsDailyData UnitsDailyData { get; set; }
        public int EditReasonId { get; set; }
        public virtual EditReason EditReason { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Id == 0)
            {
                yield return new ValidationResult(Resources.ModelErrors.UnitsManualData_Id, new string[] { "Id" });
            }

            if (this.Value < 0.0M || this.Value > decimal.MaxValue) 
            {
                yield return new ValidationResult(string.Format(Resources.ModelErrors.UnitsManualData_Value_Range, 0.01M, decimal.MaxValue), new string[] { "ManualValue" });
            }

            if (this.EditReasonId < 1)
            {
                yield return new ValidationResult(Resources.ModelErrors.UnitsManualData_InvalidEditReason, new string[] { "EditReason" });
            }
        }
    }
}
