namespace CollectingProductionDataSystem.Application.Contracts
{
    using System;
    using CollectingProductionDataSystem.Application.ProductionDataServices;
    using CollectingProductionDataSystem.Data.Contracts;

    public interface IProductionDataCalculatorService
    {
        double Calculate(string formulaCode, FormulaArguments arguments);

        IEfStatus CalculateByUnitData(decimal value, int unitDataId, string userName);
    }
}