namespace HistoricalProductionStructure.DataAccess.Common
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using Contracts;

    /// <summary>
    /// Summary description for EfStatus
    /// </summary>
    public class EfStatus : IEfStatus
    {
        private List<ValidationResult> errors;

        public int ResultRecordsCount { get; set; }

        /// <summary>
        /// If there are no errors then it is valid
        /// </summary>
        public bool IsValid => this.errors == null;

        public IReadOnlyList<ValidationResult> EfErrors => this.errors ?? new List<ValidationResult>();

        /// <summary>
        /// This converts the Entity framework errors into Validation Errors
        /// </summary>
        public IEfStatus SetErrors(IEnumerable<DbEntityValidationResult> dbErrors)
        {
            this.errors =
                dbErrors.SelectMany(
                    x => x.ValidationErrors.Select(y =>
                          new ValidationResult(y.ErrorMessage, new[] { y.PropertyName })))
                    .ToList();
            return this;
        }
        public IEfStatus SetErrors(IEnumerable<ValidationResult> errors)
        {
            this.errors = errors.ToList();
            return this;
        }
    }
}
