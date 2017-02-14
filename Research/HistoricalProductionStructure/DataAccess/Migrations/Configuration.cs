//-----------------------------------------------------------------------
// <copyright file="Configurations.cs" company="Business Management System Ltd.">
//     Copyright "2017" (c) Business Management System Ltd.. All rights reserved.
// </copyright>
// <author>Nikolay.Kostadinov</author>
//-----------------------------------------------------------------------

namespace HistoricalProductionStructure.DataAccess.Migrations
{
    #region Usings

    #endregion

    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using DataAccess;
    using Domain.Models;

    /// <summary>
    /// Summary description for Configurations
    /// </summary>  

    internal sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(AppDbContext context)
        {
            if (!context.Plants.Any())
            {

                var plant = new Plant()
                {
                    CreatedOn = DateTime.Now,
                    ShortName = "ПД",
                    FullName = "ПРОИЗВОДСТВЕНА ДЕЙНОСТ"
                };

                var factories = new List<Factory>()
                {
                    new Factory() {CreatedOn = DateTime.Now, Plant = plant, ShortName = "АВД", FullName = "Производство \"Атмосферно-вакуумна дестилация\""},             //0  - >1
                    new Factory() {CreatedOn = DateTime.Now, Plant = plant, ShortName = "КАБ", FullName = "Производство \"Компоненти за автомобилни бензини\""},          //1  - >2
                    new Factory() {CreatedOn = DateTime.Now, Plant = plant, ShortName = "КОГ", FullName = "Производство \"Каталитична обработка на горивата\""},          //2  - >3
                    new Factory() {CreatedOn = DateTime.Now, Plant = plant, ShortName = "КОГ-2", FullName = "Производство \"Каталитична обработка на горивата - 2\""},    //3  - >4
                    new Factory() {CreatedOn = DateTime.Now, Plant = plant, ShortName = "КПТО", FullName = "Производство КПТО"},                                          //4  - >5
                    new Factory() {CreatedOn = DateTime.Now, Plant = plant, ShortName = "ПП", FullName = "Производство Полипропилен"},                                    //5  - >6
                    new Factory() {CreatedOn = DateTime.Now, Plant = plant, ShortName = "АК", FullName = "Производство Азотно-кислородно"},                               //6  - >7
                    new Factory() {CreatedOn = DateTime.Now, Plant = plant, ShortName = "ТСНП", FullName = "Производство ТСНП и ПТ Росенец"},                             //7  - >8
                    new Factory() {CreatedOn = DateTime.Now, Plant = plant, ShortName = "ВиК и ОС", FullName = "Производство ВиК и ОС"},                                  //8  - >9
                    new Factory() {CreatedOn = DateTime.Now, Plant = plant, ShortName = "ТЕЦ", FullName = "Производство ТЕЦ"},                                            //9  - >10
                    new Factory() {CreatedOn = DateTime.Now, Plant = plant, ShortName = "МЦК", FullName = "Производство МЦК"},                                            //10 - >12
                    new Factory() {CreatedOn = DateTime.Now, Plant = plant, ShortName = "ТЕ", FullName = "Топлоенергия"},                                                 //11 - >13
                    new Factory() {CreatedOn = DateTime.Now, Plant = plant, ShortName = "ПТ Росенец", FullName = "ПТ Росенец"},                                           //12 - >14

                };

                factories[0].ProcessUnits = new List<ProcessUnit>() {
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName = "АД-4", FullName = "Инсталация АД-4",  },
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName = "АВД-1", FullName = "Инсталация АВД-1" },
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName = "ВДТК", FullName = "Инсталация ВДМ-2 и ТК" },
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName = "Битумна", FullName = "Инсталация Битумна" },
                };

                factories[1].ProcessUnits = new List<ProcessUnit>() {
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ХО и ГО (с.100)", FullName = "Инсталация ХО и ГО (с.100)"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ККр-с.200, с.300", FullName = "Инсталация ККр (с.200) и Абс. И Гфр. (с.300)"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ВИ-15", FullName = "Инсталация ВИ-15"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="PSA", FullName = "Инсталация PSA"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="СКА", FullName = "Инсталация СКА"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="POK", FullName = "Инсталация POK"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="МТБЕ", FullName = "Инсталация МТБЕ"}
                };

                factories[2].ProcessUnits = new List<ProcessUnit>() {
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ЦГФИ и ИНБ", FullName = "Инсталация ЦГФИ и ИНБ"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="КР-1", FullName = "Инсталация КР-1"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ХО и Х", FullName = "Инсталация Хидроочистка и хидриране"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="АГФИ и факел F-1", FullName = "Инсталация АГФИ и факел F-1"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ГО", FullName = "Инсталация ГО"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="П-37", FullName = "Парк 37"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ХО-1", FullName = "Инсталация ХО-1"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ХО-3", FullName = "Инсталация ХО-3"},
                };

                factories[3].ProcessUnits = new List<ProcessUnit>() {
                new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ХО-2", FullName = "Инсталация ХО-2"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ХО-5", FullName = "Инсталация ХО-5"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ХОБ-1", FullName = "Инсталация ХОБ-1"},
                };

                factories[4].ProcessUnits = new List<ProcessUnit>() {
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ХКГ", FullName = "Инсталация H-Oil"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ВИ-71", FullName = "Инсталация ВИ-71"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="РМДЕА и КВ", FullName = "Инсталация Регенерация на МДЕА и КВ"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ГС-4", FullName = "Инсталация ГС-4"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="МДЕА", FullName = "Инсталация МДЕА"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ГС-3", FullName = "Инсталация ГС-3"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="AO", FullName = "Инсталация АО"},
                };
                factories[5].ProcessUnits = new List<ProcessUnit>() {
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ПM", FullName = "Инсталация Полимеризация"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="С. 1300", FullName = "Секция 1300"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="С. 705", FullName = "Склад 705"},
                };

                factories[6].ProcessUnits = new List<ProcessUnit>() {
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="АК", FullName = "Инсталация Азотно-кислородно"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="Пароспътници", FullName = "МЦК - Пароспътници"},
                };

                factories[7].ProcessUnits = new List<ProcessUnit>() {
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ТСНП", FullName = "ТСНП-течни горива"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ВГ и АЖПНЕ", FullName = "ТСНП - ВГ и АЖПНЕ"},
                };

                factories[8].ProcessUnits = new List<ProcessUnit>() {
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ВиК", FullName = "Инсталация ВиК"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ЦПС", FullName = "Централна пречиствателна станция"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ВОУ", FullName = "Възел за обезводняване на нефтени утайки"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ПИНБУ", FullName = "Пещи за изгаряне на нефтени боклуци и утайки"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ДОНО", FullName = "ДОНО"},
                };

                factories[9].ProcessUnits = new List<ProcessUnit>() {
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ТЕЦ-ХОВ", FullName = "Инсталация ТЕЦ-ХОВ"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ТЕЦ-ЕЕ", FullName = "Инсталация ТЕЦ - Електроенергия"},
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ТЕЦ-ТЕ", FullName = "Инсталация ТЕЦ - Топлоенергия"},
                };

                factories[12].ProcessUnits = new List<ProcessUnit>() {
                    new ProcessUnit() { CreatedOn = DateTime.Now, ShortName ="ПТ Росенец", FullName = "ПТ Росенец"},
                };

                plant.Factories = factories;

                context.Plants.Add(plant);

                context.SaveChanges("Initial Loading");

                var processUnits = context.ProcessUnit.ToList();

                var activationDate = new DateTime(DateTime.Now.Year, 1, 1);

                foreach (var processUnit in processUnits)
                {
                    context.ProcessUnitToFactoryHistory.Add(new ProcessUnitToFactoryHistory()
                    {
                        CreatedOn = DateTime.Now,
                        ProcessUnitId = processUnit.Id,
                        ProcessUnitShortName = processUnit.ShortName,
                        ProcessUnitFullName = processUnit.FullName,
                        FactoryId = processUnit.Factory.Id,
                        FactoryShortName = processUnit.Factory.ShortName,
                        FactoryFullName = processUnit.Factory.FullName,
                        PlantId = processUnit.Factory.Plant.Id,
                        PlantShortName = processUnit.Factory.Plant.ShortName,
                        PlantFullName = processUnit.Factory.Plant.FullName,
                        ActivationDate = activationDate
                    });
                }

                context.SaveChanges("Initial Loading");

            }

            //if (!context.Roles.Any())
            //{
            //    this.CreateRoles(context);

            //    //this.SeedUsers(context);
            //}
            //this.CreateSystemAdministrator(context);
        }
    }
}