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
                                            Code = 80, PhdTotalCounterTag = "TSN_KT0101_MT_GSV_P01.VT", DirectionId = 1
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
                                         new MeasurementPointsProductsConfig { Code = 89, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P01.VT", DirectionId = 1 }, 
                                         new MeasurementPointsProductsConfig { Code = 90, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P02.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 91, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P03.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 81, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P04.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 82, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P05.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 83, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P06.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 84, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P07.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 85, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P08.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 86, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P09.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 80, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P10.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 107, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P11.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 108, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P12.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 109, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P13.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 110, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P14.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 111, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P15.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 112, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P16.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 113, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P17.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 114, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P18.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 87, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P19.VT", DirectionId = 1 },
                                         new MeasurementPointsProductsConfig { Code = 88, PhdTotalCounterTag = "TSN_KT0102_MT_GSV_P20.VT", DirectionId = 1 },
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
                                        new MeasurementPointsProductsConfig { Code = 121, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 122, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 123, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 124, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 125, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 126, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 101, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 102, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 103, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 104, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 105, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 106, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 533, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 534, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 535, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 536, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 537, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 538, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 539, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 540, PhdTotalCounterTag = "TSN_KT0103_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001004",
                                    Name = "BGNCA00005001001004",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 40, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 42, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 43, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 44, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 45, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 46, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 47, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 48, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 49, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 58, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 60, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 61, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 62, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 63, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 64, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 65, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 66, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 67, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 500, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 501, PhdTotalCounterTag = "TSN_KT0104_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001005",
                                    Name = "BGNCA00005001001005",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 41, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 50, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 51, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 52, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 53, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 54, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 55, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 56, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 57, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 59, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 68, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 69, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 70, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 71, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 72, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 73, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 74, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 75, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 520, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 521, PhdTotalCounterTag = "TSN_KT0105_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001006",
                                    Name = "BGNCA00005001001006",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 81, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 82, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 83, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 85, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 86, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 87, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 107, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 108, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 109, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 110, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 111, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 112, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 113, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 114, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 89, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 90, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 91, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 541, PhdTotalCounterTag = "TSN_KT0106_MT_GSV_P20.VT", DirectionId = 1 },
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
                                        new MeasurementPointsProductsConfig { Code = 121, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 122, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 123, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 124, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 125, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 126, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 101, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 102, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 103, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 104, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 105, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 106, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 533, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 534, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 535, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 536, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 537, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 538, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 539, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 540, PhdTotalCounterTag = "TSN_KT0107_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001008",
                                    Name = "BGNCA00005001001008",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 40, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 42, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 43, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 44, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 45, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 46, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 47, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 48, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 49, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 58, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 60, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 61, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 62, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 63, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 64, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 65, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 66, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 67, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 500, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 501, PhdTotalCounterTag = "TSN_KT0108_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001009",
                                    Name = "BGNCA00005001001009",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 41, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 50, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 51, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 52, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 53, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 54, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 55, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 56, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 57, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 59, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 68, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 69, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 70, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 71, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 72, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 73, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 74, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 75, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 520, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 521, PhdTotalCounterTag = "TSN_KT0109_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001010",
                                    Name = "BGNCA00005001001010",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 81, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 82, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 83, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 84, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 85, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 86, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 87, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 88, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 107, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 108, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 109, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 110, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 111, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 112, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 113, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 114, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 89, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 90, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 91, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 541, PhdTotalCounterTag = "TSN_KT0110_MT_GSV_P20.VT", DirectionId = 1 },
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
                                        new MeasurementPointsProductsConfig { Code = 121, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 122, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 123, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 124, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 125, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 126, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 101, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 102, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 103, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 104, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 105, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 106, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 561, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 562, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 535, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 536, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 537, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 538, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 539, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 540, PhdTotalCounterTag = "TSN_KT0111_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001012",
                                    Name = "BGNCA00005001001012",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 040, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 042, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 043, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 044, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 045, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 046, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 047, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 048, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 049, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 058, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 060, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 061, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 062, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 063, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 064, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 065, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 066, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 067, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 500, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 501, PhdTotalCounterTag = "TSN_KT0112_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001013",
                                    Name = "BGNCA00005001001013",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 041, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 050, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 051, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 052, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 053, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 054, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 055, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 056, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 057, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 059, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 068, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 069, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 070, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 071, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 072, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 073, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 074, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 075, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 520, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 521, PhdTotalCounterTag = "TSN_KT0113_MT_GSV_P20.VT", DirectionId = 1 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001014",
                                    Name = "BGNCA00005001001014",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 081, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 082, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 083, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 084, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 085, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 086, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 087, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 088, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 107, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 108, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 109, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P11.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 110, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P12.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 111, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P13.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 112, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P14.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 113, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P15.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 114, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P16.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 089, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P17.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 090, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P18.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 091, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P19.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 541, PhdTotalCounterTag = "TSN_KT0114_MT_GSV_P20.VT", DirectionId = 1 },
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
                                        new MeasurementPointsProductsConfig { Code = 472, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P01.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 473, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P02.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 491, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P03.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 492, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P04.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 493, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P05.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 494, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P06.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 901, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P07.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 902, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P08.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 903, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P09.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 904, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P10.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 910, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P11.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 911, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P12.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 912, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P13.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 913, PhdTotalCounterTag = "TSN_KT0119_MT_GSV_P14.VT", DirectionId = 2 },
                                    }
                                },

                                new MeasurementPoint
                                {
                                    ControlPoint = "KT001020",
                                    Name = "BGNCA00005001001020",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                         new MeasurementPointsProductsConfig { Code = 481, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P01.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 482, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P02.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 483, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P03.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 484, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P04.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 485, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P05.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 486, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P06.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 487, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P07.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 488, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P08.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 489, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P09.VT", DirectionId = 2 },
                                         new MeasurementPointsProductsConfig { Code = 490, PhdTotalCounterTag = "TSN_KT0120_MT_GSV_P10.VT", DirectionId = 2 },
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
                                        new MeasurementPointsProductsConfig { Code = 332, PhdTotalCounterTag = "KT0201_TT_OUT_09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 141, PhdTotalCounterTag = "KT0201_TT_OUT_04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 143, PhdTotalCounterTag = "KT0201_TT_OUT_06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 142, PhdTotalCounterTag = "KT0201_TT_OUT_05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 144, PhdTotalCounterTag = "KT0201_TT_OUT_07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 140, PhdTotalCounterTag = "KT0201_TT_OUT_03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 139, PhdTotalCounterTag = "KT0201_TT_OUT_02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 138, PhdTotalCounterTag = "KT0201_TT_OUT_01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 208, PhdTotalCounterTag = "KT0201_TT_OUT_08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 210, PhdTotalCounterTag = "KT0201_TT_OUT_10.VT", DirectionId = 1 },

                                        new MeasurementPointsProductsConfig { Code = 332, PhdTotalCounterTag = "KT0201_TT_IN_09.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 141, PhdTotalCounterTag = "KT0201_TT_IN_04.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 143, PhdTotalCounterTag = "KT0201_TT_IN_06.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 142, PhdTotalCounterTag = "KT0201_TT_IN_05.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 144, PhdTotalCounterTag = "KT0201_TT_IN_07.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 140, PhdTotalCounterTag = "KT0201_TT_IN_03.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 139, PhdTotalCounterTag = "KT0201_TT_IN_02.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 138, PhdTotalCounterTag = "KT0201_TT_IN_01.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 208, PhdTotalCounterTag = "KT0201_TT_IN_08.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 210, PhdTotalCounterTag = "KT0201_TT_IN_10.VT", DirectionId = 2 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT002002",
                                    Name = "BGNCA00005001002002",
                                    TransportTypeId = 2,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 141, PhdTotalCounterTag = "KT0202_TT_OUT_04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 143, PhdTotalCounterTag = "KT0202_TT_OUT_06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 142, PhdTotalCounterTag = "KT0202_TT_OUT_05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 144, PhdTotalCounterTag = "KT0202_TT_OUT_07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 140, PhdTotalCounterTag = "KT0202_TT_OUT_03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 138, PhdTotalCounterTag = "KT0202_TT_OUT_01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 139, PhdTotalCounterTag = "KT0202_TT_OUT_02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 146, PhdTotalCounterTag = "KT0202_TT_OUT_10.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 208, PhdTotalCounterTag = "KT0202_TT_OUT_08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 145, PhdTotalCounterTag = "KT0202_TT_OUT_09.VT", DirectionId = 1 },

                                        new MeasurementPointsProductsConfig { Code = 141, PhdTotalCounterTag = "KT0202_TT_IN_04.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 143, PhdTotalCounterTag = "KT0202_TT_IN_06.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 142, PhdTotalCounterTag = "KT0202_TT_IN_05.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 144, PhdTotalCounterTag = "KT0202_TT_IN_07.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 140, PhdTotalCounterTag = "KT0202_TT_IN_03.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 139, PhdTotalCounterTag = "KT0202_TT_IN_02.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 138, PhdTotalCounterTag = "KT0202_TT_IN_01.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 146, PhdTotalCounterTag = "KT0202_TT_IN_10.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 208, PhdTotalCounterTag = "KT0202_TT_IN_08.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 145, PhdTotalCounterTag = "KT0202_TT_IN_09.VT", DirectionId = 2 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT002003",
                                    Name = "BGNCA00005001002003",
                                    TransportTypeId = 2,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 138, PhdTotalCounterTag = "KT0203_TT_IN_01.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 139, PhdTotalCounterTag = "KT0203_TT_IN_02.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 140, PhdTotalCounterTag = "KT0203_TT_IN_03.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 141, PhdTotalCounterTag = "KT0203_TT_IN_04.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 142, PhdTotalCounterTag = "KT0203_TT_IN_05.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 143, PhdTotalCounterTag = "KT0203_TT_IN_06.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 144, PhdTotalCounterTag = "KT0203_TT_IN_07.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 208, PhdTotalCounterTag = "KT0203_TT_IN_08.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 145, PhdTotalCounterTag = "KT0203_TT_IN_09.VT", DirectionId = 1 },
                                        new MeasurementPointsProductsConfig { Code = 146, PhdTotalCounterTag = "KT0203_TT_IN_10.VT", DirectionId = 1 },

                                        new MeasurementPointsProductsConfig { Code = 138, PhdTotalCounterTag = "KT0203_TT_OUT_01.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 139, PhdTotalCounterTag = "KT0203_TT_OUT_02.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 140, PhdTotalCounterTag = "KT0203_TT_OUT_03.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 141, PhdTotalCounterTag = "KT0203_TT_OUT_04.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 142, PhdTotalCounterTag = "KT0203_TT_OUT_05.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 143, PhdTotalCounterTag = "KT0203_TT_OUT_06.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 144, PhdTotalCounterTag = "KT0203_TT_OUT_07.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 208, PhdTotalCounterTag = "KT0203_TT_OUT_08.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 145, PhdTotalCounterTag = "KT0203_TT_OUT_09.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 146, PhdTotalCounterTag = "KT0203_TT_OUT_10.VT", DirectionId = 2 },
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
                                        new MeasurementPointsProductsConfig { Code = 457, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R1_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 498, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R2_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 459, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R3_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 460, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R4_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 466, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R5_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 480, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R6_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 475, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R7_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 476, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R8_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 477, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R9_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 478, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R10_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 479, PhdTotalCounterTag = "TSN_KT0204_MT_GSV_R11_V.VT", DirectionId = 2 },
                                    }
                                },
                                new MeasurementPoint
                                {
                                    ControlPoint = "KT002005",
                                    Name = "BGNCA00005001002005",
                                    TransportTypeId = 1,
                                    MeasurementPointsProductsConfigs = 
                                    {
                                        new MeasurementPointsProductsConfig { Code = 457, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R1_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 498, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R2_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 459, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R3_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 460, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R4_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 466, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R5_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 480, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R6_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 475, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R7_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 476, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R8_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 477, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R9_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 478, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R10_V.VT", DirectionId = 2 },
                                        new MeasurementPointsProductsConfig { Code = 479, PhdTotalCounterTag = "TSN_KT0205_MT_GSV_R11_V.VT", DirectionId = 2 },
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