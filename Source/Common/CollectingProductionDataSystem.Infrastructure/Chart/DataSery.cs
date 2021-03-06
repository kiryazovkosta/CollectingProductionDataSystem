﻿/// <summary>
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
        public DataSery(string typeParam, string labelParam, string axisParam, string colorParam = null, bool visibleParam = true)
            :this(typeParam,labelParam,colorParam)
        {
            this.Axis = axisParam;
            this.Visible = visibleParam;
        }
        public DataSery(string typeParam, string labelParam ,string colorParam = null)
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
        public string Axis { get; set; }
        public bool Visible { get; set; }
        public List<Pair<TypeX,TypeY>> Values { get; set; }
    }
}
