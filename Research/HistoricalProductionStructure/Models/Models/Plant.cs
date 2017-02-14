//-----------------------------------------------------------------------
// <copyright file="Plant.cs" company="Business Management System Ltd.">
//     Copyright "2017" (c) Business Management System Ltd.. All rights reserved.
// </copyright>
// <author>Nikolay.Kostadinov</author>
//-----------------------------------------------------------------------

namespace Domain.Models
{
    #region Usings

    using System.Collections.Generic;
    using Abstract;
    using Contracts;

    #endregion

    /// <summary>
    /// Summary description for Plant
    /// </summary>
    public partial class Plant: DeletableEntity, IEntity
    {
        public Plant()
        {
            this.Factories = new HashSet<Factory>();
        }

        public int Id { get; set; }

        public string ShortName { get; set; }

        public string FullName { get; set; }

        public ICollection<Factory> Factories { get; set; }
    }
}