using Basket.API.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }
        /// <summary>
        /// Cria o carrinho de compras e salva-o no banco de dados em cache do Redis
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<ShoppingCart> GetBasket(string userName)
        {
            var basket = await _redisCache.GetStringAsync(userName);
            if (string.IsNullOrEmpty(basket))
                return null;

            return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }
        /// <summary>
        /// Actualiza o carrinho de compra (incremento e decremento de quantidades de produtos)
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        public async Task<ShoppingCart> UpdateBasket(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.UserName, JsonConvert.SerializeObject(basket));
            return await GetBasket(basket.UserName);
        }
        /// <summary>
        /// Exclui o carrinho de compras
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task DeleteBasket(string userName)
        {
            await _redisCache.RemoveAsync(userName);
        }
    }
}
