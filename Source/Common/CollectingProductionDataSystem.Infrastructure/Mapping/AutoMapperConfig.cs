namespace CollectingProductionDataSystem.Infrastructure.Mapping
{
    using System;
    using System.Collections.Generic;
    
    using System.Linq;
    using System.Reflection;
    using AutoMapper;
    using CollectingProductionDataSystem.Constants;
    using CollectingProductionDataSystem.Infrastructure.Extentions;

        public static class AutoMapperConfig
        {
            public static void RegisterMappings()
            {
                var types = Assembly.GetExecutingAssembly().GetExportedTypes();
                ICollection<Type> viewModelTypes = new List<Type>();
                foreach (var assemblyName in Namespace.viewModels)
                {
                    viewModelTypes.AddRange(Assembly.Load(assemblyName).GetExportedTypes());
                }

                LoadStandardMappings(types);
                LoadStandardMappings(viewModelTypes);

                LoadCustomMappings(types);
                LoadCustomMappings(viewModelTypes);
            }

            private static void LoadStandardMappings(IEnumerable<Type> types)
            {
                var maps = (from t in types
                            from i in t.GetInterfaces()
                            where i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IMapFrom<>) &&
                                  !t.IsAbstract &&
                                  !t.IsInterface
                            select new
                            {
                                Source = i.GetGenericArguments()[0],
                                Destination = t
                            }).ToArray();

                foreach (var map in maps)
                {
                    Mapper.CreateMap(map.Source, map.Destination);
                    Mapper.CreateMap(map.Destination, map.Source);
                }
            }

            private static void LoadCustomMappings(IEnumerable<Type> types)
            {
                var maps = (from t in types
                            from i in t.GetInterfaces()
                            where typeof(IHaveCustomMappings).IsAssignableFrom(t) &&
                                  !t.IsAbstract &&
                                  !t.IsInterface
                            select (IHaveCustomMappings)Activator.CreateInstance(t)).ToArray();

                foreach (var map in maps)
                {
                    map.CreateMappings(Mapper.Configuration);
                }
            }
        }
}