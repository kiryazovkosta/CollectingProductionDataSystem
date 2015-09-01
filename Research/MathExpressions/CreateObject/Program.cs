using System;
using System.Collections.Generic;
using System.Linq;
using ExpressionEvaluator;
using MathExpressions.Application;

namespace CreateObject
{
    class Program
    {

        static void Main(string[] args)
        {
            var calculator = new Calculator();
            var inputParams = new Dictionary<string, decimal>();
            inputParams.Add("a", 1);
            inputParams.Add("b", 2);
            Console.WriteLine(calculator.Calculate("par.a + par.b", "par", 2, inputParams));
        }

    }

}
