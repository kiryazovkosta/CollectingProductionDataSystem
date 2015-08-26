namespace CollectingProductionDataSystem.Models.Contracts
{
    using CollectingProductionDataSystem.Models.Concrete;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IValidateable
    {
        ValidationMessageCollection Validate();
    }
}
