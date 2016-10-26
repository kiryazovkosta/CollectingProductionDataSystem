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

        public bool IsComposed { get; set; }
        public string ComposedBy { get; set; }
        public DateTime? ComposedOn { get; set; }

        public bool IsApproved { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime? ApprovedOn { get; set; }

        public virtual Factory Factory { get; set; }
    }
}