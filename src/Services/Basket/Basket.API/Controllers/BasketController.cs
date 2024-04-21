using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _repository;
        private readonly DiscountGrpcService _discountGrpcService;
        public BasketController(IBasketRepository repository, DiscountGrpcService discountGrpcService)
        {
            _repository = repository;
            _discountGrpcService = discountGrpcService;
        }
        /// <summary>
        /// Retorna o carrinho existente no banco de dados e em caso nulo, cria um novo carrinho
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpGet("{userName}", Name ="GetBasket")]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
        {
            var basket = await _repository.GetBasket(userName);
            return Ok(basket ?? new ShoppingCart(userName));
        }
        /// <summary>
        /// Actualiza o carrinho de compra associado ao usuário
        /// </summary>
        /// <param name="basket"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
        {
            //Todo: Communicate with Discount.Grpc
            
            //and calculate latest prices of products into the shopping cart
            //consume Discount Grpc
            foreach(var item in basket.Items)
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= Convert.ToDecimal(coupon.Amount);
            }

            return Ok(await _repository.UpdateBasket(basket)); 
        }
        /// <summary>
        /// Exclui o carrinho do usuário do banco de dados
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [HttpDelete("{userName}", Name ="DeleteBasket")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteBasket(string userName)
        {
            await _repository.DeleteBasket(userName);
            return Ok();
        }
    }
}
