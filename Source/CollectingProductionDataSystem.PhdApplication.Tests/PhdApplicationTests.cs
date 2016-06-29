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
    public class PhdApplicationTests:IDisposable
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
        public void TestIfGetObservedShiftByDateTimeReturnsFirstShift2016_01_01_13_16_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 3, 17, 13, 16, 0);
            var expectedShiftId = 1;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsFirstShift2016_01_01_13_17_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 13, 17, 0);
            var expectedShiftId = 1;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsFirstShift2016_01_01_13_18_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 13, 18, 0);
            var expectedShiftId = 1;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsNullShift2016_01_01_13_19_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 13, 19, 0);
            var expectedShiftId = 1;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.IsNull(actualShift);
        }

        //-------------------Second Shift
        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_21_15_59MustReturnNull()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 21, 15, 59);
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(null, actualShift);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_21_16_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 21, 16, 0);
            var expectedShiftId = 2;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_21_17_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 21, 17, 0);
            var expectedShiftId = 2;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_21_18_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 21, 18, 0);
            var expectedShiftId = 2;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }


        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondNullShift2016_01_01_21_19_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 21, 19, 0);
            var expectedShiftId = 2;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.IsNull(actualShift);
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
        public void TestIfGetObservedShiftByDateTimeReturnsThirdShift2016_01_01_05_16_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 05, 16, 0);
            var expectedShiftId = 3;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsThirdShift2016_01_01_17_20_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 5, 17, 0);
            var expectedShiftId = 3;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsThirdShift2016_01_01_05_18_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 5, 18, 0);
            var expectedShiftId = 3;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsNullShift2016_01_01_05_19_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 5, 19, 0);
            var expectedShiftId = 0;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.IsNull(actualShift);
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

        public void Dispose()
        {
            this.service.Dispose();
        }
    }
}
