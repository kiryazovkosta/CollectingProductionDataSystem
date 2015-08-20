namespace CollectingProductionDataSystem.Data.Migrations
{
    using CollectingProductionDataSystem.Models.Nomenclatures;
    using System.Data.Entity.Migrations;

    internal class ProductsDataImporter
    {
        internal static void Insert(CollectingDataSystemDbContext context)
        {
            context.ProductTypes.AddOrUpdate(
                new ProductType
                {
                    Name = "Недефиниран",
                    Products = {
                        new Product
                        {
                            Code = "-1",
                            Name = "Недефиниран"
                        }
                    }
                },
                new ProductType
                {
                    Name = "Суровина",
                    Products = {
                        new Product
                        {
                            Code = "001",
                            Name = "Нефт"
                        },
                        new Product
                        {
                            Code = "002",
                            Name = "Мазут за специфична преработка"
                        },
                        new Product
                        {
                            Code = "003",
                            Name = "Природен газ***"
                        },
                        new Product
                        {
                            Code = "004",
                            Name = "МТБЕ-внос"
                        },
                        new Product
                        {
                            Code = "005",
                            Name = "Биокомпонент Б-100"
                        },
                        new Product
                        {
                            Code = "006",
                            Name = "Биоетанол (денаториран)"
                        },
                        new Product
                        {
                            Code = "007",
                            Name = "Биоетанол (неденаториран)"
                        },
                        new Product
                        {
                            Code = "008",
                            Name = "Промишлен газьол (чужд, със съдържание на сяра над 0.2%)"
                        },
                        new Product
                        {
                            Code = "009",
                            Name = "Промишлен газьол (чужд-син, със съдържание на сяра над 0.2%)"
                        },
                        new Product
                        {
                            Code = "010",
                            Name = "Газьол (чужд,със съдържание на сяра до 0.05%)"
                        },
                        new Product
                        {
                            Code = "011",
                            Name = "Газьол (чужд,със съдържание на сяра от 0.05% до 0.2%)"
                        },
                        new Product
                        {
                            Code = "012",
                            Name = "Газьол (чужд-син,със съдържание на сяра до 0.05%)"
                        },
                        new Product
                        {
                            Code = "013",
                            Name = "Газьол (чужд-син,със съдържание на сяра от 0.05% до 0.2%)"
                        },
                        new Product
                        {
                            Code = "014",
                            Name = "Котелно гориво (чуждо-други смазочни масла и други масла)"
                        },
                        new Product
                        {
                            Code = "015",
                            Name = "Котелно гориво (чуждо-тежки горива-сяра до 1.0%)"
                        },
                        new Product
                        {
                            Code = "016",
                            Name = "Котелно гориво (чуждо-тежки горива-сяра от 1.0% до 2.0%)"
                        },
                        new Product
                        {
                            Code = "017",
                            Name = "Котелно гориво (чуждо-тежки горива-сяра от 2.0% до 2.8%)"
                        },
                        new Product
                        {
                            Code = "018",
                            Name = "Котелно гориво (чуждо-тежки горива-сяра над 2.8%)"
                        },
                        new Product
                        {
                            Code = "019",
                            Name = "Промишлен газьол-внос"
                        },
                        new Product
                        {
                            Code = "021",
                            Name = "Метанол"
                        },
                        new Product
                        {
                            Code = "022",
                            Name = "Нормален хексан"
                        },
                        new Product
                        {
                            Code = "023",
                            Name = "Амоняк"
                        },
                        new Product
                        {
                            Code = "024",
                            Name = "ШМФ (внос)"
                        },
                        new Product
                        {
                            Code = "025",
                            Name = "Дизелово гориво висока сяра (внос)"
                        },
                        new Product
                        {
                            Code = "026",
                            Name = "Некондиционни продукти (вход)"
                        },
                        new Product
                        {
                            Code = "027",
                            Name = "Газов кондензат"
                        },
                        new Product
                        {
                            Code = "028",
                            Name = "Корабно остатъчно гориво"
                        },
                        new Product
                        {
                            Code = "029",
                            Name = "Мазут за СП (AR)"
                        },
                        new Product
                        {
                            Code = "030",
                            Name = "Мазут за СП (БМ)"
                        },
                    }
                },
                new ProductType
                {
                    Name = "Стоков продукт",
                    Products = {
                        new Product
                        {
                            Code = "038",
                            Name = "Автомобилен бензин А 92Н"
                        },
                        new Product
                        {
                            Code = "039",
                            Name = "Автомобилен бензин А 92Н-ЕВРО 5"
                        },
                        new Product
                        {
                            Code = "040",
                            Name = "Автомобилен бензин А 95Н-ЕВРО5"
                        },
                        new Product
                        {
                            Code = "041",
                            Name = "Автомобилен бензин А 98Н-ЕВРО5"
                        },
                        new Product
                        {
                            Code = "042",
                            Name = "Автомобилен бензин А95Н (Био-етанол- 2%)"
                        },
                        new Product
                        {
                            Code = "043",
                            Name = "Автомобилен бензин А95Н (Био-етанол- 3%)"
                        },
                        new Product
                        {
                            Code = "044",
                            Name = "Автомобилен бензин А95Н (Био-етанол- 4%)"
                        },
                        new Product
                        {
                            Code = "045",
                            Name = "Автомобилен бензин А95Н (Био-етанол- 5%)"
                        },
                        new Product
                        {
                            Code = "046",
                            Name = "Автомобилен бензин А95Н (Био-етанол- 6%)"
                        },
                        new Product
                        {
                            Code = "047",
                            Name = "Автомобилен бензин А95Н (Био-етанол- 7%)"
                        },
                        new Product
                        {
                            Code = "048",
                            Name = "Автомобилен бензин А95Н (Био-етанол- 8%)"
                        },
                        new Product
                        {
                            Code = "049",
                            Name = "Автомобилен бензин А95Н (Био-етанол- 9%)"
                        },
                        new Product
                        {
                            Code = "050",
                            Name = "Автомобилен бензин А98Н (Био-етанол- 2%)"
                        },
                        new Product
                        {
                            Code = "051",
                            Name = "Автомобилен бензин А98Н (Био-етанол- 3%)"
                        },
                        new Product
                        {
                            Code = "052",
                            Name = "Автомобилен бензин А98Н (Био-етанол- 4%)"
                        },
                        new Product
                        {
                            Code = "053",
                            Name = "Автомобилен бензин А98Н (Био-етанол- 5%)"
                        },
                        new Product
                        {
                            Code = "054",
                            Name = "Автомобилен бензин А98Н (Био-етанол- 6%)"
                        },
                        new Product
                        {
                            Code = "055",
                            Name = "Автомобилен бензин А98Н (Био-етанол- 7%)"
                        },
                        new Product
                        {
                            Code = "056",
                            Name = "Автомобилен бензин А98Н (Био-етанол- 8%)"
                        },
                        new Product
                        {
                            Code = "057",
                            Name = "Автомобилен бензин А98Н (Био-етанол- 9%)"
                        },
                        new Product
                        {
                            Code = "058",
                            Name = "Автомобилен бензин А 95Н-ЕКТО"
                        },
                        new Product
                        {
                            Code = "059",
                            Name = "Автомобилен бензин А 98Н-ЕКТО"
                        },
                        new Product
                        {
                            Code = "060",
                            Name = "Автомобилен бензин А95Н (Био-етанол- 2%) ЕКТО"
                        },
                        new Product
                        {
                            Code = "061",
                            Name = "Автомобилен бензин А95Н (Био-етанол- 3%) ЕКТО"
                        },
                        new Product
                        {
                            Code = "062",
                            Name = "Автомобилен бензин А95Н (Био-етанол- 4%) ЕКТО"
                        },
                        new Product
                        {
                            Code = "063",
                            Name = "Автомобилен бензин А95Н (Био-етанол- 5%) ЕКТО"
                        },
                        new Product
                        {
                            Code = "064",
                            Name = "Автомобилен бензин А95Н (Био-етанол- 6%) ЕКТО"
                        },
                        new Product
                        {
                            Code = "065",
                            Name = "Автомобилен бензин А95Н (Био-етанол- 7%) ЕКТО"
                        },
                        new Product
                        {
                            Code = "066",
                            Name = "Автомобилен бензин А95Н (Био-етанол- 8%) ЕКТО"
                        },
                        new Product
                        {
                            Code = "067",
                            Name = "Автомобилен бензин А95Н (Био-етанол- 9%) ЕКТО "
                        },
                        new Product
                        {
                            Code = "068",
                            Name = "Автомобилен бензин А98Н (Био-етанол- 2%) ЕКТО"
                        },
                        new Product
                        {
                            Code = "069",
                            Name = "Автомобилен бензин А98Н (Био-етанол- 3%) ЕКТО"
                        },
                        new Product
                        {
                            Code = "070",
                            Name = "Автомобилен бензин А98Н (Био-етанол- 4%) ЕКТО"
                        },
                        new Product
                        {
                            Code = "071",
                            Name = "Автомобилен бензин А98Н (Био-етанол- 5%) ЕКТО"
                        },
                        new Product
                        {
                            Code = "072",
                            Name = "Автомобилен бензин А98Н (Био-етанол- 6%) ЕКТО"
                        },
                        new Product
                        {
                            Code = "073",
                            Name = "Автомобилен бензин А98Н (Био-етанол- 7%) ЕКТО"
                        },
                        new Product
                        {
                            Code = "074",
                            Name = "Автомобилен бензин А98Н (Био-етанол- 8%) ЕКТО"
                        },
                        new Product
                        {
                            Code = "075",
                            Name = "Автомобилен бензин А98Н (Био-етанол- 9%) ЕКТО"
                        },
                        new Product
                        {
                            Code = "076",
                            Name = "Автомобилен бензин А 93Н"
                        },
                        new Product
                        {
                            Code = "077",
                            Name = "Реформат-стока"
                        },
                        new Product
                        {
                            Code = "078",
                            Name = "Нискооктанов бензин (НОБ) - лек"
                        },
                        new Product
                        {
                            Code = "079",
                            Name = "МТБЕ-стока"
                        },
                        new Product
                        {
                            Code = "080",
                            Name = "Реактивно гориво ДЖЕТ-А1"
                        },
                        new Product
                        {
                            Code = "081",
                            Name = "Дизелово гориво-0.001%S - ЕВРО 5 (лятно)"
                        },
                        new Product
                        {
                            Code = "082",
                            Name = "Дизелово гориво-0.001%S - ЕВРО 5 (лятно,син)"
                        },
                        new Product
                        {
                            Code = "083",
                            Name = "Дизелово гориво-0.001%S - ЕВРО 5 (лятно,червен)"
                        },
                        new Product
                        {
                            Code = "084",
                            Name = "Дизелово гориво-0.001%S - ЕВРО 5 (зимно)"
                        },
                        new Product
                        {
                            Code = "085",
                            Name = "Дизелово гориво-0.001%S - ЕВРО 5 (зимно,син)"
                        },
                        new Product
                        {
                            Code = "086",
                            Name = "Дизелово гориво-0.001%S - ЕВРО 5 (зимно,червен)"
                        },
                        new Product
                        {
                            Code = "087",
                            Name = "Дизелово гориво ( CFPP - 25)"
                        },
                        new Product
                        {
                            Code = "088",
                            Name = "Дизелово гориво ( CP - 16)"
                        },
                        new Product
                        {
                            Code = "089",
                            Name = "Газьол за извънпътна техника - 0,001%S"
                        },
                        new Product
                        {
                            Code = "090",
                            Name = "Газьол за извънпътна техника - 0,001%S (син)"
                        },
                        new Product
                        {
                            Code = "091",
                            Name = "Газьол за извънпътна техника - 0,001%S (червен)"
                        },
                        new Product
                        {
                            Code = "092",
                            Name = "Газьол за извънпътна техника - 0,1%S"
                        },
                        new Product
                        {
                            Code = "093",
                            Name = "Газьол за извънпътна техника - 0,1%S (син)"
                        },
                        new Product
                        {
                            Code = "094",
                            Name = "Газьол за извънпътна техника - 0,1%S (червен)"
                        },
                        new Product
                        {
                            Code = "095",
                            Name = "Гориво за дизелови двигатели B5 (лято)"
                        },
                        new Product
                        {
                            Code = "096",
                            Name = "Гориво за дизелови двигатели B5 (лято;син)"
                        },
                        new Product
                        {
                            Code = "097",
                            Name = "Гориво за дизелови двигатели B5 (лято;червен)"
                        },
                        new Product
                        {
                            Code = "098",
                            Name = "Гориво за дизелови двигатели B5 (зима)"
                        },
                        new Product
                        {
                            Code = "099",
                            Name = "Гориво за дизелови двигатели B5 (зима;син)"
                        },
                        new Product
                        {
                            Code = "100",
                            Name = "Гориво за дизелови двигатели B5 (зима;червен)"
                        },
                        new Product
                        {
                            Code = "101",
                            Name = "Гориво за дизелови двигатели B6 (лято)"
                        },
                        new Product
                        {
                            Code = "102",
                            Name = "Гориво за дизелови двигатели B6 (лято;син)"
                        },
                        new Product
                        {
                            Code = "103",
                            Name = "Гориво за дизелови двигатели B6 (лято;червен)"
                        },
                        new Product
                        {
                            Code = "104",
                            Name = "Гориво за дизелови двигатели B6 (зима)"
                        },
                        new Product
                        {
                            Code = "105",
                            Name = "Гориво за дизелови двигатели B6 (зима;син)"
                        },
                        new Product
                        {
                            Code = "106",
                            Name = "Гориво за дизелови двигатели B6 (зима;червен)"
                        },
                        new Product
                        {
                            Code = "107",
                            Name = "Дизелово гориво-0.001%S - ЕВРО 5 (лятно)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "108",
                            Name = "Дизелово гориво-0.001%S - ЕВРО 5 (лятно,син)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "109",
                            Name = "Дизелово гориво-0.001%S - ЕВРО 5 (лятно,червен)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "110",
                            Name = "Дизелово гориво-0.001%S - ЕВРО 5 (зимно)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "111",
                            Name = "Дизелово гориво-0.001%S - ЕВРО 5 (зимно,син)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "112",
                            Name = "Дизелово гориво-0.001%S - ЕВРО 5 (зимно,червен)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "113",
                            Name = "Дизелово гориво ( CFPP - 25)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "114",
                            Name = "Дизелово гориво ( CP - 16)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "115",
                            Name = "Гориво за дизелови двигатели B5 (лято)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "116",
                            Name = "Гориво за дизелови двигатели B5 (лято;син)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "117",
                            Name = "Гориво за дизелови двигатели B5 (лято;червен)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "118",
                            Name = "Гориво за дизелови двигатели B5 (зима)-ЕКТО "
                        },
                        new Product
                        {
                            Code = "119",
                            Name = "Гориво за дизелови двигатели B5 (зима;син)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "120",
                            Name = "Гориво за дизелови двигатели B5 (зима;червен)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "121",
                            Name = "Гориво за дизелови двигатели B6 (лято)-ЕКТО "
                        },
                        new Product
                        {
                            Code = "122",
                            Name = "Гориво за дизелови двигатели B6 (лято;син)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "123",
                            Name = "Гориво за дизелови двигатели B6 (лято;червен)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "124",
                            Name = "Гориво за дизелови двигатели B6 (зима)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "125",
                            Name = "Гориво за дизелови двигатели B6 (зима;син)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "126",
                            Name = "Гориво за дизелови двигатели B6 (зима;червен)-ЕКТО"
                        },
                        new Product
                        {
                            Code = "127",
                            Name = "Дизелово гориво ДС"
                        },
                        new Product
                        {
                            Code = "128",
                            Name = "Дизелово гориво ДСЗ"
                        },
                        new Product
                        {
                            Code = "129",
                            Name = "Котелно гориво - 2.7%S"
                        },
                        new Product
                        {
                            Code = "130",
                            Name = "Котелно гориво - 1.0 %S"
                        },
                        new Product
                        {
                            Code = "131",
                            Name = "Котелно гориво бункер"
                        },
                        new Product
                        {
                            Code = "132",
                            Name = "Мазут АО ( стока)"
                        },
                        new Product
                        {
                            Code = "133",
                            Name = "Гудрон-стока"
                        },
                        new Product
                        {
                            Code = "134",
                            Name = "Битум нефтен вискозен  БВ >120"
                        },
                        new Product
                        {
                            Code = "135",
                            Name = "Битум за пътни настилки тип 50/70 ( БВ-60)"
                        },
                        new Product
                        {
                            Code = "136",
                            Name = "Битум за пътни настилки тип 70/100 (БВ-90)"
                        },
                        new Product
                        {
                            Code = "137",
                            Name = "ШМФ (износ)"
                        },
                        new Product
                        {
                            Code = "138",
                            Name = "Пропан бутан ( СПБ)"
                        },
                        new Product
                        {
                            Code = "139",
                            Name = "Пропан-бутан (лек)"
                        },
                        new Product
                        {
                            Code = "140",
                            Name = "Пропан-бутан (тежък)"
                        },
                        new Product
                        {
                            Code = "141",
                            Name = "Пропилен ( стока)"
                        },
                        new Product
                        {
                            Code = "142",
                            Name = "Пропан-пропиленова фракция ( стока)"
                        },
                        new Product
                        {
                            Code = "143",
                            Name = "Пропан-технически"
                        },
                        new Product
                        {
                            Code = "144",
                            Name = "Пропан-бутан (чужд)"
                        },
                        new Product
                        {
                            Code = "145",
                            Name = "Бутан (чужд)"
                        },
                        new Product
                        {
                            Code = "146",
                            Name = "Пропан (чужд)"
                        },
                        new Product
                        {
                            Code = "147",
                            Name = "Газова сяра (буци)"
                        },
                        new Product
                        {
                            Code = "148",
                            Name = "Газова сяра ( гранули)"
                        },
                        new Product
                        {
                            Code = "149",
                            Name = "Газова сяра (замърсена)"
                        },
                        new Product
                        {
                            Code = "150",
                            Name = "Горивен газ (стока за ЛЕГБ)"
                        },
                        new Product
                        {
                            Code = "151",
                            Name = "Полиетилен гликол"
                        },
                        new Product
                        {
                            Code = "152",
                            Name = "Моноетилен гликол"
                        },
                        new Product
                        {
                            Code = "153",
                            Name = "Диетилен гликол"
                        },
                        new Product
                        {
                            Code = "154",
                            Name = "Триетилен гликол"
                        },
                        new Product
                        {
                            Code = "155",
                            Name = "Тетраетилен гликол"
                        },
                        new Product
                        {
                            Code = "156",
                            Name = "Моноетанол амин"
                        },
                        new Product
                        {
                            Code = "157",
                            Name = "Диетанол амин"
                        },
                        new Product
                        {
                            Code = "158",
                            Name = "Триетанол амин"
                        },
                        new Product
                        {
                            Code = "159",
                            Name = "МАК-4"
                        },
                        new Product
                        {
                            Code = "160",
                            Name = "ПАН-влакна"
                        },
                        new Product
                        {
                            Code = "161",
                            Name = "НЕСТАНДАРТНО ВЛАКНО КАБЕЛ"
                        },
                        new Product
                        {
                            Code = "162",
                            Name = "НЕСТАНДАРТНО ВЛАКНО СМЕС"
                        },
                        new Product
                        {
                            Code = "163",
                            Name = "НЕСТАНДАРТНО ВЛАКНО ЛЕНТА"
                        },
                        new Product
                        {
                            Code = "164",
                            Name = "НЕСТАНДАРТНО ВЛАКНО ЩАПЕЛ"
                        },
                        new Product
                        {
                            Code = "165",
                            Name = "ПАН Л 3,4 М 125 СМЕС А"
                        },
                        new Product
                        {
                            Code = "166",
                            Name = "ПАН ЩББ 3,4/102 А"
                        },
                        new Product
                        {
                            Code = "167",
                            Name = "ПАН ШБМ 2,6/120 А"
                        },
                        new Product
                        {
                            Code = "168",
                            Name = "ПАН ЩБМ 3,4/60 А"
                        },
                        new Product
                        {
                            Code = "169",
                            Name = "ПАН ЩБМ 3,4/80 А"
                        },
                        new Product
                        {
                            Code = "170",
                            Name = "ПАН ЩМ Т СИН 125 3,4/80 А"
                        },
                        new Product
                        {
                            Code = "171",
                            Name = "ПАН ЩМ ЧЕРЕН 144 3,4/60 А"
                        },
                        new Product
                        {
                            Code = "172",
                            Name = "ПАН ЩМ ЧЕРЕН 144 3,4/80 А"
                        },
                        new Product
                        {
                            Code = "173",
                            Name = "ПОЛИПРОПИЛЕН (наличност)"
                        },
                        new Product
                        {
                            Code = "174",
                            Name = "АТАКТЕН ПОЛИПРОПИЛЕН-ХОМОПОЛИМЕР"
                        },
                        new Product
                        {
                            Code = "175",
                            Name = "ПОЛИПРОПИЛЕН - ПОЛИМЕРНИ ШУШУЛКИ"
                        },
                        new Product
                        {
                            Code = "176",
                            Name = "ПОЛИПРОПИЛЕН - СМЕТЕНИ ГР.С МЕХ.ПРИМЕСИ"
                        },
                        new Product
                        {
                            Code = "177",
                            Name = "ПОЛИПРОПИЛЕН - ОТПАДЪК ЧИСТИ ГР.ОТ ЕКСТР"
                        },
                        new Product
                        {
                            Code = "178",
                            Name = "ПОЛИПРОПИЛЕН 6131 ПТ НЕОЦВ. 1К"
                        },
                        new Product
                        {
                            Code = "179",
                            Name = "ПОЛИПРОПИЛЕН 6131 ПТ НЕОЦВ. 2К"
                        },
                        new Product
                        {
                            Code = "180",
                            Name = "ПОЛИПРОПИЛЕН 6231 НЮАНС"
                        },
                        new Product
                        {
                            Code = "181",
                            Name = "ПОЛИПРОПИЛЕН 6231ПТ НЕОЦВ. I К"
                        },
                        new Product
                        {
                            Code = "182",
                            Name = "ПОЛИПРОПИЛЕН 6231ПТ НЕОЦВ. II К"
                        },
                        new Product
                        {
                            Code = "183",
                            Name = "ПОЛИПРОПИЛЕН 6331 НЕОЦ.1К"
                        },
                        new Product
                        {
                            Code = "184",
                            Name = "ПОЛИПРОПИЛЕН 6331 НЕОЦ.2К"
                        },
                        new Product
                        {
                            Code = "185",
                            Name = "ПОЛИПРОПИЛЕН 6331 НЮАНС"
                        },
                        new Product
                        {
                            Code = "186",
                            Name = "ПОЛИПРОПИЛЕН 6431 /М и DHT I K"
                        },
                        new Product
                        {
                            Code = "187",
                            Name = "ПОЛИПРОПИЛЕН 6431 /М II K"
                        },
                        new Product
                        {
                            Code = "188",
                            Name = "ПОЛИПРОПИЛЕН 6431 НЕОЦ.1К"
                        },
                        new Product
                        {
                            Code = "189",
                            Name = "ПОЛИПРОПИЛЕН 6431 НЕОЦ.2К"
                        },
                        new Product
                        {
                            Code = "190",
                            Name = "ПОЛИПРОПИЛЕН 6431 НЮАНС"
                        },
                        new Product
                        {
                            Code = "191",
                            Name = "ПОЛИПРОПИЛЕН 6531 НЕОЦ.1К"
                        },
                        new Product
                        {
                            Code = "192",
                            Name = "ПОЛИПРОПИЛЕН 6531 НЕОЦ.2К"
                        },
                        new Product
                        {
                            Code = "193",
                            Name = "ПОЛИПРОПИЛЕН 6531 НЮАНС"
                        },
                        new Product
                        {
                            Code = "194",
                            Name = "ПОЛИПРОПИЛЕН 6631 НЕОЦ.1К"
                        },
                        new Product
                        {
                            Code = "195",
                            Name = "ПОЛИПРОПИЛЕН 6631 НЕОЦ.2К"
                        },
                        new Product
                        {
                            Code = "196",
                            Name = "ПОЛИПРОПИЛЕН 6631 N"
                        },
                        new Product
                        {
                            Code = "197",
                            Name = "ПОЛИПРОПИЛЕН 6631 НЮАНС"
                        },
                        new Product
                        {
                            Code = "198",
                            Name = "ПОЛИПРОПИЛЕНОВИ АГЛОМЕРАТИ (ПИТИ)"
                        },
                        new Product
                        {
                            Code = "199",
                            Name = "ПОЛИПРОПИЛЕН АТАКТЕН - ПИТИ (БУЦИ)"
                        },
                        new Product
                        {
                            Code = "200",
                            Name = "ПОЛИПРОПИЛЕНОВ ПРАХ - С ПРИМЕСИ"
                        },
                        new Product
                        {
                            Code = "201",
                            Name = "ПОЛИПРОПИЛЕНОВ ПРАХ - ЧИСТ"
                        },
                        new Product
                        {
                            Code = "202",
                            Name = "ПОЛИПРОПИЛЕН АТАКТЕН-ЗАМЪРСЕН"
                        },
                        new Product
                        {
                            Code = "203",
                            Name = "АТАКТЕН ПОЛИПРОПИЛЕН-ХОМО-К"
                        },
                        new Product
                        {
                            Code = "204",
                            Name = "ПОЛИПРОПИЛЕН ПИТИ ЗАМЪРСЕНИ"
                        },
                        new Product
                        {
                            Code = "205",
                            Name = "Полимерен отпадък"
                        },
                        new Product
                        {
                            Code = "206",
                            Name = "Акрилнитрил"
                        },
                        new Product
                        {
                            Code = "207",
                            Name = "Ацетонитрил"
                        },
                        new Product
                        {
                            Code = "219",
                            Name = "Котелно гориво (AR) - 2.7%S"
                        },
                        new Product
                        {
                            Code = "220",
                            Name = "Корабно остатъчно гориво ( AR)"
                        },
                        new Product
                        {
                            Code = "221",
                            Name = " - Неидентифициран - "
                        },
                        new Product
                        {
                            Code = "222",
                            Name = "ШМФ (износ) - (AR)"
                        },
                        new Product
                        {
                            Code = "500",
                            Name = "Автомобилен бензин А95Н - 500"
                        },
                        new Product
                        {
                            Code = "501",
                            Name = "Автомобилен бензин А95Н - 501"
                        },
                        new Product
                        {
                            Code = "502",
                            Name = "Автомобилен бензин А95Н - 502"
                        },
                        new Product
                        {
                            Code = "503",
                            Name = "Автомобилен бензин А95Н - 503"
                        },
                        new Product
                        {
                            Code = "504",
                            Name = "Автомобилен бензин А95Н - 504"
                        },
                        new Product
                        {
                            Code = "520",
                            Name = "Автомобилен бензин А98Н - 520"
                        },
                        new Product
                        {
                            Code = "521",
                            Name = "Автомобилен бензин А98Н - 521"
                        },
                        new Product
                        {
                            Code = "522",
                            Name = "Автомобилен бензин А98Н - 522"
                        },
                        new Product
                        {
                            Code = "524",
                            Name = "Автомобилен бензин А98Н - 524"
                        },
                        new Product
                        {
                            Code = "533",
                            Name = "Гориво за дизелови двигатели - 533"
                        },
                        new Product
                        {
                            Code = "534",
                            Name = "Гориво за дизелови двигатели - 534"
                        },
                        new Product
                        {
                            Code = "535",
                            Name = "Гориво за дизелови двигатели - 535"
                        },
                        new Product
                        {
                            Code = "536",
                            Name = "Гориво за дизелови двигатели - 536"
                        },
                        new Product
                        {
                            Code = "537",
                            Name = "Гориво за дизелови двигатели - 537"
                        },
                        new Product
                        {
                            Code = "538",
                            Name = "Гориво за дизелови двигатели - 538"
                        },
                        new Product
                        {
                            Code = "539",
                            Name = "Гориво за дизелови двигатели - 539"
                        },
                        new Product
                        {
                            Code = "540",
                            Name = "Гориво за дизелови двигатели - 540"
                        },
                        new Product
                        {
                            Code = "541",
                            Name = "Гориво за дизелови двигатели - 541"
                        },
                        new Product
                        {
                            Code = "542",
                            Name = "Гориво за дизелови двигатели - 542"
                        },
                        new Product
                        {
                            Code = "543",
                            Name = "Гориво за дизелови двигатели - 543"
                        },
                        new Product
                        {
                            Code = "544",
                            Name = "Гориво за дизелови двигатели - 544"
                        },
                        new Product
                        {
                            Code = "545",
                            Name = "Гориво за дизелови двигатели - 545"
                        },
                        new Product
                        {
                            Code = "546",
                            Name = "Гориво за дизелови двигатели - 546"
                        },
                        new Product
                        {
                            Code = "561",
                            Name = "Гориво за дизелови двигатели - B6(CP)"
                        },
                        new Product
                        {
                            Code = "562",
                            Name = "Гориво за дизелови двигатели - B6(CP)-EKTO"
                        },
                        new Product
                        {
                            Code = "579",
                            Name = "Нискооктанов бензин (НОБ) - тежък"
                        },
                        new Product
                        {
                            Code = "585",
                            Name = "Автомобилен бензин А94Н"
                        },
                        new Product
                        {
                            Code = "586",
                            Name = "Бензин алкилат (стока)"
                        },
                        new Product
                        {
                            Code = "811",
                            Name = "Дизелово гориво-0.001%S - Евро 5 (Лятно) - Смесване"
                        },
                        new Product
                        {
                            Code = "821",
                            Name = "Дизелово гориво-0.001%S - Евро 5 (Лятно, Син)  - Смесване"
                        },
                        new Product
                        {
                            Code = "825",
                            Name = "Дизелово гориво Висока Сяра (Внос) - Смесване"
                        },
                        new Product
                        {
                            Code = "827",
                            Name = "Дизелово гориво Висока Сяра (Внос, Син)  - Смесване"
                        },
                        new Product
                        {
                            Code = "828",
                            Name = "Корабно Остатъчно Гориво (КОГ) - Смесване"
                        },
                        new Product
                        {
                            Code = "841",
                            Name = "Дизелово гориво-0.001%S - Евро 5 (Зимно) - Смесване"
                        },
                        new Product
                        {
                            Code = "851",
                            Name = "Дизелово гориво-0.001%S - Евро 5 (Зимно, Син) - Смесване"
                        },
                    }
                },
                new ProductType
                {
                    Name = "Полуфабрикати и компоненти",
                    Products = {
                        new Product
                        {
                            Code = "231",
                            Name = "Водород (ВИ)"
                        },
                        new Product
                        {
                            Code = "232",
                            Name = "Водород-съдържащ газ (ВСГ)"
                        },
                        new Product
                        {
                            Code = "233",
                            Name = "Въглеводороден газ (ВВГ)"
                        },
                        new Product
                        {
                            Code = "234",
                            Name = "Въглеводороди (леки)"
                        },
                        new Product
                        {
                            Code = "235",
                            Name = "Сух газ"
                        },
                        new Product
                        {
                            Code = "236",
                            Name = "Смесен горивен газ"
                        },
                        new Product
                        {
                            Code = "237",
                            Name = "Пропан бутан (НПБ)"
                        },
                        new Product
                        {
                            Code = "238",
                            Name = "Пропан бутанова фракция"
                        },
                        new Product
                        {
                            Code = "239",
                            Name = "Пропан-пропиленова фракция"
                        },
                        new Product
                        {
                            Code = "240",
                            Name = "Пропилен"
                        },
                        new Product
                        {
                            Code = "241",
                            Name = "Изо-бутан"
                        },
                        new Product
                        {
                            Code = "242",
                            Name = "Бутан-бутиленова фракция (ББФ)"
                        },
                        new Product
                        {
                            Code = "243",
                            Name = "Изо-бутан-бутиленова фракция (изо-ББФ)"
                        },
                        new Product
                        {
                            Code = "244",
                            Name = "Пропан"
                        },
                        new Product
                        {
                            Code = "245",
                            Name = "Нормален бутан"
                        },
                        new Product
                        {
                            Code = "246",
                            Name = "Рафинат II"
                        },
                        new Product
                        {
                            Code = "247",
                            Name = "Рафинат III"
                        },
                        new Product
                        {
                            Code = "248",
                            Name = "Етан-пропанова фракция"
                        },
                        new Product
                        {
                            Code = "249",
                            Name = "Фракция С5"
                        },
                        new Product
                        {
                            Code = "250",
                            Name = "Сяроводород"
                        },
                        new Product
                        {
                            Code = "251",
                            Name = "Сярна киселина"
                        },
                        new Product
                        {
                            Code = "252",
                            Name = "Отработена сярна киселина"
                        },
                        new Product
                        {
                            Code = "253",
                            Name = "Азот (газообразен)"
                        },
                        new Product
                        {
                            Code = "254",
                            Name = "Азот (течен)"
                        },
                        new Product
                        {
                            Code = "255",
                            Name = "Кислород (газообразен)"
                        },
                        new Product
                        {
                            Code = "256",
                            Name = "Кислород (течен)"
                        },
                        new Product
                        {
                            Code = "257",
                            Name = "Бензинова фракция от АД (НК-100)"
                        },
                        new Product
                        {
                            Code = "258",
                            Name = "Бензинова фракция от АД (100-180)"
                        },
                        new Product
                        {
                            Code = "259",
                            Name = "Бензинова фракция от АД"
                        },
                        new Product
                        {
                            Code = "260",
                            Name = "Бензин отгон"
                        },
                        new Product
                        {
                            Code = "261",
                            Name = "Бензин реформат"
                        },
                        new Product
                        {
                            Code = "262",
                            Name = "Бензин алкилат"
                        },
                        new Product
                        {
                            Code = "263",
                            Name = "Крекин бензин"
                        },
                        new Product
                        {
                            Code = "264",
                            Name = "МТБЕ"
                        },
                        new Product
                        {
                            Code = "265",
                            Name = "Хидроочистен бензин (ХО- Ксилоли)"
                        },
                        new Product
                        {
                            Code = "266",
                            Name = "Бензин хидрогенизат  (ХОБ-1)"
                        },
                        new Product
                        {
                            Code = "267",
                            Name = "Пиробензин"
                        },
                        new Product
                        {
                            Code = "268",
                            Name = "Керосинова фракция от АД"
                        },
                        new Product
                        {
                            Code = "269",
                            Name = "Керосинова фракция хидрирана"
                        },
                        new Product
                        {
                            Code = "270",
                            Name = "Дизелова фракция от АД"
                        },
                        new Product
                        {
                            Code = "271",
                            Name = "Дизелова фракция от ВДМ"
                        },
                        new Product
                        {
                            Code = "272",
                            Name = "Дизелова фракция от АВД-1"
                        },
                        new Product
                        {
                            Code = "273",
                            Name = "Дизелова фракция от ТК"
                        },
                        new Product
                        {
                            Code = "274",
                            Name = "Дизелова фракция от С-100"
                        },
                        new Product
                        {
                            Code = "275",
                            Name = "Дизелова фракция"
                        },
                        new Product
                        {
                            Code = "276",
                            Name = "Дизелова фракция хидрирана"
                        },
                        new Product
                        {
                            Code = "277",
                            Name = "Атмосферен газьол"
                        },
                        new Product
                        {
                            Code = "278",
                            Name = "Лек каталитичен газьол"
                        },
                        new Product
                        {
                            Code = "279",
                            Name = "Тежък каталитичен газьол"
                        },
                        new Product
                        {
                            Code = "280",
                            Name = "Мазут от първична дестилация"
                        },
                        new Product
                        {
                            Code = "281",
                            Name = "Широка маслена фракция (ШМФ)"
                        },
                        new Product
                        {
                            Code = "282",
                            Name = "Гудрон"
                        },
                        new Product
                        {
                            Code = "283",
                            Name = "Тежка гудронова фракция за собствени нужди(ТГСН)"
                        },
                        new Product
                        {
                            Code = "284",
                            Name = "Тежък гудрон"
                        },
                        new Product
                        {
                            Code = "285",
                            Name = "Газова сяра-течна "
                        },
                    }
                },
                new ProductType
                {
                    Name = "Адитиви",
                    Products = {
                        new Product
                        {
                            Code = "316",
                            Name = "Уловени нефтопродукти"
                        },
                        new Product
                        {
                            Code = "317",
                            Name = "Нефтени утайки"
                        },
                        new Product
                        {
                            Code = "318",
                            Name = "Отпадък (утайки)"
                        },
                        new Product
                        {
                            Code = "319",
                            Name = "Шлам"
                        },
                        new Product
                        {
                            Code = "320",
                            Name = "Некондиционни продукти"
                        },
                        new Product
                        {
                            Code = "321",
                            Name = "Окислени масла"
                        },
                        new Product
                        {
                            Code = "322",
                            Name = "Отработени масла***"
                        },
                        new Product
                        {
                            Code = "323",
                            Name = "Кокс"
                        },
                        new Product
                        {
                            Code = "324",
                            Name = "Незавършена продукция (в инсталациите)"
                        },
                        new Product
                        {
                            Code = "325",
                            Name = "Регенерирано МДЕА"
                        },
                        new Product
                        {
                            Code = "326",
                            Name = "Наситено МДЕА"
                        },
                        new Product
                        {
                            Code = "327",
                            Name = "Свежо МДЕА"
                        },
                        new Product
                        {
                            Code = "328",
                            Name = "Регенерирано МЕА"
                        },
                        new Product
                        {
                            Code = "329",
                            Name = "Наситено МЕА"
                        },
                        new Product
                        {
                            Code = "330",
                            Name = "Свежо МЕА"
                        },
                    }
                },
                new ProductType
                {
                    Name = "Смазочни масла за съоръжения",
                    Products = {
                        new Product
                        {
                            Code = "345",
                            Name = "Масло Shell Tellus 68"
                        },
                        new Product
                        {
                            Code = "346",
                            Name = "Масло Shell Tellus 68 /Tellus S2 M68/"
                        },
                        new Product
                        {
                            Code = "347",
                            Name = "Масло Shell Remula oil X30Gr"
                        },
                        new Product
                        {
                            Code = "348",
                            Name = "Масло АН46"
                        },
                        new Product
                        {
                            Code = "349",
                            Name = "Масло АН68 (И40А)"
                        },
                        new Product
                        {
                            Code = "350",
                            Name = "Масло МХЛ-32 (ИГП18)"
                        },
                        new Product
                        {
                            Code = "351",
                            Name = "Масло МХЛ-68 (ИГП38)"
                        },
                        new Product
                        {
                            Code = "352",
                            Name = "Масло МХЛ-46 (H46A)"
                        },
                        new Product
                        {
                            Code = "353",
                            Name = "Масло МХЛ-150"
                        },
                        new Product
                        {
                            Code = "354",
                            Name = "Масло МВК150 (К150)"
                        },
                        new Product
                        {
                            Code = "355",
                            Name = "Масло МВК220 (К220)"
                        },
                        new Product
                        {
                            Code = "356",
                            Name = "Масло М10Д ( М10Г2к)"
                        },
                        new Product
                        {
                            Code = "357",
                            Name = "Масло РМ68 ( Ролон 68)"
                        },
                        new Product
                        {
                            Code = "358",
                            Name = "Масло РМ68"
                        },
                        new Product
                        {
                            Code = "359",
                            Name = "Масло РМ150"
                        },
                        new Product
                        {
                            Code = "360",
                            Name = "Масло ТП32, (Тп22С и TbA32E)"
                        },
                        new Product
                        {
                            Code = "361",
                            Name = "Масло ТП46 (TbA46E)"
                        },
                        new Product
                        {
                            Code = "362",
                            Name = "Масло 90ЕР (Т90ЕР2)"
                        },
                        new Product
                        {
                            Code = "363",
                            Name = "Масло трансформаторно"
                        },
                        new Product
                        {
                            Code = "364",
                            Name = "Масло Shell Clavus G46"
                        },
                        new Product
                        {
                            Code = "365",
                            Name = "Масло Shell Omala 220"
                        },
                        new Product
                        {
                            Code = "366",
                            Name = "Масло Shell Donax TM"
                        },
                        new Product
                        {
                            Code = "367",
                            Name = "Приста АН460 "
                        },
                        new Product
                        {
                            Code = "368",
                            Name = "Shell Helix Ultra 5W40"
                        },
                        new Product
                        {
                            Code = "369",
                            Name = "Mobil DTE oil LIGHT"
                        },
                        new Product
                        {
                            Code = "370",
                            Name = "масло Shell Tellus S 46"
                        },
                        new Product
                        {
                            Code = "371",
                            Name = "Shell Tellus 46 /Tellus S2 M 46/"
                        },
                        new Product
                        {
                            Code = "372",
                            Name = "'Масло Турбо дизел 15W40/Rimula  15W40/Лукойл Авангард Ултра 15W40"
                        },
                        new Product
                        {
                            Code = "373",
                            Name = "'Масло Shell Vitrea M 150"
                        },
                        new Product
                        {
                            Code = "374",
                            Name = "'Shell Turbo T46"
                        },
                        new Product
                        {
                            Code = "375",
                            Name = "'Масло Приста АН 15"
                        },
                        new Product
                        {
                            Code = "376",
                            Name = "'Масло Shell Termia B /Heat transfer S2/"
                        },
                        new Product
                        {
                            Code = "377",
                            Name = "'Масло Royco 782"
                        },
                        new Product
                        {
                            Code = "378",
                            Name = "'Масло mobilube GX80W90"
                        },
                        new Product
                        {
                            Code = "379",
                            Name = "'Масло редукторно РМ 320"
                        },
                        new Product
                        {
                            Code = "380",
                            Name = "'Трансмисионо толина 90"
                        },
                        new Product
                        {
                            Code = "381",
                            Name = "'Хидравлично МХЛ - 150"
                        },
                        new Product
                        {
                            Code = "382",
                            Name = "'Емулсол 113 Е /Бориол/"
                        },
                        new Product
                        {
                            Code = "383",
                            Name = "'ЛТ2Т / Двутактово масло/"
                        },
                        new Product
                        {
                            Code = "384",
                            Name = "'adinol turbine oil 46"
                        },
                        new Product
                        {
                            Code = "385",
                            Name = "'Енергол АС-С-460"
                        },
                        new Product
                        {
                            Code = "386",
                            Name = "'Шел клавус G-32"
                        },
                        new Product
                        {
                            Code = "387",
                            Name = "'Шел Клавус G - 68"
                        },
                        new Product
                        {
                            Code = "388",
                            Name = "'Шел омала 320"
                        },
                        new Product
                        {
                            Code = "389",
                            Name = "'Шел Корена D-68"
                        },
                        new Product
                        {
                            Code = "390",
                            Name = "'Шел Турбо Т-32"
                        },
                        new Product
                        {
                            Code = "391",
                            Name = "'Shell tellus  S-68"
                        },
                        new Product
                        {
                            Code = "392",
                            Name = "'Мобилубе НD 80W90"
                        },
                        new Product
                        {
                            Code = "393",
                            Name = "'Mobil DTE 11M"
                        },
                        new Product
                        {
                            Code = "394",
                            Name = "'Mobil DTE 25"
                        },
                        new Product
                        {
                            Code = "395",
                            Name = "'Масло MOBIL DTE 16 M"
                        },
                        new Product
                        {
                            Code = "396",
                            Name = "'Масло MOBIL DTE oil Medium"
                        },
                        new Product
                        {
                            Code = "397",
                            Name = "'Масло SHELL TELLUS 100"
                        },
                        new Product
                        {
                            Code = "398",
                            Name = "'Масло ENERGOL RC 100"
                        },
                        new Product
                        {
                            Code = "399",
                            Name = "'AGIP ACER SDE 15W40"
                        },
                        new Product
                        {
                            Code = "400",
                            Name = "'AGIP SMO 20W50"
                        },
                        new Product
                        {
                            Code = "401",
                            Name = "'AGIP BLASIA 150"
                        },
                        new Product
                        {
                            Code = "402",
                            Name = "'AGIP BLASIA 100"
                        },
                        new Product
                        {
                            Code = "403",
                            Name = "'AGIP BLASIA 220"
                        },
                        new Product
                        {
                            Code = "404",
                            Name = "'AGIP BLASIA 680"
                        },
                        new Product
                        {
                            Code = "405",
                            Name = "'AGIP  BLASIA  68"
                        },
                        new Product
                        {
                            Code = "406",
                            Name = "'AGIP OTE 68"
                        },
                        new Product
                        {
                            Code = "407",
                            Name = "'AGIP OSO 46"
                        },
                        new Product
                        {
                            Code = "408",
                            Name = "'Масло хидравлично Н.Н.719.1025"
                        },
                        new Product
                        {
                            Code = "409",
                            Name = "'Масло диелектрично Н.Н. 719.1026"
                        },
                        new Product
                        {
                            Code = "410",
                            Name = "'Масло хидравлично Н.Н. 719.1028"
                        },
                        new Product
                        {
                            Code = "411",
                            Name = "'Масло диелектрично Н.Н. 719.1029"
                        },
                        new Product
                        {
                            Code = "412",
                            Name = "'AGIP OSO 68"
                        },
                        new Product
                        {
                            Code = "413",
                            Name = "'AGIP VAS 460"
                        },
                        new Product
                        {
                            Code = "414",
                            Name = "'Масло Бреокс 50А1000"
                        },
                        new Product
                        {
                            Code = "415",
                            Name = "'Масло Бреокс 1400SP"
                        },
                        new Product
                        {
                            Code = "416",
                            Name = "'TURMSILON U3000W"
                        },
                        new Product
                        {
                            Code = "417",
                            Name = "'АЕРОШЕЛФЛУИД 4"
                        },
                        new Product
                        {
                            Code = "418",
                            Name = "'Масло RARUS SHC 1025"
                        },
                        new Product
                        {
                            Code = "419",
                            Name = "'Масло RARUS 426"
                        },
                        new Product
                        {
                            Code = "420",
                            Name = "'Масло УНИВЕРСАЛ ЕНДЖИНЕ ОЙЛ 10W30"
                        },
                        new Product
                        {
                            Code = "421",
                            Name = "'ШЕЛ ОМАЛА 220HD"
                        },
                        new Product
                        {
                            Code = "422",
                            Name = "'Масло Shell Tivela S200"
                        },
                        new Product
                        {
                            Code = "423",
                            Name = "SILTERM 800"
                        },
                        new Product
                        {
                            Code = "424",
                            Name = "GEM-4-100 kluber syntetik"
                        },
                        new Product
                        {
                            Code = "425",
                            Name = "Shell Omala 150"
                        },
                        new Product
                        {
                            Code = "426",
                            Name = "Shell Tivella S220"
                        },
                        new Product
                        {
                            Code = "427",
                            Name = "Масло Aral Vitam P3580"
                        },
                        new Product
                        {
                            Code = "428",
                            Name = "Кофражно масло КМ15"
                        },
                        new Product
                        {
                            Code = "429",
                            Name = "Масло ENERGOL CS 100"
                        },
                    }
                },
                new ProductType
                {
                    Name = "Смазочни масла за влагане в производството",
                    Products = {
                        new Product
                        {
                            Code = "441",
                            Name = "Масло процесно бяло минерално (Oндина 917 и WOP15)"
                        },
                        new Product
                        {
                            Code = "442",
                            Name = "Процесно масло за ПЕВН Shell Ondina 934"
                        },
                        new Product
                        {
                            Code = "443",
                            Name = "Процесно масло за каучук (Heavy Extract и RPO620N)*"
                        },
                        new Product
                        {
                            Code = "444",
                            Name = "Процесно масло за каучук Shell Flavex 595"
                        },
                    }
                },
                new ProductType
                {
                    Name = "Добавки/присадки",
                    Products = {
                        new Product
                        {
                            Code = "457",
                            Name = "Смазващи присадки за дизелово гориво"
                        },
                        new Product
                        {
                            Code = "458",
                            Name = "Добавки за реактивно и дизелово горива  Стадис 450"
                        },
                        new Product
                        {
                            Code = "459",
                            Name = "Добавки за дизелово гориво  WASA"
                        },
                        new Product
                        {
                            Code = "460",
                            Name = "Депресатори за дизелово гориво"
                        },
                        new Product
                        {
                            Code = "461",
                            Name = "Полифункционална добавка за бензин  Керопур"
                        },
                        new Product
                        {
                            Code = "462",
                            Name = "Биодизел Б100  "
                        },
                        new Product
                        {
                            Code = "463",
                            Name = "Биоетанол (денатуриран)"
                        },
                        new Product
                        {
                            Code = "464",
                            Name = "Биоетанол (неденатуриран)"
                        },
                        new Product
                        {
                            Code = "465",
                            Name = "Нормален хексан"
                        },
                        new Product
                        {
                            Code = "466",
                            Name = "Цетанова присадка за дизелово гориво"
                        },
                        new Product
                        {
                            Code = "467",
                            Name = "АГИДОЛ-1"
                        },
                        new Product
                        {
                            Code = "468",
                            Name = "Йонол 75 за JET A-1"
                        },
                        new Product
                        {
                            Code = "469",
                            Name = "Dodigen 481"
                        },
                        new Product
                        {
                            Code = "470",
                            Name = "Маркиращ разтвор(жълт 124+ син 79)-GOM BLUE BU10Y;  HFA72"
                        },
                        new Product
                        {
                            Code = "471",
                            Name = "Маркиращ разтвор(жълт124+ червен19)- GOM RED BU10Y; HFA88"
                        },
                        new Product
                        {
                            Code = "472",
                            Name = "БЕКТО-HITEC 6437 L (за авт. бензини)"
                        },
                        new Product
                        {
                            Code = "473",
                            Name = "ДЕКТО-HITEC 4691 (за диз. горива)"
                        },
                        new Product
                        {
                            Code = "474",
                            Name = "Химикал-редуциращ dP( FLOMX7C-260CAB)"
                        },
                        new Product
                        {
                            Code = "481",
                            Name = "NEMO 6133 (ШЕЛ)"
                        },
                        new Product
                        {
                            Code = "482",
                            Name = "NEMO 2041 (ШЕЛ)"
                        },
                        new Product
                        {
                            Code = "483",
                            Name = "NEMO 2009 А (ШЕЛ)"
                        },
                        new Product
                        {
                            Code = "484",
                            Name = "NEMO 2022 С (ШЕЛ)"
                        },
                        new Product
                        {
                            Code = "485",
                            Name = "NEMO 4010 (ШЕЛ)"
                        },
                        new Product
                        {
                            Code = "491",
                            Name = "Адитив -1"
                        },
                        new Product
                        {
                            Code = "492",
                            Name = "Адитив -2"
                        },
                        new Product
                        {
                            Code = "493",
                            Name = "Адитив -3"
                        },
                        new Product
                        {
                            Code = "494",
                            Name = "Адитив -4"
                        },
                        new Product
                        {
                            Code = "901",
                            Name = "ЕКО АС 700 (ДГ)"
                        },
                        new Product
                        {
                            Code = "902",
                            Name = "ЕКО Р 996 (Бензин)"
                        },
                        new Product
                        {
                            Code = "903",
                            Name = "ЕКО Адитив (ДГ)"
                        },
                        new Product
                        {
                            Code = "904",
                            Name = "ЕКО Адитив (Бензин)"
                        },
                        new Product
                        {
                            Code = "910",
                            Name = "ОМV DPP 14 ( ДГ)"
                        },
                        new Product
                        {
                            Code = "911",
                            Name = "ОМV GPP 08 ( Бензин)"
                        },
                        new Product
                        {
                            Code = "912",
                            Name = "OMV Адитив ( ДГ)"
                        },
                        new Product
                        {
                            Code = "913",
                            Name = "OMV Адитив ( Бензин)"
                        },
                    }
                },
                new ProductType
                {
                    Name = "Енергоресурси",
                    Products = { }
                },
                new ProductType
                {
                    Name = "Пари",
                    Products = { }
                },
                new ProductType
                {
                    Name = "Химикали",
                    Products = { }
                },
                new ProductType
                {
                    Name = "Електроенергия",
                    Products = { }
                },
                new ProductType
                {
                    Name = "Преработено",
                    Products = { }
                },
                new ProductType
                {
                    Name = "Произведено",
                    Products = { }
                });

            context.SaveChanges("Initial System Loading");
        }
    }
}
