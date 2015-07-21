namespace CollectingProductionDataSystem.Data.Migrations
{
    using System.Linq;

    using CollectingProductionDataSystem.Models;
    using CollectingProductionDataSystem.Models.Mec;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<CollectingDataSystemDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(CollectingDataSystemDbContext context)
        {
            if (!context.Plants.Any())
            {
                this.CreatePlants(context);
            }

        }

        private void CreatePlants(CollectingDataSystemDbContext context)
        {
            context.Plants.AddOrUpdate(
                p => p.Id,
                new Plant
                {
                    Name = "Производствена дейност",
                    Factories = 
                    {
                        new Factory
                        {
                            Name = "Атмосферно-Вакуумна Дестилация", 
                            ProcessUnits = 
                            { 
                                new ProcessUnit {Name = "АТМОСФЕРНА ДЕСТИЛАЦИЯ-2" },
                                new ProcessUnit {Name = "АТМОСФЕРНА ДЕСТИЛАЦИЯ-3" },
                                new ProcessUnit {Name = "АТМОСФЕРНА ДЕСТИЛАЦИЯ-4" },
                                new ProcessUnit {Name = "АТМОСФЕРНА ДЕСТИЛАЦИЯ-5" },
                                new ProcessUnit {Name = "ВАКУУМНА ДЕСТИЛАЦИЯ НА МАЗУТ-1" },
                                new ProcessUnit {Name = "ДРУГ ПЕРСОНАЛ" },
                                new ProcessUnit {Name = "ВОДОРОДНА ИНСТАЛАЦИЯ" },
                                new ProcessUnit {Name = "ДРУГ ПЕРСОНАЛ" },
                                new ProcessUnit {Name = "КАТ.КРЕКИНГ - ОБЩА П/ДА" },
                                new ProcessUnit {Name = "МЕРОКС ИНСТАЛАЦИЯ" },
                                new ProcessUnit {Name = "УПРАВЛЕНИЕ АВД" },   
                            }
                        },
                        new Factory
                        {
                            Name = "Каталитична Обработка на Горива", 
                            ProcessUnits = 
                            {
                                new ProcessUnit {Name = "ХИДРООЧИСТКА-1" },
                                new ProcessUnit {Name = "ХИДРООЧИСТКА-2" },
                                new ProcessUnit {Name = "ХИДРООЧИСТКА-3" },
                                new ProcessUnit {Name = "ХИДРООЧИСТКА-4" },
                                new ProcessUnit {Name = "КАТАЛИТИЧЕН РЕФОРМИНГ-1" },
                                new ProcessUnit {Name = "ХИДРООЧИСТКА-5" },
                                new ProcessUnit {Name = "ГАЗООЧИСТКА" },
                                new ProcessUnit {Name = "ГАЗОВА СЯРА" },
                                new ProcessUnit {Name = "ГАЗОВА СЯРА 3" },
                                new ProcessUnit {Name = "ХОБ-1" },
                                new ProcessUnit {Name = "УПРАВЛЕНИЕ КОГ" },
                            }
                        },
                        new Factory
                        {
                            Name = "Каталитичен Крекинг", 
                            ProcessUnits = 
                            { 
                                new ProcessUnit { Name = "ХИДРООЧИСТКА И ГАЗООЧИСТКА" },
                                new ProcessUnit { Name = "КАТАЛИТИЧЕН КРЕКИНГ" },
                                new ProcessUnit { Name = "АБСОРБЦИЯ И ГАЗОФРАКЦИОНИРАНЕ" },
                                new ProcessUnit { Name = "ВАКУУМНА ДЕСТИЛАЦИЯ" },
                                new ProcessUnit { Name = "ТЕРМИЧЕН КРЕКИНГ" },
                                new ProcessUnit { Name = "ВОДОРОДНА ИНСТАЛАЦИЯ" },
                                new ProcessUnit { Name = "СЯРНО-КИСЕЛО АЛКИЛИРАНЕ" },
                                new ProcessUnit { Name = "РЕГЕНЕРАЦИЯ НА ОТРАБОТЕНАТА КИСЕЛИНА" },
                                new ProcessUnit { Name = "УПРАВЛЕНИЕ ККР" },
                            }
                        },
                        new Factory
                        {
                            Name = "Сярно-Кисело Алкилиране", 
                            ProcessUnits = 
                            { 
                                new ProcessUnit { Name = "АЛКИЛИРАНЕ" },
                                new ProcessUnit { Name = "СЯРНА КИСЕЛИНА И ОЛЕУМ" },
                                new ProcessUnit { Name = "БИТУМНА ИНСТАЛАЦИЯ" },
                                new ProcessUnit { Name = "ПАРК ГУДРОНИ" },
                                new ProcessUnit { Name = "ДРУГ ПЕРСОНАЛ" },
                                new ProcessUnit { Name = "УПРАВЛЕНИЕ СКА" },
                            }
                        },
                        new Factory
                        {
                            Name = "ТСНП",
                            ProcessUnits = 
                            { 
                                new ProcessUnit { Name = "П.Т.31,25/2 С ДЕПР.В-Л И Ж.П.Н.Е." },
                                new ProcessUnit { Name = "Т.1000,П.ККР И РП 2/2 8Х5000 ЗА БЕНЗ.И Д" },
                                new ProcessUnit { Name = "ПАРК Т.1021,1100,РП 2/2,ОЗС И АВТН.Е-ДА" },
                                new ProcessUnit { Name = "РЕЗЕРВОАРЕН ПАРК КАМЕНО" },
                                new ProcessUnit { Name = "ДРУГ ПЕРСОНАЛ ССЦ" },
                                new ProcessUnit { Name = "БАЗА ЗА СУРОВ НЕФТ" },
                                new ProcessUnit { Name = "БАЗА ЗА ХИМ.ПРОД.И ВТЕЧНЕНИ ГАЗОВЕ" },
                                new ProcessUnit { Name = "БАЗА ЗА ОБРАБОТКА НА ТАНКЕРИ" },
                                new ProcessUnit { Name = "ПАРЕН КОТЕЛ" },
                                new ProcessUnit { Name = "ДРУГ ПЕРСОНАЛ НБ" },
                                new ProcessUnit { Name = "ПСБ-КАМЕНО" },
                                new ProcessUnit { Name = "УПРАВЛЕНИЕ ТСНП" },
                            }
                        },
                        new Factory
                        {
                            Name = "Горивни и втечнени газове", 
                            ProcessUnits = 
                            {
                                new ProcessUnit { Name = "АГФИ, МЦК И ГС" },
                                new ProcessUnit { Name = "ЦЕНТР.ГАЗОФРАКЦ.ИНСТАЛАЦИЯ" },
                                new ProcessUnit { Name = "МЕТИЛТРЕТ.БУТИЛОВ ЕТЕР" },
                                new ProcessUnit { Name = "ВТЕЧН.ГАЗОВЕ,ЖП И АВТОНАЛ.ЕСТАКАДИ" },
                                new ProcessUnit { Name = "ДРУГ ПЕРСОНАЛ" },
                                new ProcessUnit { Name = "ГАЗОВО СТОПАНСТВО" },
                                new ProcessUnit { Name = "УПРАВЛЕНИЕ ГВГ" },
                            }
                        },
                        new Factory
                        {
                            Name = "Продуктопроводи",
                            ProcessUnits = 
                            {
                                new ProcessUnit { Name = "ПСБ-КАМЕНО" },
                                new ProcessUnit { Name = "ПСБ-КАРНОБАТ" },
                                new ProcessUnit { Name = "ПСБ-СТ.ЗАГОРА" },
                                new ProcessUnit { Name = "ПСБ-ПЛОВДИВ" },
                                new ProcessUnit { Name = "ПСБ-ВЕТРЕН" },
                                new ProcessUnit { Name = "ПСБ-ИХТИМАН" },
                                new ProcessUnit { Name = "ПСБ-АСПАРУХОВО" },
                                new ProcessUnit { Name = "ДРУГ ПЕРСОНАЛ" },
                                new ProcessUnit { Name = "УПРАВЛЕНИЕ ПП" },
                            }
                        },
                        new Factory
                        {
                            Name = "Комплексни и Специализирани Лаборатории", 
                            ProcessUnits = 
                            { 
                                new ProcessUnit { Name = "КОМПЛЕКСНА ЛАБОР. ПО НЕФТОПРЕРАБОТВАНЕ" },
                                new ProcessUnit { Name = "КЛНП - ЦЗЛ" },
                                new ProcessUnit { Name = "ГАЗОВА Л-Я ОБ" },
                                new ProcessUnit { Name = "АБОРАТ.ДЕЙНОСТ-УПРАВЛЕНИЕ" },
                            }
                        },
                        new Factory
                        {
                            Name = "Производство 1",
                            ProcessUnits = 
                            {
                                new ProcessUnit { Name = "ИНСТ. АД-2 И 3" },
                                new ProcessUnit { Name = "ИНСТ. АД-4" },
                                new ProcessUnit { Name = "ИНСТ. АД-5" },
                                new ProcessUnit { Name = "ИНСТ. ВДМ-1" },
                                new ProcessUnit { Name = "ИНСТ. ТЕРМИЧЕН КРЕКИНГ" },
                                new ProcessUnit { Name = "ИНСТ. ЗА ПР-ВО НА БИТУМИ И РП НА ТЪМНИ НЕФТОПРОДУК" },
                            }
                        },
                        new Factory
                        {
                            Name = "Производство 2", 
                            ProcessUnits = 
                            { 
                                new ProcessUnit { Name = "ИНСТ. КАТАЛИТИЧЕН РЕФОРМИНГ 1" },
                                new ProcessUnit { Name = "ИНСТ. ХИДРООЧИСТКА-1 И 3" },
                                new ProcessUnit { Name = "ИНСТ. ХИДРООЧИСТКА-2" },
                                new ProcessUnit { Name = "ИНСТ. ХИДРООЧИСТКА-4" },
                                new ProcessUnit { Name = "ИНСТ. АГФ,МЦК И ГС, ФАКЕЛНО СТОПАНСТВО" },
                                new ProcessUnit { Name = "ИНСТ. ЦГФ И ИЗОМЕРИЗАЦИЯ НА N-БУТАН" },
                                new ProcessUnit { Name = "ИНСТ. ХИДРООЧ.,РЕФОРМИНГ И ХИДРИРАНЕ" },
                            }
                        },
                        new Factory
                        {
                            Name = "Производство 3", 
                            ProcessUnits = 
                            { 
                                new ProcessUnit { Name = "ИНСТ. КАТАЛИТИЧЕН КРЕКИНГ" },
                                new ProcessUnit { Name = "ИНСТ. ВОДОРОДНА" },
                                new ProcessUnit { Name = "ИНСТ. АЛКИЛИРАНЕ" },
                                new ProcessUnit { Name = "ИНСТ. ГАЗОВА СЯРА" },
                                new ProcessUnit { Name = "ИНСТ. ГАЗОВА СЯРА-3" },
                                new ProcessUnit { Name = "ИНСТ. ГАЗООЧИСТКА-1 И 2" },
                                new ProcessUnit { Name = "ИНСТ. СЯРНА КИСЕЛИНА И ОЛЕУМ" },
                            }
                        },
                        new Factory
                        {
                            Name = "Производство 4"
                        },
                        new Factory
                        {
                            Name = "Производство 5", 
                            ProcessUnits = 
                            { 
                                new ProcessUnit { Name = "ПАРК ЗА ХИМИЧЕСКИ ПРОДУКТИ И ВТЕЧНЕНИ ГАЗОВЕ"}
                            }
                        },
                        new Factory
                        {
                            Name = "Производство 6"
                        },
                        new Factory
                        {
                            Name = "Производство 7", 
                            ProcessUnits = 
                            { 
                                new ProcessUnit { Name = "ИНСТ. ПРОПИЛЕН" },
                                new ProcessUnit { Name = "ИНСТ. СЪХРАНЕНИЕ НА ТЕЧЕН ЕТИЛЕН" },
                                new ProcessUnit { Name = "ИНСТ. ЕТИЛЕНОВ ОКИС" },
                                new ProcessUnit { Name = "ИНСТ. ЕТИЛЕНГЛИКОЛИ" },
                                new ProcessUnit { Name = "ИНСТ. МАЛОТОНАЖНИ ПРОДУКТИ" },
                                new ProcessUnit { Name = "ИНСТ. ЕТАНОЛАМИНИ" },
                            }
                        },
                        new Factory
                        {
                            Name = "Производство 8", 
                            ProcessUnits = 
                            {
                                new ProcessUnit { Name = "ИНСТ. ПЕВН-1" },
                                new ProcessUnit { Name = "ИНСТ. ПЕВН-2" },
                                new ProcessUnit { Name = "МАТЕРИАЛНИ АКТИВИ ПОЛИСТИРОЛ" },
                                new ProcessUnit { Name = "РЕАЛИЗАЦИЯ НА ПРОИЗВОДСТВЕНИ ОТПАДЪЦИ (ОПАКОВКИ)" },
                                new ProcessUnit { Name = "ИНСТ. ПОЛИМЕРИЗАЦИЯ НА ПОЛИПРОПИЛЕН" },
                                new ProcessUnit { Name = "ИНСТ. ПИРОЛЕН" },
                                new ProcessUnit { Name = "ИНСТ. АКРИЛНИТРИЛ" },
                                new ProcessUnit { Name = "МАТЕРИАЛНИ АКТИВИ ПАНВ" },
                            }
                        },
                        new Factory
                        {
                            Name = "Производство 9"
                        },
                        new Factory
                        {
                            Name = "Производство 10"
                        },
                        new Factory
                        {
                            Name = "Производство Други"
                        },
                        new Factory
                        {
                            Name = "Каталитична Обработка на Горива - 2",
                        },
                        new Factory
                        {
                            Name = "ЛЕГБ",
                        },
                        new Factory
                        {
                            Name = "Полипропилен и Очистка на ППФ",
                        },
                        new Factory
                        {
                            Name = "СОВ",
                        },
                        new Factory
                        {
                            Name = "Азотно-Кислородно Производство",
                        },
                        new Factory
                        {
                            Name = "Биологично пречистване на Води"
                        }
                        ,
                        new Factory
                        {
                            Name = "Комплекс за Преработка на Тежки Остатъци"
                        }
                    }
                },
                new Plant
                {
                    Name = "Нефтохимия",
                    Factories = 
                    {
                        new Factory
                        {
                            Name = "Производство Катализатори"
                        },
                        new Factory
                        {
                            Name = "Етилен"
                        },
                        new Factory
                        {
                            Name = "Ет. Окис И Етаноламин"
                        },
                        new Factory
                        {
                            Name = "Фенол"
                        },
                        new Factory
                        {
                            Name = "Ксилоли"
                        },
                        new Factory
                        {
                            Name = "Група за Подържане на Консерв. Оборудване"
                        },
                        new Factory
                        {
                            Name = "Комплексни и Специализирани Лаборатории"
                        },
                        new Factory
                        {
                            Name = "Централизирана Ремонтна Бригада-1"
                        },
                        new Factory
                        {
                            Name = "Заводоуправление"
                        },
                    }

                },
                new Plant
                {
                    Name = "Полимери",
                    Factories =
                    {
                        new Factory
                        {
                            Name = "Каучук и Латекси"
                        },
                        new Factory
                        {
                            Name = "ПЕВН И Полистирол"
                        },
                        new Factory
                        {
                            Name = "Полипропилен И Пиролен"
                        },
                        new Factory
                        {
                            Name = "ПАНВ И Акрилнитрил"
                        },
                        new Factory
                        {
                            Name = "Комплексни и Специализирани Лаборатории"
                        },
                        new Factory
                        {
                            Name = "Административни сгради"
                        },
                        new Factory
                        {
                            Name = "Централизирани ремонти-2"
                        },
                        new Factory
                        {
                            Name = "Заводоуправление - Полимери"
                        }
                    }
                },
                new Plant
                {
                    Name = "Енергетика",
                    Factories = 
                    { 
                        new Factory 
                        { 
                            Name = "СУРОВИННО И ОБОРОТНО ВОДОСНАБДЯВАНЕ"
                        },
                        new Factory 
                        { 
                            Name = "АЗОТНО-КИСЛОРОДНО"
                        },
                        new Factory 
                        { 
                            Name = "БИОЛОГИЧНО ПРЕЧИСТВАНЕ НА ВОДИТЕ"
                        },
                        new Factory 
                        { 
                            Name = "МЕЖДУЦЕХОВИ КОМУНИКАЦИИ"
                        },
                        new Factory 
                        { 
                            Name = "ЗАВОДОУПРАВЛЕНИЕ"},
                        new Factory 
                        { 
                            Name = "ОТДЕЛ \"ЕЛЕКТРОЕНЕРГИЕН\""
                        },
                        new Factory 
                        { 
                            Name = "ОТДЕЛ \"ТОПЛОЕНЕРГИЕН\""
                        },
                        new Factory 
                        { 
                            Name = "ГРУПА \"СЛАБОТОКОВА ТЕХНИКА\""
                        },
                        new Factory 
                        { 
                            Name = "ТОПЛОЕЛЕКТРИЧЕСКА ЦЕНТРАЛА"
                        },
                        new Factory 
                        { 
                            Name = "П-ВО \"ТОПЛОСНАБДЯВАНЕ\""
                        },
                        new Factory 
                        { 
                            Name = "ЕЛЕКТРОЕНЕРГИЙНО ПРОИЗВОДСТВО"
                        }, 
                    }
                },
                new Plant
                {
                    Name = "ЗВЕНО ИНВЕСТИТОРСКИ КОНТРОЛ",
                    Factories = 
                    {
                        new Factory
                        {
                            Name = "СТРОИТЕЛСТВО"
                        }                   
                    }
                },
                new Plant
                {
                    Name = "РЕМОНТНО-МЕХАНИЧЕН ЗАВОД",
                    Factories = 
                    { 
                        new Factory { Name = "УПРАВЛЕНИЕ"},
                        new Factory { Name = "ОТДЕЛ \"МЕХАНИЧЕН\""},
                        new Factory { Name = "ОТДЕЛ \"ТЕХНИЧЕСКИ НАДЗОР\""},
                        new Factory { Name = "ЛАБ-Я \"ТЕХНИЧ.НАДЗОР\""},
                        new Factory { Name = "РЕМОНТ"},
                    }
                },
                new Plant
                {
                    Name = "ЗВЕНО \"БРЕТ\"",
                    Factories = 
                    { 
                        new Factory { Name = "РЕМОНТ НА ЕЛ.ДВИГАТЕЛИ И ТРАНСФОРМ."}
                    }
                },
                new Plant
                {
                    Name = "Механизация и транспорт",
                    Factories = 
                    { 
                        new Factory { Name = "ЗВЕНО \"ТРАНСПОРТ\""}
                    }
                },
                new Plant
                {
                    Name = "ИНФОРМАЦИОННО ИЗЧИСЛИТЕЛЕН ЦЕНТЪР",
                    Factories = 
                    { 
                        new Factory { Name = "ИИЦ"}
                    }
                },
                new Plant
                {
                    Name = "Централно Управление",
                    Factories = 
                    { 
                        new Factory { Name = "ПД УПРАВЛЕНИЕ" },
                        new Factory { Name = "ТЕХНОЛОЗИ" },
                        new Factory { Name = "ДИСПЕЧЕРИ" },
                        new Factory { Name = "СКЛАДОВЕ" },
                        new Factory { Name = "ХИМИЧЕСКО СНАБДЯВАНЕ" },
                        new Factory { Name = "СВОБОДНО" },
                        new Factory { Name = "ОТДЕЛ \"ПРАВЕН\"" },
                        new Factory { Name = "СЧЕТОВОДСТВО" },
                        new Factory { Name = "АДМИНИСТРАТИВНА ДИРЕКЦИЯ" },
                        new Factory { Name = "МЕДИЦИНСКИ ЦЕНТЪР" },
                        new Factory { Name = "СТОЛОВО ХРАНЕНЕ" },
                        new Factory { Name = "АДМИНИСТРАТИВНИ СГРАДИ" },
                        new Factory { Name = "ОТДЕЛ \"ЕКОЛОГИЯ\"" },
                        new Factory { Name = "ИНСПЕКЦИЯ \"БЕЗОПАНОСТ НА ТРУДА\"" },
                        new Factory { Name = "ГАЗОСПАСИТЕЛНА СЛУЖБА" },
                        new Factory { Name = "ОТДЕЛ \"КИП И А\"" },
                        new Factory { Name = "ОТДЕЛ \"АВТОМАТИЗАЦИЯ НА ПРОЦЕСИ\"" },
                        new Factory { Name = "ОТДЕЛ \"ЕКСПЕДИЦИЯ\"" },
                        new Factory { Name = "ОТДЕЛ \"СНАБДЯВАНЕ\"" },
                        new Factory { Name = "ОТДЕЛ \"ТРАНСПОРТЕН\"" },
                        new Factory { Name = "ОТДЕЛ \"ПЛАНИРАНЕ И БАЛАНСИ\"" },
                        new Factory { Name = "КОНТРОЛНИ ЛАБОРАТОРИИ" },
                        new Factory { Name = "ЛУКОЙЛ - БЪЛГАРИЯ" },
                        new Factory { Name = "ОТДЕЛ \"НАУЧНО-ТЕХНИЧЕСКА ЛАБОРАТОРИЯ\"" },
                    }
                },
                new Plant 
                { 
                    Name = "ИНСТИТУТ ПО НЕФТОПРЕР.И НЕФТОХИМИЯ",
                    Factories = 
                    {
                        new Factory { Name = "ИНН" },
                    }
                },
                new Plant 
                { 
                    Name = "Продуктопроводи",
                    Factories = 
                    { 
                        new Factory { Name = "ПСБ - КАРНОБАТ" },
                        new Factory { Name = "ПСБ - СТАРА ЗАГОРА" },
                        new Factory { Name = "ПСБ--ПЛОВДИВ" },
                        new Factory { Name = "ПСБ - ВЕТРЕН" },
                        new Factory { Name = "ПСБ - ИХТИМАН" },
                        new Factory { Name = "ПРОДУКТОПРОВОДИ" },
                        new Factory { Name = "ПСБ-Аспарухово" },
                    }
                },
                new Plant 
                { 
                    Name = "Нефтохимпроект",
                    Factories = 
                    {
                        new Factory { Name = "Нефтохимпроект" },
                    } 
                },
                new Plant 
                { 
                    Name = "Външни Консуматори",
                    Factories = 
                    {
                        new Factory { Name = "Външни Консуматори" },
                    }
                },
                new Plant 
                { 
                    Name = "Трговска  Дирекция",
                    Factories = 
                    {
                        new Factory { Name = "Отдел \"ЕКСПЕДИЦИЯ\"" },
                    }
                });
            context.SaveChanges();
        }

        #region Tanks
        //            //  This method will be called after migrating to the latest version.
        //            context.ExciseStores.AddOrUpdate(
        //                new ExciseStore
        //                {
        //                    Id = 1,
        //                    Name = "BGNCA00005001"
        //                }
        //            );

        //            context.Areas.AddOrUpdate(
        //                new Area 
        //                {
        //                    //Id = 1,
        //                    Name = "Ïðîèçâîäñòâåíà ïëîùàäêà ËÍÕÁ ÀÄ",
        //                    ReadOnlyRole = "LnhbFullAccess",
        //                    FullAccessRole = "LnhbReadOnly",
        //                    IsActive = true,
        //                    InventoryParks = {
        //                        new InventoryPark
        //                        {
        //                            Name = "ÎÇÑ", 
        //                            ExciseStoreId = 1, 
        //                            FullAccessRole = "LnhbPOzsFullAccess", 
        //                            ReadOnlyRole = "LnhbPOzsReadOnly", 
        //                            IsActive = true
        //                        },
        //                        new InventoryPark
        //                        {
        //                            Name = "8x5000", 
        //                            ExciseStoreId = 1, 
        //                            FullAccessRole = "LnhbP8x5000FullAccess", 
        //                            ReadOnlyRole = "LnhbP8x5000ReadOnly", 
        //                            IsActive = true
        //                        },
        //                        new InventoryPark
        //                        {
        //                            Name = "Êàò. Êðåêèíã", 
        //                            ExciseStoreId = 1, 
        //                            FullAccessRole = "LnhbPKkrFullAccess", 
        //                            ReadOnlyRole = "LnhbPKkrReadOnly", 
        //                            IsActive = true
        //                        },
        //                        new InventoryPark
        //                        {
        //                            Name = "ÏÂÃ", 
        //                            ExciseStoreId = 1, 
        //                            FullAccessRole = "LnhbPPVGFullAccess", 
        //                            ReadOnlyRole = "LnhbPPVGReadOnly", 
        //                            IsActive = true
        //                        }
        //                    }
        //                },
        //                new Area
        //                {
        //                    //Id = 2,
        //                    Name = "Ïàðêîâ òåðìèíàë \"Ðîñåíåö\"",
        //                    ReadOnlyRole = "PtrFullAccess",
        //                    FullAccessRole = "PtrReadOnly",
        //                    IsActive = true,
        //                    InventoryParks = {
        //                        new InventoryPark
        //                        {
        //                            Name = "Õèìè÷íè ïðîäóêòè", 
        //                            ExciseStoreId = 1, 
        //                            FullAccessRole = "PtrPChemicalsFullAccess", 
        //                            ReadOnlyRole = "PtrPChemicalsReadOnly", 
        //                            IsActive = true, 
        //                            InventoryTanks = {
        //                                new InventoryTank
        //                                {
        //                                    //'NTR_P9201.LEVEL_FWL', CAST(0.000 AS Decimal(18, 3)), CAST(100000.000 AS Decimal(18, 3)), N'NTR_P9201.FWL', CAST(0.000 AS Decimal(18, 3)), CAST(500.000 AS Decimal(18, 3)), N'NTR_P9201.FWV', CAST(0.000 AS Decimal(18, 3)), CAST(100000.000 AS Decimal(18, 3)), N'NTR_P9201.OBS_DENS', CAST(150.000 AS Decimal(18, 3)), CAST(1500.000 AS Decimal(18, 3)), N'NTR_P9201.REF_DENS', CAST(150.000 AS Decimal(18, 3)), CAST(1500.000 AS Decimal(18, 3)), N'NTR_P9201.GOV', CAST(0.000 AS Decimal(18, 3)), CAST(100000.000 AS Decimal(18, 3)), N'NTR_P9201.GSV', CAST(0.000 AS Decimal(18, 3)), CAST(100000.000 AS Decimal(18, 3)), N'NTR_P9201.NSV', CAST(0.000 AS Decimal(18, 3)), CAST(100000.000 AS Decimal(18, 3)), N'NTR_P9201.WIA', CAST(0.000 AS Decimal(18, 3)), CAST(100000.000 AS Decimal(18, 3)), N'NTR_P9201.REAL_TEMP', CAST(-50.000 AS Decimal(18, 3)), CAST(100.000 AS Decimal(18, 3)), N'NTR_P9201.TOV', CAST(0.000 AS Decimal(18, 3)), CAST(100000.000 AS Decimal(18, 3)), N'NTR_P9201.WIV', CAST(0.000 AS Decimal(18, 3)), CAST(100000.000 AS Decimal(18, 3)), N'NTR_P9201.MXV', CAST(0.000 AS Decimal(18, 3)), CAST(100000.000 AS Decimal(18, 3)), N'NTR_P9201.AVRM', CAST(0.000 AS Decimal(18, 3)), CAST(100000.000 AS Decimal(18, 3)))
        //                                    ControlPoint = "KT2001", 
        //                                    TankName = "P-9201",
        //                                    IsActive = true,
        //                                    PhdTagProductId = "NTR_P9201.PROD_ID", 
        //                                    PhdTagProductName = "NTR_P9201.PROD", 
        //                                    PhdTagLiquidLevel = "NTR_P9201.LEVEL_MM", 
        //                                    LiquidLevelLowExtreme = 0.000m, 
        //                                    LiquidLevelHighExtreme = 7500.000m, 
        //                                    PhdTagProductLevel = "NTR_P9201.LEVEL_FWL", 
        //                                    ProductLevelLowExtreme = 0.000m,
        //                                    ProductLevelHighExtreme = 
        //                                    PhdTagFreeWaterLevel], 
        //                                    FreeWaterLevelLowExtreme], 
        //                                    FreeWaterLevelHighExtreme], 
        //                                    PhdTagFreeWaterVolume], 
        //                                    FreeWaterVolumeLowExtreme], 
        //                                    FreeWaterVolumeHighExtreme], 
        //                                    PhdTagObservableDensity], 
        //                                    ObservableDensityLowExtreme], 
        //                                    ObservableDensityHighExtreme], 
        //                                    PhdTagReferenceDensity], 
        //                                    ReferenceDensityLowExtreme], 
        //                                    ReferenceDensityHighExtreme], 
        //                                    PhdTagGrossObservableVolume], 
        //                                    GrossObservableVolumeLowExtreme], 
        //                                    GrossObservableVolumeHighExtreme], 
        //                                    PhdTagGrossStandardVolume], 
        //                                    GrossStandardVolumeLowExtreme], 
        //                                    GrossStandardVolumeHighExtreme], 
        //                                    PhdTagNetStandardVolume], 
        //                                    NetStandardVolumeLowExtreme], 
        //                                    NetStandardVolumeHighExtreme], 
        //                                    PhdTagWeightInAir], 
        //                                    WeightInAirLowExtreme], 
        //                                    WeightInAirHighExtreme], 
        //                                    PhdTagAverageTemperature], 
        //                                    AverageTemperatureLowExtreme], 
        //                                    AverageTemperatureHighExtreme], 
        //                                    PhdTagTotalObservableVolume], 
        //                                    TotalObservableVolumeLowExtreme], 
        //                                    TotalObservableVolumeHighExtreme], 
        //                                    PhdTagWeightInVacuum], 
        //                                    WeightInVacuumLowExtreme], 
        //                                    WeightInVacuumHighExtreme], 
        //                                    PhdTagMaxVolume], 
        //                                    MaxVolumeLowExtreme], 
        //                                    MaxVolumeHighExtreme], 
        //                                    PhdTagAvailableRoom], 
        //                                    AvailableRoomLowExtreme], 
        //                                    AvailableRoomHighExtreme]
        //                                }
        //                            }
        //                        },
        //                        new InventoryPark
        //                        {
        //                            Name = "Ñóðîâèííà áàçà", 
        //                            ExciseStoreId = 1, 
        //                            FullAccessRole = "PtrPResourcesFullAccess", 
        //                            ReadOnlyRole = "PtrPResourcesReadOnly", 
        //                            IsActive = true
        //                        },
        //                        new InventoryPark
        //                        {
        //                            Name = "Äðåíàæíè åìêîñòè", 
        //                            ExciseStoreId = 1, 
        //                            FullAccessRole = "PtrPDrenFullAccess", 
        //                            ReadOnlyRole = "PtrPDrenReadOnly", 
        //                            IsActive = true
        //                        }
        //                    }
        //                }
        //            );

        //            context.InventoryParks.AddOrUpdate(
        //                //new InventoryPark
        //                //{
        //                //    Id = 1,
        //                //    Name = "Õèìè÷íè ïðîäóêòè", 
        //                //    ExciseStoreId = 1, 
        //                //    AreaId = 2, 
        //                //    FullAccessRole = "PtrPChemicalsFullAccess", 
        //                //    ReadOnlyRole = "PtrPChemicalsReadOnly", 
        //                //    IsActive = true
        //                //},
        //                //new InventoryPark
        //                //{
        //                //    Id = 2,
        //                //    Name = "Ñóðîâèííà áàçà", 
        //                //    ExciseStoreId = 1, 
        //                //    AreaId = 2, 
        //                //    FullAccessRole = "PtrPResourcesFullAccess", 
        //                //    ReadOnlyRole = "PtrPResourcesReadOnly", 
        //                //    IsActive = true
        //                //},
        //                //new InventoryPark
        //                //{
        //                //    Id = 3,
        //                //    Name = "Äðåíàæíè åìêîñòè", 
        //                //    ExciseStoreId = 1, 
        //                //    AreaId = 2, 
        //                //    FullAccessRole = "PtrPDrenFullAccess", 
        //                //    ReadOnlyRole = "PtrPDrenReadOnly", 
        //                //    IsActive = true
        //                //},


        ////7, N'ÐÏ 2/2', N'BGNCA00005001', 1, N'LnhbPRP2_2FullAccess', N'LnhbPRP2_2ReadOnly', 1)
        ////8, N'Òèòóë 1000', N'BGNCA00005001', 1, N'LnhbPT1000FullAccess', N'LnhbPT1000ReadOnly', 1)
        ////9, N'Òèòóë 1100', N'BGNCA00005001', 1, N'LnhbPT1100FullAccess', N'LnhbPT1100ReadOnly', 1)
        ////10, N'506', N'BGNCA00005001', 1, N'LnhbP506FullAccess', N'LnhbP506ReadOnly', 1)
        ////11, N'626', N'BGNCA00005001', 1, N'LnhbP626FullAccess', N'LnhbP626ReadOnly', 1)
        ////12, N'750', N'BGNCA00005001', 1, N'LnhbP750FullAccess', N'LnhbP750ReadOnly', 1)
        ////13, N'1613', N'BGNCA00005001', 1, N'LnhbP1613FullAccess', N'LnhbP1613ReadOnly', 1)
        ////14, N'3x10000', N'BGNCA00005001', 1, N'LnhbP3x10000FullAccess', N'LnhbP3x10000ReadOnly', 1)
        ////15, N'ÀÄ-2 (Ò.27)', N'BGNCA00005001', 1, N'LnhbPT27FullAccess', N'LnhbPT27ReadOnly', 1)
        ////16, N'Áèòóìè', N'BGNCA00005001', 1, N'LnhbPBitumiFullAccess', N'LnhbPBitumiReadOnly', 1)
        ////17, N'ÂÄÌ-1', N'BGNCA00005001', 1, N'LnhbPVDM1FullAccess', N'LnhbPVDM1ReadOnly', 1)
        ////18, N'Ïîëèïðîïèëåí', N'BGNCA00005001', 1, N'LnhbPPolyFullAccess', N'LnhbPPolyReadOnly', 1)
        ////19, N'Òèòóë 1026', N'BGNCA00005001', 1, N'LnhbPT1026FullAccess', N'LnhbPT1026ReadOnly', 1)
        ////20, N'Òèòóë 25/2', N'BGNCA00005001', 1, N'LnhbPT25_2FullAccess', N'LnhbPT25_2ReadOnly', 1)
        ////21, N'Òèòóë 31', N'BGNCA00005001', 1, N'LnhbPT31FullAccess', N'LnhbPT31ReadOnly', 1)
        ////22, N'ÕÎ-2', N'BGNCA00005001', 1, N'LnhbPHO2FullAccess', N'LnhbPHO2ReadOnly', 1)
        ////23, N'ÕÎ-4', N'BGNCA00005001', 1, N'LnhbPHO4FullAccess', N'LnhbPHO4ReadOnly', 1)
        ////24, N'Êàìåíî', N'BGNCA00005001', 1, N'LnhbPKamenoFullAccess', N'LnhbPKamenoReadOnly', 1)
        ////26, N'Ìàñëåíî Ñòîïàíñòâî', N'BGNCA00005001', 1, N'LnhbPMSFullAccess', N'LnhbPMSReadOnly', 1)
        ////27, N'ÐÏ ÊÏÒÎ', N'BGNCA00005001', 1, N'LnhbPKptoFullAccess', N'LnhbPKptoReadOnly', 1)

        //            );

        //        }
        #endregion
    }
}
