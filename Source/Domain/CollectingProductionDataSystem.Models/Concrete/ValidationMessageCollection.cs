namespace CollectingProductionDataSystem.Models.Concrete
{
    using CollectingProductionDataSystem.Models.Contracts;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ValidationMessageCollection : List<ValidationMessage>, IValidationMessageCollection
    {
        public ValidationMessageCollection()
        {
        }

        [JsonConstructor]
        public ValidationMessageCollection(IEnumerable<ValidationMessage> inputMessages)
        {
            if (inputMessages != null)
            {
                this.AddRange(inputMessages);
            }
        }

        public IValidationMessageCollection Errors
        {
            get
            {
                return new ValidationMessageCollection(this.Where(x => x.Type == MessageType.Error));
            }
        }

        public IValidationMessageCollection Infos
        {
            get
            {
                return new ValidationMessageCollection(this.Where(x => x.Type == MessageType.Info));
            }
        }

        public IValidationMessageCollection Warnings
        {
            get
            {
                return new ValidationMessageCollection(this.Where(x => x.Type == MessageType.Warning));
            }
        }

        public void AddMany(IValidationMessageCollection validationResult)
        {
            this.AddRange(validationResult);
        }

        public void AddError(string text)
        {
            this.Add(new ValidationMessage(MessageType.Error, text));
        }

        public void AddError(string field, string text)
        {
            this.Add(new ValidationMessage(MessageType.Error, field, text));
        }

        public void AddWarning(string text)
        {
            this.Add(new ValidationMessage(MessageType.Warning, text));
        }

        public void AddInfos(string text)
        {
            this.Add(new ValidationMessage(MessageType.Info, text));
        }

        public bool ContainsError(string field, string text)
        {
            return this.Contains(new ValidationMessage(MessageType.Error, field, text));
        }
    }
}
