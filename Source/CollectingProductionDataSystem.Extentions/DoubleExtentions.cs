using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Extentions
{
    public static class DoubleExtentions
    {
        /// <summary>
        /// Filters the not a number or infinity double values.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <returns></returns>
        public static double FilterNotANumberValues(this double result)
        {
            if (double.IsNaN(result) || double.IsInfinity(result))
            {
                result = 0;
            }

            return result;
        }
    }
}
