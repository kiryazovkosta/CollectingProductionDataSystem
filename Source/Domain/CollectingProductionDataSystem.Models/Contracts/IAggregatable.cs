namespace CollectingProductionDataSystem.Models.Contracts
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public interface IAggregatable
    {
        string Code { get; set; }

        string AggregationMembers { get; set; }
    }
}