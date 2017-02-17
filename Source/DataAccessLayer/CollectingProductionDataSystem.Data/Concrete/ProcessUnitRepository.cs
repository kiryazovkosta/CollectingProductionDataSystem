//-----------------------------------------------------------------------
// <copyright file="ProcessUnitRepository.cs" company="Business Management System Ltd.">
//     Copyright "2017" (c) Business Management System Ltd.. All rights reserved.
// </copyright>
// <author>Nikolay.Kostadinov</author>
//-----------------------------------------------------------------------

namespace CollectingProductionDataSystem.Data.Concrete
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts;
    using Models.Productions;

    #endregion

    /// <summary>
    /// Summary description for ProcessUnitRepository
    /// </summary>
    class ProcessUnitRepository : DeletableEntityRepository<ProcessUnit>
    {
        public ProcessUnitRepository(IDbContext context)
            : base(context)
        {
        }

        public override void Add(ProcessUnit entity)
        {
            base.Add(entity);
            var factory = this.Context.Set<Factory>().FirstOrDefault(x => x.Id == entity.FactoryId);
            entity.FactoryHistories.Add(new ProcessUnitToFactoryHistory());
        }

        public override void Update(ProcessUnit entity)
        {
            base.Update(entity);
        }

        public override void Delete(ProcessUnit entity)
        {
            base.Delete(entity);
        }
    }
}