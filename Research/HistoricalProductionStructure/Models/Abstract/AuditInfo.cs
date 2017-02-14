namespace Domain.Abstract
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Contracts;

    public abstract class AuditInfo : IAuditInfo
    {
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Specifies whether or not the CreatedOn property should be automatically set.
        /// </summary>
        [NotMapped]
        public bool PreserveCreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string CreatedFrom { get; set; }

        public string ModifiedFrom { get; set; }
        
    }
}
