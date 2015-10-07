namespace CollectingProductionDataSystem.Data.Migrations
{
    using CollectingProductionDataSystem.Models.Productions;
    using System.Data.Entity.Migrations;

    internal class ProductionDataImporter
    {
        internal static void Insert(CollectingDataSystemDbContext context)
        {
            context.Plants.AddOrUpdate(
                p => p.Id,
                new Plant
                {
                    ShortName = "ПД",
                    FullName = "ПРОИЗВОДСТВЕНА ДЕЙНОСТ",
                    Factories = {
                        new Factory
                        {
                            ShortName = "АВД",
                            FullName = "Производство АВД",
                            ProcessUnits = {
                                new ProcessUnit
                                {
                                    ShortName = "АД-4",
                                    FullName = "Инсталация АД-4",
                                    UnitsConfigs = { }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "АВД-1",
                                    FullName = "Инсталация АВД-1",
                                    UnitsConfigs = {
                                        new UnitConfig
                                        {
                                            Code = "1A0100",
                                            Position = "01-FICQ-001",
                                            Name = "Нефт вход АВД-1",
                                            DirectionId = 2,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FICQ-001.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A0200",
                                            Position = "01-FIQ-646А",
                                            Name = "Бензин отгони",
                                            DirectionId = 2,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-646A.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A0300",
                                            Position = "01-FIQ-014",
                                            Name = "Нефт - некондиция от Е-10",
                                            DirectionId = 3,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-014.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A0400",
                                            Position = "01-FIQ-147",
                                            Name = "Некондиция от Р-1",
                                            DirectionId = 2,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-147.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A0500",
                                            Position = "01-FIQ-134",
                                            Name = "Сух газ от Е-101",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-134.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A0600",
                                            Position = "01-FIQ-135",
                                            Name = "Сух газ от Е-104",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-135.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A0700",
                                            Position = "01-FICQ-048",
                                            Name = "Пропан-бутан към ЦГФИ",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FICQ-048.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A0701",
                                            Position = "01-FICQ-048",
                                            Name = "Пропан-бутан към парк Братово",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A0800",
                                            Position = "01-FIQ-050",
                                            Name = "Бензин връх К105 н.к100 към ХО-1",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-050.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A0801",
                                            Position = "01-FIQ-050",
                                            Name = "Бензин връх К105 н.к100 към т.31",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A0802",
                                            Position = "01-FIQ-050",
                                            Name = "Бензин връх К105 н.к100 към Мерокс",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A0803",
                                            Position = "01-FIQ-050",
                                            Name = "Бензин връх К105 н.к100 към КПТО",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A0804",
                                            Position = "01-FIQ-050",
                                            Name = "Бензин връх К105 н.к100 към Некондиция",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A0900",
                                            Position = "01-FIQ-045",
                                            Name = "Бензин дъно К-105 фр.85-180 към КР-1",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-045.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A0901",
                                            Position = "01-FIQ-045",
                                            Name = "Бензин дъно К-105 фр.85-180 към т.31",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A0902",
                                            Position = "01-FIQ-045",
                                            Name = "Бензин дъно К-105 фр.85-180 към КПТО",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A0903",
                                            Position = "01-FIQ-045",
                                            Name = "Бензин дъно К-105 фр.85-180 към некондиция",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1000",
                                            Position = "01-FIQ-047",
                                            Name = "Лека дизелова фракция към ХО-2",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-047.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1001",
                                            Position = "01-FIQ-047",
                                            Name = "Лека дизелова фракция към ХО-3",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1002",
                                            Position = "01-FIQ-047",
                                            Name = "Лека дизелова фракция към ХО-5",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1003",
                                            Position = "01-FIQ-047",
                                            Name = "Лека дизелова фракция към ТСНП",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1004",
                                            Position = "01-FIQ-047",
                                            Name = "Лека дизелова фракция към некондиция",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1100",
                                            Position = "01-FIQ-046",
                                            Name = "Тежка дизелова фракция към ХО-2",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-046.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1101",
                                            Position = "01-FIQ-046",
                                            Name = "Тежка дизелова фракция към ХО-3",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1102",
                                            Position = "01-FIQ-046",
                                            Name = "Тежка дизелова фракция към ХО-5",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1103",
                                            Position = "01-FIQ-046",
                                            Name = "Тежка дизелова фракция към ТСНП",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1104",
                                            Position = "01-FIQ-046",
                                            Name = "Тежка дизелова фракция към некондиция",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1105",
                                            Position = "01-FIQ-046",
                                            Name = "Тежка дизелова фракция към промивка БИ",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1106",
                                            Position = "01-FIQ-046",
                                            Name = "Тежка дизелова фракция към КПТО",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "М",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1200",
                                            Position = "01-FIQ-049",
                                            Name = "ААтмосферен Газьол към ХО-5",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-049.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1201",
                                            Position = "01-FIQ-049",
                                            Name = "Атмосферен Газьол към ККр",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1202",
                                            Position = "01-FIQ-049",
                                            Name = "Атмосферен Газьол към КПТО",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1203",
                                            Position = "01-FIQ-049",
                                            Name = "Атмосферен Газьол към некондиция",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1300",
                                            Position = "01-FIС-090",
                                            Name = "Мазут към П-1",
                                            DirectionId = 3,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIC-090.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1400",
                                            Position = "01-FIСQ-201",
                                            Name = "Мазут към ВД-2",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FICQ-201.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1401",
                                            Position = "01-FIСQ-201",
                                            Name = "Мазут към Р-2",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1402",
                                            Position = "01-FIСQ-201",
                                            Name = "Мазут към Р-1",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1500",
                                            Position = "01-FIQ-117",
                                            Name = "Нефопродукт от Е-3 към ХО-2",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-117.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1501",
                                            Position = "01-FIQ-117",
                                            Name = "Нефопродукт от Е-3 към ХО-3",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1502",
                                            Position = "01-FIQ-117",
                                            Name = "Нефопродукт от Е-3 към ХО-5",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1503",
                                            Position = "01-FIQ-117",
                                            Name = "Нефопродукт от Е-3 към некондиция",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1600",
                                            Position = "01-FIСQ-086",
                                            Name = "ШМФ към ККр",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FICQ-086.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1601",
                                            Position = "01-FIСQ-086",
                                            Name = "ШМФ към КПТО",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1602",
                                            Position = "01-FIСQ-086",
                                            Name = "ШМФ към промивка БИ",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1700",
                                            Position = "01-FIQ-087",
                                            Name = "ШМФ към Р-2",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-087.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1701",
                                            Position = "01-FIQ-087",
                                            Name = "ШМФ към Р-1",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1800",
                                            Position = "01-FICQ-088",
                                            Name = "ТДФ от ВДМ1 към ХО-2",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FICQ-088.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1801",
                                            Position = "01-FICQ-088",
                                            Name = "ТДФ от ВДМ1 към ХО-3",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1802",
                                            Position = "01-FICQ-088",
                                            Name = "ТДФ от ВДМ1 към ХО-5",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1803",
                                            Position = "01-FICQ-088",
                                            Name = "ТДФ от ВДМ1 към ТСНП",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1804",
                                            Position = "01-FICQ-088",
                                            Name = "ТДФ от ВДМ1 към промивка БИ",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A1900",
                                            Position = "01-FICQ-085",
                                            Name = "Слоп",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FICQ-085.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A2000",
                                            Position = "01-FIQ-084",
                                            Name = "Гудрон към БИ",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-084.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A2001",
                                            Position = "01-FIQ-084",
                                            Name = "Гудрон към КПТО",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A2002",
                                            Position = "01-FIQ-084",
                                            Name = "Гудрон към ТК",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A2100",
                                            Position = "01-FIQ-083",
                                            Name = "Гудрон към БИ",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-083.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A2101",
                                            Position = "01-FIQ-083",
                                            Name = "Гудрон към КПТО",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A2102",
                                            Position = "01-FIQ-083",
                                            Name = "Гудрон към ТК",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A2200",
                                            Position = "01-FICQ-082",
                                            Name = "Гудрон към БИ",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FICQ-082.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A2201",
                                            Position = "01-FICQ-082",
                                            Name = "Гудрон към КПТО",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A2202",
                                            Position = "01-FICQ-082",
                                            Name = "Гудрон към ТК",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "M",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A2300",
                                            Position = "01-FIQ-119",
                                            Name = "Обор. вода към АД_1",
                                            DirectionId = 2,
                                            MeasureUnitId = 14,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-119.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A2400",
                                            Position = "01-FIQ-120",
                                            Name = "Обор. вода към АД_2",
                                            DirectionId = 2,
                                            MeasureUnitId = 14,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-120.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A2500",
                                            Position = "01-FIQ-121",
                                            Name = "Обор. вода към ВД_1",
                                            DirectionId = 2,
                                            MeasureUnitId = 14,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-121.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A2600",
                                            Position = "01-FIQ-122",
                                            Name = "Обор. вода към ВД_2",
                                            DirectionId = 2,
                                            MeasureUnitId = 14,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-122.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A2700",
                                            Position = "01-FIQ-139",
                                            Name = "Свежа вода",
                                            DirectionId = 2,
                                            MeasureUnitId = 14,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-139.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A2800",
                                            Position = "01-FIQ-026",
                                            Name = "Деминерализирана вода",
                                            DirectionId = 2,
                                            MeasureUnitId = 14,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-026.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A2900",
                                            Position = "01-FIQ-125",
                                            Name = "КИП въздух",
                                            DirectionId = 2,
                                            MeasureUnitId = 15,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-125.TOTS_L",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A3000",
                                            Position = "01-FIQ-133",
                                            Name = "Азот",
                                            DirectionId = 2,
                                            MeasureUnitId = 15,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-133.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A3100",
                                            Position = "01-FIQ-126",
                                            Name = "ВГГ за АВД-1",
                                            DirectionId = 2,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-126.TOTS_L"
                                        },


                                        new UnitConfig
                                        {
                                            Code = "1A3200",
                                            Position = "01-FIQ-089",
                                            Name = "Пара ВД",
                                            DirectionId = 2,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-089.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A3201",
                                            Position = "01-TIQ-257",
                                            Name = "Температура",
                                            DirectionId = 2,
                                            MeasureUnitId = 6,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-TIQ-257.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A3202",
                                            Position = "01-PIQ-064",
                                            Name = "Налягане",
                                            DirectionId = 2,
                                            MeasureUnitId = 5,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-PIQ-064.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A3203",
                                            Position = "01-EIQ-089",
                                            Name = "Количество енергия",
                                            DirectionId = 2,
                                            MeasureUnitId = 12,
                                            MaterialTypeId = 1,
                                            IsCalculated = true,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-EIQ-089.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A3300",
                                            Position = "01-FIQ-118",
                                            Name = "Пара АД",
                                            DirectionId = 2,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-118.TOTS_L",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A3301",
                                            Position = "01-TIQ-318",
                                            Name = "Температура",
                                            DirectionId = 2,
                                            MeasureUnitId = 6,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-TIQ-318.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A3302",
                                            Position = "01-PIQ-096",
                                            Name = "Налягане",
                                            DirectionId = 2,
                                            MeasureUnitId = 5,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-PIQ-096.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A3303",
                                            Position = "01-EIQ-118",
                                            Name = "Количество енергия",
                                            DirectionId = 1,
                                            MeasureUnitId = 12,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-EIQ-118.TOTS_L",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A3400",
                                            Position = "01-FIQ-452",
                                            Name = "Котел изход пара",
                                            DirectionId = 1,
                                            MeasureUnitId = 12,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-FIQ-452.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A3401",
                                            Position = "01-TICA-452",
                                            Name = "Температура",
                                            DirectionId = 1,
                                            MeasureUnitId = 6,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-TIQ-452.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A3402",
                                            Position = "01-PIQ-452",
                                            Name = "Налягане",
                                            DirectionId = 1,
                                            MeasureUnitId = 5,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-PIQ-452.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A3403",
                                            Position = "01-EIQ- 452",
                                            Name = "Количество енергия",
                                            DirectionId = 1,
                                            MeasureUnitId = 12,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "AV1_01-EIQ-452.TOTS_L"
                                        },
                                        new UnitConfig
                                        {
                                            Position = null,
                                            Name = "Деемулгатор",
                                            DirectionId = 2,
                                            MeasureUnitId = 4,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "C",
                                            IsDeleted = true
                                        },
                                        new UnitConfig
                                        {
                                            Position = null,
                                            Name = "Филмообразуващ инхибитор",
                                            DirectionId = 2,
                                            MeasureUnitId = 4,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "C",
                                            IsDeleted = true
                                        },
                                        new UnitConfig
                                        {
                                            Position = null,
                                            Name = "Неутрализатор",
                                            DirectionId = 2,
                                            MeasureUnitId = 4,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "C",
                                            IsDeleted = true
                                        },
                                        new UnitConfig
                                        {
                                            Position = null,
                                            Name = "NaOH",
                                            DirectionId = 2,
                                            MeasureUnitId = 4,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "C",
                                            IsDeleted = true
                                        },
                                        new UnitConfig
                                        {
                                            Position = null,
                                            Name = "Поглътител на кислород",
                                            DirectionId = 2,
                                            MeasureUnitId = 4,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "C",
                                            IsDeleted = true
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1A3500",
                                            Position = "Електромер",
                                            Name = "Ел.енергия",
                                            DirectionId = 2,
                                            MeasureUnitId = 12,
                                            MaterialTypeId = 2,
                                            CollectingDataMechanism = "C",
                                            IsDeleted = true
                                        },
                                    },
                                    UnitsDailyConfigs = {
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A0100",
                                            MeasureUnitId = 11,
                                            Name = "Нефт",
                                            AggregationMembers = "1A0100;1A0804;1A0903;1A1004;1A1104;1A1105;1A1203;1A1402;1A1503;1A1804",
                                            AggregationFormula = "p.p0-(p.p1+p.p2+p.p3+p.p4+p.p5+p.p6+p.p7+p.p8+p.p9)"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A0200",
                                            MeasureUnitId = 11,
                                            Name = "Некондиция",
                                            AggregationMembers = "1A0804;1A0903;1A1004;1A1104;1A1105;1A1203;1A1402;1A1503;1A1804",
                                            AggregationFormula = "p.p0+p.p1+p.p2+p.p3+p.p4+p.p5+p.p6+p.p7+p.p8"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A0300",
                                            MeasureUnitId = 11,
                                            Name = "Некондиционни продукти",
                                            AggregationMembers = "1A0300;1A0400",
                                            AggregationFormula = "p.p0+p.p1"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A0400",
                                            MeasureUnitId = 11,
                                            Name = "Бензин отгон",
                                            AggregationMembers = "1A0200",
                                            AggregationFormula = "p.p0"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A9100",
                                            MeasureUnitId = 11,
                                            Name = "Сума преработка",
                                            AggregationMembers = "1A0100;1A0300;1A0400;1A0200",
                                            AggregationFormula = "p.p0+p.p1+p.p2+p.p3"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A0500",
                                            MeasureUnitId = 11,
                                            Name = "Бензинова фракция от АД",
                                            AggregationMembers = "1A0800;1A0801;1A0802;1A0803;1A0900;1A0901;1A0902",
                                            AggregationFormula = "p.p0+p.p1+p.p2+p.p3+p.p4+p.p5+p.p6"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A0600",
                                            MeasureUnitId = 11,
                                            Name = "Керосинова фракция от АД",
                                            AggregationMembers = "1A1000;1A1001",
                                            AggregationFormula = "p.p0+p.p1"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A0700",
                                            MeasureUnitId = 11,
                                            Name = "Дизелова фракция от АД",
                                            AggregationMembers = "1A1002;1A1003;1A1100;1A1101;1A1102;1A1103;1A1106",
                                            AggregationFormula = "p.p0+p.p1+p.p2+p.p3+p.p4+p.p5+p.p6"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A0800",
                                            MeasureUnitId = 11,
                                            Name = "Дизелова фракция за ТСНП",
                                            AggregationMembers = "1A1803",
                                            AggregationFormula = "p.p0"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A0801",
                                            MeasureUnitId = 11,
                                            Name = "Дизелова фракция за ХО-1,2,3",
                                            AggregationMembers = "1A1500;1A1501;1A1800;1A1801",
                                            AggregationFormula = "p.p0+p.p1+p.p2+p.p3"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A0802",
                                            MeasureUnitId = 11,
                                            Name = "Дизелова фракция за ХО-4,5",
                                            AggregationMembers = "1A1502;1A1802",
                                            AggregationFormula = "p.p0+p.p1"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A0900",
                                            MeasureUnitId = 11,
                                            Name = "Атмосферен газьол за ККр",
                                            AggregationMembers = "1A1201",
                                            AggregationFormula = "p.p0"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A0901",
                                            MeasureUnitId = 11,
                                            Name = "Атмосферен газьол за ХО-5",
                                            AggregationMembers = "1A1200",
                                            AggregationFormula = "p.p0"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A0902",
                                            MeasureUnitId = 11,
                                            Name = "Атмосферен газьол за КПТО",
                                            AggregationMembers = "1A1202",
                                            AggregationFormula = "p.p0"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A1000",
                                            MeasureUnitId = 11,
                                            Name = "ШМФ",
                                            AggregationMembers = "1A1600;1A1601;1A1602;1A1700;1A1701",
                                            AggregationFormula = "p.p0+p.p1+p.p2+p.p3+p.p4"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A1100",
                                            MeasureUnitId = 11,
                                            Name = "Мазут от първична дестилация",
                                            AggregationMembers = "1A1400;1A1401;1A1402",
                                            AggregationFormula = "p.p0+p.p1+p.p2"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A1200",
                                            MeasureUnitId = 11,
                                            Name = "Гудрон за ТСНП",
                                            AggregationMembers = "1A2000;1A2001;1A2002;1A2100;1A2101;1A2102;1A2200;1A2201;1A2202",
                                            AggregationFormula = "p.p0+p.p1+p.p2+p.p3+p.p4+p.p5+p.p6+p.p7+p.p8"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A1300",
                                            MeasureUnitId = 11,
                                            Name = "Пропан-бутан за ЦГФИ",
                                            AggregationMembers = "1A0700",
                                            AggregationFormula = "p.p0"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A1301",
                                            MeasureUnitId = 11,
                                            Name = "Пропан-бутан за Братово",
                                            AggregationMembers = "1A0701",
                                            AggregationFormula = "p.p0"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A1400",
                                            MeasureUnitId = 11,
                                            Name = "Сух газ за АГФИ",
                                            AggregationMembers = "1A0500;1A0600",
                                            AggregationFormula = "p.p0+p.p1"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A9200",
                                            MeasureUnitId = 11,
                                            Name = "Сума произведено",
                                            AggregationCurrentLevel = true,
                                            AggregationMembers = "2A0500;2A0600;2A0700;2A0800;2A0801;2A0802;2A0900;2A0901;2A0902;2A1000;2A1100;2A1200;2A1300;2A1301;2A1400",
                                            AggregationFormula = "p.p0+p.p1+p.p2+p.p3+p.p4+p.p5+p.p6+p.p7+p.p8+p.p9+p.p10+p.p11+p.p12+p.p13+p.p14"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A9201",
                                            MeasureUnitId = 11,
                                            Name = "Загуби",
                                            AggregationCurrentLevel = true,
                                            AggregationMembers = "2A9100;2A9200",
                                            AggregationFormula = "p.p0-p.p1"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2A9202",
                                            MeasureUnitId = 11,
                                            Name = "Общо",
                                            AggregationCurrentLevel = true,
                                            AggregationMembers = "2A9200;2A9201",
                                            AggregationFormula = "p.p0+p.p1"
                                        },
                                
                                    },
                                    ProductionPlanConfigs = 
                                    {
                                        new ProductionPlanConfig { Percentages = 15.86m, Name = "Бензинова фракция от АД" },
                                        new ProductionPlanConfig { Percentages = 5.09m, Name = "Керосинова фракция от АД" },                        
                                        new ProductionPlanConfig { Percentages = 24.20m, Name = "Сума Дизелови фракции" },        
                                        new ProductionPlanConfig { Percentages = 24.64m, Name = "Сума Атмосферен газьол" },        
                                        new ProductionPlanConfig { Percentages = 24.79m, Name = "Гудрон за ТСНП" },               
                                        new ProductionPlanConfig { Percentages = 0.55m, Name = "Сума Пропан-бутан" },                 
                                        new ProductionPlanConfig { Percentages = 0.25m, Name = "Сух газ за АГФИ" },                 
                                        new ProductionPlanConfig { Percentages = 0.20m, Name = "Загуби" },              
                                        new ProductionPlanConfig { Percentages = 100.00m, Name = "Общо" },
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ВДМ-2",
                                    FullName = "Инсталация ВДМ-2",
                                    UnitsConfigs = { }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ТК",
                                    FullName = "Инсталация ТК",
                                    UnitsConfigs = { }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "Битумна",
                                    FullName = "Инсталация Битумна",
                                    UnitsConfigs = { }
                                }
                            }
                        },
                        new Factory
                        {
                            ShortName = "КАБ",
                            FullName = " Производство КАБ",
                            ProcessUnits = {
                                new ProcessUnit
                                {
                                    ShortName = "ХО и ГО (с.100)",
                                    FullName = "Инсталация ХО и ГО (с.100)",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ККр (с.200)",
                                    FullName = "Инсталация ККр (с.200)",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "АГФИ (с.300)",
                                    FullName = "Инсталация Абс. И Гфр. (с.300)",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ВИ-15",
                                    FullName = "Инсталация ВИ-15",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "PSA",
                                    FullName = "Инсталация PSA",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "СКА",
                                    FullName = "Инсталация СКА",
                                    UnitsConfigs = 
                                    {
                                        new UnitConfig
                                        {
                                            Code = "1S0100",
                                            Position = "100FQI001",
                                            Name = "Изо ББФ",
                                            DirectionId = 1,
                                            MeasureUnitId = 5,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "SKA_100FQI001.TOTALIZER_S.OLDAV"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1S0200",
                                            Position = "100FQI004",
                                            Name = "Изобутан",
                                            DirectionId = 1,
                                            MeasureUnitId = 5,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "SKA_100FQI004.TOTALIZER_S.OLDAV"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1S0300",
                                            Position = "100FQI036",
                                            Name = "Сярна киселина",
                                            DirectionId = 1,
                                            MeasureUnitId = 5,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "SKA_100FQI036.TOTALIZER_S.OLDAV"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1S0400",
                                            Position = "100FQI069",
                                            Name = "Натриева основа",
                                            DirectionId = 1,
                                            MeasureUnitId = 5,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "SKA_100FQI069.TOTALIZER_S.OLDAV"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1S0500",
                                            Position = "100FQI049",
                                            Name = "Изобутан",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "SKA_100FQI049.TOTALIZER_S.OLDAV"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1S0600",
                                            Position = "100FQI042",
                                            Name = "Алкилат",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "SKA_100FQI042.TOTALIZER_S.OLDAV"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1S0700",
                                            Position = "100FQI053",
                                            Name = "Нормален бутан",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "SKA_100FQI053.TOTALIZER_S.OLDAV"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1S0800",
                                            Position = "100FQI035",
                                            Name = "Пропан бутанова фракция за ПВГ",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "SKA_100FQI035.TOTALIZER_S.OLDAV"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1S0900",
                                            Position = "100FQI081",
                                            Name = "Пропан бутанова фракция за АГФИ",
                                            DirectionId = 1,
                                            MeasureUnitId = 11,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "SKA_100FQI081.TOTALIZER_S.OLDAV"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1S1000",
                                            Position = "100FQI075",
                                            Name = "Кип въздух СКА",
                                            DirectionId = 2,
                                            MeasureUnitId = 15,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "SKA_100FQI075.TOTALIZER_S.OLDAV"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1S1100",
                                            Position = "300FQI002",
                                            Name = "Свежа вода",
                                            DirectionId = 2,
                                            MeasureUnitId = 15,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "SKA_300FQI002.TOTALIZER_S.OLDAV"
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1S1200",
                                            Position = "100FQI058",
                                            Name = "Азот",
                                            DirectionId = 2,
                                            MeasureUnitId = 15,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "SKA_100FQI058.TOTALIZER_S.OLDAV",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1S1300",
                                            Position = "100FQI041",
                                            Name = "Водна пара - вход",
                                            DirectionId = 1,
                                            MeasureUnitId = 12,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "SKA_100FQI041.TOTALIZER_S.OLDAV",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1S1301",
                                            Position = "100TQI050",
                                            Name = "Температура",
                                            DirectionId = 1,
                                            MeasureUnitId = 6,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "SKA_100TQI050.DACB.PV",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1S1302",
                                            Position = "100PQI031",
                                            Name = "Налягане",
                                            DirectionId = 1,
                                            MeasureUnitId = 5,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "A",
                                            PreviousShiftTag = "SKA_100PQI031.DACB.PV",
                                        },
                                        new UnitConfig
                                        {
                                            Code = "1S1303",
                                            Position = "100FQI041",
                                            Name = "Количество енергия",
                                            DirectionId = 1,
                                            MeasureUnitId = 12,
                                            MaterialTypeId = 1,
                                            CollectingDataMechanism = "C",
                                            PreviousShiftTag = null,
                                        },
                                    },
                                    UnitsDailyConfigs = 
                                    {
                                        new UnitsDailyConfig
                                        {
                                            Code = "2S0100",
                                            MeasureUnitId = 11,
                                            Name = "Изо ББФ",
                                            AggregationMembers = "1S0100",
                                            AggregationFormula = "p.p0"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2S0200",
                                            MeasureUnitId = 11,
                                            Name = "Изобутан",
                                            AggregationMembers = "1S0200",
                                            AggregationFormula = "p.p0"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2S0300",
                                            MeasureUnitId = 11,
                                            Name = "Сярна киселина",
                                            AggregationMembers = "1S0300",
                                            AggregationFormula = "p.p0"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2S9100",
                                            MeasureUnitId = 11,
                                            Name = "Сума преработка",
                                            AggregationMembers = "1S0100;1S0200;1S0300",
                                            AggregationFormula = "p.p0+p.p1+p.p2"
                                        },


                                        new UnitsDailyConfig
                                        {
                                            Code = "2S0400",
                                            MeasureUnitId = 11,
                                            Name = "Бензин Алкилат",
                                            AggregationMembers = "1S0600",
                                            AggregationFormula = "p.p0"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2S0500",
                                            MeasureUnitId = 11,
                                            Name = "Пропан бутанова фракция",
                                            AggregationMembers = "1S0800;1S0900",
                                            AggregationFormula = "p.p0+p.p1"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2S0600",
                                            MeasureUnitId = 11,
                                            Name = "Нормален бутан",
                                            AggregationMembers = "1S0700",
                                            AggregationFormula = "p.p0"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2S0700",
                                            MeasureUnitId = 11,
                                            Name = "Изобутан",
                                            AggregationMembers = "1S0500",
                                            AggregationFormula = "p.p0"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2S0800",
                                            MeasureUnitId = 11,
                                            Name = "Отработена сярна киселина",
                                            AggregationMembers = "1S0300",
                                            AggregationFormula = "p.p0"
                                        },

                                        new UnitsDailyConfig
                                        {
                                            Code = "2S9200",
                                            MeasureUnitId = 11,
                                            Name = "Сума произведено",
                                            AggregationCurrentLevel = true,
                                            AggregationMembers = "2S0400;2S0500;2S0600;2S0700",
                                            AggregationFormula = "p.p0+p.p1+p.p2+p.p3"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2S9201",
                                            MeasureUnitId = 11,
                                            Name = "Загуби",
                                            AggregationCurrentLevel = true,
                                            AggregationMembers = "2S9100;2S9200",
                                            AggregationFormula = "p.p0-p.p1"
                                        },
                                        new UnitsDailyConfig
                                        {
                                            Code = "2S9202",
                                            MeasureUnitId = 11,
                                            Name = "Общо",
                                            AggregationCurrentLevel = true,
                                            AggregationMembers = "2S9200;2S9201",
                                            AggregationFormula = "p.p0+p.p1"
                                        },
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "POK",
                                    FullName = "Инсталация POK",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "МТБЕ",
                                    FullName = "Инсталация МТБЕ",
                                    UnitsConfigs = {
                                    }
                                }
                            }
                        },
                        new Factory
                        {
                            ShortName = "КОГ",
                            FullName = "Производство КОГ",
                            ProcessUnits = {
                                new ProcessUnit
                                {
                                    ShortName = "ЦГФИ и ИНБ",
                                    FullName = "Инсталация ЦГФИ и ИНБ",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "КР-1",
                                    FullName = "Инсталация КР-1",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ХХ",
                                    FullName = "Инсталация Хидроочистка и хидриране",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "АГФИ и факел F-1",
                                    FullName = "Инсталация АГФИ и факел F-1",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ХО-1",
                                    FullName = "Инсталация ХО-1",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ХО-3",
                                    FullName = "Инсталация ХО-3",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ГО-2",
                                    FullName = "Инсталация ГО-2",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "П-37",
                                    FullName = "Парк 37",
                                    UnitsConfigs = {
                                    }
                                }
                            }
                        },
                        new Factory
                        {
                            ShortName = "КОГ-2",
                            FullName = "Производство КОГ-2",
                            ProcessUnits = {
                                new ProcessUnit
                                {
                                    ShortName = "ХО-2",
                                    FullName = "Инсталация ХО-2",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ХО-5",
                                    FullName = "Инсталация ХО-5",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ХОБ-1",
                                    FullName = "Инсталация ХОБ-1",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ХО-4",
                                    FullName = "Инсталация ХО-4",
                                    UnitsConfigs = {
                                    }
                                }
                            }
                        },
                        new Factory
                        {
                            ShortName = "КПТО",
                            FullName = "Производство КПТО",
                            ProcessUnits = {
                                new ProcessUnit
                                {
                                    ShortName = "H-Oil",
                                    FullName = "Инсталация H-Oil",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ВИ-71",
                                    FullName = "Инсталация ВИ-71",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "РМДЕА и КВ",
                                    FullName = "Инсталация Регенерация на МДЕА и КВ",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ГС-4",
                                    FullName = "Инсталация ГС-4",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "МДЕА и АО",
                                    FullName = "Инсталация МДЕА и АО",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ГС-3",
                                    FullName = "Инсталация ГС-3",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ГС-2",
                                    FullName = "Инсталация ГС-2",
                                    UnitsConfigs = {
                                    }
                                }
                            }
                        },
                        new Factory
                        {
                            ShortName = "ПП",
                            FullName = "Производство Полипропилен",
                            ProcessUnits = {
                                new ProcessUnit
                                {
                                    ShortName = "ПП",
                                    FullName = "Инсталация Полипропилен",
                                    UnitsConfigs = {
                                    }
                                }
                            }
                        },
                        new Factory
                        {
                            ShortName = "АК",
                            FullName = "Производство Азотно-кислородно",
                            ProcessUnits = {
                                new ProcessUnit
                                {
                                    ShortName = "АК",
                                    FullName = "Инсталация Азотно-кислородно",
                                    UnitsConfigs = {
                                    }
                                }
                            }
                        },
                        new Factory
                        {
                            ShortName = "ТСНП и ПТР",
                            FullName = "Производство ТСНП и ПТ Росенец",
                            ProcessUnits = 
                            {
                                new ProcessUnit
                                {
                                    ShortName = "ПТР",
                                    FullName = "ПТ Росенец",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ТСНП",
                                    FullName = "ТСНП",
                                    UnitsConfigs = {
                                    }
                                }
                            }
                        },
                        new Factory
                        {
                            ShortName = "ВиК и ОС",
                            FullName = "Производство ВиК и ОС",
                            ProcessUnits = 
                            {
                                new ProcessUnit
                                {
                                    ShortName = "СВ",
                                    FullName = "Свежа вода",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "СВВК",
                                    FullName = "Свежа вода - външни консуматори",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ПВ",
                                    FullName = "Питейна вода",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ПВВК",
                                    FullName = "Питейна вода - външни консуматори",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "Ел ВиК",
                                    FullName = "Ел. енергия ВиК",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ХООБ",
                                    FullName = "Химикали за обр. на оборотната вода",
                                    UnitsConfigs = {
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ОС",
                                    FullName = "Очистни съоръжение",
                                    UnitsConfigs = {
                                    }
                                }
                            }
                        },
                        new Factory
                        {
                            ShortName = "ТЕЦ",
                            FullName = "Производство ТЕЦ",
                            ProcessUnits = {
                                new ProcessUnit
                                {
                                    ShortName = "ТЕЦ",
                                    FullName = "Инсталация ТЕЦ",
                                    UnitsConfigs = {
                                    }
                                }
                            }
                        },
                    }
                });

            context.SaveChanges();
        }
    }
}
