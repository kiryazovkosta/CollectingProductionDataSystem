namespace CollectingProductionDataSystem.GetTransactions
{
    using System;
    using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
    using System.Linq;
    using System.ServiceProcess;
    using System.Transactions;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Models.Transactions;
    using log4net;

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

        internal static void ProcessTransactionData()
        {
            try
            {
                logger.Info("Begin synchronization!");
                
                using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister())))
                {
                    var max = context.MaxAsoMeasuringPointDataSequenceNumberMap.All().FirstOrDefault();
                    if (max != null)
                    {
                        logger.InfoFormat("Maximum transaction sequence number is {0}", max.MaxSequenceNumber);
                        var maxSequenceNumber = max.MaxSequenceNumber;
                        var adapter = new AsoDataSetTableAdapters.flow_MeasuringPointsDataTableAdapter();
                        var table = new AsoDataSet.flow_MeasuringPointsDataDataTable();
                        adapter.Fill(table, maxSequenceNumber);
                        if (table.Rows.Count > 0)
                        {
                            foreach (AsoDataSet.flow_MeasuringPointsDataRow row in table.Rows)
                            {
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
                                tr.ProductId = row.ProductNumber;
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

                                context.MeasuringPointsConfigsDatas.Add(tr);
                            }

                            using (TransactionScope scope = new TransactionScope())
                            {
                                var status = context.SaveChanges("Aso2Sql");
                                long maxValue = Convert.ToInt64(table.Compute("max(SequenceNumber)", string.Empty));
                                logger.Info(maxValue);
                                max.MaxSequenceNumber = maxValue;
                                context.MaxAsoMeasuringPointDataSequenceNumberMap.Update(max);
                                context.SaveChanges("Aso2Sql");
                                scope.Complete();
                                logger.InfoFormat("Successfully synchronization {0} records from Aso to Cpds", status.ResultRecordsCount);
                                logger.InfoFormat("Maximum sequence number updated to {0}.", maxValue);
                            }
                        }
                    }

                    logger.Info("End synchronization");
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        internal static void ProcessScalesData()
        {
            try
            {
                logger.Info("Begin scale synchronization!");

                using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister())))
                {
                    var max = context.MaxAsoMeasuringPointDataSequenceNumberMap.All().FirstOrDefault();
                    if (max != null)
                    {
                        logger.InfoFormat("Last scale fetching date-time is {0}", max.LastFetchScales);
                        var lastFetchScalesDateTime = max.LastFetchScales;

                        var scaleAdapter = new ScaleDataSetTableAdapters.ScaleDataTableAdapter();
                        var scaleTable = new ScaleDataSet.ScaleDataDataTable();
                        scaleAdapter.Fill(scaleTable, lastFetchScalesDateTime);

                        if (scaleTable.Rows.Count > 0)
                        {
                            var scales = context.MeasuringPointConfigs.All().Where(x => x.WeightScaleNumber != null).ToList();

                            foreach (ScaleDataSet.ScaleDataRow row in scaleTable.Rows)
                            {
                                var tr = new MeasuringPointsConfigsData();
                                var scale = scales.Where(s => s.WeightScaleNumber == row.SCALE_NUM).First();

                                tr.MeasuringPointId = scale.Id;
                                tr.MeasuringPointConfigId = scale.Id;
                                tr.TransactionNumber = Convert.ToInt64(row.TRS_NUM);
                                tr.RowId = -1;
                                tr.TransactionBeginTime = row.TRS_BGN_TIME;
                                tr.TransactionEndTime = row.TRS_END_TIME;
                                tr.ExciseStoreId = scale.ControlPoint;
                                tr.ZoneId = scale.ZoneId;
                                tr.BaseProductNumber = Convert.ToInt32(row.PRODUCT_ID);
                                tr.BaseProductName = row.PRODUCT_NAME;
                                tr.ProductNumber = Convert.ToInt32(row.PRODUCT_ID);
                                tr.ProductName = row.PRODUCT_NAME;
                                tr.ProductId = Convert.ToInt32(row.PRODUCT_ID);
                                tr.FlowDirection = row.DIRECTION==2?1:2;
                                tr.EngineeringUnitMass = string.Empty;
                                tr.EngineeringUnitVolume = string.Empty;
                                tr.EngineeringUnitDensity = string.Empty;
                                tr.EngineeringUnitTemperature = string.Empty;
                                if (row.DIRECTION == 2)
                                {
                                    // Output
                                    tr.TotalizerBeginMass = row.TOT_BGN_MASS_OUT;
                                    tr.TotalizerEndMass = row.TOT_MASS_OUT;
                                    tr.TotalizerBeginCommonMass = row.TOT_BGN_PROD_NET_MASS_OUT;
                                    tr.TotalizerEndCommonMass = row.TOT_PROD_NET_MASS_OUT;
                                }
                                else 
                                { 
                                    // Input
                                    tr.TotalizerBeginMass = row.TOT_BGN_MASS_IN;
                                    tr.TotalizerEndMass = row.TOT_MASS_IN;
                                    tr.TotalizerBeginCommonMass = row.TOT_BGN_PROD_NET_MASS_IN;
                                    tr.TotalizerEndCommonMass = row.TOT_PROD_NET_MASS_IN;
                                }

                                if (!row.IsQTY_NET_MASSNull())
                                {
                                    tr.Mass = row.QTY_NET_MASS;
                                }
                                
                                tr.InsertTimestamp = DateTime.Now;
                                context.MeasuringPointsConfigsDatas.Add(tr);
                                logger.InfoFormat("Successfully added transaction {0} from control point {1}", row.TRS_NUM, scale.ControlPoint);
                            }

                            using (var scope = new TransactionScope())
                            {
                                var status = context.SaveChanges("Scale2Sql");
                                var now = DateTime.Now;
                                max.LastFetchScales = now;
                                context.MaxAsoMeasuringPointDataSequenceNumberMap.Update(max);
                                context.SaveChanges("Scale2Sql");
                                scope.Complete();
                                logger.InfoFormat("Successfully sync {0} transactions between ASO.SCALE and Cpds", status.ResultRecordsCount);
                                logger.InfoFormat("Last scale transaction data-time updated to {0}.", now);
                            }

                        }
                    }
                }

                logger.Info("End scale synchronization");
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }

        private static MeasuringPointConfig GetMeasuringPointConfig(string scaleNumber,ProductionData context)
        {
 	        throw new NotImplementedException();
        }

    }
}