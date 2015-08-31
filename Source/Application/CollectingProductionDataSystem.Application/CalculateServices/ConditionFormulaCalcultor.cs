using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Application.CalculateServices
{
    public class ConditionFormulaCalcultor : IConditionFormulaCalcultor
    {
        public decimal Calc(decimal a, decimal b, decimal c)
        {
            decimal sum = a;
            sum += b;
            sum += c;
            return sum;
        }
    }
}
