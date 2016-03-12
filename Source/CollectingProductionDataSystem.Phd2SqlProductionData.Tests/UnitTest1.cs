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
                new Shift() { Id = 1, Name = "Първа смяна", BeginTicks = 180600000000, ReadOffsetTicks = 468000000000, ReadPollTimeSlotTicks = 72000000000 },
                new Shift() { Id = 2, Name = "Втора смяна", BeginTicks = 468600000000, ReadOffsetTicks = 756000000000, ReadPollTimeSlotTicks = 72000000000 },
                new Shift() { Id = 3, Name = "Трета смяна", BeginTicks = -107400000000, ReadOffsetTicks = 180000000000, ReadPollTimeSlotTicks = 72000000000 },
            };
        }

        [TestMethod]
        public void CheckThatIfThereIsOneMinuteTillTheEndOfFirstShiftPollTimeSlotForceFlagIsTrue()
        {
            // Arrange 
            var targetDate = new DateTime(2016, 01, 01, 14, 58, 0);
                var targetShift = shifts[0]; //FirstShift
            // Act  
            var actual = service.CheckIfForcedCalculationNeeded(targetDate,targetShift);
            
            // Assert 
            Assert.AreEqual(true, actual);
        }

        [TestMethod]
        public void CheckThatIfThereIsOneMinuteTillTheEndOfSecondShiftPollTimeSlotForceFlagIsTrue()
        {
            // Arrange 
            var targetDate = new DateTime(2016, 01, 01, 22, 58, 0);
            var targetShift = shifts[1]; //SecondShift
            // Act  
            var actual = service.CheckIfForcedCalculationNeeded(targetDate, targetShift);

            // Assert 
            Assert.AreEqual(true, actual);
        }
        [TestMethod]
        public void CheckThatIfThereIsOneMinuteTillTheEndOfThirdShiftPollTimeSlotForceFlagIsTrue()
        {
            // Arrange 
            var targetDate = new DateTime(2016, 01, 01, 6, 58, 0);
            var targetShift = shifts[2]; //ThirdShift
            // Act  
            var actual = service.CheckIfForcedCalculationNeeded(targetDate, targetShift);

            // Assert 
            Assert.AreEqual(true, actual);
        }
    }
}
