namespace CollectingProductionDataSystem.Application.MonthlyServices
{
    using System;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Nomenclatures;

    /// <summary>
    /// 
    /// </summary>
    public class InnerPipelineDto
    {
        private InnerPipelineData pipelineData = new InnerPipelineData
        {
            Mass = 0,
            Volume = 0
        };

        public int Id { get { return this.Quantity.Id; } }

        public DateTime RecordTimestamp
        {
            get
            {
                return this.Quantity.RecordTimestamp;
            }
        }

        public Product Product { get; set; }

        public InnerPipelineData Quantity
        {
            get
            {
                return this.pipelineData ?? new InnerPipelineData
                {
                    Id = 0,
                    Volume = 0,
                    Mass = 0
                };
            }
            set
            {
                this.pipelineData = value;
            }
        }
    }
}