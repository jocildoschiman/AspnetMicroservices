using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ILogger<CheckoutOrderCommandHandler> _logger;
        /// <summary>
        /// Injecção de dependência
        /// </summary>
        /// <param name="orderRepository"></param>
        /// <param name="mapper"></param>
        /// <param name="emailService"></param>
        /// <param name="logger"></param>
        public CheckoutOrderCommandHandler(IOrderRepository orderRepository, IMapper mapper, 
            IEmailService emailService, ILogger<CheckoutOrderCommandHandler> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _emailService = emailService;
            _logger = logger;
        }
        /// <summary>
        /// Lança um novo pedido no banco de dados
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var orderEntity = _mapper.Map<Order>(request);
            var newOrder = await _orderRepository.AddAsync(orderEntity);

            _logger.LogInformation($"Order {newOrder.Id} is successfully created.");

            await SendMail(newOrder);

            return newOrder.Id;
        }
        /// <summary>
        /// Envio de e-mail logo após o pedido é feito
        /// </summary>
        /// <param name="newOrder"></param>
        /// <returns></returns>
        private async Task SendMail(Order newOrder)
        {
            var email = new Email() { To = "schimanscky@hotmail.com", Body = $"Order was created", Subject = "New Order" };
            try
            {
                await _emailService.SendMail(email);
            }
            catch (Exception error)
            {
                _logger.LogError($"Order {newOrder.Id} failed due to an error with the mail service: {error.Message}");
            }
        }
    }
}
