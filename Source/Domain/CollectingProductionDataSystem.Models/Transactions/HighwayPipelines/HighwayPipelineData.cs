namespace CollectingProductionDataSystem.Models.Transactions.HighwayPipelines
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;
    using System.ComponentModel.DataAnnotations;
    using System.Collections.Generic;

    public partial class HighwayPipelineData : DeletableEntity, IEntity, IValidatableObject
    {
        public int Id { get; set; }
        public DateTime RecordTimestamp { get; set; }
        public int HighwayPipelineConfigId { get; set; } 
        public string ProductName { get; set; }
        public int ProductCode { get; set; }
        public decimal? Volume { get; set; }
        public decimal? Mass { get; set; }
        public bool IsApproved { get; set; }
        public virtual HighwayPipelineConfig HighwayPipelineConfig { get; set; }
        public virtual HighwayPipelineManualData HighwayPipelineManualData { get; set; }

        [NotMapped]
        public decimal RealVolume
        { 
            get
            {
                if (this.HighwayPipelineManualData != null)
                {
                    return this.HighwayPipelineManualData.Volume;
                }
                else
                {
                    if (this.Volume.HasValue)
                    {
                        return this.Volume.Value;
                    }
                    else
                    {
                        return default(decimal);
                    }
                }
            }
        }

        [NotMapped]
        public decimal RealMass
        { 
            get
            {
                if (this.HighwayPipelineManualData != null)
                {
                    return this.HighwayPipelineManualData.Mass;
                }
                else
                {
                    if (this.Mass.HasValue)
                    {
                        return this.Mass.Value;
                    }
                    else
                    {
                        return default(decimal);
                    }
                }
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.Volume < 0.0M || this.Volume > decimal.MaxValue) 
            {
                yield return new ValidationResult(string.Format(Resources.ModelErrors.UnitsManualData_Value_Range, 0.0M, decimal.MaxValue), new string[] { "Volume" });
            }

            if (this.Mass < 0.0M || this.Mass > decimal.MaxValue) 
            {
                yield return new ValidationResult(string.Format(Resources.ModelErrors.UnitsManualData_Value_Range, 0.0M, decimal.MaxValue), new string[] { "Mass" });
            }
        }
    }
}
