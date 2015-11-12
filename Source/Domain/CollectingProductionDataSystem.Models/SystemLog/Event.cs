/// <summary>
/// Summary description for Events
/// </summary>
namespace CollectingProductionDataSystem.Models.SystemLog
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Contracts;

    public class Event : IEntity
    {
        public string EventId { get; set; }
        public System.DateTime EventTimeUtc { get; set; }
        public System.DateTime EventTime { get; set; }
        public string EventType { get; set; }
        public decimal EventSequence { get; set; }
        public decimal EventOccurrence { get; set; }
        public int EventCode { get; set; }
        public int EventDetailCode { get; set; }
        public string Message { get; set; }
        public string ApplicationPath { get; set; }
        public string ApplicationVirtualPath { get; set; }
        public string MachineName { get; set; }
        public string RequestUrl { get; set; }
        public string ExceptionType { get; set; }
        public string Details { get; set; }

        [NotMapped]
        public int Id { get; set; }
    }
}
