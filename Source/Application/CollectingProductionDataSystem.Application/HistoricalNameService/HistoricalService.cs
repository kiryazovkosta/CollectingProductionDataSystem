//-----------------------------------------------------------------------
// <copyright file="HistoricalService.cs" company="Business Management System Ltd.">
//     Copyright "2017" (c) Business Management System Ltd.. All rights reserved.
// </copyright>
// <author>Nikolay.Kostadinov</author>
//-----------------------------------------------------------------------

namespace CollectingProductionDataSystem.Application.HistoricalNameService
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Contracts;
    using Data.Concrete;
    using Data.Contracts;
    using Models.Contracts;
    using Models.Productions;

    #endregion

    /// <summary>
    /// Summary description for HistoricalService
    /// </summary>
    public class HistoricalService : IHistoricalService
    {
        private readonly IProductionData data;
        public HistoricalService(IProductionData dataParam)
        {
            this.data = dataParam;
        }

        public IEnumerable<Factory> GetActualFactories(DateTime targetDate, int? factoryId = null)
        {
            var jRecords = this.GetActualStateOfProcessUnitsAndFactories(targetDate);

            if (factoryId != null)
            {
                jRecords = jRecords.Where(x => x.ProcessUnitToFactoryHistory.FactoryId == factoryId.Value);
            }


            var factories = new Dictionary<int, Factory>();


            foreach (var historyRecord in jRecords)
            {
                var newHistoryProcessUnit = historyRecord.ProcessUnit;
                newHistoryProcessUnit.ShortName = historyRecord.ProcessUnitToFactoryHistory.ProcessUnitShortName;
                newHistoryProcessUnit.FullName = historyRecord.ProcessUnitToFactoryHistory.ProcessUnitFullName;

                if (!factories.ContainsKey(historyRecord.ProcessUnitToFactoryHistory.FactoryId))
                {
                    var newFactory = new Factory()
                    {
                        Id = historyRecord.ProcessUnitToFactoryHistory.FactoryId,
                        ShortName = historyRecord.ProcessUnitToFactoryHistory.FactoryShortName,
                        FullName = historyRecord.ProcessUnitToFactoryHistory.FactoryFullName
                    };
                    newFactory.ProcessUnits.Add(newHistoryProcessUnit);

                    factories.Add(newFactory.Id, newFactory);
                }
                else
                {
                    factories[historyRecord.ProcessUnitToFactoryHistory.FactoryId].ProcessUnits.Add(newHistoryProcessUnit);
                }
            }

            return factories.Select(x => x.Value).OrderBy(x => x.Id);
        }


        private Dictionary<int, ProcessUnit> GetActualProcessUnits(DateTime targetDate)
        {
            var jRecords = this.GetActualStateOfProcessUnitsAndFactories(targetDate);

            var processUnits = new Dictionary<int, ProcessUnit>();

            foreach (var historyRecord in jRecords)
            {
                if (!processUnits.ContainsKey(historyRecord.ProcessUnitToFactoryHistory.ProcessUnitId))
                {
                    var newProcessUnit = new ProcessUnit()
                    {
                        Id = historyRecord.ProcessUnitToFactoryHistory.ProcessUnitId,
                        ShortName = historyRecord.ProcessUnitToFactoryHistory.ProcessUnitShortName,
                        FullName = historyRecord.ProcessUnitToFactoryHistory.ProcessUnitFullName,
                        Factory = new Factory()
                        {
                            Id = historyRecord.ProcessUnitToFactoryHistory.FactoryId,
                            ShortName = historyRecord.ProcessUnitToFactoryHistory.FactoryShortName,
                            FullName = historyRecord.ProcessUnitToFactoryHistory.FactoryFullName
                        }
                    };

                    processUnits.Add(newProcessUnit.Id, newProcessUnit);
                }
            }

            return processUnits;
        }

        public void SetHistoricalProcessUnitParams(IEnumerable<IConfigable> entity, DateTime targetDate)
        {
            var historyProcessUnit = this.ProcessUnitToFactoryHistories(targetDate).ToDictionary(x => x.ProcessUnitId);
            foreach (var record in entity)
            {
                record.Config.HistorycalProcessUnit = this.CreateHistoryRecord(historyProcessUnit, record.Config.ProcessUnitId);
            }
        }

        public void SetHistoricalProcessUnitParams(IEnumerable<IProcessUnitCangeable> enumerableEntity, DateTime targetDate)
        {
            var historyProcessUnit = this.ProcessUnitToFactoryHistories(targetDate).ToDictionary(x => x.ProcessUnitId);
            foreach (var record in enumerableEntity)
            {
                {
                    record.HistorycalProcessUnit = this.CreateHistoryRecord(historyProcessUnit, record.ProcessUnitId);
                }
            }
        }

        private ProcessUnit CreateHistoryRecord(Dictionary<int, ProcessUnitToFactoryHistory> historyProcessUnit, int processUnitId)
        {
            if (historyProcessUnit.ContainsKey(processUnitId))
            {
                return new ProcessUnit()
                {
                    Id = historyProcessUnit[processUnitId].ProcessUnitId,
                    ShortName = historyProcessUnit[processUnitId].ProcessUnitShortName,
                    FullName = historyProcessUnit[processUnitId].ProcessUnitFullName,
                    Factory = new Factory()
                    {
                        Id = historyProcessUnit[processUnitId].FactoryId,
                        ShortName = historyProcessUnit[processUnitId].FactoryShortName,
                        FullName = historyProcessUnit[processUnitId].FactoryFullName,
                        Plant = new Plant()
                        {
                            Id = historyProcessUnit[processUnitId].PlantId,
                            ShortName = historyProcessUnit[processUnitId].PlantShortName,
                            FullName = historyProcessUnit[processUnitId].PlantFullName,
                        }
                    }
                };
            }
            else
            {
                return null;
            }
        }

        IEnumerable<ProcessUnit> IHistoricalService.GetActualProcessUnits(DateTime targetDate)
        {
            return this.GetActualProcessUnits(targetDate).Values.OrderBy(x=>x.Id).ToList();
        }

        private IQueryable<ProcessUnitHistoryDto> GetActualStateOfProcessUnitsAndFactories(DateTime targetDate)
        {
            var records = this.ProcessUnitToFactoryHistories(targetDate);

            var jRecords = records.Join(this.data.ProcessUnits.All().Include(self => self.FactoryHistories),
                recs => recs.ProcessUnitId,
                pu => pu.Id,
                (recs, pu) => new ProcessUnitHistoryDto() { ProcessUnitToFactoryHistory = recs, ProcessUnit = pu });

            return jRecords;
        }

        private IOrderedQueryable<ProcessUnitToFactoryHistory> ProcessUnitToFactoryHistories(DateTime targetDate)
        {
            var records = this.data.ProcessUnitToFactoryHistory.All().Include(x => x.Factory).Include(x => x.ProcessUnit)
                .Where(x => x.ActivationDate <= targetDate)
                .GroupBy(x => x.ProcessUnitId)
                .SelectMany(x => x.Where(y => y.ActivationDate == x.Max(z => z.ActivationDate))).OrderBy(x => x.ProcessUnitId);
            return records;
        }

    }
    internal class ProcessUnitHistoryDto
    {
        public ProcessUnitToFactoryHistory ProcessUnitToFactoryHistory { get; set; }
        public ProcessUnit ProcessUnit { get; set; }
    }
}