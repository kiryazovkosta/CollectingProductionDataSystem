//-----------------------------------------------------------------------
// <copyright file="Factory.cs" company="Business Management System Ltd.">
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
    using Models;
    using Abstract;
    using Contracts;

    #endregion

    /// <summary>
    /// Summary description for Factory
    /// </summary>
    public partial class Factory:DeletableEntity, IEntity
    {
        //private ICollection<MonthlyTechnologicalReportsData> monthlyTechnologicalReportsDatas;
        private ICollection<ProcessUnitToFactoryHistory> processUnitsHistory;
        public Factory()
        {
            this.ProcessUnits = new HashSet<ProcessUnit>();
            this.ProcessUnitsHistory = new HashSet<ProcessUnitToFactoryHistory>();
        }

        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
        public Nullable<int> PlantId { get; set; }
        public virtual Plant Plant { get; set; }
        public virtual ICollection<ProcessUnit> ProcessUnits { get; set; }
        //public virtual ICollection<MonthlyTechnologicalReportsData> MonthlyTechnologicalReportsDatas
        //{
        //    get { return monthlyTechnologicalReportsDatas; }
        //    set { this.monthlyTechnologicalReportsDatas = value; }
        //}

        public virtual ICollection<ProcessUnitToFactoryHistory> ProcessUnitsHistory
        {
            get { return this.processUnitsHistory; }
            set { this.processUnitsHistory = value; }
        }

        public string FactorySortableName
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


        public override string ToString() => $"{this.Id}, {this.ShortName}, {this.FullName}";
    }
}