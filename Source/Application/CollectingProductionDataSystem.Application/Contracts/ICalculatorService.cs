namespace CollectingProductionDataSystem.Application.Contracts
{
    using System;
    using System.Collections.Generic;
    using ExpressionEvaluator;

    public interface ICalculatorService
    {
        double Calculate(string expression, string expressionArgumentName, int argumentsCount, Dictionary<string, double> arguments, string formulaCode);
    }
}