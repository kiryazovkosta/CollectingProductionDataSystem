using System;
using CollectingProductionDataSystem.Application.MailerService;
using CollectingProductionDataSystem.Data;
using CollectingProductionDataSystem.Data.Concrete;
using CollectingProductionDataSystem.Data.Contracts;
using CollectingProductionDataSystem.PhdApplication.Contracts;
using CollectingProductionDataSystem.PhdApplication.PrimaryDataServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ninject;
using log4net;

namespace CollectingProductionDataSystem.PhdApplication.Tests
{
    [TestClass]
    public class PhdApplicationTests
    {
        private readonly IPhdPrimaryDataService service;
        public PhdApplicationTests()
        {
            var moqObj = new Mock<IKernel>();
            this.service = new PhdPrimaryDataService(new ProductionData(new CollectingDataSystemDbContext()),
                LogManager.GetLogger("CollectingProductionDataSystem.Phd2SqlProductionData"), 
            new MailerService(LogManager.GetLogger("CollectingProductionDataSystem.Phd2SqlProductionData")));
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsFirstShift2016_01_01_12_59_59MustReturnNull()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 12, 59, 59);
            
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(null, actualShift);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsFirstShift2016_01_01_13_10_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 3, 17, 13, 10, 0);
            var expectedShiftId = 1;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsFirstShift2016_01_01_13_11_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 13, 11, 0);
            var expectedShiftId = 1;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);
            
            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsFirstShift2016_01_01_13_15_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 13, 15 , 0);
            var expectedShiftId = 1;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsFirstShift2016_01_01_13_20_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 13, 20, 0);
            var expectedShiftId = 1;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        //[TestMethod]
        //public void TestIfGetObservedShiftByDateTimeReturnsFirstShift2016_01_01_15_00_00()
        //{
        //    // Arrange
        //    var targetDateTime = new DateTime(2016, 1, 1, 15, 0, 0);
        //    var expectedShiftId = 1;
        //    // Act
        //    var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

        //    // Assert
        //    Assert.AreEqual(expectedShiftId, actualShift.Id);
        //}

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsFirstShift2016_01_01_13_25_01MustReturnNull()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 13, 25, 1);
           
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(null, actualShift);
        }
        //-------------------Second Shift
        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_21_09_59MustReturnNull()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 21, 9, 59);
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(null, actualShift);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_21_10_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 21, 10, 0);
            var expectedShiftId = 2;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_21_11_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 21, 11, 0);
            var expectedShiftId = 2;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_21_15_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 21, 15, 0);
            var expectedShiftId = 2;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        //[TestMethod]
        //public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_22_59_00()
        //{
        //    // Arrange
        //    var targetDateTime = new DateTime(2016, 1, 1, 22, 59, 0);
        //    var expectedShiftId = 2;
        //    // Act
        //    var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

        //    // Assert
        //    Assert.AreEqual(expectedShiftId, actualShift.Id);
        //}

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_21_20_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 21, 20, 0);
            var expectedShiftId = 2;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_21_25_01MustReturnNull()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 21, 25, 1);

            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(null, actualShift);
        }

        //-------------------Third Shift
        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsThirdShift2016_01_01_05_09_59МustReturnNull()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 5, 9, 59);
            
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(null, actualShift);
        }
        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsThirdShift2016_01_01_05_10_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 05, 10, 0);
            var expectedShiftId = 3;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsThirdShift2016_01_01_05_11_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 5, 11, 0);
            var expectedShiftId = 3;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsThirdShift2016_01_01_05_15_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 5, 15, 0);
            var expectedShiftId = 3;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsThirdShift2016_01_01_05_20_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 5, 20, 0);
            var expectedShiftId = 3;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        //[TestMethod]
        //public void TestIfGetObservedShiftByDateTimeReturnsThirdShift2016_01_01_07_9_59()
        //{
        //    // Arrange
        //    var targetDateTime = new DateTime(2016, 1, 1, 7, 9, 59);
        //    var expectedShiftId = 3;
        //    // Act
        //    var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

        //    // Assert
        //    Assert.AreEqual(expectedShiftId, actualShift.Id);
        //}

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsThirdShift2016_01_01_05_25_01MustReturnNull()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 5, 25, 1);
            
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(null, actualShift);
        }
    }
}
