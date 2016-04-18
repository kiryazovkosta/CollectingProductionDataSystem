using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Infrastructure.Contracts
{
    public interface IProgressMessage
    {
        int ProgressValue { get; set; }

        string Message { get; set; }
    }
}
