namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    using System;
    using System.Collections.Generic;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Transactions;
    using CollectingProductionDataSystem.Models.Transactions.HighwayPipelines;

    public partial class Product: DeletableEntity, IEntity
    {
        private ICollection<TankData> tanksDatas;
        private ICollection<UnitConfig> unitConfigs;
        private ICollection<UnitDailyConfig> unitsDailyConfigs;
        private ICollection<MeasuringPointsConfigsData> measuringPointsConfigsDatas;
        private ICollection<MeasuringPointProductsConfig> measuringPointProductsConfigs;
        private ICollection<HighwayPipelineConfig> highwayPipelineConfigs;
        private ICollection<InnerPipelineData> innerPipelineDatas;
        private ICollection<InProcessUnitData> inProcessUnitDatas;

        public Product()
        {
            this.tanksDatas = new HashSet<TankData>();
            this.unitConfigs = new HashSet<UnitConfig>();
            this.unitsDailyConfigs = new HashSet<UnitDailyConfig>();
            this.measuringPointsConfigsDatas = new HashSet<MeasuringPointsConfigsData>();
            this.measuringPointProductsConfigs = new HashSet<MeasuringPointProductsConfig>();
            this.highwayPipelineConfigs = new HashSet<HighwayPipelineConfig>();
            this.innerPipelineDatas = new HashSet<InnerPipelineData>();
            this.inProcessUnitDatas = new HashSet<InProcessUnitData>();
        }

        public int Id { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public int ProductTypeId { get; set; }
        public virtual TankMasterProduct TankMasterProduct { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual ICollection<TankData> TanksDatas 
        {
            get { return this.tanksDatas; }
            set { this.tanksDatas = value; }
        }
        public virtual ICollection<UnitConfig> UnitConfigs 
        {
            get { return this.unitConfigs; }
            set { this.unitConfigs = value; }
        }
        public virtual ICollection<UnitDailyConfig> UnitsDailyConfigs
        {
            get { return this.unitsDailyConfigs; }
            set { this.unitsDailyConfigs = value; }
        }
        public virtual ICollection<MeasuringPointsConfigsData> MeasuringPointsConfigsDatas 
        {
            get { return this.measuringPointsConfigsDatas; }
            set { this.measuringPointsConfigsDatas = value; }
        }
        public virtual ICollection<MeasuringPointProductsConfig> MeasuringPointProductsConfigs 
        {
            get { return this.measuringPointProductsConfigs; }
            set { this.measuringPointProductsConfigs = value; }
        }
        public virtual ICollection<HighwayPipelineConfig> HighwayPipelineConfigs 
        {
            get { return this.highwayPipelineConfigs; }
            set { this.highwayPipelineConfigs = value; }
        }
        public virtual ICollection<InnerPipelineData> InnerPipelineDatas 
        {
            get { return this.innerPipelineDatas; }
            set { this.innerPipelineDatas = value; }
        }
        public virtual ICollection<InProcessUnitData> InProcessUnitDatas 
        {
            get { return this.inProcessUnitDatas; }
            set { this.inProcessUnitDatas = value; }
        }
    }
}
