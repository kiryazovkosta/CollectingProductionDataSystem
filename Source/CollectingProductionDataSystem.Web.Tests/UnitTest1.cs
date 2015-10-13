using System;
using AutoMapper;
using CollectingProductionDataSystem.Models.Identity;
using CollectingProductionDataSystem.Models.Nomenclatures;
using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CollectingProductionDataSystem.Web.Tests
{
    [TestClass]
    public class UnitTest1
    {

        [TestMethod]
        public void CheckMappinRoleViewModelToApplicationRole()
        {
            Mapper.CreateMap<ShiftViewModel,Shift>();

            var map = new ShiftViewModel();
                map.CreateMappings(Mapper.Configuration);

            Mapper.AssertConfigurationIsValid();
        }
    }
}
