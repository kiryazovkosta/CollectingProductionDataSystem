namespace CollectingProductionDataSystem.Models.Transactions.HighwayPipelines
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Transactions.HighwayPipelines;

    public partial class HighwayPipelineManualData : DeletableEntity, IEntity, IValidatableObject
    {
        public int Id { get; set; }
        public decimal Volume { get; set; }
        public decimal Mass { get; set; }
        public virtual HighwayPipelineData HighwayPipelineData { get; set; } 

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Id == 0)
            {
                yield return new ValidationResult(Resources.ModelErrors.UnitsManualData_Id, new string[] { "Id" });
            }

            if (this.Volume < 0.0M || this.Volume > decimal.MaxValue) 
            {
                yield return new ValidationResult(string.Format(Resources.ModelErrors.UnitsManualData_Value_Range, 0.0M, decimal.MaxValue), new string[] { "Volume" });
            }

            if (this.Mass < 0.0M || this.Mass > decimal.MaxValue) 
            {
                yield return new ValidationResult(string.Format(Resources.ModelErrors.UnitsManualData_Value_Range, 0.0M, decimal.MaxValue), new string[] { "Mass" });
            }
        }
    }
}
