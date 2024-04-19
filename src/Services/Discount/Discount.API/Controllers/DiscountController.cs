using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Discount.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DiscountController : ControllerBase
    {
        /// <summary>
        /// Injecção do repositório-alvo
        /// </summary>
        private readonly IDiscountRepository _repository;

        public DiscountController(IDiscountRepository repository)
        {
            _repository = repository?? throw new ArgumentNullException(nameof(repository));
        }
        /// <summary>
        /// Retorna os dados de um cupon/desconto (recebendo como parámetro o ID do produto)
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        [HttpGet("{productName}", Name ="GetDiscount")]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            return Ok(await _repository.GetDiscount(productName));
        }
        /// <summary>
        /// Cria e salva um novo cupon/desconto no banco de dados
        /// </summary>
        /// <param name="coupon"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> CreateDiscount([FromBody] Coupon coupon)
        {
            await _repository.CreateDiscount(coupon);
            return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
        }
        /// <summary>
        /// Actualiza os dados do cupon/desconto no banco de dados
        /// </summary>
        /// <param name="coupon"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Coupon>> UpdateDiscount([FromBody] Coupon coupon)
        {
            return Ok(await _repository.UpdateDiscount(coupon));
        }
        /// <summary>
        /// Exclui um cupon no banco de dados (recebendo como parámetro o ID do produto)
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        [HttpDelete("{productName}", Name ="DeleteDiscount")]
        [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteDiscount(string productName)
        {
            return Ok(await _repository.DeleteDiscount(productName));
        }

    }
}
