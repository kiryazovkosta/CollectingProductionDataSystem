namespace CollectingProductionDataSystem.Models.Transactions.HighwayPipelines
{
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using System.Collections.Generic;

    public class HighwayPipelineConfig : DeletableEntity, IEntity
    {
        private ICollection<HighwayPipelineData> highwayPipelineDatas;

        public HighwayPipelineConfig()
        {
            this.highwayPipelineDatas = new HashSet<HighwayPipelineData>();
        }

        public int Id { get; set; }
        public int PipeNumber { get; set; }
        public string Name { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<HighwayPipelineData> HighwayPipelineDatas 
        {
            get { return this.highwayPipelineDatas; }
            set { this.highwayPipelineDatas = value; }
        }
    }
}
