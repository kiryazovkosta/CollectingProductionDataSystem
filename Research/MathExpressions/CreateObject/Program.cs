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
            inputParams.Add("a", 2);
            inputParams.Add("b", 2);
            Console.WriteLine(calculator.Calculate("Math.Pow(Convert.ToDouble(par.a) ,Convert.ToDouble(par.b))", "par", 2, inputParams)); 
        }
    }

 
}
