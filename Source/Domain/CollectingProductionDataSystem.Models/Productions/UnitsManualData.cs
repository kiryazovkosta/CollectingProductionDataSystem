namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public partial class UnitsManualData : DeletableEntity, IEntity, IValidatableObject
    {
        public int Id { get; set; }
        public decimal Value { get; set; }
        public virtual UnitsData UnitsData { get; set; }
        public int EditReasonId { get; set; }
        public virtual EditReason EditReason { get; set; }

        /// <summary>
        /// Determines whether the specified object is valid.
        /// </summary>
        /// <param name="validationContext">The validation context.</param>
        /// <returns>A collection that holds failed-validation information.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Value <= 0m)
            {
                 yield return new ValidationResult("Не ме кефи стойността", new string[] { "Value" });    
            }

            if (this.EditReason == null)
            {
                yield return new ValidationResult("Не ме кефи стойността", new string[] { "Value" });   
            }
            
        }
    }
}