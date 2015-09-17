using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingProductionDataSystem.Infrastructure.Extentions
{
    public static class DateTimeExtentions
    {
        public static bool Between(this DateTime current, DateTime begin, DateTime end)
        {
            var begintime = begin.TimeOfDay;
            var endTime = end.TimeOfDay;
            var currentTime = current.TimeOfDay;

            if (begin > end)
            {
                throw new ArgumentException("The begin time must be less than end time");
            }

            return (begintime <= currentTime) && (currentTime <= endTime);
        }
    }
}
