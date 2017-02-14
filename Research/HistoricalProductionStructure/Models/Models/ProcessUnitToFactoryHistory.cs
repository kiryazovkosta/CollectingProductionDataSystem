//-----------------------------------------------------------------------
// <copyright file="ProcessUnitToFactoryHistory.cs" company="Business Management System Ltd.">
//     Copyright "2017" (c) Business Management System Ltd.. All rights reserved.
// </copyright>
// <author>Nikolay.Kostadinov</author>
//-----------------------------------------------------------------------

namespace Domain.Models
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Runtime.Remoting.Messaging;
    using System.Text;
    using System.Threading.Tasks;
    using Abstract;
    using Contracts;

    #endregion

    /// <summary>
    /// Summary description for ProcessUnitToFactoryHistory
    /// </summary>
    public partial class ProcessUnitToFactoryHistory: DeletableEntity, IEntity
    {
        public int Id { get; set; }
        public int ProcessUnitId { get; set; }
        public virtual ProcessUnit ProcessUnit { get; set; }
        public DateTime ActivationDate { get; set; }
        public string ProcessUnitShortName { get; set; }
        public string ProcessUnitFullName { get; set; }
        public int FactoryId { get; set; }
        public virtual Factory Factory { get; set; }
        public string FactoryShortName { get; set; }
        public string FactoryFullName { get; set; }
        public int PlantId { get; set; }
        public string PlantShortName { get; set; }
        public string PlantFullName { get; set; }
        public override string ToString() => $"{this.FactoryShortName} -> {this.ProcessUnitShortName} -> {this.ActivationDate}";

    }
}