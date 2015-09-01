using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using ExpressionEvaluator;
using MathExpressions.Application;

namespace CreateObject
{
    class Program
    {


        static void DoTest()
        {
            var v = new Vars() { myDoubleVar = 1234.5678 };
            string parsestr = "(vars.myDoubleVar / 4 * 3) + (vars.myDoubleVar / (2 + 5))";
            var reg = new TypeRegistry();
            reg.RegisterType("vars", v);
            var p = new CompiledExpression(parsestr) { TypeRegistry = reg };
            p.Parse();
            p.Compile();
            Console.WriteLine("Result: {0}", p.Eval());
        }

        static void Main(string[] args)
        {
            DoTest();
            Console.ReadLine();
        }

        //static void Main(string[] args)
        //{
        //    var calculator = new Calculator();
        //    var inputParams = new Dictionary<string, decimal>();
        //    inputParams.Add("a", 1);
        //    inputParams.Add("b", 2);
        //    Console.WriteLine(calculator.Calculate("par.a+par.b","par",2,inputParams));
        //}

    }

    class Vars
    {
        public DateTime myDateVar { get; set; }
        public double myDoubleVar { get; set; }
    }

}
