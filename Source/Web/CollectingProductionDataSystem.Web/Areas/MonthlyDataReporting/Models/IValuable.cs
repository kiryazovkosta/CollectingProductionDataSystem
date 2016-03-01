using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Web.Areas.MonthlyDataReporting.Models
{
    public interface IValuable
    {
        int Id { get; set; }
        DateTime RecordTimestamp { get; set; }
        decimal? Value { get; set; }
        int UnitMonthlyConfigId { get; set; }
        UnitManualMonthlyDataViewModel UnitManualMonthlyData { get; set; }
    }
}
