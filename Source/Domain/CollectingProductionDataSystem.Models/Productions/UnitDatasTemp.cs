namespace CollectingProductionDataSystem.Models.Productions
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Text;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class UnitDatasTemp : IEntity
    {

        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int UnitConfigId { get; set; }
        public int ShiftId { get; set; }
        public decimal? Value { get; set; }
        public virtual UnitConfig UnitConfig { get; set; }
        public int Confidence { get; set; }

        [NotMapped]
        public double RealValue
        {
            get
            {
                if (this.Value.HasValue)
                {
                    return (double)this.Value.Value;
                }
                else
                {
                    return default(double);
                }
            }
        }

        public string Stringify()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} | ", this.Id.ToString().PadLeft(10, '0'));
            sb.AppendFormat("{0:10} | ", this.UnitConfig.Code);
            sb.AppendFormat("{0:50} | ", this.UnitConfig.Name);
            sb.AppendFormat("{0:0.00000} | ", this.Value ?? 0);
            sb.AppendFormat("{0} | ", ShiftId);
            sb.AppendFormat("{0} | ", this.UnitConfig.CollectingDataMechanism);
            sb.AppendFormat("{0} | ", this.UnitConfig.PreviousShiftTag);
            sb.AppendFormat("{0} | ", this.Confidence);
            return sb.ToString();
        }
    }
}
