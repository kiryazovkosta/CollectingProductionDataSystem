namespace CollectingProductionDataSystem.GetTransactions
{
    using System;
    using System.Data;
    using System.Linq;
    using System.ServiceProcess;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Infrastructure.Log;
    using CollectingProductionDataSystem.Models.Transactions;
    using Uniformance.PHD;
    using log4net;
    using System.Collections.Generic;
    using System.Data.Entity;

    static class GetTransactionsMain
    {
        private static readonly ILog logger;

        static GetTransactionsMain()
        {
            logger = LogManager.GetLogger("CollectingProductionDataSystem.GetTransactions");
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        internal static void Main()
        {
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[] 
            {
                new GetTransactions(logger)
            };
            ServiceBase.Run(servicesToRun);
        }

        internal static void ProcessTransactionsData()
        {
            try
            {
                logger.Info("Begin synchronization!");
                
                using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister(),new Logger())))
                {
                    var max = context.MaxAsoMeasuringPointDataSequenceNumberMap.All().FirstOrDefault();
                    if (max != null)
                    {
                        GetTransactionsFromAso(max, context);
                        var status = context.SaveChanges("Aso2Sql");
                        max.LastTransactionsFetchingDateTime = DateTime.Now;
                        context.MaxAsoMeasuringPointDataSequenceNumberMap.Update(max);
                        context.SaveChanges("Aso2Sql");
                        logger.InfoFormat("Successfully synchronization {0} records from Aso to Cpds", status.ResultRecordsCount);
                        logger.InfoFormat("Last transactions fetching date-time was updated to {0}.", DateTime.Now);
                    }

                    logger.Info("End synchronization");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
 
        private static void GetTransactionsFromAso(MaxAsoMeasuringPointDataSequenceNumber max, ProductionData context)
        {
            logger.InfoFormat("Last transaction fetching date-time was {0}", max.LastTransactionsFetchingDateTime);
            var maxLastTransactionsFetchingDateTime = max.LastTransactionsFetchingDateTime;
            var adapter = new AsoDataSetTableAdapters.flow_MeasuringPointsDataTableAdapter();
            var table = new AsoDataSet.flow_MeasuringPointsDataDataTable();
            adapter.Fill(table, maxLastTransactionsFetchingDateTime.Value);
            if (table.Rows.Count > 0)
            {
                var mesurinpPointsByTransactions = context.MeasuringPointConfigs.All().Where(x => x.IsUsedPhdTotalizers == true).Select(x => x.Id).ToList();

                foreach (AsoDataSet.flow_MeasuringPointsDataRow row in table.Rows)
                {
                    logger.InfoFormat("Processing transaction {0} from control point {1}", row.TransactionNumber, row.MeasuringPointId);

                    if (mesurinpPointsByTransactions.Contains(row.MeasuringPointId))
                    {
                        continue;
                    }

                    var tr = new MeasuringPointsConfigsData();
                    tr.MeasuringPointId = row.MeasuringPointId;
                    tr.MeasuringPointConfigId = row.MeasuringPointId;
                    tr.TransactionNumber = row.TransactionNumber;
                    tr.RowId = row.RowId;
                    tr.TransactionBeginTime = row.TransactionBeginTime;
                    tr.TransactionEndTime = row.TransactionEndTime;
                    tr.ExciseStoreId = row.ExciseStoreId;
                    tr.ZoneId = row.ZoneId;
                    tr.BaseProductNumber = row.BaseProductNumber;
                    tr.BaseProductType = row.BaseProductType;
                    tr.BaseProductName = row.BaseProductName;
                    tr.ProductNumber = row.ProductNumber;
                    var prod = context.Products.All().Where(p => p.Code == row.ProductNumber).FirstOrDefault();
                    tr.ProductId = prod.Id;
                    tr.ProductType = row.ProductType;
                    tr.ProductName = row.ProductName;
                    tr.FlowDirection = row.FlowDirection;
                    tr.EngineeringUnitMass = row.EngineeringUnitMass;
                    tr.EngineeringUnitVolume = row.EngineeringUnitVolume;
                    tr.EngineeringUnitDensity = row.EngineeringUnitDensity;
                    tr.EngineeringUnitTemperature = row.EngineeringUnitTemperature;
                    tr.FactorMass = row.FactorMass;
                    tr.FactorVolume = row.FactorVolume;
                    tr.FactorDensity = row.FactorDensity;
                    tr.FactorTemperature = row.FactorTemperature;
                    if (!row.IsTotalizerBeginGrossObservableVolumeNull())
                    {
                        tr.TotalizerBeginGrossObservableVolume = row.TotalizerBeginGrossObservableVolume;
                    }
                    if (!row.IsTotalizerEndGrossObservableVolumeNull())
                    {
                        tr.TotalizerEndGrossObservableVolume = row.TotalizerEndGrossObservableVolume;
                    }
                    if (!row.IsTotalizerBeginGrossStandardVolumeNull())
                    {
                        tr.TotalizerBeginGrossStandardVolume = row.TotalizerBeginGrossStandardVolume;
                    }
                    if (!row.IsTotalizerEndGrossStandardVolumeNull())
                    {
                        tr.TotalizerEndGrossStandardVolume = row.TotalizerEndGrossStandardVolume;
                    }
                    if (!row.IsTotalizerBeginMassNull())
                    {
                        tr.TotalizerBeginMass = row.TotalizerBeginMass;
                    }
                    if (!row.IsTotalizerEndMassNull())
                    {
                        tr.TotalizerEndMass = row.TotalizerEndMass;
                    }
                    if (!row.IsTotalizerBeginGrossObservableCommonVolumeNull())
                    {
                        tr.TotalizerBeginGrossObservableCommonVolume = row.TotalizerBeginGrossObservableCommonVolume;
                    }
                    if (!row.IsTotalizerEndGrossObservableCommonVolumeNull())
                    {
                        tr.TotalizerEndGrossObservableCommonVolume = row.TotalizerEndGrossObservableCommonVolume;
                    }
                    if (!row.IsTotalizerBeginGrossStandardCommonVolumeNull())
                    {
                        tr.TotalizerBeginGrossStandardCommonVolume = row.TotalizerBeginGrossStandardCommonVolume;
                    }
                    if (!row.IsTotalizerEndGrossStandardCommonVolumeNull())
                    {
                        tr.TotalizerEndGrossStandardCommonVolume = row.TotalizerEndGrossStandardCommonVolume;
                    }
                    if (!row.IsTotalizerBeginCommonMassNull())
                    {
                        tr.TotalizerBeginCommonMass = row.TotalizerBeginCommonMass;
                    }
                    if (!row.IsTotalizerEndCommonMassNull())
                    {
                        tr.TotalizerEndCommonMass = row.TotalizerEndCommonMass;
                    }
                    if (!row.IsGrossObservableVolumeNull())
                    {
                        tr.GrossObservableVolume = row.GrossObservableVolume;
                    }
                    if (!row.IsGrossStandardVolumeNull())
                    {
                        tr.GrossStandardVolume = row.GrossStandardVolume;
                    }
                    if (!row.IsMassNull())
                    {
                        tr.Mass = row.Mass;
                    }
                    if (!row.IsAverageObservableDensityNull())
                    {
                        tr.AverageObservableDensity = row.AverageObservableDensity;
                    }
                    if (!row.IsAverageReferenceDensityNull())
                    {
                        tr.AverageReferenceDensity = row.AverageReferenceDensity;
                    }
                    if (!row.IsAverageTemperatureNull())
                    {
                        tr.AverageTemperature = row.AverageTemperature;
                    }
                    if (!row.IsTotalizerBeginGrossObservableVolumeReverseNull())
                    {
                        tr.TotalizerBeginGrossObservableVolumeReverse = row.TotalizerBeginGrossObservableVolumeReverse;
                    }
                    if (!row.IsTotalizerEndGrossObservableVolumeReverseNull())
                    {
                        tr.TotalizerEndGrossObservableVolumeReverse = row.TotalizerEndGrossObservableVolumeReverse;
                    }
                    if (!row.IsTotalizerBeginGrossStandardVolumeReverseNull())
                    {
                        tr.TotalizerBeginGrossStandardVolumeReverse = row.TotalizerBeginGrossStandardVolumeReverse;
                    }
                    if (!row.IsTotalizerEndGrossStandardVolumeReverseNull())
                    {
                        tr.TotalizerEndGrossStandardVolumeReverse = row.TotalizerEndGrossStandardVolumeReverse;
                    }
                    if (!row.IsTotalizerBeginMassReverseNull())
                    {
                        tr.TotalizerBeginMassReverse = row.TotalizerBeginMassReverse;
                    }
                    if (!row.IsTotalizerEndMassReverseNull())
                    {
                        tr.TotalizerEndMassReverse = row.TotalizerEndMassReverse;
                    }
                    if (!row.IsTotalizerBeginGrossObservableCommonVolumeReverseNull())
                    {
                        tr.TotalizerBeginGrossObservableCommonVolumeReverse = row.TotalizerBeginGrossObservableCommonVolumeReverse;
                    }
                    if (!row.IsTotalizerEndGrossObservableCommonVolumeReverseNull())
                    {
                        tr.TotalizerEndGrossObservableCommonVolumeReverse = row.TotalizerEndGrossObservableCommonVolumeReverse;
                    }
                    if (!row.IsTotalizerBeginGrossStandardCommonVolumeReverseNull())
                    {
                        tr.TotalizerBeginGrossStandardCommonVolumeReverse = row.TotalizerBeginGrossStandardCommonVolumeReverse;
                    }
                    if (!row.IsTotalizerEndGrossStandardCommonVolumeReverseNull())
                    {
                        tr.TotalizerEndGrossStandardCommonVolumeReverse = row.TotalizerEndGrossStandardCommonVolumeReverse;
                    }
                    if (!row.IsTotalizerBeginCommonMassReverseNull())
                    {
                        tr.TotalizerBeginCommonMassReverse = row.TotalizerBeginCommonMassReverse;
                    }
                    if (!row.IsTotalizerEndCommonMassReverseNull())
                    {
                        tr.TotalizerEndCommonMassReverse = row.TotalizerEndCommonMassReverse;
                    }
                    if (!row.IsGrossObservableVolumeReverseNull())
                    {
                        tr.GrossObservableVolumeReverse = row.GrossObservableVolumeReverse;
                    }
                    if (!row.IsGrossStandardVolumeReverseNull())
                    {
                        tr.GrossStandardVolumeReverse = row.GrossStandardVolumeReverse;
                    }
                    if (!row.IsMassReverseNull())
                    {
                        tr.MassReverse = row.MassReverse;
                    }
                    if (!row.IsAverageObservableDensityReverseNull())
                    {
                        tr.AverageObservableDensityReverse = row.AverageObservableDensityReverse;
                    }
                    if (!row.IsAverageReferenceDensityReverseNull())
                    {
                        tr.AverageReferenceDensityReverse = row.AverageReferenceDensityReverse;
                    }
                    if (!row.IsAverageTemperatureReverseNull())
                    {
                        tr.AverageTemperatureReverse = row.AverageTemperatureReverse;
                    }
                    tr.InsertTimestamp = row.InsertTimestamp;
                    if (!row.IsResultIdNull())
                    {
                        tr.ResultId = row.ResultId;
                    }
                    if (!row.IsAdditiveResultIdNull())
                    {
                        tr.AdditiveResultId = row.AdditiveResultId;
                    }
                    if (!row.IsTamasCreateTimestampNull())
                    {
                        tr.TamasCreateTimestamp = row.TamasCreateTimestamp;
                    }
                    if (!row.IsTamasRecipeTransIdNull())
                    {
                        tr.TamasRecipeTransId = row.TamasRecipeTransId;
                    }
                    if (!row.IsEpksSequenceNumberNull())
                    {
                        tr.EpksSequenceNumber = row.EpksSequenceNumber;
                    }
                    if (!row.IsMartaRowNumNull())
                    {
                        tr.MartaRowNum = row.MartaRowNum;
                    }
                    if (!row.IsAlcoholContentNull())
                    {
                        tr.AlcoholContent = row.AlcoholContent;
                    }
                    if (!row.IsAlcoholContentReverseNull())
                    {
                        tr.AlcoholContentReverse = row.AlcoholContentReverse;
                    }
                    if (!row.IsBatchIdNull())
                    {
                        tr.BatchId = row.BatchId;
                    }

                    logger.InfoFormat("Processing sequence number {0}", row.SequenceNumber);
                    context.MeasuringPointsConfigsDatas.Add(tr);
                }
            }
        }

        internal static void ProcessActiveTransactionsData()
        { 
            try
            {
                logger.Info("Begin active transactions synchronization!");
                var now = DateTime.Now;
                var today = DateTime.Today;
                var fiveOClock = today.AddHours(5);
                
                var ts = now - fiveOClock;
                if(ts.TotalMinutes > 2 && ts.Hours == 0)
                {
                    using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister(),new Logger())))
                    {
                        var currentDate = today.AddDays(-1);
                        var existsActiveTransactionData = context.ActiveTransactionsDatas.All().Where(x => x.RecordTimestamp == currentDate).Any();
                        if (!existsActiveTransactionData)
                        {
                            var activeTransactionsTags = context.MeasuringPointConfigs.All().Where(x => !String.IsNullOrEmpty(x.ActiveTransactionStatusTag)).ToList();
                            if (activeTransactionsTags.Count > 0)
                            {
                                using (PHDHistorian oPhd = new PHDHistorian())
                                {
                                    using (PHDServer defaultServer = new PHDServer(Properties.Settings.Default.PHD_HOST))
                                    {
                                        SetPhdConnectionSettings(defaultServer, oPhd, ts);

                                        var tagsList = new Tags();
                                        foreach (var item in activeTransactionsTags)
                                        {
                                            var activeTransactionData = ProcessMeasuringPoint(tagsList, item, oPhd, currentDate, context);
                                            if (activeTransactionData != null)
                                            {
                                                context.ActiveTransactionsDatas.Add(activeTransactionData); 
                                            }
                                        }

                                        context.SaveChanges("Phd2Sql");
                                    }
                                }
                            }
                        }
                    }
                }

                logger.Info("End active transactions synchronization!");
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
 
        private static ActiveTransactionsData ProcessMeasuringPoint(Tags tagsList, MeasuringPointConfig item, PHDHistorian oPhd, DateTime currentDate, ProductionData context)
        {
            tagsList.RemoveAll();
            tagsList.Add(new Tag { TagName = item.ActiveTransactionStatusTag });
            var result = oPhd.FetchRowData(tagsList);
            var row = result.Tables[0].Rows[0];
            if (row["Value"].ToString() == "1")
            {
                tagsList.RemoveAll();
                tagsList.Add(new Tag { TagName = item.ActiveTransactionProductTag });
                if (item.DirectionId == 1)
                {
                    tagsList.Add(new Tag { TagName = item.ActiveTransactionMassTag });
                }
                else if (item.DirectionId == 2)
                {
                    tagsList.Add(new Tag { TagName = item.ActiveTransactionMassReverseTag });
                }
                else 
                { 
                    tagsList.Add(new Tag { TagName = item.ActiveTransactionMassTag });
                    tagsList.Add(new Tag { TagName = item.ActiveTransactionMassReverseTag });
                }

                var activeTransactionData = new ActiveTransactionsData
                {
                    RecordTimestamp = currentDate,
                    MeasuringPointConfigId = item.Id
                };

                var valueResult = oPhd.FetchRowData(tagsList);
                foreach (DataRow valueRow in valueResult.Tables[0].Rows)
                {
                    if (valueRow[0].ToString().Equals(item.ActiveTransactionProductTag))
                    {
                        int code = Convert.ToInt32(valueRow["Value"]);
                        var product = context.Products.All().Where(x => x.Code == code).FirstOrDefault();
                        activeTransactionData.ProductId = product != null ? product.Id : 1;
                    }
                    else 
                    {
                        var value = 0m;
                        if (valueRow[0].ToString().Equals(item.ActiveTransactionMassTag))
                        {
                            logger.Info(item.ActiveTransactionMassTag);
                            if(decimal.TryParse(valueRow["Value"].ToString(), out value))
                            {
                                if (!string.IsNullOrEmpty(item.MassCorrectionFactor))
                                {
                                    logger.Info(item.MassCorrectionFactor);
                                    value = value * Convert.ToDecimal(item.MassCorrectionFactor); 
                                }
                                activeTransactionData.Mass = value;
                            }
                        }
                        else if (valueRow[0].ToString().Equals(item.ActiveTransactionMassReverseTag))
                        {
                            logger.Info(item.ActiveTransactionMassTag);
                            if(decimal.TryParse(valueRow["Value"].ToString(), out value))
                            {
                                if (!string.IsNullOrEmpty(item.MassCorrectionFactor))
                                {
                                    logger.Info(item.MassCorrectionFactor);
                                    value = value * Convert.ToDecimal(item.MassCorrectionFactor); 
                                }
                                activeTransactionData.MassReverse = value;
                            }
                        }
                    }
                }

                return activeTransactionData;
            }

            return null;
        }
 
        private static void SetPhdConnectionSettings(PHDServer defaultServer, PHDHistorian oPhd, TimeSpan ts)
        {
            defaultServer.Port = Properties.Settings.Default.PHD_PORT;
            defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
            oPhd.DefaultServer = defaultServer;
            oPhd.StartTime = string.Format("NOW - {0}M", ts.Minutes - 2);
            oPhd.EndTime = string.Format("NOW - {0}M", ts.Minutes - 2);
            oPhd.Sampletype = SAMPLETYPE.Snapshot;
            oPhd.MinimumConfidence = 100;
            oPhd.MaximumRows = 1;
        }

        private static void SetPhdConnectionSettings(PHDServer defaultServer, PHDHistorian oPhd, string recordTimestamp)
        {
            defaultServer.Port = Properties.Settings.Default.PHD_PORT;
            defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
            oPhd.DefaultServer = defaultServer;
            oPhd.StartTime = recordTimestamp;
            oPhd.EndTime = recordTimestamp;
            oPhd.Sampletype = SAMPLETYPE.Snapshot;
            oPhd.MinimumConfidence = 100;
            oPhd.MaximumRows = 1;
        }


        internal static void ProcessScaleTransactionsData()
        {
            try
            {
                logger.Info("Begin scale transactions synchronization!");
                {
                    var now = DateTime.Now;
                    var today = DateTime.Today;
                    var fiveOClock = today.AddHours(5);

                    logger.InfoFormat("Today.Ticks: {0}", today.Ticks);
                
                    var ts = now - fiveOClock;
                    if(ts.TotalMinutes > 2 && ts.Hours == 0)
                    {
                        var phdValues = new Dictionary<int, long>();

                        using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister(),new Logger())))
                        {
                            if (!context.MeasuringPointsConfigsDatas.All().Where(x => x.TransactionNumber >= today.Ticks).Any())
                            {

                                var currentPhdTimestamp = string.Format("NOW-{0}H{1}M", Math.Truncate(ts.TotalHours), ts.Minutes);
                                var previousPhdTimestamp = string.Format("NOW-{0}H{1}M", Math.Truncate(ts.TotalHours) + 24, ts.Minutes);

                                var scaleMeasuringPointProducts = context.MeasuringPointProductsConfigs
                                    .All()
                                    .Include(x => x.MeasuringPointConfig)
                                    .Include(x => x.MeasuringPointConfig.Zone)
                                    .Include(x => x.Product)
                                    .Include(x => x.Product.ProductType)
                                    .Include(x => x.Direction)
                                    .ToList();
                                foreach (var scaleMeasuringPointProduct in scaleMeasuringPointProducts)
                                {
                                    using (PHDHistorian oPhd = new PHDHistorian())
                                    {
                                        using (PHDServer defaultServer = new PHDServer(Properties.Settings.Default.PHD_HOST))
                                        {

                                            SetPhdConnectionSettings(defaultServer, oPhd, currentPhdTimestamp);
                                            var result = oPhd.FetchRowData(scaleMeasuringPointProduct.PhdProductTotalizerTag);
                                            var row = result.Tables[0].Rows[0];
                                            var value = Convert.ToInt64(row["Value"]);
                                            phdValues.Add(scaleMeasuringPointProduct.Id, value);
                                            logger.InfoFormat("Processing data for {0} {1} {2}", scaleMeasuringPointProduct.PhdProductTotalizerTag, currentPhdTimestamp, value);
                                        }
                                    }
                                }

                                foreach (var scaleMeasuringPointProduct in scaleMeasuringPointProducts)
                                {
                                    using (PHDHistorian oPhd = new PHDHistorian())
                                    {
                                        using (PHDServer defaultServer = new PHDServer(Properties.Settings.Default.PHD_HOST))
                                        {
                                            SetPhdConnectionSettings(defaultServer, oPhd, previousPhdTimestamp);
                                            var result = oPhd.FetchRowData(scaleMeasuringPointProduct.PhdProductTotalizerTag);
                                            var row = result.Tables[0].Rows[0];
                                            var value = Convert.ToInt64(row["Value"]);
                                            phdValues[scaleMeasuringPointProduct.Id] = phdValues[scaleMeasuringPointProduct.Id] - value;
                                            logger.InfoFormat("Processing data for {0} {1} {2} {3}", scaleMeasuringPointProduct.PhdProductTotalizerTag, currentPhdTimestamp, value, phdValues[scaleMeasuringPointProduct.Id]);
                                        }
                                    }
                                }

                                foreach (KeyValuePair<int, long> entry in phdValues)
                                {
                                    if (entry.Value > 0)
                                    {
                                        var scaleProduct = scaleMeasuringPointProducts.FirstOrDefault(x => x.Id == entry.Key);
                                        logger.InfoFormat("Processing virtual transaction for {0} {1} {2} {3}", entry.Key, scaleProduct.PhdProductTotalizerTag, entry.Value, scaleProduct.Direction.Name);

                                        decimal? mass = null;
                                        decimal? revMass = null;
                                        if (scaleProduct.MeasuringPointConfig.DirectionId == 1)
                                        {
                                            mass = entry.Value;
                                        }
                                        else if (scaleProduct.MeasuringPointConfig.DirectionId == 2)
                                        {
                                            revMass = entry.Value;
                                        }
                                        else
                                        {
                                            if (scaleProduct.DirectionId == 1)
                                            {
                                                mass = entry.Value;
                                            }
                                            else
                                            {
                                                revMass = entry.Value;
                                            }
                                        }

                                        var measuringPointConfigData = new MeasuringPointsConfigsData
                                        {
                                            MeasuringPointConfigId = scaleProduct.MeasuringPointConfigId,
                                            TransactionNumber = today.Ticks + entry.Key,
                                            RowId = -1,
                                            Mass = mass,
                                            MassReverse = revMass,
                                            TransactionBeginTime = today,
                                            TransactionEndTime = today.AddHours(4),
                                            InsertTimestamp = DateTime.Now,
                                            MeasuringPointId = scaleProduct.MeasuringPointConfigId,
                                            ZoneId = scaleProduct.MeasuringPointConfig.Zone.Id,
                                            ProductId = scaleProduct.Product.Code,
                                            ProductNumber = scaleProduct.Product.Code,
                                            ProductName = scaleProduct.Product.Name,
                                            ProductType = scaleProduct.Product.ProductType.Id,
                                            BaseProductNumber = scaleProduct.Product.Code,
                                            BaseProductName = scaleProduct.Product.Name,
                                            BaseProductType = scaleProduct.Product.ProductType.Id,
                                            FlowDirection = scaleProduct.DirectionId
                                        };
                                        context.MeasuringPointsConfigsDatas.Add(measuringPointConfigData);
                                    }
                                }

                                context.SaveChanges("Phd2SqlTotalizer");
                            }
                        }
                    }
                }

                logger.Info("End scale transactions synchronization!");
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
    }
}