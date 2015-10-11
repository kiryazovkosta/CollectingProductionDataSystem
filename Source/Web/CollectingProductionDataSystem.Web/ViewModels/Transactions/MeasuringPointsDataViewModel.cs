using AutoMapper;
using CollectingProductionDataSystem.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CollectingProductionDataSystem.Models.Transactions;

namespace CollectingProductionDataSystem.Web.ViewModels.Transactions
{
    public class MeasuringPointsDataViewModel : IMapFrom<MeasuringPointsConfigsData>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public decimal AvtoQuantity { get; set; }

        public decimal JpQuantity { get; set; }

        public decimal SeaQuantity { get; set; }

        public decimal PipeQuantity { get; set; }

        public decimal TotalQuantity { get; set; }
        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<MeasuringPointsConfigsData, MeasuringPointsDataViewModel>()
                .ForMember(p => p.ProductName, opt => opt.MapFrom(p => p.ProductName));
        }
    }
}