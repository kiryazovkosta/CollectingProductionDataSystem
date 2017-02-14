//-----------------------------------------------------------------------
// <copyright file="ProcessUnitToFactoryHistoryMap.cs" company="Business Management System Ltd.">
//     Copyright "2017" (c) Business Management System Ltd.. All rights reserved.
// </copyright>
// <author>Nikolay.Kostadinov</author>
//-----------------------------------------------------------------------

namespace CollectingProductionDataSystem.Data.Mappings
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Models.Productions;

    #endregion

    /// <summary>
    /// Summary description for ProcessUnitToFactoryHistoryMap
    /// </summary>
    public class ProcessUnitToFactoryHistoryMap: EntityTypeConfiguration<ProcessUnitToFactoryHistory>
    {
        public ProcessUnitToFactoryHistoryMap()
        {
            this.HasKey(t => new { t.Id });

            //relationships
            this.HasRequired(x => x.ProcessUnit)
                .WithMany(y => y.FactoryHistories)
                .HasForeignKey(d => d.ProcessUnitId).WillCascadeOnDelete(false);

            this.HasRequired(x => x.Factory)
                .WithMany(x => x.ProcessUnitsHistory)
                .HasForeignKey(fk => fk.FactoryId).WillCascadeOnDelete(false);
        }
    }
}