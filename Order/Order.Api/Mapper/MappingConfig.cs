using AutoMapper;
using Order.Domain.Dtos.Event;
using Order.Domain.Dtos.Order;
using Order.Domain.Entities;

namespace Order.Api.Mapper;

public class MappingConfig
{
    public static MapperConfiguration RegisterMaps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
			#region Auth
			// ajustar
			//config.CreateMap<RegisterDto, User>().ReverseMap();
			#endregion

			#region Order

			config.CreateMap<OrderDto, Domain.Entities.Order>().ReverseMap();
			config.CreateMap<AddOrderDto, OrderDto>().ReverseMap();
			config.CreateMap<OrderItemDto, OrderItem>().ReverseMap();
			config.CreateMap<AddOrderItemDto, OrderItem>().ReverseMap();

			#endregion

		});
        return mappingConfig;
    }
}
