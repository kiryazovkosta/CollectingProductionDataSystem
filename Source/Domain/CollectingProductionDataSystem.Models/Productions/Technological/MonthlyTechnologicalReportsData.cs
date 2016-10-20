namespace CollectingProductionDataSystem.Models.Productions.Technological
{
    using Abstract;
    using Contracts;
    using System;

    public class MonthlyTechnologicalReportsData : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public int FactoryId { get; set; }
        public DateTime Month { get; set; }
        public string Message { get; set; }
        public bool Approved { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public virtual Factory Factory { get; set; }
    }
}
