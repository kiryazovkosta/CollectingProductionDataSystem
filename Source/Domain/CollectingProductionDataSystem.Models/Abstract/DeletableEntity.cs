namespace CollectingProductionDataSystem.Models.Abstract
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using CollectingProductionDataSystem.Models.Contracts;

    public abstract class DeletableEntity : AuditInfo, IDeletableEntity
    {
        [Editable(false)]
        public bool IsDeleted { get; set; }

        [Editable(false)]
        public DateTime? DeletedOn { get; set; }

        public string DeletedFrom { get; set; }
    }
}
