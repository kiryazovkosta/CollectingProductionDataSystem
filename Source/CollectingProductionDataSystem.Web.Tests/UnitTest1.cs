using System;
using AutoMapper;
using CollectingProductionDataSystem.Models.Identity;
using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CollectingProductionDataSystem.Web.Tests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void CheckMappinRoleViewModelToApplicationRole()
        {
            Mapper.CreateMap<ApplicationUser,EditUserViewModel>();

            Mapper.AssertConfigurationIsValid();
        }
    }
}
