using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Infrastructure.Contracts
{
    public interface IProgressRegistrator
    {
        void RegisterProgress(int value);
    }
}
