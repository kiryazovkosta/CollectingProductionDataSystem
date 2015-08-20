namespace CollectingProductionDataSystem.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using CollectingProductionDataSystem.Models;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using CollectingProductionDataSystem.Models.Productions;
    using CollectingProductionDataSystem.Models.Transactions;
    using CollectingProductionDataSystem.Models.Identity;
    using Microsoft.AspNet.Identity;
    using CollectingProductionDataSystem.Data.Identity;

    internal sealed class Configuration : DbMigrationsConfiguration<CollectingDataSystemDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(CollectingDataSystemDbContext context)
        {
            try
            {
                if (!context.Products.Any())
            {
                ProductsDataImporter.Insert(context);
            }

            if (!context.MeasureUnits.Any())
            {
                this.CreateMeasureUnits(context);
            }

            if (!context.Directions.Any())
            {
                this.CreateDirections(context);
            }

            if (!context.MaterialTypes.Any())
            {
                this.CreateMaterialTypes(context);
            }

            if (!context.Factories.Any())
            {
               ProductionDataImporter.Insert(context);
            }

            if (!context.InventoryTanks.Any())
            {
                TanksDataImporter.Insert(context);
            }

            if (!context.MeasurementPoints.Any())
            {
                this.CreateMeasurementPoints(context);
            }

            if (!context.Users.Any())
            {
                this.CreateSystemAdministrator(context);
            }
            }
            catch (Exception eex)
            {
                int x = 0;
            }
        }


        private void CreateMeasureUnits(CollectingDataSystemDbContext context)
        {
            context.MeasureUnits.AddOrUpdate(
                new MeasureUnit
                {
                    Name = "Килограм",
                    Code = "Кг",
                },
                new MeasureUnit
                {
                    Name = "Литри (температура от 15°C)",
                    Code = "Л15",
                },
                new MeasureUnit
                {
                    Name = "Литри (температура от 20°C)",
                    Code = "Л20",
                },
                new MeasureUnit
                {
                    Name = "Килограми за текуща смяна",
                    Code = "Kг/тс",
                },
                new MeasureUnit
                {
                    Name = "Физична атмосфера",
                    Code = "атм",
                },
                new MeasureUnit
                {
                    Name = "Градуси Целзий",
                    Code = "C°",
                },
                new MeasureUnit
                {
                    Name = "Грам",
                    Code = "Гр",
                },
                new MeasureUnit
                {
                    Name = "Хектолитър",
                    Code = "HTL",
                },
                new MeasureUnit
                {
                    Name = "Гигаджаул",
                    Code = "GJO",
                },
                new MeasureUnit
                {
                    Name = "Литри",
                    Code = "Л",
                },
                new MeasureUnit
                {
                    Name = "Тон (1000 кг)",
                    Code = "Тон",
                },
                new MeasureUnit
                {
                    Name = "Мегаватчас",
                    Code = "мВч",
                },
                new MeasureUnit
                {
                    Name = "Киловатчас",
                    Code = "кВч",
                },
                new MeasureUnit
                {
                    Name = "Кубичен метър",
                    Code = "m3",
                });

            context.SaveChanges("Initial System Loading");
        }

        private void CreateDirections(CollectingDataSystemDbContext context)
        {
            context.Directions.AddOrUpdate(
                d => d.Id,
                new Direction
                {
                    Name = "Изход",
                },
                new Direction
                {
                    Name = "Вход",
                },
                new Direction
                {
                    Name = "Вход / Изход",
                });
            //context.SaveChanges();
        }

        private void CreateMaterialTypes(CollectingDataSystemDbContext context)
        {
            context.MaterialTypes.AddOrUpdate(
                m => m.Id,
                new MaterialType
                {
                    Name = "Материален",
                },
                new MaterialType
                {
                    Name = "Енергиен",
                },
                new MaterialType
                {
                    Name = "Нормиран",
                },
                new MaterialType
                {
                    Name = "Ненормиран",
                });

            context.SaveChanges("Initial System Loading");
        }

        private void CreateMeasurementPoints(CollectingDataSystemDbContext context)
        {
            context.TransportTypes.AddOrUpdate(
                new TransportType
                {
                    Name = "Автоекспедиция"
                },
                new TransportType
                {
                    Name = "Ж.П. експедиция"
                },
                new TransportType
                {
                    Name = "Танкери",
                },
                new TransportType
                {
                    Name = "Тръбопроводи"
                });
            context.SaveChanges();

            context.Ikunks.AddOrUpdate(
                new Ikunk
                {
                    Name = "ИКУНК 001 - Нова и Стара АНЕ",
                    Zones = {
                        new Zone
                        {
                            Name = "Остров 1",
                            MeasurementPoints = {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001001",
                                    Name = "BGNCA00005001001001",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = {
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 74,
                                            PhdTotalCounterTag = "TSN_KT0101_MT_GSV_P01.VT",
                                            DirectionId = 1
                                        }
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001002",
                                    Name = "BGNCA00005001001002",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = {
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 83,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P01.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 84,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P02.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 85,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P03.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 75,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P04.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 76,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P05.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 77,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P06.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 78,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P07.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 79,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P08.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 80,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P09.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 74,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P10.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 101,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P11.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 102,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P12.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 103,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P13.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 104,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P14.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 105,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P15.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 106,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P16.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 107,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P17.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 108,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P18.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 81,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P19.VT",
                                            DirectionId = 1
                                        },
                                        new MeasurementPointsProductsConfig
                                        {
                                            ProductId = 82,
                                            PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P20.VT",
                                            DirectionId = 1
                                        }
                                    }
                                },
                            }
                        }
                    }
                });
            context.SaveChanges();
        }

        private void CreateSystemAdministrator(CollectingDataSystemDbContext context)
        {
            var role = context.Roles.FirstOrDefault(x => x.Name == "Administrator");

            if (role == null)
            {
                role = new ApplicationRole()
                {
                    Id = 1,
                    Name = "Administrator"
                };
                var roleManager = new RoleManager<ApplicationRole, int>(new RoleStoreIntPk(context));
                roleManager.Create(role);
            }

            if (context.Users.Where(x => x.UserName == "Administrator").FirstOrDefault() == null)
            {
                var user = new ApplicationUser()
                {
                    Email = "Nikolay.Kostadinov@bmsys.eu",
                    UserName = "Administrator"
                };
                user.Roles.Add(new UserRoleIntPk()
                {
                    UserId = 1,
                    RoleId = 1
                });
                var manager = new UserManager<ApplicationUser, int>(new UserStoreIntPk(context));
                manager.Create(user, "12345678");
            }

            context.SaveChanges();
        }
    }
}