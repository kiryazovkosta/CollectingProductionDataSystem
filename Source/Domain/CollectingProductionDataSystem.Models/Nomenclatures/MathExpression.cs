using System;
using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;

namespace CollectingProductionDataSystem.Models.Nomenclatures
{
    public class MathExpression : DeletableEntity, IEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Expression { get; set; }

        public string Description { get; set; }
    }
}
