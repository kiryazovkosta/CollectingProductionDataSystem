using System;
using CollectingProductionDataSystem.Data;
using CollectingProductionDataSystem.Data.Concrete;
using CollectingProductionDataSystem.PhdApplication.Contracts;
using CollectingProductionDataSystem.PhdApplication.PrimaryDataServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net;

namespace CollectingProductionDataSystem.PhdApplication.Tests
{
    [TestClass]
    public class PhdApplicationTests
    {
        private readonly IPrimaryDataService service;
        public PhdApplicationTests()
        {
            this.service = new PhdPrimaryDataService(new ProductionData(new CollectingDataSystemDbContext()), LogManager.GetLogger("CollectingProductionDataSystem.Phd2SqlProductionData"));
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
        public void TestIfGetObservedShiftByDateTimeReturnsFirstShift2016_01_01_13_00_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 13, 0, 0);
            var expectedShiftId = 1;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsFirstShift2016_01_01_13_01_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 13, 1, 0);
            var expectedShiftId = 1;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);
            
            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsFirstShift2016_01_01_14_00_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 14, 0, 0);
            var expectedShiftId = 1;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsFirstShift2016_01_01_14_59_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 14, 59, 0);
            var expectedShiftId = 1;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsFirstShift2016_01_01_15_00_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 15, 0, 0);
            var expectedShiftId = 1;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsFirstShift2016_01_01_15_01_00MustReturnNull()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 15, 0, 1);
           
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(null, actualShift);
        }
        //-------------------Second Shift
        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_20_59_59MustReturnNull()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 20, 59, 59);
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(null, actualShift);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_21_00_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 21, 0, 0);
            var expectedShiftId = 2;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_21_01_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 21, 1, 0);
            var expectedShiftId = 2;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_22_00_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 22, 0, 0);
            var expectedShiftId = 2;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_22_59_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 22, 59, 0);
            var expectedShiftId = 2;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_23_00_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 23, 0, 0);
            var expectedShiftId = 2;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsSecondShift2016_01_01_23_00_01MustReturnNull()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 23, 0, 1);

            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(null, actualShift);
        }

        //-------------------Third Shift
        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsThirdShift2016_01_01_04_59_59МustReturnNull()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 4, 59, 59);
            
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(null, actualShift);
        }
        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsThirdShift2016_01_01_05_00_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 05, 0, 0);
            var expectedShiftId = 3;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsThirdShift2016_01_01_05_01_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 5, 1, 0);
            var expectedShiftId = 3;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsThirdShift2016_01_01_06_00_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 6, 0, 0);
            var expectedShiftId = 3;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsThirdShift2016_01_01_06_59_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 6, 59, 0);
            var expectedShiftId = 3;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsThirdShift2016_01_01_07_00_00()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 7, 0, 0);
            var expectedShiftId = 3;
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(expectedShiftId, actualShift.Id);
        }

        [TestMethod]
        public void TestIfGetObservedShiftByDateTimeReturnsThirdShift2016_01_01_07_00_01MustReturnNull()
        {
            // Arrange
            var targetDateTime = new DateTime(2016, 1, 1, 7, 0, 1);
            
            // Act
            var actualShift = this.service.GetObservedShiftByDateTime(targetDateTime);

            // Assert
            Assert.AreEqual(null, actualShift);
        }
    }
}
