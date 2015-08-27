namespace CollectingProductionDataSystem.Validators.Abstract
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Validators.Abstract;

    public interface IValidatorFactory
    {
        IValidator<UnitsManualData> UnitManualDataValidator { get; }
    }
}
