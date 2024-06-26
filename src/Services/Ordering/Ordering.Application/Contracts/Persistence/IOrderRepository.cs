﻿using Ordering.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Persistence
{
    public interface IOrderRepository: IAsyncRepository<Order>
    {
        /// <summary>
        /// Retorna os pedidos associados ao usuário passado como parámetro
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<IEnumerable<Order>> GetOrderByUserName(string userName);

    }
}
