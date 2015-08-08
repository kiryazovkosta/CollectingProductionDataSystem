namespace CollectingProductionDataSystem.Common.Contracts
{
    using System;

    public interface IActiveEntity
    {
        bool IsActive { get; set; }
    }
}