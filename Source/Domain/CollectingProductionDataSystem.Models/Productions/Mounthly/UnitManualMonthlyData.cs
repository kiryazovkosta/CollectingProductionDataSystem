namespace CollectingProductionDataSystem.Models.Productions.Mounthly
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions.Mounthly;

    public class UnitManualMonthlyData : DeletableEntity, IEntity, IValidatableObject
    {
        public int Id { get; set; }

        public decimal Value { get; set; }

        public virtual UnitMonthlyData UnitMonthlyData { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Id == 0)
            {
                yield return new ValidationResult(Resources.ModelErrors.UnitsManualData_Id, new string[] { "Id" });
            }

            if (/*this.Value < 0.0M || */this.Value > decimal.MaxValue) 
            {
                yield return new ValidationResult(string.Format(Resources.ModelErrors.UnitsManualData_Value_Range, 0.01M, decimal.MaxValue), new string[] { "ManualValue" });
            }
        }
    }
}