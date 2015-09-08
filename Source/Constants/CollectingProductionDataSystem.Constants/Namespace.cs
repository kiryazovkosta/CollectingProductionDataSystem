using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Constants
{
    public static class Namespace
    {
        public static string[] viewModels = new string[]{"CollectingProductionDataSystem.Web", "CollectingProductionDataSystem.Models"};

        public static string[] ViewModels
        {
            get
            {
                return viewModels;
            }
        }
    }
}
