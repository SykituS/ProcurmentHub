using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrpcShared;
using ProcurementHub.Model.CustomModels;
using ProcurementHub.Model.Models;

namespace ProcurementHub.Infrastructure
{
    public class MapperConfig
    {
        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GRPCPerson, PersonsModel>()
                    .ForMember(dest => dest.FullName, act => act.MapFrom(src => src.FirstName + " " + src.LastName))
                    .ReverseMap();

                cfg.CreateMap<GRPCRestaurant, TeamRestaurantsModel>();
                cfg.CreateMap<GRPCFullOrderInformations, OrderModel>()
                    .ForMember(dest => dest.ID, act => act.MapFrom(src => Guid.Parse(src.Id)))
                    .ForMember(dest => dest.TeamRestaurantID, act => act.MapFrom(src => src.RestaurantId))
                    .ForMember(dest => dest.Restaurants, act => act.MapFrom(src => src.Restaurant))
                    .ForMember(dest => dest.OrderPayedBy, act => act.MapFrom(src => src.OrderPayedBy))
                    .ForMember(dest => dest.OrderStartedBy, act => act.MapFrom(src => src.OrderStartedBy))
                    .ForMember(dest => dest.OrderStartedOn, act => act.MapFrom(src => src.StartedOn.ToDateTime()))
                    .ForMember(dest => dest.OrderFinishedOn, act => act.MapFrom(src => src.FinishedOn.ToDateTime()))
                    .ForMember(dest => dest.TotalPriceOfOrder, act => act.MapFrom(src => decimal.Parse(src.TotalPriceOfOrder.Price)));

                cfg.CreateMap<GRPCFullOrderItem, OrderItemsModel>()
                    .ForMember(dest => dest.ItemSelectedBy, act => act.MapFrom(src => src.SelectedBy))
                    .ForMember(dest => dest.TeamOrdersID, act => act.MapFrom(src => Guid.Parse(src.TeamOrderId)))
                    .ForMember(dest => dest.TeamRestaurantsItemPrice, act => act.MapFrom(src => decimal.Parse(src.RestaurantItem.Price.Price)))
                    .ForMember(dest => dest.TeamRestaurantsItemName, act => act.MapFrom(src => src.RestaurantItem.Name))
                    .ForMember(dest => dest.TeamRestaurantsItemDescription, act => act.MapFrom(src => src.RestaurantItem.Description))
                    .ForMember(dest => dest.TotalPriceOfItem, act => act.MapFrom(src => decimal.Parse(src.TotalPriceOfItem.Price)))
                    .ForMember(dest => dest.DividedPrice, act => act.MapFrom(src => decimal.Parse(src.DividePrice.Price)))
                    .ForMember(dest => dest.DivideToken, act => act.MapFrom(src => src.DivideToken == "" ? (Guid?)null : Guid.Parse(src.DivideToken)));
            });
            
            return config.CreateMapper();
        }
    }
}
