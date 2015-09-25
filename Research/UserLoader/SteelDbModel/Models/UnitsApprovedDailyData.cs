using System;
using System.Collections.Generic;

namespace SteelDbModel.Models
{
    public partial class UnitsApprovedDailyData
    {
        public int Id { get; set; }
        public System.DateTime RecordDate { get; set; }
        public int ProcessUnitId { get; set; }
        public bool Approved { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
        public string DeletedFrom { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public string CreatedFrom { get; set; }
        public string ModifiedFrom { get; set; }
    }
}
