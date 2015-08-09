namespace CollectingProductionDataSystem.Common
{
    using System;

    public interface IActiveEntity
    {
        bool IsActive { get; set; }
    }
}