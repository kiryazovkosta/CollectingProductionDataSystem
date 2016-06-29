namespace CollectingProductionDataSystem.Application.CalculatorService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CollectingProductionDataSystem.Application.Contracts;
    using ExpressionEvaluator;
    using Infrastructure.Contracts;
    public class CalculatorService : ICalculatorService
    {
        private readonly ILogger logger;

        public CalculatorService()
        {
            this.logger = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculatorService" /> class.
        /// </summary>
        /// <param name="loggerParam">The logger param.</param>
        public CalculatorService(ILogger loggerParam)
        {
            this.logger = loggerParam;
        }

        /// <summary>
        /// Calculates the specified expression.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <param name="expressionArgumentName">Name of the expression argument.</param>
        /// <param name="argumentsCount">The arguments count.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        public double Calculate(string expression, string expressionArgumentName, int argumentsCount, Dictionary<string, double> arguments, string formulaCode)
        {
            if (argumentsCount != arguments.Count)
            {
                string message = $"Invalid arguments count. Expected {argumentsCount} actual {arguments?.Count}";
                throw new ArgumentException(message);
            }

            var inputObject = ObjectBuilder.CreateObject(arguments);
            try
            {
                var reg = new TypeRegistry();
                reg.RegisterType(expressionArgumentName + "s", inputObject.GetType());
                reg.RegisterSymbol(expressionArgumentName, inputObject);
                reg.RegisterType("Math", typeof(Math));
                reg.RegisterType("Convert", typeof(Convert));
                var p = new CompiledExpression(expression) { TypeRegistry = reg };
                return (double) p.Eval();
            }
            catch (Exception ex) when (OnlyLogThisError(ex, expression, expressionArgumentName, argumentsCount, arguments, formulaCode))
            {
                throw ex;
            }
        }

        private bool OnlyLogThisError(Exception ex, string expression, string expressionArgumentName, int argumentsCount, Dictionary<string, double> arguments,string formulaCode)
        {
            var customDetails = new string[] { $"{nameof(formulaCode)}: {formulaCode}",
                                               $"{nameof(expression)}: {expression}",
                                               $"{nameof(expressionArgumentName)}: {expressionArgumentName}",
                                               $"{nameof(argumentsCount)}: {argumentsCount}",
                                               $"argument actual count: {arguments?.Count}",
                                               $"{nameof(arguments)}: \n{string.Join("\n",arguments.Select(x=>$"\t{x.Key}: {x.Value}").ToArray())}"
                };
            this.logger?.Error("Calculator error", this, ex, customDetails);
            return false;
        }
    }

}
