using CollectingProductionDataSystem.Models.Abstract;
using CollectingProductionDataSystem.Models.Contracts;
namespace CollectingProductionDataSystem.Models.Productions.Qpt
{
    /// <summary>
    /// 
    /// </summary>
    public partial class Density2FactorAlpha : DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public decimal Density { get; set; }
        public decimal FactorAlpha { get; set; }
    }
}
