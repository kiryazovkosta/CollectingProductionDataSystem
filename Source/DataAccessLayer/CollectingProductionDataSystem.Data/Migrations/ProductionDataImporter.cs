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
                                    }
                                },
                                new ProcessUnit
                                {
                                    ShortName = "ВДТК",
                                    FullName = "Инсталация ВДМ-2 и ТК",
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
                                    ShortName = "ККр-с.200, с.300",
                                    FullName = "Инсталация ККр (с.200) и Абс. И Гфр. (с.300)",
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
                                    UnitsConfigs = {

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
