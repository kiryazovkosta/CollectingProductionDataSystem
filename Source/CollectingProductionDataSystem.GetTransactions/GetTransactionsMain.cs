namespace CollectingProductionDataSystem.GetTransactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceProcess;
    using System.Text;
    using System.Threading.Tasks;
    using CollectingProductionDataSystem.Data;
    using log4net;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Models.Transactions;

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

                            var status = context.SaveChanges("System Loading");
                            logger.InfoFormat("Successfully synchronization {0} records from Aso to Cpds", status.ResultRecordsCount);
                            long maxValue = Convert.ToInt64(table.Compute("max(SequenceNumber)", string.Empty));
                            logger.Info(maxValue);
                            max.MaxSequenceNumber = maxValue;
                            context.MaxAsoMeasuringPointDataSequenceNumberMap.Update(max);
                            context.SaveChanges("ASO2SQL");
                            logger.InfoFormat("Maximum sequence number updated to {0}.", maxValue);
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
                    var scaleAdapter = new ScaleDataSetTableAdapters.ScaleDataTableAdapter();
                    var scaleTable = new ScaleDataSet.ScaleDataDataTable();
                    scaleAdapter.Fill(scaleTable, DateTime.Now.AddDays(-5));
                    foreach (ScaleDataSet.ScaleDataRow row in scaleTable.Rows)
                    {
                        //var tr = new MeasuringPointsConfigsData();
                        //    tr.MeasuringPointId = row.MeasuringPointId;
                        //    tr.TransactionNumber = Convert.ToInt64(row.TRS_NUM);
                        //    tr.RowId = -1;
                        //    tr.TransactionBeginTime = row.TRS_BGN_TIME;
                        //    tr.TransactionEndTime = row.TRS_END_TIME;
                        //    tr.ExciseStoreId = row.ExciseStoreId;
                        //    tr.ZoneId = row.ZoneId;
                        //    tr.BaseProductNumber = Convert.ToInt32(row.PRODUCT_ID);
                        //    tr.BaseProductName = row.PRODUCT_NAME;
                        //    tr.ProductNumber = Convert.ToInt32(row.PRODUCT_ID);
                        //    tr.ProductName = row.PRODUCT_NAME;
                        //    tr.FlowDirection = row.DIRECTION;
                        //    tr.EngineeringUnitMass = string.Empty;
                        //    tr.EngineeringUnitVolume = string.Empty;
                        //    tr.EngineeringUnitDensity = string.Empty;
                        //    tr.EngineeringUnitTemperature = string.Empty;
                                
                        //    if (!row.Is)
                        //    {
                        //        tr.TotalizerBeginMass = row.TotalizerBeginMass;
                        //    }
                        //    if (!row.IsTotalizerEndMassNull())
                        //    {
                        //        tr.TotalizerEndMass = row.TotalizerEndMass;
                        //    }

                        //    if (!row.IsTotalizerBeginCommonMassNull())
                        //    {
                        //        tr.TotalizerBeginCommonMass = row.TotalizerBeginCommonMass;
                        //    }
                        //    if (!row.IsTotalizerEndCommonMassNull())
                        //    {
                        //        tr.TotalizerEndCommonMass = row.TotalizerEndCommonMass;
                        //    }

                        //    if (!row.IsQTY_NET_MASSNull())
                        //    {
                        //        tr.Mass = row.QTY_NET_MASS;
                        //    }
                                
                        //    tr.InsertTimestamp = row.InsertTimestamp;

                        //    context.MeasuringPointsConfigsDatas.Add(tr);
                    }
                }

                logger.Info("End scale synchronization");
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
    }
}