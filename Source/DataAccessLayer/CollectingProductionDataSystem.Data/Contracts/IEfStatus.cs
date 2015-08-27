namespace CollectingProductionDataSystem.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Data.Common;

    public interface IEfStatus
    {
        IReadOnlyList<ValidationResult> EfErrors { get; }
        bool IsValid { get; }
        int ResultRecordsCount { get; set; }
        IEfStatus SetErrors(System.Collections.Generic.IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> errors);
        IEfStatus SetErrors(System.Collections.Generic.IEnumerable<System.Data.Entity.Validation.DbEntityValidationResult> dbErrors);
    }
}
