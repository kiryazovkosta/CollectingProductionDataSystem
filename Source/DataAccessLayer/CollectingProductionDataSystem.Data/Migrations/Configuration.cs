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
            if (!context.Products.Any())
            {
                ProductsDataImporter.Insert(context);
            }

            if (!context.TankMasterProducts.Any())
            {
                this.CreateTankMasterProducts(context);
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
 
        private void CreateTankMasterProducts(CollectingDataSystemDbContext context)
        {
            context.TankMasterProducts.AddOrUpdate(
                new TankMasterProduct { TankMasterProductId = 1, Name = "NEFT", ProductCode = 1},
                new TankMasterProduct { TankMasterProductId = 2, Name = "Mazut za specificna prerabotka", ProductCode = 2},
                new TankMasterProduct { TankMasterProductId = 4, Name = "MTBE - vnos", ProductCode = 4},
                new TankMasterProduct { TankMasterProductId = 5, Name = "Biokomponent B-100", ProductCode = 5},
                new TankMasterProduct { TankMasterProductId = 6, Name = "Bioetanol (denatoriran)", ProductCode = 6},
                new TankMasterProduct { TankMasterProductId = 8, Name = "Prom. gasoil (chujd, S>0.2%)", ProductCode = 8},
                new TankMasterProduct { TankMasterProductId = 9, Name = "Prom. gasoil (chujd-sin, S>0.2%)", ProductCode = 9},
                new TankMasterProduct { TankMasterProductId = 10, Name = "Gasiol (chujd, S<=0.05%)", ProductCode = 10},
                new TankMasterProduct { TankMasterProductId = 11, Name = "Gasiol (chujd, S= 0.05% - 0.2%))", ProductCode = 11},
                new TankMasterProduct { TankMasterProductId = 12, Name = "Gasiol (chujd-sin, S<=0.05%)", ProductCode = 12},
                new TankMasterProduct { TankMasterProductId = 13, Name = "Gasiol (chujd-sin, S= 0.05% - 0.2%))", ProductCode = 13},
                new TankMasterProduct { TankMasterProductId = 14, Name = "K.G c. smaz", ProductCode = 14},
                new TankMasterProduct { TankMasterProductId = 15, Name = "K.G. c <1%", ProductCode = 15},
                new TankMasterProduct { TankMasterProductId = 16, Name = "K.G. c 1%-2%", ProductCode = 16},
                new TankMasterProduct { TankMasterProductId = 17, Name = "K.G. c 2%-2.8%", ProductCode = 17},
                new TankMasterProduct { TankMasterProductId = 18, Name = "K.G. c >2.8%", ProductCode = 18},
                new TankMasterProduct { TankMasterProductId = 19, Name = "100% Etanol", ProductCode = 19},
                new TankMasterProduct { TankMasterProductId = 20, Name = "100% Biodiesel", ProductCode = 20},
                new TankMasterProduct { TankMasterProductId = 21, Name = "Metanol", ProductCode = 21},
                new TankMasterProduct { TankMasterProductId = 22, Name = "Normalen hexan", ProductCode = 22},
                new TankMasterProduct { TankMasterProductId = 24, Name = "SMF  ( vnos )", ProductCode = 24},
                new TankMasterProduct { TankMasterProductId = 25, Name = "DG visoka syara ( vnos )", ProductCode = 25},
                new TankMasterProduct { TankMasterProductId = 26, Name = "Neconditionni produkti ( vhod )", ProductCode = 26},
                new TankMasterProduct { TankMasterProductId = 27, Name = "Gazov kondenzat", ProductCode = 27},
                new TankMasterProduct { TankMasterProductId = 28, Name = "Korabno ostatachno gorivo", ProductCode = 28},
                new TankMasterProduct { TankMasterProductId = 29, Name = "Mazut SP(AR)", ProductCode = 29},
                new TankMasterProduct { TankMasterProductId = 30, Name = "Mazut SP(BM)", ProductCode = 30},

                new TankMasterProduct { TankMasterProductId = 38, Name = "Awtomobilen benzin A92H", ProductCode = 38},
                new TankMasterProduct { TankMasterProductId = 40, Name = "Avtomobilen benzin A95-EVRO5", ProductCode = 40},
                new TankMasterProduct { TankMasterProductId = 41, Name = "Avtomobilen benzin A98-EVRO5", ProductCode = 41},
                new TankMasterProduct { TankMasterProductId = 42, Name = "Avtomobilen benzin A95 B2", ProductCode = 42},
                new TankMasterProduct { TankMasterProductId = 43, Name = "Avtomobilen benzin A95 B4", ProductCode = 43},
                new TankMasterProduct { TankMasterProductId = 44, Name = "Avtomobilen benzin A98 B4", ProductCode = 44},
                new TankMasterProduct { TankMasterProductId = 45, Name = "Avtomobilen benzin A95 B5", ProductCode = 45},
                new TankMasterProduct { TankMasterProductId = 46, Name = "Avtomobilen benzin A95 B6", ProductCode = 46},
                new TankMasterProduct { TankMasterProductId = 47, Name = "Avtomobilen benzin A95 B7", ProductCode = 47},
                new TankMasterProduct { TankMasterProductId = 50, Name = "Avtomobilen benzin A98 B2", ProductCode = 50},
                new TankMasterProduct { TankMasterProductId = 53, Name = "Avtomobilen benzin A98H B5", ProductCode = 53},
                new TankMasterProduct { TankMasterProductId = 54, Name = "Avtomobilen benzin A98H B6", ProductCode = 54},
                new TankMasterProduct { TankMasterProductId = 55, Name = "Avtomobilen benzin A98H B7", ProductCode = 55},
                new TankMasterProduct { TankMasterProductId = 76, Name = "Benzin A93H", ProductCode = 76},
                new TankMasterProduct { TankMasterProductId = 77, Name = "Reformat", ProductCode = 77},
                new TankMasterProduct { TankMasterProductId = 78, Name = "Niskooktanov benzin (NOB)-lek", ProductCode = 78},
                new TankMasterProduct { TankMasterProductId = 79, Name = "MTBE - stoka", ProductCode = 79},
                new TankMasterProduct { TankMasterProductId = 80, Name = "Reaktivno gorivo JET -A1", ProductCode = 80},
                new TankMasterProduct { TankMasterProductId = 81, Name = "Dizelovo gorivo-0.001%s EVRO 5 liatno", ProductCode = 81},
                new TankMasterProduct { TankMasterProductId = 82, Name = "Diesel-0.001%S-EBPO5(Summer-sin)", ProductCode = 82},
                new TankMasterProduct { TankMasterProductId = 84, Name = "Dizelovo gorivo-0.001%S EVRO 5 zimno", ProductCode = 84},
                new TankMasterProduct { TankMasterProductId = 85, Name = "Diesel-0.001%S-EBPO5(Winter-sin)", ProductCode = 85},
                new TankMasterProduct { TankMasterProductId = 88, Name = "Dizelovo gorivo (CP-16)", ProductCode = 88},
                new TankMasterProduct { TankMasterProductId = 89, Name = "Gazjol za izvynpytna tehn. 0.001%S", ProductCode = 89},
                new TankMasterProduct { TankMasterProductId = 90, Name = "Gasoil IPT -0.001%S -sin", ProductCode = 90},
                new TankMasterProduct { TankMasterProductId = 92, Name = "Gazjol za izvynpytna tehn. 0.1%S", ProductCode = 92},
                new TankMasterProduct { TankMasterProductId = 93, Name = "Gasoil IPT -0.1%S-sin", ProductCode = 93},
                new TankMasterProduct { TankMasterProductId = 95, Name = "Gorivo za diz. dv. B5 (lqtno)", ProductCode = 95},
                new TankMasterProduct { TankMasterProductId = 98, Name = "Gorivo za diz. dv. B6 (winter)", ProductCode = 98},
                new TankMasterProduct { TankMasterProductId = 101, Name = "Gorivo za diz. dv. B6 (summer)", ProductCode = 101},
                new TankMasterProduct { TankMasterProductId = 121, Name = "Gorivo za diz. dv. B6 ( Summer ) _ EKTO", ProductCode = 121},
                new TankMasterProduct { TankMasterProductId = 124, Name = "Gorivo za diz. dv. B6 ( winter ) _ EKTO", ProductCode = 124},
                new TankMasterProduct { TankMasterProductId = 129, Name = "Kotelno gorivo - 2.7%S", ProductCode = 129},
                new TankMasterProduct { TankMasterProductId = 130, Name = "Kotelno gorivo - 1.0%S", ProductCode = 130},
                new TankMasterProduct { TankMasterProductId = 131, Name = "K.G. bunker", ProductCode = 131},
                new TankMasterProduct { TankMasterProductId = 132, Name = "Mazut", ProductCode = 132},
                new TankMasterProduct { TankMasterProductId = 135, Name = "Bitum za pytni nast. tip 50/70 (BV-60)", ProductCode = 135},
                new TankMasterProduct { TankMasterProductId = 136, Name = "Bitum za pytni nast. tip 70/100 (BV-90)", ProductCode = 136},
                new TankMasterProduct { TankMasterProductId = 137, Name = "SMF", ProductCode = 137},
                new TankMasterProduct { TankMasterProductId = 138, Name = "Propan butan (SPB)", ProductCode = 138},
                new TankMasterProduct { TankMasterProductId = 139, Name = "Propan Butan (Lek)", ProductCode = 139},
                new TankMasterProduct { TankMasterProductId = 140, Name = "Propan butan (tezhak)", ProductCode = 140},
                new TankMasterProduct { TankMasterProductId = 219, Name = "K.G (AR)", ProductCode = 219},
                new TankMasterProduct { TankMasterProductId = 228, Name = "V REMONT !!!", ProductCode = -2},
                new TankMasterProduct { TankMasterProductId = 229, Name = "V REMONT S VODA !!!", ProductCode = -3},
                new TankMasterProduct { TankMasterProductId = 230, Name = "Prazen", ProductCode = -4},
                new TankMasterProduct { TankMasterProductId = 237, Name = "Propan butan (NPB)", ProductCode = 237},
                new TankMasterProduct { TankMasterProductId = 238, Name = "Propan butanova frakcia", ProductCode = 238},
                new TankMasterProduct { TankMasterProductId = 239, Name = "Propan-Propilenova Frakcia (PPF)", ProductCode = 239},
                new TankMasterProduct { TankMasterProductId = 240, Name = "Propilen", ProductCode = 240},
                new TankMasterProduct { TankMasterProductId = 241, Name = "Izo-butan", ProductCode = 241},
                new TankMasterProduct { TankMasterProductId = 242, Name = "Butan-butilenova frakcia", ProductCode = 242},
                new TankMasterProduct { TankMasterProductId = 243, Name = "Izo-butan-butilenova frakcia", ProductCode = 243},
                new TankMasterProduct { TankMasterProductId = 245, Name = "Butan", ProductCode = 245},
                new TankMasterProduct { TankMasterProductId = 249, Name = "Frakcia C5", ProductCode = 249},
                new TankMasterProduct { TankMasterProductId = 257, Name = "Benzinova frakcija ot AD ( NK-100 )", ProductCode = 257},
                new TankMasterProduct { TankMasterProductId = 258, Name = "Benzinova frakcija ot AD ( 100 - 180 )", ProductCode = 258},
                new TankMasterProduct { TankMasterProductId = 259, Name = "Benzinova frakcija ot AD", ProductCode = 259},
                new TankMasterProduct { TankMasterProductId = 260, Name = "Benzin otgon", ProductCode = 260},
                new TankMasterProduct { TankMasterProductId = 261, Name = "Benzin reformat", ProductCode = 261},
                new TankMasterProduct { TankMasterProductId = 262, Name = "Benzin alkilat", ProductCode = 262},
                new TankMasterProduct { TankMasterProductId = 263, Name = "Krekin Benzin", ProductCode = 263},
                new TankMasterProduct { TankMasterProductId = 264, Name = "MTBE", ProductCode = 264},
                new TankMasterProduct { TankMasterProductId = 265, Name = "Hidroochisten benzin ( XO - Ksiloli )", ProductCode = 265},
                new TankMasterProduct { TankMasterProductId = 266, Name = "Benzin hidrogenizat (HOB-1)", ProductCode = 266},
                new TankMasterProduct { TankMasterProductId = 268, Name = "Kerosinova frakcija AD", ProductCode = 268},
                new TankMasterProduct { TankMasterProductId = 269, Name = "Kerosinova frakcija hidrirana", ProductCode = 269},
                new TankMasterProduct { TankMasterProductId = 270, Name = "Dizelova frakcija ot AD", ProductCode = 270},
                new TankMasterProduct { TankMasterProductId = 276, Name = "Dizelova frakcija hidrirana", ProductCode = 276},
                new TankMasterProduct { TankMasterProductId = 280, Name = "Mazut parvichna destilacia", ProductCode = 280},
                new TankMasterProduct { TankMasterProductId = 281, Name = "Shiroka maslena frakcia (ShMF)", ProductCode = 281},
                new TankMasterProduct { TankMasterProductId = 282, Name = "Gudron", ProductCode = 282},
                new TankMasterProduct { TankMasterProductId = 320, Name = "Nekondicionni produkti", ProductCode = 320},
                new TankMasterProduct { TankMasterProductId = 322, Name = "Otraboteno maslo", ProductCode = 322},
                new TankMasterProduct { TankMasterProductId = 343, Name = "Uloveni neftoprodukti ( Neft )", ProductCode = 316},
                new TankMasterProduct { TankMasterProductId = 450, Name = "Gorivo za diz.dv.B6 (CP)", ProductCode = 561},
                new TankMasterProduct { TankMasterProductId = 451, Name = "Gor. za diz.dv.B6 (CP)EKTO", ProductCode = 562},
                new TankMasterProduct { TankMasterProductId = 456, Name = "Niskooktanov benzin (NOB)-heavy", ProductCode = 579},
                new TankMasterProduct { TankMasterProductId = 457, Name = "Smazvashta prisadka", ProductCode = 457},
                new TankMasterProduct { TankMasterProductId = 458, Name = "Stadis 450", ProductCode = 458},
                new TankMasterProduct { TankMasterProductId = 460, Name = "Depresatori za dizelovo gorivo", ProductCode = 460},
                new TankMasterProduct { TankMasterProductId = 466, Name = "Cetanova prisadka za dizelovo gorivo", ProductCode = 466},
                new TankMasterProduct { TankMasterProductId = 474, Name = "Himikal red. dp", ProductCode = 474}
            );

            context.SaveChanges();
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

            context.SaveChanges();
        }

        private void CreateEditReasons(CollectingDataSystemDbContext context)
        {
            context.EditReasons.AddOrUpdate(
                e => e.Id,
                new EditReason 
                { 
                    Name = "Грешно показание",
                },
                new EditReason 
                { 
                    Name = "Развален прибор",
                },
                new EditReason 
                { 
                    Name = "Направление",
                },
                new EditReason 
                { 
                    Name = "Ръчна стойност",
                },
                new EditReason 
                { 
                    Name = "Друга"
                });
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

            context.SaveChanges();
        }

        private void CreateMeasurementPoints(CollectingDataSystemDbContext context)
        {
            context.TransportTypes.AddOrUpdate(
                new TransportType
                {
                    Name = "Автотранспорт"
                },
                new TransportType
                {
                    Name = "Ж.П. Транспорт"
                },
                new TransportType
                {
                    Name = "Морски транспорт",
                },
                new TransportType
                {
                    Name = "Тръбопровод"
                },
                new TransportType
                {
                    Name = "Доставка тръбопровод"
                });
            context.SaveChanges();

            context.Ikunks.AddOrUpdate(
                new Ikunk
                {
                    Name = "ИКУНК 001 - Нова АНЕ",
                    Zones = 
                    {
                        new Zone
                        {
                            Name = "Остров 1",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001001",
                                    Name = "BGNCA00005001001001",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig
                                        {
                                            Code = 80, ProductId = 77, PhdTotalCounterTag = "TSN_KT0101_MT_GSV_P01.VT", DirectionId = 1
                                        }
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001002",
                                    Name = "BGNCA00005001001002",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                         new MeasurementPointsProductsConfig { Code = 89, ProductId = 86, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P01.VT", DirectionId = 1 }, 
                                         new MeasurementPointsProductsConfig { Code = 90, ProductId = 87, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P02.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 90, ProductId = 88, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P03.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P04.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 82, ProductId = 79, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P05.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 83, ProductId = 80, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P06.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P07.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 85, ProductId = 82, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P08.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 86, ProductId = 83, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P09.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 80, ProductId = 77, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P10.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 107, ProductId = 104, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P11.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 108, ProductId = 105, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P12.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 109, ProductId = 106, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P13.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 110, ProductId = 107, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P14.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 111, ProductId = 108, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P15.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 112, ProductId = 109, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P16.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 113, ProductId = 110, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P17.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 114, ProductId = 111, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P18.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 87, ProductId = 84, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P19.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },
                            }
                        },
                        new Zone 
                        {
                            Name = "Остров 2",
                            MeasurementPoints =
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001003",
                                    Name = "BGNCA00005001001003",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 121, ProductId = 118, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 122, ProductId = 119, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 123, ProductId = 120, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 124, ProductId = 121, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 125, ProductId = 122, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 126, ProductId = 123, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 101, ProductId = 98, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 102, ProductId = 99, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 103, ProductId = 100, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 104, ProductId = 101, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 105, ProductId = 102, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 106, ProductId = 103, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 533, ProductId = 256, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 534, ProductId = 257, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 535, ProductId = 258, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 536, ProductId = 259, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 537, ProductId = 260, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 538, ProductId = 261, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 539, ProductId = 262, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 540, ProductId = 263, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001004",
                                    Name = "BGNCA00005001001004",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 42, ProductId = 39, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 43, ProductId = 40, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 44, ProductId = 41, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 45, ProductId = 42, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 46, ProductId = 43, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 47, ProductId = 44, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 48, ProductId = 45, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 49, ProductId = 46, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 58, ProductId = 55, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 60, ProductId = 57, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 61, ProductId = 58, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 62, ProductId = 59, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 63, ProductId = 60, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 64, ProductId = 61, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 65, ProductId = 62, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 66, ProductId = 63, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 67, ProductId = 64, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 500, ProductId = 223, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 501, ProductId = 224, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001005",
                                    Name = "BGNCA00005001001005",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 41, ProductId = 38, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 50, ProductId = 47, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 51, ProductId = 48, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 52, ProductId = 49, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 53, ProductId = 50, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 54, ProductId = 51, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 55, ProductId = 52, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 56, ProductId = 53, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 57, ProductId = 54, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 59, ProductId = 56, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 68, ProductId = 65, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 69, ProductId = 66, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 70, ProductId = 67, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 71, ProductId = 68, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 72, ProductId = 69, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 73, ProductId = 70, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 74, ProductId = 71, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 75, ProductId = 72, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 520, ProductId = 243, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 521, ProductId = 244, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001006",
                                    Name = "BGNCA00005001001006",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 82, ProductId = 79, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 83, ProductId = 80, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 85, ProductId = 82, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 86, ProductId = 83, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 87, ProductId = 84, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 107, ProductId = 104, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 108, ProductId = 105, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 109, ProductId = 106, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 110, ProductId = 107, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 111, ProductId = 108, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 112, ProductId = 109, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 113, ProductId = 110, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 114, ProductId = 111, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 89, ProductId = 86, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 90, ProductId = 87, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 90, ProductId = 88, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 541, ProductId = 264, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                }
                            }
                        },
                        new Zone 
                        {
                            Name = "Остров 3",
                            MeasurementPoints =
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001007",
                                    Name = "BGNCA00005001001007",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 121, ProductId = 118, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 122, ProductId = 119, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 123, ProductId = 120, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 124, ProductId = 121, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 125, ProductId = 122, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 126, ProductId = 123, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 101, ProductId = 98, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 102, ProductId = 99, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 103, ProductId = 100, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 104, ProductId = 101, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 105, ProductId = 102, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 106, ProductId = 103, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 533, ProductId = 256, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 534, ProductId = 257, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 535, ProductId = 258, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 536, ProductId = 259, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 537, ProductId = 260, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 538, ProductId = 261, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 539, ProductId = 262, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 540, ProductId = 263, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001008",
                                    Name = "BGNCA00005001001008",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 42, ProductId = 39, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 43, ProductId = 40, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 44, ProductId = 41, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 45, ProductId = 42, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 46, ProductId = 43, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 47, ProductId = 44, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 48, ProductId = 45, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 49, ProductId = 46, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 58, ProductId = 55, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 60, ProductId = 57, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 61, ProductId = 58, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 62, ProductId = 59, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 63, ProductId = 60, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 64, ProductId = 61, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 65, ProductId = 62, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 66, ProductId = 63, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 67, ProductId = 64, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 500, ProductId = 223, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 501, ProductId = 224, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001009",
                                    Name = "BGNCA00005001001009",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 41, ProductId = 38, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 50, ProductId = 47, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 51, ProductId = 48, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 52, ProductId = 49, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 53, ProductId = 50, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 54, ProductId = 51, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 55, ProductId = 52, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 56, ProductId = 53, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 57, ProductId = 54, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 59, ProductId = 56, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 68, ProductId = 65, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 69, ProductId = 66, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 70, ProductId = 67, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 71, ProductId = 68, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 72, ProductId = 69, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 73, ProductId = 70, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 74, ProductId = 71, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 75, ProductId = 72, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 520, ProductId = 243, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 521, ProductId = 244, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001010",
                                    Name = "BGNCA00005001001010",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 82, ProductId = 79, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 83, ProductId = 80, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 85, ProductId = 82, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 86, ProductId = 83, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 87, ProductId = 84, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 107, ProductId = 104, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 108, ProductId = 105, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 109, ProductId = 106, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 110, ProductId = 107, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 111, ProductId = 108, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 112, ProductId = 109, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 113, ProductId = 110, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 114, ProductId = 111, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 89, ProductId = 86, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 90, ProductId = 87, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 90, ProductId = 88, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 541, ProductId = 264, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P20.VT", DirectionId = 1 }
                                    }
                                },
                            }
                        },
                        new Zone 
                        {
                            Name = "Остров 4",
                            MeasurementPoints =
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001011",
                                    Name = "BGNCA00005001001011",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 121, ProductId = 118, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 122, ProductId = 119, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 123, ProductId = 120, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 124, ProductId = 121, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 125, ProductId = 122, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 126, ProductId = 123, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 101, ProductId = 98, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 102, ProductId = 99, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 103, ProductId = 100, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 104, ProductId = 101, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 105, ProductId = 102, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 106, ProductId = 103, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 561, ProductId = 284, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 562, ProductId = 285, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 535, ProductId = 258, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 536, ProductId = 259, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 537, ProductId = 260, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 538, ProductId = 261, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 539, ProductId = 262, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 540, ProductId = 263, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001012",
                                    Name = "BGNCA00005001001012",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 42, ProductId = 39, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 43, ProductId = 40, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 44, ProductId = 41, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 45, ProductId = 42, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 46, ProductId = 43, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 47, ProductId = 44, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 48, ProductId = 45, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 49, ProductId = 46, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 58, ProductId = 55, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 60, ProductId = 57, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 61, ProductId = 58, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 62, ProductId = 59, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 63, ProductId = 60, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 64, ProductId = 61, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 65, ProductId = 62, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 66, ProductId = 63, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 67, ProductId = 64, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 500, ProductId = 223, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 501, ProductId = 224, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001013",
                                    Name = "BGNCA00005001001013",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 41, ProductId = 38, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 50, ProductId = 47, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 51, ProductId = 48, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 52, ProductId = 49, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 53, ProductId = 50, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 54, ProductId = 51, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 55, ProductId = 52, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 56, ProductId = 53, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 57, ProductId = 54, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 59, ProductId = 56, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 68, ProductId = 65, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 69, ProductId = 66, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 70, ProductId = 67, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 71, ProductId = 68, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 72, ProductId = 69, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 73, ProductId = 70, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 74, ProductId = 71, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 75, ProductId = 72, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 520, ProductId = 243, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 521, ProductId = 244, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001014",
                                    Name = "BGNCA00005001001014",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 82, ProductId = 79, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 83, ProductId = 80, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 85, ProductId = 82, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 86, ProductId = 83, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 87, ProductId = 84, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 107, ProductId = 104, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 108, ProductId = 105, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 109, ProductId = 106, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 110, ProductId = 107, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 111, ProductId = 108, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 112, ProductId = 109, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 113, ProductId = 110, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 114, ProductId = 111, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 89, ProductId = 86, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 90, ProductId = 87, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 90, ProductId = 88, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 541, ProductId = 264, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },
                            }
                        },

                        new Zone 
                        {
                            Name = "Остров 5",
                            MeasurementPoints =
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001019",
                                    Name = "BGNCA00005001001019",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 472, ProductId = 442, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P01.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 473, ProductId = 443, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P02.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 491, ProductId = 523, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P03.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 492, ProductId = 524, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P04.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 493, ProductId = 525, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P05.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 494, ProductId = 526, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P06.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 901, ProductId = 532, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P07.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 902, ProductId = 533, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P08.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 903, ProductId = 534, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P09.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 904, ProductId = 535, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P10.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 910, ProductId = 536, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P11.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 911, ProductId = 537, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P12.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 912, ProductId = 538, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P13.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 913, ProductId = 539, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P14.VT", DirectionId = 2 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001020",
                                    Name = "BGNCA00005001001020",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                         new MeasurementPointsProductsConfig { Code = 481, ProductId = 451, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P01.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 482, ProductId = 452, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P02.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 483, ProductId = 453, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P03.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 484, ProductId = 454, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P04.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 485, ProductId = 455, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P05.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 486, ProductId = 456, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P06.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 487, ProductId = 457, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P07.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 488, ProductId = 458, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P08.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 489, ProductId = 459, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P09.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 490, ProductId = 460, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P10.VT", DirectionId = 2 },
                                    }
                                }
                            }
                        }
                    }
                },
                new Ikunk
                {
                    Name = "ИКУНК 02 - Втечнен газ",
                    Zones = 
                    {
                        new Zone
                        {
                            Name = "РП Втечнени газове",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT002001",
                                    Name = "BGNCA00005001002001",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 332, ProductId = 205, PhdTotalCounterTag = "KT0201_TT_OUT_09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 141, ProductId = 205, PhdTotalCounterTag = "KT0201_TT_OUT_04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 143, ProductId = 205, PhdTotalCounterTag = "KT0201_TT_OUT_06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 142, ProductId = 205, PhdTotalCounterTag = "KT0201_TT_OUT_05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 144, ProductId = 205, PhdTotalCounterTag = "KT0201_TT_OUT_07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 140, ProductId = 205, PhdTotalCounterTag = "KT0201_TT_OUT_03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 139, ProductId = 205, PhdTotalCounterTag = "KT0201_TT_OUT_02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 138, ProductId = 205, PhdTotalCounterTag = "KT0201_TT_OUT_01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 208, ProductId = 205, PhdTotalCounterTag = "KT0201_TT_OUT_08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 210, ProductId = 207, PhdTotalCounterTag = "KT0201_TT_OUT_10.VT", DirectionId = 1 },

                                        new MeasurementPointsProductsConfig { Code = 332,  ProductId = 205, PhdTotalCounterTag = "KT0201_TT_IN_09.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 141,  ProductId = 205, PhdTotalCounterTag = "KT0201_TT_IN_04.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 143,  ProductId = 205, PhdTotalCounterTag = "KT0201_TT_IN_06.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 142,  ProductId = 205, PhdTotalCounterTag = "KT0201_TT_IN_05.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 144,  ProductId = 205, PhdTotalCounterTag = "KT0201_TT_IN_07.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 140,  ProductId = 205, PhdTotalCounterTag = "KT0201_TT_IN_03.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 139,  ProductId = 205, PhdTotalCounterTag = "KT0201_TT_IN_02.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 138,  ProductId = 205, PhdTotalCounterTag = "KT0201_TT_IN_01.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 208, ProductId = 205, PhdTotalCounterTag = "KT0201_TT_IN_08.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 210, ProductId = 207, PhdTotalCounterTag = "KT0201_TT_IN_10.VT", DirectionId = 2 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT002002",
                                    Name = "BGNCA00005001002002",
                                    TransportTypeId = 2,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 141,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_OUT_04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 143,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_OUT_06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 142,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_OUT_05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 144,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_OUT_07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 140,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_OUT_03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 138,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_OUT_01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 139,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_OUT_02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 146,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_OUT_10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 208, ProductId = 205, PhdTotalCounterTag = "KT0202_TT_OUT_08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 145,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_OUT_09.VT", DirectionId = 1 },

                                        new MeasurementPointsProductsConfig { Code = 141,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_IN_04.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 143,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_IN_06.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 142,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_IN_05.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 144,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_IN_07.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 140,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_IN_03.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 139,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_IN_02.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 138,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_IN_01.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 146,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_IN_10.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 208, ProductId = 205, PhdTotalCounterTag = "KT0202_TT_IN_08.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 145,  ProductId = 205, PhdTotalCounterTag = "KT0202_TT_IN_09.VT", DirectionId = 2 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT002003",
                                    Name = "BGNCA00005001002003",
                                    TransportTypeId = 2,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 138, ProductId = 205, PhdTotalCounterTag = "KT0203_TT_IN_01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 139, ProductId = 205, PhdTotalCounterTag = "KT0203_TT_IN_02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 140, ProductId = 205, PhdTotalCounterTag = "KT0203_TT_IN_03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 141, ProductId = 205, PhdTotalCounterTag = "KT0203_TT_IN_04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 142, ProductId = 205, PhdTotalCounterTag = "KT0203_TT_IN_05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 143, ProductId = 205, PhdTotalCounterTag = "KT0203_TT_IN_06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 144, ProductId = 205, PhdTotalCounterTag = "KT0203_TT_IN_07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 208, ProductId = 205, PhdTotalCounterTag = "KT0203_TT_IN_08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 145,  ProductId = 205, PhdTotalCounterTag = "KT0203_TT_IN_09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 146,  ProductId = 205, PhdTotalCounterTag = "KT0203_TT_IN_10.VT", DirectionId = 1 },

                                        new MeasurementPointsProductsConfig { Code = 138,  ProductId = 205, PhdTotalCounterTag = "KT0203_TT_OUT_01.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 139,  ProductId = 205, PhdTotalCounterTag = "KT0203_TT_OUT_02.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 140,  ProductId = 205, PhdTotalCounterTag = "KT0203_TT_OUT_03.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 141,  ProductId = 205, PhdTotalCounterTag = "KT0203_TT_OUT_04.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 142,  ProductId = 205, PhdTotalCounterTag = "KT0203_TT_OUT_05.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 143,  ProductId = 205, PhdTotalCounterTag = "KT0203_TT_OUT_06.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 144,  ProductId = 205, PhdTotalCounterTag = "KT0203_TT_OUT_07.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 208, ProductId = 205, PhdTotalCounterTag = "KT0203_TT_OUT_08.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 145,  ProductId = 205, PhdTotalCounterTag = "KT0203_TT_OUT_09.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 146,  ProductId = 205, PhdTotalCounterTag = "KT0203_TT_OUT_10.VT", DirectionId = 2 },
                                    }
                                }
                            }
                        },
                        new Zone
                        {
                            Name = "Парк 8Х5000",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT002004",
                                    Name = "BGNCA00005001002004",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 45, ProductId = 427, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R1_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 49, ProductId = 468, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R2_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 45, ProductId = 429, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R3_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 46, ProductId = 430, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R4_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 46, ProductId = 436, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R5_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 48, ProductId = 450, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R6_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 47, ProductId = 445, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R7_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 47, ProductId = 446, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R8_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 47, ProductId = 447, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R9_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 47, ProductId = 448, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R10_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 47, ProductId = 449, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R11_V.VT", DirectionId = 2 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT002005",
                                    Name = "BGNCA00005001002005",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 45, ProductId = 427, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R1_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 49, ProductId = 468, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R2_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 45, ProductId = 429, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R3_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 46, ProductId = 430, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R4_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 46, ProductId = 436, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R5_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 48, ProductId = 450, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R6_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 47, ProductId = 445, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R7_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 47, ProductId = 446, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R8_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 47, ProductId = 447, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R9_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 47, ProductId = 448, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R10_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 47, ProductId = 449, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R11_V.VT", DirectionId = 2 },
                                    }
                                }
                            }
                        }
                    }
                },
                new Ikunk
                {
                    Name = "ИКУНК 03 - База Камено",
                    Zones = 
                    {
                        new Zone
                        {
                            Name = "База Камено",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT003001",
                                    Name = "BGNCA00005001003001",
                                    TransportTypeId = 4,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "TSN_KT0301_QN1_T.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "TSN_KT0301_QN2_T.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "TSN_KT0301_QN3_T.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 89, ProductId = 86, PhdTotalCounterTag = "TSN_KT0301_QN4_T.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "TSN_KT0301_QN5_T.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "TSN_KT0301_QN1_TN.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "TSN_KT0301_QN2_TN.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "TSN_KT0301_QN3_TN.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 89, ProductId = 86, PhdTotalCounterTag = "TSN_KT0301_QN4_TN.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "TSN_KT0301_QN5_TN.VT", DirectionId = 2 },
                                    }
                                }
                            }
                        }
                    }
                },
                new Ikunk
                {
                    Name = "ИКУНК 03 - База Камено",
                    Zones = 
                    {
                        new Zone
                        {
                            Name = "15 коловоз",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT051002",
                                    Name = "BGNCA00005001051002",
                                    TransportTypeId = 2,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "TSN_KT0502_MT_GSV_R1_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "TSN_KT0502_MT_GSV_R2_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 87, ProductId = 84, PhdTotalCounterTag = "TSN_KT0502_MT_GSV_R3_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "TSN_KT0502_MT_GSV_R4_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 89, ProductId = 86, PhdTotalCounterTag = "TSN_KT0502_MT_GSV_R5_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 127,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0502_MT_GSV_R6_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 128,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0502_MT_GSV_R7_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 544,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0502_MT_GSV_R8_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 545,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0502_MT_GSV_R9_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 546,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0502_MT_GSV_R10_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT051005",
                                    Name = "BGNCA00005001051005",
                                    TransportTypeId = 2,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 101, ProductId = 98, PhdTotalCounterTag = "TSN_KT0505_MT_GSV_R1_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 104, ProductId = 101, PhdTotalCounterTag = "TSN_KT0505_MT_GSV_R2_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "TSN_KT0505_MT_GSV_R3_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "TSN_KT0505_MT_GSV_R4_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 533, ProductId = 256, PhdTotalCounterTag = "TSN_KT0505_MT_GSV_R5_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 534, ProductId = 257, PhdTotalCounterTag = "TSN_KT0505_MT_GSV_R6_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 535, ProductId = 258, PhdTotalCounterTag = "TSN_KT0505_MT_GSV_R7_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 536, ProductId = 259, PhdTotalCounterTag = "TSN_KT0505_MT_GSV_R8_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 537, ProductId = 260, PhdTotalCounterTag = "TSN_KT0505_MT_GSV_R9_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 538, ProductId = 261, PhdTotalCounterTag = "TSN_KT0505_MT_GSV_R10_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT051006",
                                    Name = "BGNCA00005001051006",
                                    TransportTypeId = 2,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "TSN_KT0506_MT_GSV_R1_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 78, ProductId = 75, PhdTotalCounterTag = "TSN_KT0506_MT_GSV_R2_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 41, ProductId = 38, PhdTotalCounterTag = "TSN_KT0506_MT_GSV_R3_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 50, ProductId = 47, PhdTotalCounterTag = "TSN_KT0506_MT_GSV_R4_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 51, ProductId = 48, PhdTotalCounterTag = "TSN_KT0506_MT_GSV_R5_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 52, ProductId = 49, PhdTotalCounterTag = "TSN_KT0506_MT_GSV_R6_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 53, ProductId = 50, PhdTotalCounterTag = "TSN_KT0506_MT_GSV_R7_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 54, ProductId = 51, PhdTotalCounterTag = "TSN_KT0506_MT_GSV_R8_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 55, ProductId = 52, PhdTotalCounterTag = "TSN_KT0506_MT_GSV_R9_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 56, ProductId = 53, PhdTotalCounterTag = "TSN_KT0506_MT_GSV_R10_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 57, ProductId = 54, PhdTotalCounterTag = "TSN_KT0506_MT_GSV_R11_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 579,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0506_MT_GSV_R12_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 501, ProductId = 224, PhdTotalCounterTag = "TSN_KT0506_MT_GSV_R13_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 502,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0506_MT_GSV_R14_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 503,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0506_MT_GSV_R15_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT051007",
                                    Name = "BGNCA00005001051007",
                                    TransportTypeId = 2,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "TSN_KT0507_MT_GSV_R1_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 42, ProductId = 39, PhdTotalCounterTag = "TSN_KT0507_MT_GSV_R2_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 43, ProductId = 40, PhdTotalCounterTag = "TSN_KT0507_MT_GSV_R3_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 44, ProductId = 41, PhdTotalCounterTag = "TSN_KT0507_MT_GSV_R4_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 45, ProductId = 42, PhdTotalCounterTag = "TSN_KT0507_MT_GSV_R5_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 46, ProductId = 43, PhdTotalCounterTag = "TSN_KT0507_MT_GSV_R6_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 47, ProductId = 44, PhdTotalCounterTag = "TSN_KT0507_MT_GSV_R7_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 48, ProductId = 45, PhdTotalCounterTag = "TSN_KT0507_MT_GSV_R8_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 49, ProductId = 46, PhdTotalCounterTag = "TSN_KT0507_MT_GSV_R9_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 500, ProductId = 223, PhdTotalCounterTag = "TSN_KT0507_MT_GSV_R10_V.VT", DirectionId = 1 },
                                    }
                                }
                            }
                        },
                        new Zone
                        {
                            Name = "16 коловоз",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT051001",
                                    Name = "BGNCA00005001051001",
                                    TransportTypeId = 2,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "TSN_KT0501_MT_GSV_R1_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "TSN_KT0501_MT_GSV_R2_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 87, ProductId = 84, PhdTotalCounterTag = "TSN_KT0501_MT_GSV_R3_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "TSN_KT0501_MT_GSV_R4_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 89, ProductId = 86, PhdTotalCounterTag = "TSN_KT0501_MT_GSV_R5_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 128,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0501_MT_GSV_R6_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 127,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0501_MT_GSV_R7_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 541,  ProductId = 264, PhdTotalCounterTag = "TSN_KT0501_MT_GSV_R8_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 542,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0501_MT_GSV_R9_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 543,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0501_MT_GSV_R10_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT051003",
                                    Name = "BGNCA00005001051003",
                                    TransportTypeId = 2,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 41, ProductId = 38, PhdTotalCounterTag = "TSN_KT0503_MT_GSV_R1_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 50, ProductId = 47, PhdTotalCounterTag = "TSN_KT0503_MT_GSV_R2_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 51, ProductId = 48, PhdTotalCounterTag = "TSN_KT0503_MT_GSV_R3_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 52, ProductId = 49, PhdTotalCounterTag = "TSN_KT0503_MT_GSV_R4_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 53, ProductId = 50, PhdTotalCounterTag = "TSN_KT0503_MT_GSV_R5_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 54, ProductId = 51, PhdTotalCounterTag = "TSN_KT0503_MT_GSV_R6_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 55, ProductId = 52, PhdTotalCounterTag = "TSN_KT0503_MT_GSV_R7_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 56, ProductId = 53, PhdTotalCounterTag = "TSN_KT0503_MT_GSV_R8_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 57, ProductId = 54, PhdTotalCounterTag = "TSN_KT0503_MT_GSV_R9_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 520, ProductId = 243, PhdTotalCounterTag = "TSN_KT0503_MT_GSV_R10_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT051004",
                                    Name = "BGNCA00005001051004",
                                    TransportTypeId = 2,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 101, ProductId = 98, PhdTotalCounterTag = "TSN_KT0504_MT_GSV_R1_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 104, ProductId = 101, PhdTotalCounterTag = "TSN_KT0504_MT_GSV_R2_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "TSN_KT0504_MT_GSV_R3_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "TSN_KT0504_MT_GSV_R4_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 533, ProductId = 256, PhdTotalCounterTag = "TSN_KT0504_MT_GSV_R5_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 534, ProductId = 257, PhdTotalCounterTag = "TSN_KT0504_MT_GSV_R6_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 535, ProductId = 258, PhdTotalCounterTag = "TSN_KT0504_MT_GSV_R7_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 536, ProductId = 259, PhdTotalCounterTag = "TSN_KT0504_MT_GSV_R8_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 537, ProductId = 260, PhdTotalCounterTag = "TSN_KT0504_MT_GSV_R9_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 538, ProductId = 261, PhdTotalCounterTag = "TSN_KT0504_MT_GSV_R10_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT051008",
                                    Name = "BGNCA00005001051008",
                                    TransportTypeId = 2,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "TSN_KT0508_MT_GSV_R1_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 42, ProductId = 39, PhdTotalCounterTag = "TSN_KT0508_MT_GSV_R2_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 43, ProductId = 40, PhdTotalCounterTag = "TSN_KT0508_MT_GSV_R3_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 44, ProductId = 41, PhdTotalCounterTag = "TSN_KT0508_MT_GSV_R4_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 45, ProductId = 42, PhdTotalCounterTag = "TSN_KT0508_MT_GSV_R5_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 46, ProductId = 43, PhdTotalCounterTag = "TSN_KT0508_MT_GSV_R6_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 47, ProductId = 44, PhdTotalCounterTag = "TSN_KT0508_MT_GSV_R7_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 48, ProductId = 45, PhdTotalCounterTag = "TSN_KT0508_MT_GSV_R8_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 49, ProductId = 46, PhdTotalCounterTag = "TSN_KT0508_MT_GSV_R9_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 500, ProductId = 223, PhdTotalCounterTag = "TSN_KT0508_MT_GSV_R10_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT051009",
                                    Name = "BGNCA00005001051009",
                                    TransportTypeId = 2,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                         new MeasurementPointsProductsConfig { Code = 80, ProductId = 77, PhdTotalCounterTag = "TSN_KT0509_MT_GSV_R1_V.VT", DirectionId = 1 },
                                    }
                                }
                            }
                        },
                        new Zone
                        {
                            Name = "17 коловоз",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT051010",
                                    Name = "BGNCA00005001051010",
                                    TransportTypeId = 2,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 4, ProductId = 5, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R1_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 6, ProductId = 7, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R2_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 7, ProductId = 8, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R3_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R4_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 41, ProductId = 38, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R5_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 42, ProductId = 39, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R6_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 43, ProductId = 40, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R7_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 44, ProductId = 41, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R8_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 45, ProductId = 42, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R9_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 47, ProductId = 44, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R10_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 78, ProductId = 75, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R11_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 50, ProductId = 47, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R12_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 51, ProductId = 48, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R13_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 52, ProductId = 49, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R14_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 53, ProductId = 50, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R15_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 54, ProductId = 51, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R16_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 58, ProductId = 55, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R17_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 59, ProductId = 56, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R18_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 47, ProductId = 44, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R19_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 48, ProductId = 45, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R20_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 49, ProductId = 46, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R21_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 55, ProductId = 52, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R22_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 56, ProductId = 53, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R23_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 57, ProductId = 54, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R24_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 79, ProductId = 76, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R25_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 60, ProductId = 57, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R26_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 61, ProductId = 58, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R27_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 62, ProductId = 59, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R28_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 63, ProductId = 60, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R29_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 64, ProductId = 61, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R30_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 65, ProductId = 62, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R31_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 66, ProductId = 63, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R32_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 67, ProductId = 64, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R33_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 68, ProductId = 65, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R34_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 69, ProductId = 66, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R35_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 70, ProductId = 67, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R36_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 71, ProductId = 68, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R37_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 72, ProductId = 69, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R38_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 73, ProductId = 70, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R39_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 74, ProductId = 71, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R40_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 75, ProductId = 72, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R41_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 579,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R42_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 501, ProductId = 224, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R43_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 502,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R44_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 503,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R45_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 504,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R46_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 520, ProductId = 243, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R47_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 521, ProductId = 244, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R48_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 522,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R49_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 524,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0510_MT_GSV_R50_V.VT", DirectionId = 2 },
                                    }
                                }
                            }
                        },
                        new Zone
                        {
                            Name = "18 коловоз",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT051011",
                                    Name = "BGNCA00005001051011",
                                    TransportTypeId = 2,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 5, ProductId = 6, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R1_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 45, ProductId = 429, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R2_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 46, ProductId = 430, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R3_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 80, ProductId = 77, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R4_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R5_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 82, ProductId = 79, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R6_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 83, ProductId = 80, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R7_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R8_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 85, ProductId = 82, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R9_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 86, ProductId = 83, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R10_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 87, ProductId = 84, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R11_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R12_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 89, ProductId = 86, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R13_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 90, ProductId = 87, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R14_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 90, ProductId = 88, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R15_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 92, ProductId = 89, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R16_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 93, ProductId = 90, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R17_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 94, ProductId = 91, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R18_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 101, ProductId = 98, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R19_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 102, ProductId = 99, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R20_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 103, ProductId = 100, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R21_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 104, ProductId = 101, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R22_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 105, ProductId = 102, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R23_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 106, ProductId = 103, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R24_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 107, ProductId = 104, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R25_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 108, ProductId = 105, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R26_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 109, ProductId = 106, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R27_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 110, ProductId = 107, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R28_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 111, ProductId = 108, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R29_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 112, ProductId = 109, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R30_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 113, ProductId = 110, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R31_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 114, ProductId = 111, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R32_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 127,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R33_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 128,  ProductId = 205, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R34_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 25, ProductId = 26, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R35_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 121, ProductId = 118, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R36_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 122, ProductId = 119, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R37_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 123, ProductId = 120, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R38_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 124, ProductId = 121, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R39_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 125, ProductId = 122, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R40_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 126, ProductId = 123, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R41_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 533, ProductId = 256, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R42_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 534, ProductId = 257, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R43_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 535, ProductId = 258, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R44_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 536, ProductId = 259, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R45_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 537, ProductId = 260, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R46_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 538, ProductId = 261, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R47_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 539, ProductId = 262, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R48_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 540, ProductId = 263, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R49_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 541, ProductId = 264, PhdTotalCounterTag = "TSN_KT0511_MT_GSV_R50_V.VT", DirectionId = 2 },
                                    }
                                }
                            }
                        }
                    }
                },
                new Ikunk
                {
                    Name = "ИКУНК 052 - Втори ходови коловоз",
                    Zones = 
                    {
                        new Zone
                        {
                            Name = "Втори ходови коловоз",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT052012",
                                    Name = "BGNCA00005001052012",
                                    TransportTypeId = 2,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 137,  ProductId = 205, PhdTotalCounterTag = "KT0512_TT_OUT_06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 24, ProductId = 25, PhdTotalCounterTag = "KT0512_TT_OUT_07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 319, ProductId = 376, PhdTotalCounterTag = "KT0512_TT_OUT_04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 132, ProductId = 129, PhdTotalCounterTag = "KT0512_TT_OUT_05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 214, ProductId = 210, PhdTotalCounterTag = "KT0512_TT_OUT_10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 213, ProductId = 209, PhdTotalCounterTag = "KT0512_TT_OUT_09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 212, ProductId = 208, PhdTotalCounterTag = "KT0512_TT_OUT_08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT0512_TT_OUT_01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 28, ProductId = 29, PhdTotalCounterTag = "KT0512_TT_OUT_03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 220, ProductId = 216, PhdTotalCounterTag = "KT0512_TT_OUT_02.VT", DirectionId = 1 },

                                        new MeasurementPointsProductsConfig { Code = 137,  ProductId = 205, PhdTotalCounterTag = "KT0512_TT_IN_06.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 24, ProductId = 25, PhdTotalCounterTag = "KT0512_TT_IN_07.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 319, ProductId = 376, PhdTotalCounterTag = "KT0512_TT_IN_04.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 132, ProductId = 129, PhdTotalCounterTag = "KT0512_TT_IN_05.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 214, ProductId = 210, PhdTotalCounterTag = "KT0512_TT_IN_10.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 213, ProductId = 209, PhdTotalCounterTag = "KT0512_TT_IN_09.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 212, ProductId = 208, PhdTotalCounterTag = "KT0512_TT_IN_08.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT0512_TT_IN_01.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 28, ProductId = 29, PhdTotalCounterTag = "KT0512_TT_IN_03.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 220, ProductId = 216, PhdTotalCounterTag = "KT0512_TT_IN_02.VT", DirectionId = 2 },
                                    }
                                }
                            }
                        }
                    }
                },
                new Ikunk
                {
                    Name = "ИКУНК 06 - Битумна естакада",
                    Zones = 
                    {
                        new Zone
                        {
                            Name = "Парк Битуми",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT006001",
                                    Name = "BGNCA00005001006001",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 319, ProductId = 376, PhdTotalCounterTag = "KT0601_TT_OUT_04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 332,  ProductId = 205, PhdTotalCounterTag = "KT0601_TT_OUT_10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT0601_TT_OUT_01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 15, ProductId = 16, PhdTotalCounterTag = "KT0601_TT_OUT_05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 28, ProductId = 29, PhdTotalCounterTag = "KT0601_TT_OUT_03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 220, ProductId = 216, PhdTotalCounterTag = "KT0601_TT_OUT_02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 133, ProductId = 130, PhdTotalCounterTag = "KT0601_TT_OUT_09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 148,  ProductId = 205, PhdTotalCounterTag = "KT0601_TT_OUT_07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 147, ProductId = 205,  PhdTotalCounterTag = "KT0601_TT_OUT_06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 209, ProductId = 206, PhdTotalCounterTag = "KT0601_TT_OUT_08.VT", DirectionId = 1 },



                                        new MeasurementPointsProductsConfig { Code = 319, ProductId = 376, PhdTotalCounterTag = "KT0601_TT_IN_04.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 332,  ProductId = 205, PhdTotalCounterTag = "KT0601_TT_IN_10.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT0601_TT_IN_01.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 15, ProductId = 16, PhdTotalCounterTag = "KT0601_TT_IN_05.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 28, ProductId = 29, PhdTotalCounterTag = "KT0601_TT_IN_03.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 220, ProductId = 216, PhdTotalCounterTag = "KT0601_TT_IN_02.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 133, ProductId = 130, PhdTotalCounterTag = "KT0601_TT_IN_09.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 148,  ProductId = 205, PhdTotalCounterTag = "KT0601_TT_IN_07.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 147,  ProductId = 205, PhdTotalCounterTag = "KT0601_TT_IN_06.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 209, ProductId = 206, PhdTotalCounterTag = "KT0601_TT_IN_08.VT", DirectionId = 2 },
                                    }
                                }
                            }
                        },
                        new Zone
                        {
                            Name = "Адитиви Битумна Естакада",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT006002",
                                    Name = "BGNCA00005001006002",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {

                                    }
                                }
                            }
                        },
                         new Zone
                        {
                            Name = "Маслено стопанство",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT006003",
                                    Name = "BGNCA00005001006003",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 322, ProductId = 379, PhdTotalCounterTag = "TSN_KT0603_QN1_T.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 349, ProductId = 394, PhdTotalCounterTag = "TSN_KT0603_QN2_T.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 354, ProductId = 399, PhdTotalCounterTag = "TSN_KT0603_QN3_T.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 355, ProductId = 400, PhdTotalCounterTag = "TSN_KT0603_QN4_T.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 361, ProductId = 406, PhdTotalCounterTag = "TSN_KT0603_QN5_T.VT", DirectionId = 1 },

                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT006004",
                                    Name = "BGNCA00005001006004",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 322, ProductId = 379, PhdTotalCounterTag = "TSN_KT0604_QN1_T.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 349, ProductId = 394, PhdTotalCounterTag = "TSN_KT0604_QN2_T.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 354, ProductId = 399, PhdTotalCounterTag = "TSN_KT0604_QN3_T.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 355, ProductId = 400, PhdTotalCounterTag = "TSN_KT0604_QN4_T.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 361, ProductId = 406, PhdTotalCounterTag = "TSN_KT0604_QN5_T.VT", DirectionId = 2 },

                                    }
                                }
                            }
                        },
                        new Zone
                        {
                            Name = "Биоетанол",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT006005",
                                    Name = "BGNCA00005001006005",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 6, ProductId = 7, PhdTotalCounterTag = "TSN_KT0605_MT_GSV_01_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 7, ProductId = 8, PhdTotalCounterTag = "TSN_KT0605_MT_GSV_02_V.VT", DirectionId = 2 },
                                    }
                                }
                            }
                        },
                    }
                },
                new Ikunk
                {
                    Name = "ИКУНК 07 - Полипропилен",
                    Zones = 
                    {
                        new Zone
                        {
                            Name = "Полипропилен",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT007001",
                                    Name = "BGNCA00005001007001",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 22, ProductId = 23, PhdTotalCounterTag = "TSN_KT0701_1_QN1_T.VT", DirectionId = 2 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT007002",
                                    Name = "BGNCA00005001007002",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 240, ProductId = 319, PhdTotalCounterTag = "TSN_KT0701_2_QN1_T.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT007003",
                                    Name = "BGNCA00005001007003",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 21, ProductId = 22, PhdTotalCounterTag = "TSN_KT0701_3_QN1_T.VT", DirectionId = 2 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT007004",
                                    Name = "BGNCA00005001007004",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 240, ProductId = 319, PhdTotalCounterTag = "TSN_KT0701_4_QN1_T.VT", DirectionId = 2 },
                                    }
                                }
                            }
                        }
                    }
                },
                new Ikunk
                {
                    Name = "ИКУНК 08 - ВОН",
                    Zones = 
                    {
                        new Zone
                        {
                            Name = "ВОН",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ008010",
                                    Name = "BGNCA00005002008810",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 1, ProductId = 2, PhdTotalCounterTag = "KT0810_F001_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 1, ProductId = 2, PhdTotalCounterTag = "KT0810_F001_QBN_V.VT", DirectionId = 2 },
                                    }
                                }
                            }
                        }
                    }
                },
                new Ikunk
                {
                    Name = "ИКУНК 09 (ПИРС3)",
                    Zones = 
                    {
                        new Zone
                        {
                            Name = "ИКУНК 09 (ПИРС3)",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ009001",
                                    Name = "BGNCA00005001009001",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                         new MeasurementPointsProductsConfig { Code = 14, ProductId = 15, PhdTotalCounterTag = "KT0901_R014.VOL15_PROD1.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 219, ProductId = 215, PhdTotalCounterTag = "KT0901_R129.VOL15_PROD2.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT0901_R130.VOL15_PROD3.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 15, ProductId = 16, PhdTotalCounterTag = "KT0901_R015.VOL15_PROD4.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 28, ProductId = 29, PhdTotalCounterTag = "KT0901_R028.VOL15_PROD5.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ009002",
                                    Name = "BGNCA00005001009002",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "KT0902_R081.VOL15_PROD1.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 82, ProductId = 79, PhdTotalCounterTag = "KT0902_R082.VOL15_PROD2.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "KT0902_R084.VOL15_PROD3.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 85, ProductId = 82, PhdTotalCounterTag = "KT0902_R085.VOL15_PROD4.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 8, ProductId = 9, PhdTotalCounterTag = "KT0902_R008.VOL15_PROD5.VT", DirectionId = 1 },
                                    }
                                },
                                //new MeasurementPoint
                                //{
                                //    ControlPoint = "КТ009004",
                                //    Name = "BGNCA00005001009004",
                                //    TransportTypeId = 3,
                                //    MeasurementPointsProductsConfigs = 
                                //    {
                                //    }
                                //},
                                //new MeasurementPoint
                                //{
                                //    ControlPoint = "КТ009005",
                                //    Name = "BGNCA00005001009005",
                                //    TransportTypeId = 3,
                                //    MeasurementPointsProductsConfigs = 
                                //    {
                                //    }
                                //},
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ009006",
                                    Name = "BGNCA00005001009006",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 15, ProductId = 16, PhdTotalCounterTag = "KT0906_R015.VOL15_PROD1.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ009007",
                                    Name = "BGNCA00005001009007",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 22, ProductId = 23, PhdTotalCounterTag = "KT0907_F022.2VOL15_PROD1.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 22, ProductId = 23, PhdTotalCounterTag = "KT0907_R022.VOL15_PROD1.VT", DirectionId = 2 },
                                    }
                                }
                            }
                        }
                    }
                },
                 new Ikunk
                {
                    Name = "ИКУНК 21 (ПИРС 1)",
                    Zones = 
                    {
                        new Zone
                        {
                            Name = "ИКУНК 21 (ПИРС 1)",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ021001",
                                    Name = "BGNCA00005001021001",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                         new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT2101_F038_QAN_V.VT", DirectionId = 1 },	
                                         new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT2101_F038_QBN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 39, ProductId = 36, PhdTotalCounterTag = "KT2101_F039_QAN_V.VT", DirectionId = 1 },	
                                         new MeasurementPointsProductsConfig { Code = 39, ProductId = 36, PhdTotalCounterTag = "KT2101_F039_QBN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT2101_F040_QAN_V.VT", DirectionId = 1 },	
                                         new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT2101_F040_QBN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 76, ProductId = 73, PhdTotalCounterTag = "KT2101_F076_QAN_V.VT", DirectionId = 1 },	
                                         new MeasurementPointsProductsConfig { Code = 76, ProductId = 73, PhdTotalCounterTag = "KT2101_F076_QBN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT2101_F077_QAN_V.VT", DirectionId = 1 },	
                                         new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT2101_F077_QBN_V.VT", DirectionId = 1 },

                                         new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT2101_R038_QAN_V.VT", DirectionId = 2 },	
                                         new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "КT2101_R038_QBN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 39, ProductId = 36, PhdTotalCounterTag = "KT2101_R039_QAN_V.VT", DirectionId = 2 },	
                                         new MeasurementPointsProductsConfig { Code = 39, ProductId = 36, PhdTotalCounterTag = "КT2101_R039_QBN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT2101_R040_QAN_V.VT", DirectionId = 2 },	
                                         new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "КT2101_R040_QBN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 76, ProductId = 73, PhdTotalCounterTag = "KT2101_R076_QAN_V.VT", DirectionId = 2 },	
                                         new MeasurementPointsProductsConfig { Code = 76, ProductId = 73, PhdTotalCounterTag = "КТ2101_R076_QBN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT2101_R077_QAN_V.VT", DirectionId = 2 },	
                                         new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT2101_R077_QBN_V.VT", DirectionId = 2 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ021002",
                                    Name = "BGNCA00005001021002",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT2102_R038_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 39, ProductId = 36, PhdTotalCounterTag = "KT2102_R039_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT2102_R040_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 76, ProductId = 73, PhdTotalCounterTag = "KT2102_R076_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT2102_R077_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT2102_R038_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 39, ProductId = 36, PhdTotalCounterTag = "KT2102_R039_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT2102_R040_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 76, ProductId = 73, PhdTotalCounterTag = "KT2102_R076_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT2102_R077_QBN_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ021003",
                                    Name = "BGNCA00005001021003",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 78, ProductId = 75, PhdTotalCounterTag = "KT2103_F078_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 78, ProductId = 75, PhdTotalCounterTag = "KT2103_F078_QBN_V.VT", DirectionId = 2 },

                                        new MeasurementPointsProductsConfig { Code = 78, ProductId = 75, PhdTotalCounterTag = "KT2103_R078_QAN_V.VT", DirectionId = 1 },	
                                        new MeasurementPointsProductsConfig { Code = 78, ProductId = 75, PhdTotalCounterTag = "KT2103_R078_QBN_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ021004",
                                    Name = "BGNCA00005001021004",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "KT2104_F081_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "KT2104_F081_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "KT2104_F084_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "KT2104_F084_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 87, ProductId = 84, PhdTotalCounterTag = "KT2104_F087_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 87, ProductId = 84, PhdTotalCounterTag = "KT2104_F087_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "KT2104_F088_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "KT2104_F088_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 25, ProductId = 26, PhdTotalCounterTag = "KT2104_F025_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 25, ProductId = 26, PhdTotalCounterTag = "KT2104_F025_QBN_V.VT", DirectionId = 2 },

                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "KT2104_R081_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "KT2104_R084_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 87, ProductId = 84, PhdTotalCounterTag = "KT2104_R087_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "KT2104_R088_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 25, ProductId = 26, PhdTotalCounterTag = "KT2104_R025_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "KT2104_R081_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "KT2104_R084_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 87, ProductId = 84, PhdTotalCounterTag = "KT2104_R087_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "KT2104_R088_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 25, ProductId = 26, PhdTotalCounterTag = "KT2104_R025_QBN_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ021005",
                                    Name = "BGNCA00005001021005",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 219, ProductId = 215, PhdTotalCounterTag = "KT2105_F129_QAN_V.VT", DirectionId = 2 },	
                                        new MeasurementPointsProductsConfig { Code = 219, ProductId = 215, PhdTotalCounterTag = "KT2105_F129_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT2105_F130_QAN_V.VT", DirectionId = 2 },	
                                        new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT2105_F130_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 220, ProductId = 216, PhdTotalCounterTag = "KT2105_F131_QAN_V.VT", DirectionId = 2 },	
                                        new MeasurementPointsProductsConfig { Code = 220, ProductId = 216, PhdTotalCounterTag = "KT2105_F131_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 31, ProductId = 32, PhdTotalCounterTag = "KT2105_F132_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 31, ProductId = 32, PhdTotalCounterTag = "KT2105_F132_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 29, ProductId = 30, PhdTotalCounterTag = "KT2105_F014_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 29, ProductId = 30, PhdTotalCounterTag = "KT2105_F014_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 221, ProductId = 217, PhdTotalCounterTag = "KT2105_F015_QAN_V.VT", DirectionId = 2 },	
                                        new MeasurementPointsProductsConfig { Code = 221, ProductId = 217, PhdTotalCounterTag = "KT2105_F015_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 28, ProductId = 29, PhdTotalCounterTag = "KT2105_F028_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 28, ProductId = 29, PhdTotalCounterTag = "KT2105_F028_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 30, ProductId = 31, PhdTotalCounterTag = "KT2105_F002_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 30, ProductId = 31, PhdTotalCounterTag = "KT2105_F002_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 222, ProductId = 218, PhdTotalCounterTag = "KT2105_F137_QAN_V.VT", DirectionId = 2 },	
                                        new MeasurementPointsProductsConfig { Code = 222, ProductId = 218, PhdTotalCounterTag = "KT2105_F137_QBN_V.VT", DirectionId = 2 },


                                        new MeasurementPointsProductsConfig { Code = 219, ProductId = 215, PhdTotalCounterTag = "KT2105_R129_QAN_V.VT", DirectionId = 1 },	
                                        new MeasurementPointsProductsConfig { Code = 219, ProductId = 215, PhdTotalCounterTag = "KT2105_R129_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT2105_R130_QAN_V.VT", DirectionId = 1 },	
                                        new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT2105_R130_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 220, ProductId = 216, PhdTotalCounterTag = "KT2105_R131_QAN_V.VT", DirectionId = 1 },	
                                        new MeasurementPointsProductsConfig { Code = 220, ProductId = 216, PhdTotalCounterTag = "KT2105_R131_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 31, ProductId = 32, PhdTotalCounterTag = "KT2105_R132_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 31, ProductId = 32, PhdTotalCounterTag = "KT2105_R132_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 29, ProductId = 30, PhdTotalCounterTag = "KT2105_R014_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 29, ProductId = 30, PhdTotalCounterTag = "KT2105_R014_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 221, ProductId = 217, PhdTotalCounterTag = "KT2105_R015_QAN_V.VT", DirectionId = 1 },	
                                        new MeasurementPointsProductsConfig { Code = 221, ProductId = 217, PhdTotalCounterTag = "KT2105_R015_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 28, ProductId = 29, PhdTotalCounterTag = "KT2105_R028_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 28, ProductId = 29, PhdTotalCounterTag = "KT2105_R028_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 30, ProductId = 31, PhdTotalCounterTag = "KT2105_R002_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 30, ProductId = 31, PhdTotalCounterTag = "KT2105_R002_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 222, ProductId = 218, PhdTotalCounterTag = "KT2105_R137_QAN_V.VT", DirectionId = 1 },	
                                        new MeasurementPointsProductsConfig { Code = 222, ProductId = 218, PhdTotalCounterTag = "KT2105_R137_QBN_V.VT", DirectionId = 1 },
                                    }
                                },
                                //new MeasurementPoint
                                //{
                                //    ControlPoint = "КТ021006",
                                //    Name = "BGNCA00005001021006",
                                //    TransportTypeId = 3,
                                //    MeasurementPointsProductsConfigs = 
                                //    {

                                //    }
                                //}
                            }
                        }
                    }
                },
                 new Ikunk
                {
                    Name = "ИКУНК 22 (ПИРС 2)",
                    Zones = 
                    {
                        new Zone
                        {
                            Name = "ИКУНК 22 (ПИРС 2)",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ022001",
                                    Name = "BGNCA00005001022001",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT2201_R038_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 39, ProductId = 36, PhdTotalCounterTag = "KT2201_R039_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT2201_R040_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 76, ProductId = 73, PhdTotalCounterTag = "KT2201_R076_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT2201_R077_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT2201_R038_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 39, ProductId = 36, PhdTotalCounterTag = "KT2201_R039_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT2201_R040_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 76, ProductId = 73, PhdTotalCounterTag = "KT2201_R076_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT2201_R077_QBN_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ022002",
                                    Name = "BGNCA00005001022002",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT2202_R038_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 39, ProductId = 36, PhdTotalCounterTag = "KT2202_R039_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT2202_R040_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 76, ProductId = 73, PhdTotalCounterTag = "KT2202_R076_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT2202_R077_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT2202_R038_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 39, ProductId = 36, PhdTotalCounterTag = "KT2202_R039_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT2202_R040_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 76, ProductId = 73, PhdTotalCounterTag = "KT2202_R076_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT2202_R077_QBN_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ022003",
                                    Name = "BGNCA00005001022003",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "KT2203_R081_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "KT2203_R084_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 87, ProductId = 84, PhdTotalCounterTag = "KT2203_R087_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "KT2203_R088_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 25, ProductId = 26, PhdTotalCounterTag = "KT2203_R025_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "KT2203_R081_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "KT2203_R084_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 87, ProductId = 84, PhdTotalCounterTag = "KT2203_R087_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "KT2203_R088_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 25, ProductId = 26, PhdTotalCounterTag = "KT2203_R025_QBN_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ022004",
                                    Name = "BGNCA00005001022004",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 219, ProductId = 215, PhdTotalCounterTag = "KT2204_F129_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT2204_F130_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 220, ProductId = 216, PhdTotalCounterTag = "KT2204_F131_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 31, ProductId = 32, PhdTotalCounterTag = "KT2204_F132_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 29, ProductId = 30, PhdTotalCounterTag = "KT2204_F014_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 221, ProductId = 217, PhdTotalCounterTag = "KT2204_F015_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 28, ProductId = 29, PhdTotalCounterTag = "KT2204_F028_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 30, ProductId = 31, PhdTotalCounterTag = "KT2204_F002_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 222, ProductId = 218, PhdTotalCounterTag = "KT2204_F137_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 219, ProductId = 215, PhdTotalCounterTag = "KT2204_F129_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT2204_F130_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 220, ProductId = 216, PhdTotalCounterTag = "KT2204_F131_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 31, ProductId = 32, PhdTotalCounterTag = "KT2204_F132_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 29, ProductId = 30, PhdTotalCounterTag = "KT2204_F014_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 221, ProductId = 217, PhdTotalCounterTag = "KT2204_F015_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 28, ProductId = 29, PhdTotalCounterTag = "KT2204_F028_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 30, ProductId = 31, PhdTotalCounterTag = "KT2204_F002_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 222, ProductId = 218, PhdTotalCounterTag = "KT2204_F137_QBN_V.VT", DirectionId = 2 },

                                        new MeasurementPointsProductsConfig { Code = 219, ProductId = 215, PhdTotalCounterTag = "KT2204_R129_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT2204_R130_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 220, ProductId = 216, PhdTotalCounterTag = "KT2204_R131_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 31, ProductId = 32, PhdTotalCounterTag = "KT2204_R132_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 29, ProductId = 30, PhdTotalCounterTag = "KT2204_R014_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 221, ProductId = 217, PhdTotalCounterTag = "KT2204_R015_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 28, ProductId = 29, PhdTotalCounterTag = "KT2204_R028_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 30, ProductId = 31, PhdTotalCounterTag = "KT2204_R002_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 222, ProductId = 218, PhdTotalCounterTag = "KT2204_R137_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 219, ProductId = 215, PhdTotalCounterTag = "KT2204_R129_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT2204_R130_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 220, ProductId = 216, PhdTotalCounterTag = "KT2204_R131_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 31, ProductId = 32, PhdTotalCounterTag = "KT2204_R132_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 29, ProductId = 30, PhdTotalCounterTag = "KT2204_R014_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 221, ProductId = 217, PhdTotalCounterTag = "KT2204_R015_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 28, ProductId = 29, PhdTotalCounterTag = "KT2204_R028_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 30, ProductId = 31, PhdTotalCounterTag = "KT2204_R002_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 222, ProductId = 218, PhdTotalCounterTag = "KT2204_R137_QBN_V.VT", DirectionId = 1 },
                                    }
                                }
                            }
                        }
                    }
                },
                 new Ikunk
                {
                    Name = "ИКУНК 23 (ПИРС3)",
                    Zones = 
                    {
                        new Zone
                        {
                            Name = "ИКУНК 23 (ПИРС3)",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ023001",
                                    Name = "BGNCA00005001023001",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "KT2301_R081_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "KT2301_R084_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 87, ProductId = 84, PhdTotalCounterTag = "KT2301_R087_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "KT2301_R088_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 25, ProductId = 26, PhdTotalCounterTag = "KT2301_R025_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "KT2301_R081_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "KT2301_R084_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 87, ProductId = 84, PhdTotalCounterTag = "KT2301_R087_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "KT2301_R088_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 25, ProductId = 26, PhdTotalCounterTag = "KT2301_R025_QAN_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ023002",
                                    Name = "BGNCA00005001023002",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 78, ProductId = 75, PhdTotalCounterTag = "KT2302_R078_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 78, ProductId = 75, PhdTotalCounterTag = "KT2302_R078_QBN_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ023003",
                                    Name = "BGNCA00005001023001",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT2303_R038_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 39, ProductId = 36, PhdTotalCounterTag = "KT2303_R039_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT2303_R040_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 76, ProductId = 73, PhdTotalCounterTag = "KT2303_R076_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT2303_R077_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT2303_R038_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 39, ProductId = 36, PhdTotalCounterTag = "KT2303_R039_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT2303_R040_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 76, ProductId = 73, PhdTotalCounterTag = "KT2303_R076_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT2303_R077_QBN_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ023004",
                                    Name = "BGNCA00005001023001",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT2304_R038_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 39, ProductId = 36, PhdTotalCounterTag = "KT2304_R039_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT2304_R040_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 76, ProductId = 73, PhdTotalCounterTag = "KT2304_R076_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT2304_R077_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT2304_R038_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 39, ProductId = 36, PhdTotalCounterTag = "KT2304_R039_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT2304_R040_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 76, ProductId = 73, PhdTotalCounterTag = "KT2304_R076_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT2304_R077_QBN_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ023005",
                                    Name = "BGNCA00005001023001",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 79, ProductId = 76, PhdTotalCounterTag = "KT2305_F079_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 4, ProductId = 5, PhdTotalCounterTag = "KT2305_F004_QBN_V.VT", DirectionId =  2 },
                                        new MeasurementPointsProductsConfig { Code = 41, ProductId = 38, PhdTotalCounterTag = "KT2305_F041_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT2305_F038_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT2305_F040_QBN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 79, ProductId = 76, PhdTotalCounterTag = "KT2305_F079_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 4, ProductId = 5, PhdTotalCounterTag = "KT2305_F004_QAN_V.VT", DirectionId =  2 },
                                        new MeasurementPointsProductsConfig { Code = 41, ProductId = 38, PhdTotalCounterTag = "KT2305_F041_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT2305_F038_QAN_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT2305_F040_QAN_V.VT", DirectionId = 2 },

                                        new MeasurementPointsProductsConfig { Code = 79, ProductId = 76, PhdTotalCounterTag = "KT2305_R079_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 4, ProductId = 5, PhdTotalCounterTag = "KT2305_R004_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 41, ProductId = 38, PhdTotalCounterTag = "KT2305_R041_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT2305_R038_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT2305_R040_QAN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 79, ProductId = 76, PhdTotalCounterTag = "KT2305_R079_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 4, ProductId = 5, PhdTotalCounterTag = "KT2305_R004_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 41, ProductId = 38, PhdTotalCounterTag = "KT2305_R041_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT2305_R038_QBN_V.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT2305_R040_QBN_V.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ023007",
                                    Name = "BGNCA00005001023007",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                         new MeasurementPointsProductsConfig { Code = 21, ProductId = 22, PhdTotalCounterTag = "KT2307_F021_QAN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 21, ProductId = 22, PhdTotalCounterTag = "KT2307_F021_QBN_V.VT", DirectionId = 2 },

                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ023008",
                                    Name = "BGNCA00005001023008",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                         new MeasurementPointsProductsConfig { Code = 22, ProductId = 23, PhdTotalCounterTag = "KT2308_F022_QAN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 22, ProductId = 23, PhdTotalCounterTag = "KT2308_F022_QBN_V.VT", DirectionId = 2 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ023009",
                                    Name = "BGNCA00005001023009",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                         new MeasurementPointsProductsConfig { Code = 219, ProductId = 215, PhdTotalCounterTag = "KT2309_F129_QAN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT2309_F130_QAN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 220, ProductId = 216, PhdTotalCounterTag = "KT2309_F131_QAN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 31, ProductId = 32, PhdTotalCounterTag = "KT2309_F132_QAN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 29, ProductId = 30, PhdTotalCounterTag = "KT2309_F014_QAN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 221, ProductId = 217, PhdTotalCounterTag = "KT2309_F015_QAN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 28, ProductId = 29, PhdTotalCounterTag = "KT2309_F028_QAN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 30, ProductId = 31, PhdTotalCounterTag = "KT2309_F002_QAN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 222, ProductId = 218, PhdTotalCounterTag = "KT2309_F137_QAN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 219, ProductId = 215, PhdTotalCounterTag = "KT2309_F129_QBN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT2309_F130_QBN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 220, ProductId = 216, PhdTotalCounterTag = "KT2309_F131_QBN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 31, ProductId = 32, PhdTotalCounterTag = "KT2309_F132_QBN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 29, ProductId = 30, PhdTotalCounterTag = "KT2309_F014_QBN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 221, ProductId = 217, PhdTotalCounterTag = "KT2309_F015_QBN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 28, ProductId = 29, PhdTotalCounterTag = "KT2309_F028_QBN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 30, ProductId = 31, PhdTotalCounterTag = "KT2309_F002_QBN_V.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 222, ProductId = 218, PhdTotalCounterTag = "KT2309_F137_QBN_V.VT", DirectionId = 2 },

                                         new MeasurementPointsProductsConfig { Code = 219, ProductId = 215, PhdTotalCounterTag = "KT2309_R129_QAN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT2309_R130_QAN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 220, ProductId = 216, PhdTotalCounterTag = "KT2309_R131_QAN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 31, ProductId = 32, PhdTotalCounterTag = "KT2309_R132_QAN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 29, ProductId = 30, PhdTotalCounterTag = "KT2309_R014_QAN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 221, ProductId = 217, PhdTotalCounterTag = "KT2309_R015_QAN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 28, ProductId = 29, PhdTotalCounterTag = "KT2309_R028_QAN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 30, ProductId = 31, PhdTotalCounterTag = "KT2309_R002_QAN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 222, ProductId = 218, PhdTotalCounterTag = "KT2309_R137_QAN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 219, ProductId = 215, PhdTotalCounterTag = "KT2309_R129_QBN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT2309_R130_QBN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 220, ProductId = 216, PhdTotalCounterTag = "KT2309_R131_QBN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 31, ProductId = 32, PhdTotalCounterTag = "KT2309_R132_QBN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 29, ProductId = 30, PhdTotalCounterTag = "KT2309_R014_QBN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 221, ProductId = 217, PhdTotalCounterTag = "KT2309_R015_QBN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 28, ProductId = 29, PhdTotalCounterTag = "KT2309_R028_QBN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 30, ProductId = 31, PhdTotalCounterTag = "KT2309_R002_QBN_V.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 222, ProductId = 218, PhdTotalCounterTag = "KT2309_R137_QBN_V.VT", DirectionId = 1 },
                                    }
                                }
                            }
                        }
                    }
                },
                 new Ikunk
                {
                    Name = "ИКУНК 24 (ПИРС 3)",
                    Zones = 
                    {
                        new Zone
                        {
                            Name = "ИКУНК 24 (ПИРС 3)",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ024001",
                                    Name = "BGNCA00005001024001",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 82, ProductId = 79, PhdTotalCounterTag = "KT2401_R828.VOL15_P1", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ024002_2",
                                    Name = "BGNCA00005001024002_2",
                                    TransportTypeId = 3,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 82, ProductId = 79, PhdTotalCounterTag = "KT2402_1_R082.VOL15_P1", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 85, ProductId = 82, PhdTotalCounterTag = "KT2402_1_R085.VOL15_P2", DirectionId = 1 },

                                    }
                                }
                            }
                        }
                    }
                },
                 new Ikunk
                {
                    Name = "ПТ Росенец  към ИКУНК 011",
                    Zones = 
                    {
                        new Zone
                        {
                            Name = "Магистрални тръбопроводи",
                            MeasurementPoints = 
                            {
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ011001",
                                    Name = "BGNCA00005002011001",
                                    TransportTypeId = 5,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT1101_3_F038.2VOL15_PROD1.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT1101_3_F040.2VOL15_PROD2.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "KT1101_3_F081.2VOL15_PROD3.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "KT1101_3_F084.2VOL15_PROD4.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "KT1101_3_F088.2VOL15_PROD5.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 89, ProductId = 86, PhdTotalCounterTag = "KT1101_3_F089.2VOL15_PROD5.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 585, ProductId = 287, PhdTotalCounterTag = "KT1101_3_F585.2VOL15_PROD5.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT1101_3_F077.2VOL15_PROD5.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 78, ProductId = 75, PhdTotalCounterTag = "KT1101_3_F078.2VOL15_PROD5.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 579,  ProductId = 205, PhdTotalCounterTag = "KT1101_3_F579.2VOL15_PROD5.VT", DirectionId = 2 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ011007",
                                    Name = "BGNCA00005002011007",
                                    TransportTypeId = 5,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT1101_3_R038.VOL15_PROD1.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT1101_3_R040.VOL15_PROD2.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "KT1101_3_R081.VOL15_PROD3.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "KT1101_3_R084.VOL15_PROD4.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "KT1101_3_R088.VOL15_PROD5.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 89, ProductId = 86, PhdTotalCounterTag = "KT1101_3_R089.2VOL15_PROD5.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 585, ProductId = 287, PhdTotalCounterTag = "KT1101_3_R585.2VOL15_PROD5.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT1101_3_R077.2VOL15_PROD5.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 78, ProductId = 75, PhdTotalCounterTag = "KT1101_3_R078.2VOL15_PROD5.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 579,  ProductId = 205, PhdTotalCounterTag = "KT1101_3_R579.2VOL15_PROD5.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ011002",
                                    Name = "BGNCA00005002011002",
                                    TransportTypeId = 5,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 1, ProductId = 2, PhdTotalCounterTag = "KT1102_3_F001.2VOL15_PROD1.VT", DirectionId = 2 },

                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ011008",
                                    Name = "BGNCA00005002011008",
                                    TransportTypeId = 5,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 1, ProductId = 2, PhdTotalCounterTag = "KT1102_3_R001.VOL15_PROD1.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ011003",
                                    Name = "BGNCA00005002011003",
                                    TransportTypeId = 5,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 1, ProductId = 2, PhdTotalCounterTag = "KT1103_3_F001.2VOL15_PROD1.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 219, ProductId = 215, PhdTotalCounterTag = "KT1103_3_F219.2VOL15_PROD2.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 30, ProductId = 31, PhdTotalCounterTag = "KT1103_3_F030.2VOL15_PROD3.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 29, ProductId = 30, PhdTotalCounterTag = "KT1103_3_F029.2VOL15_PROD4.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 31, ProductId = 32, PhdTotalCounterTag = "KT1103_3_F031.2VOL15_PROD5.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 221, ProductId = 217, PhdTotalCounterTag = "KT1103_3_F221.2VOL15_PROD1.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 137,  ProductId = 205, PhdTotalCounterTag = "KT1103_3_F137.2VOL15_PROD2.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 222, ProductId = 218, PhdTotalCounterTag = "KT1103_3_F222.2VOL15_PROD3.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT1103_3_F130.2VOL15_PROD4.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 132, ProductId = 129, PhdTotalCounterTag = "KT1103_3_F132.2VOL15_PROD5.VT", DirectionId = 2 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ011009",
                                    Name = "BGNCA00005002011009",
                                    TransportTypeId = 5,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 1, ProductId = 2, PhdTotalCounterTag = "KT1103_3_R001.VOL15_PROD1.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 219, ProductId = 215, PhdTotalCounterTag = "KT1103_3_R219.VOL15_PROD2.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 30, ProductId = 31, PhdTotalCounterTag = "KT1103_3_R030.VOL15_PROD3.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 29, ProductId = 30, PhdTotalCounterTag = "KT1103_3_R029.VOL15_PROD4.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 31, ProductId = 32, PhdTotalCounterTag = "KT1103_3_R031.VOL15_PROD5.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 221, ProductId = 217, PhdTotalCounterTag = "KT1103_3_R221.VOL15_PROD1.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 137,  ProductId = 205, PhdTotalCounterTag = "KT1103_3_R137.VOL15_PROD2.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 222, ProductId = 218, PhdTotalCounterTag = "KT1103_3_R222.VOL15_PROD3.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 130, ProductId = 127, PhdTotalCounterTag = "KT1103_3_R130.VOL15_PROD4.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 132, ProductId = 129, PhdTotalCounterTag = "KT1103_3_R132.VOL15_PROD5.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ011004",
                                    Name = "BGNCA00005002011004",
                                    TransportTypeId = 5,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                       new MeasurementPointsProductsConfig { Code = 78, ProductId = 75, PhdTotalCounterTag = "KT1104_F078.2VOL15_PROD1.VT", DirectionId = 2 },
                                       new MeasurementPointsProductsConfig { Code = 92, ProductId = 89, PhdTotalCounterTag = "KT1104_F092.2VOL15_PROD2.VT", DirectionId = 2 },
                                       new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT1104_F038.2VOL15_PROD3.VT", DirectionId = 2 },
                                       new MeasurementPointsProductsConfig { Code = 579,  ProductId = 205, PhdTotalCounterTag = "KT1104_F579.2VOL15_PROD4.VT", DirectionId = 2 },
                                       new MeasurementPointsProductsConfig { Code = 585, ProductId = 287, PhdTotalCounterTag = "KT1104_F585.2VOL15_PROD5.VT", DirectionId = 2 },
                                       new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT1104_F078.2VOL15_PROD1.VT", DirectionId = 2 },
                                       new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "KT1104_F092.2VOL15_PROD2.VT", DirectionId = 2 },
                                       new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "KT1104_F038.2VOL15_PROD3.VT", DirectionId = 2 },
                                       new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "KT1104_F579.2VOL15_PROD4.VT", DirectionId = 2 },
                                       new MeasurementPointsProductsConfig { Code = 586, ProductId = 288, PhdTotalCounterTag = "KT1104_F585.2VOL15_PROD5.VT", DirectionId = 2 },

                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ011010",
                                    Name = "BGNCA00005002011010",
                                    TransportTypeId = 5,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 78, ProductId = 75, PhdTotalCounterTag = "KT1104_R078.VOL15_PROD1.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 92, ProductId = 89, PhdTotalCounterTag = "KT1104_R092.VOL15_PROD2.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT1104_R038.VOL15_PROD3.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 579,  ProductId = 205, PhdTotalCounterTag = "KT1104_R579.VOL15_PROD4.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 585, ProductId = 287, PhdTotalCounterTag = "KT1104_R585.VOL15_PROD5.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT1104_R078.VOL15_PROD1.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "KT1104_R092.VOL15_PROD2.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "KT1104_R038.VOL15_PROD3.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "KT1104_R579.VOL15_PROD4.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 586, ProductId = 288, PhdTotalCounterTag = "KT1104_R585.VOL15_PROD5.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ011005",
                                    Name = "BGNCA00005002011005",
                                    TransportTypeId = 5,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "KT1105_F081.2VOL15_PROD1.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "KT1105_F084.2VOL15_PROD2.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT1105_F038.2VOL15_PROD3.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT1105_F040.2VOL15_PROD4.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "KT1105_F088.2VOL15_PROD5.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 585, ProductId = 287, PhdTotalCounterTag = "KT1105_F585.2VOL15_PROD1.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 89, ProductId = 86, PhdTotalCounterTag = "KT1105_F089.2VOL15_PROD2.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 78, ProductId = 75, PhdTotalCounterTag = "KT1105_F078.2VOL15_PROD3.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT1105_F077.2VOL15_PROD4.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 579,  ProductId = 205, PhdTotalCounterTag = "KT1105_F579.2VOL15_PROD5.VT", DirectionId = 2 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ011011",
                                    Name = "BGNCA00005002011011",
                                    TransportTypeId = 5,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 81, ProductId = 78, PhdTotalCounterTag = "KT1105_R081.VOL15_PROD1.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, ProductId = 81, PhdTotalCounterTag = "KT1105_R084.VOL15_PROD2.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 38, ProductId = 35, PhdTotalCounterTag = "KT1105_R038.VOL15_PROD3.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 40, ProductId = 37, PhdTotalCounterTag = "KT1105_R040.VOL15_PROD4.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "KT1105_R088.VOL15_PROD5.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 585, ProductId = 287, PhdTotalCounterTag = "KT1105_R585.VOL15_PROD1.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 89, ProductId = 86, PhdTotalCounterTag = "KT1105_R089.VOL15_PROD2.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 78, ProductId = 75, PhdTotalCounterTag = "KT1105_R078.VOL15_PROD3.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT1105_R077.VOL15_PROD4.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 579,  ProductId = 205, PhdTotalCounterTag = "KT1105_R579.VOL15_PROD5.VT", DirectionId = 1 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ011006",
                                    Name = "BGNCA00005002011006",
                                    TransportTypeId = 5,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 21, ProductId = 22, PhdTotalCounterTag = "KT1106_F021.2VOL15_PROD1.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 41, ProductId = 38, PhdTotalCounterTag = "KT1106_F041.2VOL15_PROD2.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 4, ProductId = 5, PhdTotalCounterTag = "KT1106_F004.2VOL15_PROD3.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 78, ProductId = 75, PhdTotalCounterTag = "KT1106_F078.2VOL15_PROD4.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 79, ProductId = 76, PhdTotalCounterTag = "KT1106_F079.2VOL15_PROD5.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 579,  ProductId = 205, PhdTotalCounterTag = "KT1106_F579.2VOL15_PROD1.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 585, ProductId = 287, PhdTotalCounterTag = "KT1106_F585.2VOL15_PROD2.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "KT1106_F088.2VOL15_PROD3.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT1106_F077.2VOL15_PROD4.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 586, ProductId = 288, PhdTotalCounterTag = "KT1106_F586.2VOL15_PROD5.VT", DirectionId = 2 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "КТ011012",
                                    Name = "BGNCA00005002011012",
                                    TransportTypeId = 5,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 21, ProductId = 22, PhdTotalCounterTag = "KT1106_R021.VOL15_PROD1.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 41, ProductId = 38, PhdTotalCounterTag = "KT1106_R041.VOL15_PROD2.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 4, ProductId = 5, PhdTotalCounterTag = "KT1106_R004.VOL15_PROD3.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 78, ProductId = 75, PhdTotalCounterTag = "KT1106_R078.VOL15_PROD4.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 79, ProductId = 76, PhdTotalCounterTag = "KT1106_R079.VOL15_PROD5.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 579,  ProductId = 205, PhdTotalCounterTag = "KT1106_R579.VOL15_PROD1.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 585, ProductId = 287, PhdTotalCounterTag = "KT1106_R585.VOL15_PROD2.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, ProductId = 85, PhdTotalCounterTag = "KT1106_R088.VOL15_PROD3.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 77, ProductId = 74, PhdTotalCounterTag = "KT1106_R077.VOL15_PROD4.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 586, ProductId = 288, PhdTotalCounterTag = "KT1106_R586.VOL15_PROD5.VT", DirectionId = 1 },
                                    }
                                }
                            }
                        }
                    }
                }
                );
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