/// <summary>
/// Summary description for DataSery
/// </summary>
namespace CollectingProductionDataSystem.Infrastructure.Chart
{
    using System;
    using System.Linq;

    public class Pair <XType,YType>
    {
        public Pair(){}

        public Pair(XType xValue, YType yValue)
        {
            this.X = xValue;
            this.Y = yValue;
        }
        public XType X { get; set; }
        public YType Y { get; set; }      
        
    }
}
