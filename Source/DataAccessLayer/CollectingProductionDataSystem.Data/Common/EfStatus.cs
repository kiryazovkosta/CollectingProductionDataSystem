/// <summary>
/// Summary description for EfStatus
/// </summary>
namespace CollectingProductionDataSystem.Data.Common
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Data.Contracts;

    public class EfStatus : IEfStatus
    {
        private List<ValidationResult> errors;

        private int resultRecordsCount;

        public int ResultRecordsCount
        {
            get { return this.resultRecordsCount; }
            set { this.resultRecordsCount = value; }
        }

        /// <summary>
        /// If there are no errors then it is valid
        /// </summary>
        public bool IsValid { get { return this.errors == null; } }

        public IReadOnlyList<ValidationResult> EfErrors
        {
            get { return this.errors ?? new List<ValidationResult>(); }
        }

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
            errors = errors.ToList();
            return this;
        }
    }
}
