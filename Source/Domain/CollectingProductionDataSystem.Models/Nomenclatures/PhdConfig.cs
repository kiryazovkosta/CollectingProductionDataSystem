using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;

namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    public class PhdConfig : DeletableEntity, IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        public string HostName { get; set; }

        public string HostIpAddress { get; set; }

        public int Position { get; set; }

        public bool IsActive { get; set; }

    }
}
