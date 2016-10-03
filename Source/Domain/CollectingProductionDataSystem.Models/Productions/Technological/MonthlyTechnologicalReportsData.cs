namespace CollectingProductionDataSystem.Models.Productions.Technological
{
    using System;

    public class MonthlyTechnologicalReportsData
    {
        public int Id { get; set; }
        public int FactoryId { get; set; }
        public DateTime Month { get; set; }
        public string Message { get; set; }
        public bool IsApproved { get; set; }
        public string ApprovedBy { get; set; }

        public virtual Factory Factory { get; set; }
    }
}
