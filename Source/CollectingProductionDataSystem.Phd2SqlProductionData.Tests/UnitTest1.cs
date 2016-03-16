using System;
using System.Collections.Generic;
using CollectingProductionDataSystem.Models.Nomenclatures;
using CollectingProductionDataSystem.Phd2SqlProductionData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using log4net;

namespace CollectingProductionDataSystem.Phd2SqlProductionData.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private readonly Phd2SqlProductionDataService service;
        private readonly List<Shift> shifts;
        public UnitTest1() 
        {
            service = new Phd2SqlProductionDataService(LogManager.GetLogger("CollectingProductionDataSystem.Phd2SqlProductionData"));
            shifts = CreateShifts();
        }
 
        /// <summary>
        /// Creates the shifts.
        /// </summary>
        /// <returns></returns>
        private List<Shift> CreateShifts()
        {
            return new List<Shift>
            {
                new Shift() { Id = 1, Name = "Първа смяна", BeginTicks = 180600000000, ReadOffsetTicks = 474000000000, ReadPollTimeSlotTicks = 6000000000, EndTicks= 474000000000 , ShiftDurationTicks=288000000000},
                new Shift() { Id = 2, Name = "Втора смяна", BeginTicks = 468600000000, ReadOffsetTicks = 762000000000, ReadPollTimeSlotTicks = 6000000000 , EndTicks= 762000000000 , ShiftDurationTicks=288000000000},
                new Shift() { Id = 3, Name = "Трета смяна", BeginTicks = -107400000000, ReadOffsetTicks = 186000000000, ReadPollTimeSlotTicks = 6000000000 , EndTicks= 1050000000000 , ShiftDurationTicks=288000000000},
            };
        }

        [TestMethod]
        public void CheckThatIfThereIs_13_10_0_FirstShiftPollTimeSlotForceFlagIsFalse()
        {
            // Arrange 
            var targetDate = new DateTime(2016, 01, 01, 13, 10, 0);
            var targetShift = shifts[0]; //FirstShift
            // Act  
            var actual = service.CheckIfForcedCalculationNeeded(targetDate, targetShift);

            // Assert 
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CheckThatIfThereIs_13_18_0_FirstShiftPollTimeSlotForceFlagIsFalse()
        {
            // Arrange 
            var targetDate = new DateTime(2016, 01, 01, 13, 18, 0);
            var targetShift = shifts[0]; //FirstShift
            // Act  
            var actual = service.CheckIfForcedCalculationNeeded(targetDate, targetShift);

            // Assert 
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CheckThatIfThereIsOneMinuteTillTheEndOfFirstShiftPollTimeSlotForceFlagIsTrue()
        {
            // Arrange 
            var targetDate = new DateTime(2016, 01, 01, 13, 19, 0);
                var targetShift = shifts[0]; //FirstShift
            // Act  
            var actual = service.CheckIfForcedCalculationNeeded(targetDate,targetShift);
            
            // Assert 
            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void CheckThatIfThereIs_21_10_0_SecondShiftPollTimeSlotForceFlagIsFalse()
        {
            // Arrange 
            var targetDate = new DateTime(2016, 01, 01, 21, 10, 0);
            var targetShift = shifts[1]; //FirstShift
            // Act  
            var actual = service.CheckIfForcedCalculationNeeded(targetDate, targetShift);

            // Assert 
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CheckThatIfThereIs_21_18_0_FirstShiftPollTimeSlotForceFlagIsFalse()
        {
            // Arrange 
            var targetDate = new DateTime(2016, 01, 01, 21, 18, 0);
            var targetShift = shifts[1]; //FirstShift
            // Act  
            var actual = service.CheckIfForcedCalculationNeeded(targetDate, targetShift);

            // Assert 
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CheckThatIfThereIsOneMinuteTillTheEndOfSecondShiftPollTimeSlotForceFlagIsTrue()
        {
            // Arrange 
            var targetDate = new DateTime(2016, 01, 01, 21, 19, 0);
            var targetShift = shifts[1]; //SecondShift
            // Act  
            var actual = service.CheckIfForcedCalculationNeeded(targetDate, targetShift);

            // Assert 
            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void CheckThatIfThereIs_05_10_0_ThirdShiftPollTimeSlotForceFlagIsFalse()
        {
            // Arrange 
            var targetDate = new DateTime(2016, 01, 01, 5, 10, 0);
            var targetShift = shifts[0]; //FirstShift
            // Act  
            var actual = service.CheckIfForcedCalculationNeeded(targetDate, targetShift);

            // Assert 
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CheckThatIfThereIs_13_18_0_ThirdShiftPollTimeSlotForceFlagIsFalse()
        {
            // Arrange 
            var targetDate = new DateTime(2016, 01, 01, 5, 18, 0);
            var targetShift = shifts[0]; //FirstShift
            // Act  
            var actual = service.CheckIfForcedCalculationNeeded(targetDate, targetShift);

            // Assert 
            Assert.AreEqual(false, actual);
        }

        [TestMethod]
        public void CheckThatIfThereIsOneMinuteTillTheEndOfThirdShiftPollTimeSlotForceFlagIsTrue()
        {
            // Arrange 
            var targetDate = new DateTime(2016, 01, 01, 5, 19, 0);
            var targetShift = shifts[2]; //ThirdShift
            // Act  
            var actual = service.CheckIfForcedCalculationNeeded(targetDate, targetShift);

            // Assert 
            Assert.AreEqual(true, actual);
        }
    }
}
