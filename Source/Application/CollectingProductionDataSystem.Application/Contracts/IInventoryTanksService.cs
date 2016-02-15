namespace CollectingProductionDataSystem.Application.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Application.TankDataServices;

    public interface IInventoryTanksService
    {
        IEnumerable<StatusOfTankDto> ReadDataForDay(DateTime targetDay, int? areaId, int? parkId);
    }
}