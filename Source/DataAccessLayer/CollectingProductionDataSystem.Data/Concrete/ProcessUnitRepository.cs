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
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts;
    using Models.Productions;

    #endregion

    /// <summary>
    /// Summary description for ProcessUnitRepository
    /// </summary>
    public class ProcessUnitRepository : DeletableEntityRepository<ProcessUnit>
    {
        public ProcessUnitRepository(IDbContext context)
            : base(context)
        {
        }

        public override void Add(ProcessUnit entity)
        {
            base.Add(entity);
            var factory = this.Context.Set<Factory>().Include(x=>x.Plant).FirstOrDefault(x => x.Id == entity.FactoryId);
            this.Context.Set<ProcessUnitToFactoryHistory>().Add(new ProcessUnitToFactoryHistory(entity, factory));
        }

        public override void Update(ProcessUnit entity)
        {
            base.Update(entity);
            var factory = this.Context.Set<Factory>().Include(x => x.Plant).FirstOrDefault(x => x.Id == entity.FactoryId);
            this.Context.Set<ProcessUnitToFactoryHistory>().Add(new ProcessUnitToFactoryHistory(entity, factory));
        }
    }
}