using CollectingProductionDataSystem.Models.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Application.TankDataServices
{
    public class StatusOfTankDto
    {
        private TankStatusData tankStatusData = new TankStatusData
        {
            TankStatusId = 0
        };

        public int Id { get { return this.Quantity.Id; } }

        public DateTime RecordTimestamp
        {
            get
            {
                return this.Quantity.RecordTimestamp;
            }
        }

        public TankConfig Tank { get; set; }

        public TankStatusData Quantity
        {
            get
            {
                return this.tankStatusData ?? new TankStatusData
                {
                    Id = 0,
                    TankStatusId = 0
                };
            }
            set
            {
                this.tankStatusData = value;
            }
        }
    }
}
