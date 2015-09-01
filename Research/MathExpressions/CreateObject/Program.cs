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
            var inputParams = new Dictionary<string, double>();
            inputParams.Add("a", 2);
            inputParams.Add("b", 8);
            Console.WriteLine(calculator.Calculate("Math.Pow(par.a ,par.b)", "par", 2, inputParams)); 
        }
    }

 
}
