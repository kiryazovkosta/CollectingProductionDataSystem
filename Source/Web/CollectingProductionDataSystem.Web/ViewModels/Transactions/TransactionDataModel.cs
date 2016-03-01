namespace CollectingProductionDataSystem.Web.ViewModels.Transactions
{
    using System;
    using System.Linq;

    public class TransactionDataModel 
    { 
        public int MeasuringPointId { get; set; }
        public int DirectionId { get; set; }
        public int TransportId { get; set; }
        public int ProductId { get; set; }
        public int FlowDirection { get; set; }
        public decimal? Mass { get; set; }
        public decimal? MassReverse { get; set; }
        public decimal RealMass
        {
            get 
            {
                if (this.Mass.HasValue && this.Mass.Value > 0)
                {
                    return this.Mass.Value;
                }
                else if (this.MassReverse.HasValue)
                {
                    return this.MassReverse.Value;   
                }
                else
                {
                    return 0.0m;
                }
            }
        }
    }
}