/// <summary>
/// Summary description for Message
/// </summary>
namespace CollectingProductionDataSystem.Models.UtilityEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public class Message : DeletableEntity, IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        public DateTime ValidUntill { get; set; }

        public string MessageText { get; set; }

        public MessageType MessageType { get; set; }
    }

    public enum MessageType
    {
        Info = 1,
        Warning = 2,
        Error = 3
    }
}
