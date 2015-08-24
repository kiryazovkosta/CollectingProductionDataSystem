﻿using System;
using System.Data.Entity;
using System.Linq;
using System.Transactions;
using CollectingProductionDataSystem.Data;
using CollectingProductionDataSystem.Models.Inventories;
using CollectingProductionDataSystem.Models.Nomenclatures;
using CollectingProductionDataSystem.Models.UtilityEntities;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CollectingProductionDataSystem.Data.Tests
{
    [TestClass]
    public class SaveChangesTests
    {
        public CollectingDataSystemDbContext dbContext { get; set; }
        private static TransactionScope tranScope;

        public SaveChangesTests()
        {
            this.dbContext = new CollectingDataSystemDbContext();
        }

        [TestInitialize]
        public void InitializeTest()
        {
            tranScope = new TransactionScope();
        }

        [TestCleanup]
        public void TestTearDown()
        {
            tranScope.Dispose();
        }

        [TestMethod]
        public void TestIfAddingNewRecordCouseAddingLogRecord()
        {
            //Arrange
            var auditRecordsCount = this.dbContext.Set<AuditLogRecord>().CountAsync().Result;

            //Act
            var newProduct = new Product { Name = "Test Product", ProductTypeId = 1 };
            this.dbContext.Products.Add(newProduct);
            this.dbContext.SaveChanges("TestUser");
            var auditRecord = this.dbContext.AuditLogRecords.FirstOrDefault(x => x.EntityName == "Product" && x.EntityId == newProduct.Id && x.OperationType == EntityState.Added);

            //Assert
            var actualRecordCount = this.dbContext.Set<AuditLogRecord>().CountAsync().Result;
            var expectedRecordsCount = auditRecordsCount + 1;
            Assert.AreEqual(expectedRecordsCount, actualRecordCount);
            Assert.AreEqual(EntityState.Added, auditRecord.OperationType, "Invalid Operation Type");
            Assert.AreEqual(auditRecord.UserName, "TestUser");
        }

        [TestMethod]
        public void TestIfDeletingARecordCouseAddingLogRecord()
        {
            //Arrange
            //Arrange
            var deletedRecord = new Product { Name = "Test Product", ProductTypeId = 1 };
            this.dbContext.Products.Add(deletedRecord);
            this.dbContext.SaveChanges("AnotherTestUser");
            //remove Log record for create operation
            var auditLogRecord = this.dbContext.AuditLogRecords.FirstOrDefault(x => x.EntityName == "Product" && x.EntityId == deletedRecord.Id && x.OperationType == EntityState.Added);
            this.dbContext.AuditLogRecords.Remove(auditLogRecord);
            this.dbContext.SaveChanges();

            var auditRecordsCount = this.dbContext.Set<AuditLogRecord>().CountAsync().Result;

            //Act
            this.dbContext.Products.Remove(deletedRecord);
            this.dbContext.SaveChanges("TestUser");
            var auditRecord = this.dbContext.AuditLogRecords.FirstOrDefault(x => x.EntityName == "Product" && x.EntityId == deletedRecord.Id);

            //Assert
            var actualRecordCount = this.dbContext.Set<AuditLogRecord>().CountAsync().Result;
            var expectedRecordsCount = auditRecordsCount + 1;
            Assert.AreEqual(expectedRecordsCount, actualRecordCount);
            Assert.AreEqual(EntityState.Deleted, auditRecord.OperationType, "Invalid Operation Type");
            Assert.AreEqual(auditRecord.UserName, "TestUser");
        }

        [TestMethod]
        public void TestIfModifingARecordCouseAddingLogRecords()
        {
            //Arrange
            var modifiedRecord = new Product { Name = "Test Product", ProductTypeId = 1, Code = 0 };
            this.dbContext.Products.Add(modifiedRecord);
            this.dbContext.SaveChanges("AnotherTestUser");
            //remove Log record for create operation
            var auditLogRecord = this.dbContext.AuditLogRecords.FirstOrDefault(x => x.EntityName == "Product" && x.EntityId == modifiedRecord.Id && x.OperationType == EntityState.Added);
            this.dbContext.AuditLogRecords.Remove(auditLogRecord);
            this.dbContext.SaveChanges();

            var auditRecordsCount = this.dbContext.Set<AuditLogRecord>().CountAsync().Result;

            //Act
            modifiedRecord.Name = "Modified";
            modifiedRecord.Code = 5000;
            this.dbContext.SaveChanges("TestUser");
            var auditRecords = this.dbContext.AuditLogRecords.Where(x => x.EntityName == "Product" && x.EntityId == modifiedRecord.Id).ToList();

            //Assert
            var actualRecordCount = this.dbContext.Set<AuditLogRecord>().CountAsync().Result;
            var expectedRecordsCount = auditRecordsCount + 2;
            Assert.AreEqual(expectedRecordsCount, actualRecordCount);
            foreach (var record in auditRecords)
            {
                Assert.AreEqual(EntityState.Modified, record.OperationType, "Invalid Operation Type");
                Assert.AreEqual(record.UserName, "TestUser");
            }

            var nameResord = auditRecords.FirstOrDefault(x => x.FieldName == "Name");
            var codeRecord = auditRecords.FirstOrDefault(x => x.FieldName == "Code");
            Assert.AreEqual("Test Product", nameResord.OldValue);
            Assert.AreEqual("Modified", nameResord.NewValue);
            Assert.AreEqual("0", codeRecord.OldValue);
            Assert.AreEqual("5000", codeRecord.NewValue);
        }

        [TestMethod]
        public void TestIfModifingAnPreviouseEnptyPropertyDoesNotCauseException()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            //Arrange
            var modifiedRecord = new TankData { RecordTimestamp = DateTime.Now, ParkId = 1, ProductId = 1, TankConfigId = 1, LiquidLevel = null };
            this.dbContext.TanksData.Add(modifiedRecord);
            this.dbContext.SaveChanges("AnotherTestUser");
            //remove Log record for create operation
            var auditLogRecord = this.dbContext.AuditLogRecords.FirstOrDefault(x => x.EntityName == "TankData" && x.EntityId == modifiedRecord.Id && x.OperationType == EntityState.Added);
            this.dbContext.AuditLogRecords.Remove(auditLogRecord);
            this.dbContext.SaveChanges();

            var auditRecordsCount = this.dbContext.Set<AuditLogRecord>().CountAsync().Result;

            //Act
            modifiedRecord.LiquidLevel = 1.2M;
            this.dbContext.SaveChanges("TestUser");
            var auditRecords = this.dbContext.AuditLogRecords.FirstOrDefault(x => x.EntityName == "TankData" && x.EntityId == modifiedRecord.Id);

            //Assert
            var actualRecordCount = this.dbContext.Set<AuditLogRecord>().CountAsync().Result;
            var expectedRecordsCount = auditRecordsCount + 1;
            Assert.AreEqual(expectedRecordsCount, actualRecordCount, "Audit records count is wrong! ");

            Assert.AreEqual(EntityState.Modified, auditRecords.OperationType, "Invalid Operation Type");
            Assert.AreEqual(auditRecords.UserName, "TestUser");


            Assert.AreEqual("", auditRecords.OldValue);
            Assert.AreEqual("1.2", auditRecords.NewValue);
        }
    }
}