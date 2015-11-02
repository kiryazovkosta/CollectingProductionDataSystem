/// <summary>
/// Summary description for ApplicationUserPark
/// </summary>
namespace CollectingProductionDataSystem.Models.Identity
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Inventories;

    public class ApplicationUserPark
    {
        public int ApplicationUserId { get; set; }

        public int ParkId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }

        public virtual Park Park { get; set; }
    }
}
