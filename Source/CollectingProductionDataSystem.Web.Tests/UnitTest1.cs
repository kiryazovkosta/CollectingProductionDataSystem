using System;
using System.Collections.Generic;
using AutoMapper;
using CollectingProductionDataSystem.Models.Identity;
using CollectingProductionDataSystem.Models.Nomenclatures;
using CollectingProductionDataSystem.Models.Productions;
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
            Mapper.CreateMap<RoleViewModel, ApplicationRole>();
            var mapper = new RoleViewModel();
            mapper.CreateMappings(Mapper.Configuration);
            Mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void CheckMappinUnitConfigToUnitConfigVievModel()
        {
            Mapper.CreateMap<UnitConfig, UnitConfigViewModel>();

            var mapper = new UnitConfigViewModel();
            mapper.CreateMappings(Mapper.Configuration);
            var mapper1 = new RelatedUnitConfigsViewModel();
            mapper1.CreateMappings(Mapper.Configuration);

            Mapper.AssertConfigurationIsValid();
        }
        [TestMethod]
        public void CheckMappinIEnumerable_UnitDailyConfig_To_IEnumerable_UnitConfigUnitDailyConfigViewModel()
        {
            Mapper.CreateMap<IEnumerable<UnitDailyConfig>, IEnumerable<UnitConfigUnitDailyConfigViewModel>>();

            var mapper = new UnitConfigUnitDailyConfigViewModel();
            mapper.CreateMappings(Mapper.Configuration);

            Mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void CheckMappinIEnumerable_UnitConfig_To_IEnumerable_RelatedUnitConfigsViewModel()
        {
            Mapper.CreateMap<IEnumerable<UnitConfig>, IEnumerable<RelatedUnitConfigsViewModel>>();

            var mapper = new RelatedUnitDailyConfigsViewModel();
            mapper.CreateMappings(Mapper.Configuration);

            Mapper.AssertConfigurationIsValid();
        }
    }
}
