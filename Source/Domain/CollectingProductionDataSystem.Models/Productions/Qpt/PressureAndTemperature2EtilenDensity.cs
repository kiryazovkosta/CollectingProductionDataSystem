using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Models.Productions.Qpt
{
    public partial class PressureAndTemperature2EtilenDensity
    {
        public int Id { get; set; }
        public decimal Pressure { get; set; }
        public decimal Temperature { get; set; }
        public decimal Density { get; set; }
    }
}
