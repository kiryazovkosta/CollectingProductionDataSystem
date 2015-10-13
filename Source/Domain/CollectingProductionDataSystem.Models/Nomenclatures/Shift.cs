/// <summary>
/// Summary description for ShiftList
/// </summary>
namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Abstract;
    using CollectingProductionDataSystem.Models.Contracts;

    public class Shift : DeletableEntity, IEntity
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id { get; set; }

        public string Name { get; set; }

        public TimeSpan BeginTime { get; set; }

        public TimeSpan ReadOffset { get; set; }

        public TimeSpan ReadPollTimeSlot { get; set; }

    }
}
