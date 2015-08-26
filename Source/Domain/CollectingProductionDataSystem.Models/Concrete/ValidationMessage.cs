namespace CollectingProductionDataSystem.Models.Concrete
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ValidationMessage
    {
        private MessageType type;
        private string field;
        private string text;

        [JsonConstructor]
        public ValidationMessage()
        {
        }

        public ValidationMessage(MessageType type, string field, string text)
        {
            this.type = type;
            this.field = field;
            this.text = text;
        }

        public ValidationMessage(MessageType type, string text)
        {
            this.type = type;
            this.text = text;
        }

        public MessageType Type
        {
            get { return this.type; }
            set { this.type = value; }
        }

        public string Field
        {
            get { return this.field; }
            set { this.field = value; }
        }

        public string Text
        {
            get { return this.text; }
            set { this.text = value; }
        }

        public override bool Equals(object obj)
        {
            ValidationMessage validationMessage = (ValidationMessage)obj;
            return validationMessage.Type == this.type
                && validationMessage.Text == this.text
                && validationMessage.Field == this.field;
        }

        public override int GetHashCode()
        {
            return this.type.GetHashCode()
                ^ (this.field == null ? 0 : this.field.GetHashCode())
                ^ (this.text == null ? 0 : this.text.GetHashCode());
        }
    }
}
