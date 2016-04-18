namespace CollectingProductionDataSystem.Models.Productions.Mounthly
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public class UnitRecalculatedMonthlyData : DeletableEntity, IEntity, IValidatableObject
    {
        public int Id { get; set; }

        public decimal EnteredValue { get; set; }

        public decimal CorrectionValue { get; set; }

        public decimal TotalValue { get; set; }

        public virtual UnitMonthlyData UnitMonthlyData { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Id == 0)
            {
                yield return new ValidationResult(Resources.ModelErrors.UnitsManualData_Id, new string[] { "Id" });
            }
        }
    }
}