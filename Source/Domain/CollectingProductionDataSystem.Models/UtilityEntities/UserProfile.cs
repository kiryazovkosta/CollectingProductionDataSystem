namespace CollectingProductionDataSystem.Models.UtilityEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using AutoMapper;
    using CollectingProductionDataSystem.Infrastructure.Mapping;
    using CollectingProductionDataSystem.Models.Identity;
    using CollectingProductionDataSystem.Models.Inventories;
    using CollectingProductionDataSystem.Models.Productions;

    [Serializable]
    public class UserProfile : IMapFrom<ApplicationUser>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        /// <summary>
        /// Creates the mappings.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<ApplicationUser, UserProfile>()
                .ForMember(p => p.Parks, opt => opt.MapFrom(p => p.ApplicationUserParks.Select(x => x.Park)))
                .ForMember(p => p.ProcessUnits, opt => opt.MapFrom(p => p.ApplicationUserProcessUnits.Select(x => x.ProcessUnit)));

        }

        public string FullName => $"{$"{this?.FirstName} {this?.MiddleName}".Trim()} {this?.LastName}".Trim();
        
        public string Occupation { get; set; }

        public IEnumerable<RoleModel> UserRoles { get; set; }

        public IEnumerable<ProcessUnitModel> ProcessUnits { get; set; }

        public IEnumerable<ParkModel> Parks { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ParkModel : IMapFrom<Park>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AreaModel Area { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class AreaModel : IMapFrom<Area>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ProcessUnitModel : IMapFrom<ProcessUnit>
    {
        public int Id { get; set; }
        public string ShortName { get; set; }

        public string FullName { get; set; }

        public FactoryModel Factory { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class FactoryModel : IMapFrom<Factory>
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public string FullName { get; set; }
    }

    [Serializable]
    public class RoleModel : IMapFrom<ApplicationRole>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
