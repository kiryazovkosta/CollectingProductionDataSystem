namespace CollectingProductionDataSystem.Application.TankDataServices
{
    using CollectingProductionDataSystem.Application.Contracts;
    using CollectingProductionDataSystem.Data.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    public class InventoryTanksService : IInventoryTanksService
    {
        private readonly IProductionData data;
        public InventoryTanksService(IProductionData dataParam)
        {
            this.data = dataParam;
        }

        public IEnumerable<StatusOfTankDto> ReadDataForDay(DateTime targetDay, int? areaId, int? parkId)
        {
            return this.data.Tanks.All()
                .Where( t => t.Park.AreaId == (areaId??t.Park.AreaId) && t.ParkId == (parkId ?? t.ParkId))
                .Include(x => x.Park)
                .Include(x => x.Park.Area)
                .Include(x => x.TankStatusDatas)
                .Select(x => new StatusOfTankDto
                {
                    Tank = x,
                    Quantity = x.TankStatusDatas
                            .Where(z => z.RecordTimestamp <= targetDay && z.IsDeleted == false)
                            .OrderByDescending(y => y.RecordTimestamp)
                            .FirstOrDefault()
                });
        }
    }
}
