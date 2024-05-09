using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;
        private readonly ILogger<UpdateOrderCommandHandler> _logger;
        /// <summary>
        /// Injecta os objectos que serão usados na classe
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        public UpdateOrderCommandHandler(IMapper mapper, IOrderRepository repository, 
            ILogger<UpdateOrderCommandHandler> logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }
        /// <summary>
        /// Actualiza o pedido no banco de dados
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<Unit> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
            //Actualizar o pedido:
            var orderToUpdate = await _repository.GetByIdAsync(request.Id);
            if(orderToUpdate == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }
            _mapper.Map(request, orderToUpdate, typeof(UpdateOrderCommand), typeof(Order));
            await _repository.UpdateAsync(orderToUpdate);

            _logger.LogInformation($"Order {orderToUpdate.Id} is successfully updated.");

            return Unit.Value;
        }

    }
}
