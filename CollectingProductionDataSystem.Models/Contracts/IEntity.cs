using System.ComponentModel.DataAnnotations;

namespace CollectingProductionDataSystem.Models.Contracts
{
    public interface IEntity
    {
        int Id { get; set; }
    }
}
