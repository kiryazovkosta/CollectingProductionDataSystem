using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Constants
{
    public static class DbConnection
    {
        private static readonly string defaultConnectionString = "CollectingPrimaryDataSystemConnection";

        public static string DefaultConnectionString
        {
            get
            {
                return defaultConnectionString;
            }
        }
    }
}