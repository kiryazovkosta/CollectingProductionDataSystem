using System;
using CollectingProductionDataSystem.Models.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CollectingProductionDataSystem.Models.Tests
{
    [TestClass]
    public class ApplicationUserTests
    {
        [TestMethod]
        public void CheckIfFullNameIsOkWith_FirstName_MiddleName_LastName()
        {
            //Arrange
            var user = new ApplicationUser { FirstName = "First", MiddleName = "Middle", LastName = "Last" };
            var expected = "First Middle Last";
            //Act
            var actual = user.FullName;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIfFullNameIsOkWith_FirstName_Space_LastName()
        {
            //Arrange
            var user = new ApplicationUser { FirstName = "First", MiddleName = " ", LastName = "Last" };
            var expected = "First Last";
            //Act
            var actual = user.FullName;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIfFullNameIsOkWith_FirstName_Null_LastName()
        {
            //Arrange
            var user = new ApplicationUser { FirstName = "First", MiddleName = null, LastName = "Last" };
            var expected = "First Last";
            //Act
            var actual = user.FullName;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIfFullNameIsOkWith_FirstName_Middle_Space()
        {
            //Arrange
            var user = new ApplicationUser { FirstName = "First", MiddleName = "Middle", LastName = " " };
            var expected = "First Middle";
            //Act
            var actual = user.FullName;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIfFullNameIsOkWith_FirstName_Middle_Null()
        {
            //Arrange
            var user = new ApplicationUser { FirstName = "First", MiddleName = "Middle", LastName = null };
            var expected = "First Middle";
            //Act
            var actual = user.FullName;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIfFullNameIsOkWith_FirstName_Space_Space()
        {
            //Arrange
            var user = new ApplicationUser { FirstName = "First", MiddleName = " ", LastName = " " };
            var expected = "First";
            //Act
            var actual = user.FullName;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIfFullNameIsOkWith_FirstName_Null_Null()
        {
            //Arrange
            var user = new ApplicationUser { FirstName = "First", MiddleName = null, LastName = null };
            var expected = "First";
            //Act
            var actual = user.FullName;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIfFullNameIsOkWith_Space_Space_Space_Return_UserName()
        {
            //Arrange
            var user = new ApplicationUser { UserName = "UserName", FirstName = " ", MiddleName = " ", LastName = " " };
            var expected = "UserName";
            //Act
            var actual = user.FullName;
            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CheckIfFullNameIsOkWith_Null_Null_NullReturn_UserName()
        {
            //Arrange
            var user = new ApplicationUser { UserName = "UserName", FirstName = null, MiddleName = null, LastName = null };
            var expected = "UserName";
            //Act
            var actual = user.FullName;
            //Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
