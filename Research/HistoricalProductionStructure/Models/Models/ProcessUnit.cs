//-----------------------------------------------------------------------
// <copyright file="ProcessUnit.cs" company="Business Management System Ltd.">
//     Copyright "2017" (c) Business Management System Ltd.. All rights reserved.
// </copyright>
// <author>Nikolay.Kostadinov</author>
//-----------------------------------------------------------------------

namespace Domain.Models
{

    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Abstract;
    using Contracts;
    #endregion

    /// <summary>
    /// Summary description for ProcessUnit
    /// </summary>
    public partial class ProcessUnit: DeletableEntity, IEntity
    {
        //private ICollection<ApplicationUserProcessUnit> applicationUserProcessUnits;
        //private ICollection<ProductionPlanConfig> productionPlanConfigs;
        //private ICollection<InProcessUnitData> inProcessUnitDatas;
        //private ICollection<UnitMonthlyConfig> unitMonthlyConfigs;
        private ICollection<ProcessUnitToFactoryHistory> factoryHistory;

        public ProcessUnit()
        {
            this.factoryHistory = new HashSet<ProcessUnitToFactoryHistory>();
        }


        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public int FactoryId { get; set; }
        public virtual Factory Factory { get; set; }

        public virtual ICollection<ProcessUnitToFactoryHistory> FactoryHistories
        {
            get { return this.factoryHistory; }
            set { this.factoryHistory = value; }
        }

        public string SortableName
        {
            get
            {
                var sb = new StringBuilder();
                sb.Append(this.Id.ToString("d2"));
                sb.Append(" ");
                sb.Append(this.ShortName);
                return sb.ToString();
            }
        }
    }
}