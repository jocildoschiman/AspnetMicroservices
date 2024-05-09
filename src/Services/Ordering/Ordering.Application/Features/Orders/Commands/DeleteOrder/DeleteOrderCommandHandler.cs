using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand>
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _repository;
        private readonly ILogger<DeleteOrderCommandHandler> _logger;
        /// <summary>
        /// Injecção de dependência
        /// </summary>
        /// <param name="mapper"></param>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        public DeleteOrderCommandHandler(IMapper mapper, IOrderRepository repository, 
            ILogger<DeleteOrderCommandHandler> logger)
        {
            _mapper = mapper;
            _repository = repository;
            _logger = logger;
        }
        /// <summary>
        /// Exclui um pedido do banco de dados
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<Unit> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var orderToDelete = await _repository.GetByIdAsync(request.Id);
            if(orderToDelete == null)
            {
                throw new NotFoundException(nameof(Order), request.Id);
            }
            await _repository.DeleteAsync(orderToDelete);
            _logger.LogInformation($"Order {orderToDelete.Id} is successfully deleted.");

            return Unit.Value;
        }
    }
}
