namespace CollectingProductionDataSystem.Application.HighwayPipelinesDataServices
{
    using System;
    using CollectingProductionDataSystem.Models.Transactions.HighwayPipelines;

    public class HighwayPipelineDto
    {
        private HighwayPipelineData highwayPipelinesData = new HighwayPipelineData
        {
            HighwayPipelineConfigId = 0,
            Mass = 0,
            Volume = 0,
        };

        public int Id { get { return this.Quantity.Id; } }

        public DateTime RecordTimestamp
        {
            get
            {
                return this.Quantity.RecordTimestamp;
            }
        }

        public HighwayPipelineConfig HighwayPipeline { get; set; }

        public HighwayPipelineData Quantity
        {
            get
            {
                return this.highwayPipelinesData ?? new HighwayPipelineData
                {
                    Id = 0,
                    HighwayPipelineConfigId = 0,
                    Mass = 0,
                    Volume = 0,
                };
            }
            set
            {
                this.highwayPipelinesData = value;
            }
        }
    }
}
