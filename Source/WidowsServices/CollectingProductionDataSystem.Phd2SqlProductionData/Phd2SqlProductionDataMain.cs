namespace CollectingProductionDataSystem.Phd2SqlProductionData
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Net.Mail;
    using System.ServiceProcess;
    using CollectingProductionDataSystem.Data;
    using CollectingProductionDataSystem.Data.Concrete;
    using CollectingProductionDataSystem.Infrastructure.Log;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Transactions;
    using CollectingProductionDataSystem.Phd2SqlProductionData.Models;
    using CollectingProductionDataSystem.PhdApplication.PrimaryDataServices;
    using Uniformance.PHD;
    using log4net;
    using CollectingProductionDataSystem.Models.Productions;
    using System.Text;

    static class Phd2SqlProductionDataMain
    {
        private static readonly ILog logger;

        private static readonly NinjectConfig ninject;

        static Phd2SqlProductionDataMain()
        {
            logger = LogManager.GetLogger("CollectingProductionDataSystem.Phd2SqlProductionData");
            ninject = new NinjectConfig();

        }

        internal static void Main()
        {
            ServiceBase[] servicesToRun;
            servicesToRun = new ServiceBase[] 
            {
                new Phd2SqlProductionDataService(logger)
            };
            ServiceBase.Run(servicesToRun);
        }

        internal static void ProcessPrimaryProductionData(PrimaryDataSourceType dataSource)
        {
            try
            {
                logger.Info("Sync primary data started!");
                var kernel = ninject.Kernel;
                var service = kernel.GetService(typeof(PhdPrimaryDataService)) as PhdPrimaryDataService;
                for (int offsetInHours = 0; offsetInHours < Properties.Settings.Default.SYNC_PRIMARY_HOURS_OFFSET; offsetInHours += 8)
                {
                    var offset = offsetInHours == 0 ? offsetInHours : offsetInHours * -1;
                    var insertedRecords = service.ReadAndSaveUnitsDataForShift(DateTime.Now, offset, dataSource);
                    logger.InfoFormat("Successfully added {0} UnitsData records to CollectingPrimaryDataSystem", insertedRecords);
                    if (insertedRecords > 0)
                    {
                        SendEmail("SAPO - Shift data", string.Format("Successfully added {0} recorts to database", insertedRecords));
                    }
                }
                logger.Info("Sync primary data finished!");
            }
            catch (DataException validationException)
            {
                LogValidationDataException(validationException);
                SendEmail("SAPO - ERROR", validationException.Message);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                SendEmail("SAPO - ERROR", ex.Message);
            }
        }

        private static void LogValidationDataException(DataException validationException)
        {
            var dbEntityException = validationException.InnerException as DbEntityValidationException;
            if (dbEntityException != null)
            {
                foreach (var validationErrors in dbEntityException.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        logger.ErrorFormat("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    }
                }
            }
            else
            {
                logger.Error(validationException.ToString());
            }
        }

        internal static void ProcessInventoryTanksData()
        {
            try
            {
                logger.Info("Sync inventory tanks data started!");
                var now = DateTime.Now;

                using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister(), new Logger())))
                {
                    DateTime recordTime = new DateTime(now.Year, now.Month, now.Day, now.Hour, 0, 0);
                    var tanks = context.Tanks.All().Select(t => new Tank()
                    {
                        TankId = t.Id,
                        ParkId = t.ParkId,
                        ControlPoint = t.ControlPoint,
                        UnusableResidueLevel = 0.05m,
                        PhdTagProductId = t.PhdTagProductId,
                        PhdTagLiquidLevel = t.PhdTagLiquidLevel,
                        PhdTagProductLevel = t.PhdTagProductLevel,
                        PhdTagFreeWaterLevel = t.PhdTagFreeWaterLevel,
                        PhdTagReferenceDensity = t.PhdTagReferenceDensity,
                        PhdTagNetStandardVolume = t.PhdTagNetStandardVolume,
                        PhdTagWeightInAir = t.PhdTagWeightInAir,
                        PhdTagWeightInVaccum = t.PhdTagWeightInVacuum
                    });

                    // TODO: Mechanism to get all data for past period
                    for (int hours = Properties.Settings.Default.MIN_GET_INVENTORY_HOURS_INTERVAL; hours < Properties.Settings.Default.MAX_GET_INVENTORY_HOURS_INTERVAL; hours++)
                    {
                        var checkedDateTime = recordTime.AddHours(-hours);
                        var ts = recordTime - checkedDateTime;
                        var getRecordTimestamp = String.Format("NOW-{0}H{1}M", ts.TotalHours, now.Minute - 2);

                        if (context.TanksData.All().Where(td => td.RecordTimestamp == checkedDateTime).Any())
                        {
                            logger.InfoFormat("There are already a records for that time of the day: {0:yyyy-MM-dd HH:ss:mm}!", checkedDateTime);
                            continue;
                        }

                        if (tanks.Count() > 0)
                        {
                            using (PHDHistorian oPhd = new PHDHistorian())
                            {
                                using (PHDServer defaultServer = new PHDServer(Properties.Settings.Default.PHD_HOST))
                                {
                                    defaultServer.Port = Properties.Settings.Default.PHD_PORT;
                                    defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
                                    oPhd.DefaultServer = defaultServer;
                                    oPhd.StartTime = getRecordTimestamp;
                                    oPhd.EndTime = getRecordTimestamp;
                                    oPhd.Sampletype = SAMPLETYPE.Raw;
                                    oPhd.MinimumConfidence = 100;
                                    oPhd.MaximumRows = 1;

                                    var tanksDataList = new List<TankData>();

                                    foreach (var t in tanks)
                                    {
                                        try
                                        {
                                            var tankData = new TankData();
                                            tankData.RecordTimestamp = checkedDateTime;
                                            tankData.TankConfigId = t.TankId;
                                            tankData.ParkId = t.ParkId;
                                            tankData.NetStandardVolume = t.UnusableResidueLevel;

                                            Tags tags = new Tags();
                                            SetPhdTag(t.PhdTagProductId, tags);
                                            SetPhdTag(t.PhdTagLiquidLevel, tags);
                                            SetPhdTag(t.PhdTagProductLevel, tags);
                                            SetPhdTag(t.PhdTagFreeWaterLevel, tags);
                                            SetPhdTag(t.PhdTagReferenceDensity, tags);
                                            SetPhdTag(t.PhdTagNetStandardVolume, tags);
                                            SetPhdTag(t.PhdTagWeightInAir, tags);
                                            SetPhdTag(t.PhdTagWeightInVaccum, tags);

                                            DataSet dsGrid = oPhd.FetchRowData(tags);
                                            foreach (DataRow row in dsGrid.Tables[0].Rows)
                                            {
                                                var tagName = string.Empty;
                                                var tagValue = 0m;

                                                foreach (DataColumn dc in dsGrid.Tables[0].Columns)
                                                {
                                                    if (dc.ColumnName.Equals("Tolerance") || dc.ColumnName.Equals("HostName") || dc.ColumnName.Equals("Units"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (dc.ColumnName.Equals("Confidence") && !row[dc].ToString().Equals("100"))
                                                    {
                                                        continue;
                                                    }
                                                    else if (dc.ColumnName.Equals("TagName"))
                                                    {
                                                        tagName = row[dc].ToString();
                                                    }
                                                    else if (dc.ColumnName.Equals("Value"))
                                                    {
                                                        if (!row.IsNull("Value"))
                                                        {
                                                            tagValue = Convert.ToDecimal(row[dc]);
                                                        }
                                                    }
                                                }

                                                if (tagName.Contains(".PROD_ID"))
                                                {
                                                    var prId = Convert.ToInt32(tagValue);
                                                    var product = context.TankMasterProducts.All().Where(x => x.TankMasterProductCode == prId).FirstOrDefault();
                                                    if (product != null)
                                                    {
                                                        tankData.ProductId = product.Id;
                                                        var pr = context.Products.All().Where(p => p.Id == product.Id).FirstOrDefault();
                                                        if (pr != null)
                                                        {
                                                            tankData.ProductName = pr.Name;
                                                        }
                                                        else
                                                        {
                                                            tankData.ProductName = "N/A";
                                                            SendEmail("Phd2Sql Inventory", "There is a product with unknown id in TankMaster configuration");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        tankData.ProductId = Convert.ToInt32(tagValue);
                                                        tankData.ProductName = "N/A";
                                                        SendEmail("Phd2Sql Inventory", "There is a product with unknown id in TankMaster configuration");
                                                    }
                                                }
                                                else if (tagName.EndsWith(".LL") || tagName.EndsWith(".LEVEL_MM"))
                                                {
                                                    tankData.LiquidLevel = tagValue;
                                                }
                                                else if (tagName.EndsWith(".LL_FWL") || tagName.EndsWith(".LEVEL_FWL"))
                                                {
                                                    tankData.ProductLevel = tagValue;
                                                }
                                                else if (tagName.EndsWith(".FWL"))
                                                {
                                                    tankData.FreeWaterLevel = tagValue;
                                                }
                                                else if (tagName.EndsWith(".DREF") || tagName.EndsWith(".REF_DENS"))
                                                {
                                                    tankData.ReferenceDensity = tagValue;
                                                }
                                                else if (tagName.EndsWith(".NSV"))
                                                {
                                                    tankData.NetStandardVolume = tagValue;
                                                }
                                                else if (tagName.EndsWith(".WIA"))
                                                {
                                                    tankData.WeightInAir = tagValue;
                                                }
                                                else if (tagName.EndsWith(".WIV"))
                                                {
                                                    tankData.WeightInVacuum = tagValue;
                                                }
                                            }

                                            tanksDataList.Add(tankData);
                                        }
                                        catch (Exception ex)
                                        {
                                            logger.ErrorFormat("Tank Id [{0}] Exception:\n\n\n", t.TankId, ex.ToString());
                                        }
                                    }

                                    if (tanksDataList.Count > 0)
                                    {
                                        context.TanksData.BulkInsert(tanksDataList, "Phd2SqlLoading");
                                        context.SaveChanges("Phd2SqlLoading");
                                        logger.InfoFormat("Successfully added {0} TanksData records for: {1:yyyy-MM-dd HH:ss:mm}!", tanksDataList.Count, checkedDateTime);
                                    }
                                }
                            }
                        }
                    }

                    logger.Info("Sync inventory tanks data finished!");
                }
            }
            catch (DataException validationException)
            {
                LogValidationDataException(validationException);
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
            }
        }

        internal static void ProcessMeasuringPointsData()
        {
            //try
            //{
            //    logger.Info("Sync measurements points data started!");
            //    //var now = DateTime.Now;
            //    //var today = DateTime.Today;
            //    //var fiveOClock = today.AddHours(5);

            //    //var ts = now - fiveOClock;
            //    //if (ts.TotalMinutes > 2 && ts.Hours == 0)
            //    {
            //        using (var context = new ProductionData(new CollectingDataSystemDbContext(new AuditablePersister(),new Logger())))
            //        {
            //            var measuringPoints = context.MeasuringPointConfigs.All().Where(x => !string.IsNullOrEmpty(x.TotalizerCurrentValueTag));

            //            if (measuringPoints.Count() > 0)
            //            {
            //                using(PHDHistorian oPhd = new PHDHistorian())
            //                { 
            //                    using(PHDServer defaultServer = new PHDServer("srv-vm-mes-phd"))
            //                    {
            //                        defaultServer.Port = 3150;
            //                        defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
            //                        oPhd.DefaultServer = defaultServer;
            //                        oPhd.StartTime = "NOW - 2M";
            //                        oPhd.EndTime = "NOW - 2M";
            //                        oPhd.Sampletype = SAMPLETYPE.Snapshot;
            //                        oPhd.MinimumConfidence = 49;
            //                        oPhd.ReductionType = REDUCTIONTYPE.Average;
            //                        oPhd.MaximumRows = 1;

            //                        foreach (var item in measuringPoints)
            //                        {
            //                            var measurementPointData = new MeasuringPointProductsData();
            //                            measurementPointData.MeasuringPointConfigId = item.Id;
            //                            DataSet dsGrid = oPhd.FetchRowData(item.TotalizerCurrentValueTag);
            //                            foreach (DataRow row in dsGrid.Tables[0].Rows)
            //                            {
            //                                foreach (DataColumn dc in dsGrid.Tables[0].Columns)
            //                                {
            //                                    if (dc.ColumnName.Equals("Tolerance") || dc.ColumnName.Equals("HostName"))
            //                                    {
            //                                        continue;
            //                                    }
            //                                    else if (dc.ColumnName.Equals("Value"))
            //                                    {
            //                                        if (!string.IsNullOrWhiteSpace(row[dc].ToString()))
            //                                        {
            //                                            measurementPointData.Value = Convert.ToDecimal(row[dc]);
            //                                            logger.InfoFormat("Value: {0}", measurementPointData.Value ?? 0);
            //                                        }
            //                                    }
            //                                }
            //                            }
            //                        }
            //                    }
            //                }


            //                //using (PHDHistorian oPhd = new PHDHistorian())
            //                //{
            //                //    using (PHDServer oPhd = new PHDServer("srv-vm-mes-phd""))
            //                //    {
            //                //        oPhd.Port = 3150;
            //                //        defaultServer.APIVersion = Uniformance.PHD.SERVERVERSION.RAPI200;
            //                //        oPhd.DefaultServer = defaultServer;
            //                //        oPhd.StartTime = "NOW - 2M";
            //                //        oPhd.EndTime = "NOW - 2M";
            //                //        oPhd.Sampletype = SAMPLETYPE.Snapshot;
            //                //        oPhd.MinimumConfidence = 49;
            //                //        oPhd.ReductionType = REDUCTIONTYPE.Average;
            //                //        oPhd.MaximumRows = 1;

            //                //        foreach (var item in measuringPoints)
            //                //        {
            //                //            var measurementPointData = new MeasuringPointProductsData();
            //                //            measurementPointData.MeasuringPointConfigId = item.Id;
            //                //            DataSet dsGrid = oPhd.FetchRowData(item.TotalizerCurrentValueTag);
            //                //            var confidence = 100;
            //                //            foreach (DataRow row in dsGrid.Tables[0].Rows)
            //                //            {
            //                //                foreach (DataColumn dc in dsGrid.Tables[0].Columns)
            //                //                {
            //                //                    if (dc.ColumnName.Equals("Tolerance") || dc.ColumnName.Equals("HostName"))
            //                //                    {
            //                //                        continue;
            //                //                    }
            //                //                    else if (dc.ColumnName.Equals("Confidence"))
            //                //                    {
            //                //                        if (!string.IsNullOrWhiteSpace(row[dc].ToString()))
            //                //                        {
            //                //                            confidence = Convert.ToInt32(row[dc]);
            //                //                        }
            //                //                        else
            //                //                        {
            //                //                            confidence = 0;
            //                //                            break;
            //                //                        }
            //                //                    }
            //                //                    else if (dc.ColumnName.Equals("Value"))
            //                //                    {
            //                //                        if (!string.IsNullOrWhiteSpace(row[dc].ToString()))
            //                //                        {
            //                //                            measurementPointData.Value = Convert.ToDecimal(row[dc]);
            //                //                        }
            //                //                    }
            //                //                }
            //                //            }
            //                //            if (confidence > Properties.Settings.Default.INSPECTION_DATA_MINIMUM_CONFIDENCE &&
            //                //                measurementPointData.RecordTimestamp != null)
            //                //            {
            //                //                measurementPointData.RecordTimestamp = currentDate;
            //                //                context.MeasurementPointsProductsDatas.Add(measurementPointData);
            //                //            }
            //                //        }

            //                //        //context.SaveChanges("Phd2Sql");
            //                //    }
            //                //}
            //            }
            //        }
            //    }

            //    logger.Info("Sync measurements points data finished!");
            //}
            //catch (DataException validationException)
            //{
            //    LogValidationDataException(validationException);
            //}
            //catch (Exception ex)
            //{
            //    logger.Error(ex);
            //}
        }

        private static void SetPhdTag(string tagName, Tags tags)
        {
            if (!string.IsNullOrEmpty(tagName))
            {
                tags.Add(new Tag(tagName));
            }
        }

        private static void SendEmail(string titleParam, string bodyParam)
        {
            string to = Properties.Settings.Default.SMTP_TO;
            string from = Properties.Settings.Default.SMTP_FROM;
            MailMessage message = new MailMessage(from, to);
            message.Subject = @titleParam;
            message.Body = @bodyParam;
            SmtpClient client = new SmtpClient(Properties.Settings.Default.SMTP_SERVER);
            client.UseDefaultCredentials = true;

            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message, ex);
            }
        }
    }
}