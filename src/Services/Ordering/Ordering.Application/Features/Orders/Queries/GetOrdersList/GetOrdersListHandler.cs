using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListHandler : IRequestHandler<GetOrdersListQuery, List<OrdersVm>>
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        /// <summary>
        /// Injecção de dependência
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="mapper"></param>
        public GetOrdersListHandler(IOrderRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        /// <summary>
        /// Retorna a lista de pedidos associados ao usuário
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<OrdersVm>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _repository.GetOrderByUserName(request.UserName);
            return _mapper.Map<List<OrdersVm>>(orderList);
        }
    }
}
