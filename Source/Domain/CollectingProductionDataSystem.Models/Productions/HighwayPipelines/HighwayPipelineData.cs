namespace CollectingProductionDataSystem.Models.Productions.HighwayPipelines
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public partial class HighwayPipelineData : AuditInfo, IApprovableEntity, IEntity
    {
        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int HighwayPipelineConfigId { get; set; } 
        public string ProductName { get; set; }
        public int ProductCode { get; set; }
        public decimal? Value { get; set; }
        public bool IsApproved { get; set; }
        public virtual HighwayPipelineConfig HighwayPipelineConfig { get; set; }
        public virtual HighwayPipelineManualData HighwayPipelineManualData { get; set; }

        [NotMapped]
        public double RealValue 
        { 
            get
            {
                if (this.HighwayPipelineManualData != null)
                {
                    return (double)this.HighwayPipelineManualData.Value;
                }
                else
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
        }
    }
}
