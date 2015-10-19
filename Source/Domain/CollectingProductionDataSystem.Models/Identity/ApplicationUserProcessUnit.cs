/// <summary>
/// Summary description for ApplicationUserProcessUnit
/// </summary>
namespace CollectingProductionDataSystem.Models.Identity
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Productions;

    public class ApplicationUserProcessUnit
    {
        public int ApplicationUserId { get; set; }

        public int ProcessUnitId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual ProcessUnit ProcessUnit { get; set; }
    }
}
