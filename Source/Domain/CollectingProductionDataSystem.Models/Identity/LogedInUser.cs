/// <summary>
/// Summary description for LogedInUser
/// </summary>
namespace CollectingProductionDataSystem.Models.Identity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Contracts;

    public class LogedInUser : IEntity
    {
        public LogedInUser()
        {
            this.TimeStamp = DateTime.Now;
        }
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public int LogedUsersCount { get; set; }
    }
}
