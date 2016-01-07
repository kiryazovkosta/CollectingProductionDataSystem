namespace CollectingProductionDataSystem.Models.Productions.HighwayPipelines
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public partial class HighwayPipelineManualData : DeletableEntity, IEntity, IValidatableObject
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public virtual HighwayPipelineData HighwayPipelineData { get; set; } 

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Id == 0)
            {
                yield return new ValidationResult(Resources.ModelErrors.UnitsManualData_Id, new string[] { "Id" });
            }

            if (this.Value < 0.0M || this.Value > decimal.MaxValue) 
            {
                yield return new ValidationResult(string.Format(Resources.ModelErrors.UnitsManualData_Value_Range, 0.0M, decimal.MaxValue), new string[] { "ManualValue" });
            }
        }
    }
}
