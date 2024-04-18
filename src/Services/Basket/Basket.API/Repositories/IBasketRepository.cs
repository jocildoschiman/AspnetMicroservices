using Basket.API.Entities;
using System.Threading.Tasks;

namespace Basket.API.Repositories
{
    public interface IBasketRepository
    {
        /// <summary>
        /// Este método vai criar e retornar o carrinho de compra do usuário
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<ShoppingCart> GetBasket(string userName);
        /// <summary>
        /// Este método actualiza o carrinho do usuário
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        Task<ShoppingCart> UpdateBasket(ShoppingCart basket);
        /// <summary>
        /// Este método exclui o carrinho do usuário do banco de dados
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task DeleteBasket (string userName);
    }
}
