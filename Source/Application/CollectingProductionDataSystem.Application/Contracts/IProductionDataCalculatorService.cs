namespace CollectingProductionDataSystem.Application.Contracts
{
    using System;
    using CollectingProductionDataSystem.Application.ProductionDataServices;
    using CollectingProductionDataSystem.Data.Contracts;

    public interface IProductionDataCalculatorService
    {
        double Calculate(string formulaCode, FormulaArguments arguments);

        IEfStatus CalculateByUnitData(int unitDataId, string userName, decimal newValue, decimal oldValue = 0);
    }
}