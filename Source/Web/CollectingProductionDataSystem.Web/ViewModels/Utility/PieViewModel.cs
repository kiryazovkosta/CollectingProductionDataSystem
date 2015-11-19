using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CollectingProductionDataSystem.Web.ViewModels.Utility
{
    public class PieViewModel
    {
        public string Category { get; set; }
        public int Value { get; set; }
        public string Color { get; set; }
        public bool Explode{ get; set; }
    }
}