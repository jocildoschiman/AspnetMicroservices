using Ordering.Application.Features.Orders.Queries.GetOrdersList;
using Ordering.Domain.Entities;
using AutoMapper;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;

namespace Ordering.Application.Mappings
{
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Cria o perfil de mapeamento usando o AutoMapper
        /// </summary>
        public MappingProfile()
        {
            CreateMap<Order, OrdersVm>().ReverseMap();
            CreateMap<Order, CheckoutOrderCommand>().ReverseMap();
            CreateMap<Order, UpdateOrderCommand>().ReverseMap();
        }
    }
}
