namespace CollectingProductionDataSystem.Models.Contracts
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    public interface IConvertable
    {
        bool IsConverted { get; set; }
    }
}