using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    public class ProductionShift : DeletableEntity, IEntity
    {
        public int Id { get; set; }

        public string BeginTime { get; set; }

        public string EndTime { get; set; }

        public int BeginMinutes { get; set; }

        public int OffsetMinutes { get; set; }


        //1   Първа смяна 05:01   13:00   780     840
        //2   Втора смяна 13:01   21:00   1260    1320    
        //3   Втора смяна 21:01   05:00   1740    1800 
    }
}
