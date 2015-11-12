namespace CollectingProductionDataSystem.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity.Validation;

    public interface IEfStatus
    {
        IReadOnlyList<ValidationResult> EfErrors { get; }
        bool IsValid { get; }
        int ResultRecordsCount { get; set; }
        IEfStatus SetErrors(IEnumerable<ValidationResult> errors);
        IEfStatus SetErrors(IEnumerable<DbEntityValidationResult> dbErrors);
    }
}
