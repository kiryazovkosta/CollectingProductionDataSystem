﻿using System;
using System.Collections.Generic;
using AutoMapper;
using CollectingProductionDataSystem.Application.Contracts;
using CollectingProductionDataSystem.Application.UnitDailyDataServices;
using CollectingProductionDataSystem.Models.Identity;
using CollectingProductionDataSystem.Models.Nomenclatures;
using CollectingProductionDataSystem.Models.Productions;
using CollectingProductionDataSystem.Models.SystemLog;
using CollectingProductionDataSystem.Web.AppStart;
using CollectingProductionDataSystem.Web.Areas.Administration.ViewModels;
using CollectingProductionDataSystem.Web.Areas.NomManagement.Models.ViewModels;
using CollectingProductionDataSystem.Web.Areas.PlanConfiguration.Models;
using CollectingProductionDataSystem.Web.Areas.ShiftReporting.ViewModels;
using CollectingProductionDataSystem.Web.ViewModels.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CollectingProductionDataSystem.Web.Areas.DailyReporting.ViewModels;
using Ninject;

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
        public void CheckMappinCreateRoleViewModelToApplicationRole()
        {
            Mapper.CreateMap<CreateRoleViewModel, ApplicationRole>();
            var mapper = new CreateRoleViewModel();
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
        public void CheckMappinIEnumerable_PlanNorm_To_IEnumerable_PlanNormViewModel()
        {
            Mapper.CreateMap<IEnumerable<PlanNorm>, IEnumerable<PlanNormViewModel>>();

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

        [TestMethod]
        public void CheckMappinIEnumerable_MultiShift_To_IEnumerable_UnitsReportsDataViewModel()
        {
            Mapper.CreateMap<IEnumerable<MultiShift>, IEnumerable<UnitsReportsDataViewModel>>();

            var mapper = new UnitsReportsDataViewModel();
            mapper.CreateMappings(Mapper.Configuration);

            Mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void CheckMappin_Event_To_EventView_Model()
        {
            Mapper.CreateMap<Event, EventViewModel>();

            //var mapper = new EventViewModel();
            //mapper.CreateMappings(Mapper.Configuration);

            Mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void Check_If_Injected_ITestUnitDailyCalculationService_IsSingleton_Model()
        {
            // Arrange 
            IKernel kernel = new Ninject.StandardKernel();
            kernel.Bind<ITestUnitDailyCalculationService>().ToMethod(context => TestUnitDailyCalculationService.GetInstance()).InSingletonScope();

            // Act
            var test1 = kernel.Get<ITestUnitDailyCalculationService>();
            var test2 = kernel.Get<ITestUnitDailyCalculationService>();

            // Assert
            Assert.AreSame(test1,test2);
        }


        [TestMethod]
        public void Check_Dates_Comparison_Must_Be_TRUE()
        {
            // Arrange 
            var targetDateTime = DateTime.Now;
            DateTime targetDate = new DateTime(targetDateTime.Year, targetDateTime.Month, 1);

            // Act
            var actualDateTime = DateTime.Now;
            var result = actualDateTime.Date >= targetDate;
            // Assert
            Assert.AreEqual(true, result);
        }    
        
        [TestMethod]
        public void Check_Dates_Comparison_Must_Be_False()
        {
            // Arrange 
            var targetDateTime = DateTime.Now;
            DateTime targetDate = new DateTime(targetDateTime.Year, targetDateTime.Month, 1);

            // Act
            var actualDateTime = new DateTime(2016, 06, 01).AddMilliseconds(-1);
            var result = actualDateTime.Date >= targetDate;
            // Assert
            Assert.AreEqual(false, result);
        }
    }
}
