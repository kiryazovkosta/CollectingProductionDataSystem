namespace CollectingProductionDataSystem.Models.Contracts
{
    using CollectingProductionDataSystem.Models.Concrete;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IValidationMessageCollection : IEnumerable<ValidationMessage>
    {
        IValidationMessageCollection Errors { get; }

        IValidationMessageCollection Infos { get; }

        IValidationMessageCollection Warnings { get; }

        void Clear();

        void AddMany(IValidationMessageCollection validationResult);

        void AddError(string text);

        void AddError(string field, string text);

        void AddWarning(string text);

        void AddInfos(string text);

        bool ContainsError(string field, string text);
    }
}
