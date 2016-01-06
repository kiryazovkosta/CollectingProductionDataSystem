using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CollectingProductionDataSystem.Infrastructure.Chart;

namespace CollectingProductionDataSystem.Infrastructure.Chart
{
    public class ChartViewModel<XType, YType>
    {
        public string Title { get; set; }
        public IEnumerable<DataSery<XType, YType>> DataSeries { get; set; }
    }
}