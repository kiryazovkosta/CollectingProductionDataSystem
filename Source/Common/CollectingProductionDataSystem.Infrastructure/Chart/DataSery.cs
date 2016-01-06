/// <summary>
/// Summary description for DataSery
/// </summary>
namespace CollectingProductionDataSystem.Infrastructure.Chart
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    public class DataSery<TypeX, TypeY>
    {
        public DataSery(string typeParam, string labelParam, string colorParam = null)
            :this()
        {
            this.Type = typeParam;
            this.Label = labelParam;
            this.Color = colorParam;
        }

        public DataSery() 
        {
            Values = new List<Pair<TypeX, TypeY>>();
        }
        public string Type { get; set; }
        public string Label { get; set; }
        public string Color { get; set; }
        public List<Pair<TypeX,TypeY>> Values { get; set; }
    }
}
