using AutoMapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using Discount.Grpc.Repositories;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Discount.Grpc.Services
{
    public class DiscountService : DiscountProtoService.DiscountProtoServiceBase
    {
        private readonly IDiscountRepository _repository;
        private readonly ILogger<DiscountService> _logger;
        private readonly IMapper _mapper;
        public DiscountService(IDiscountRepository repository, ILogger<DiscountService> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        /// <summary>
        /// Retorna os dados do desconto com base no ID do produto passado como parámetro
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        /// <exception cref="RpcException"></exception>
        public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            var coupon = await _repository.GetDiscount(request.ProductName);
            if(coupon == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));
            }
            _logger.LogInformation("Discount is retrivied for ProductName : {productName}, Amount: {amount}", coupon.ProductName, coupon.Amount);
            var couponModel = _mapper.Map<CouponModel>(coupon);
            return couponModel;
        }
        /// <summary>
        /// Cria o desconto e salva-o no banco de dados
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _repository.CreateDiscount(coupon);
            _logger.LogInformation("Discount is successfully created. ProductName: {ProductName}", coupon.ProductName);
            return _mapper.Map<CouponModel>(coupon);
        }
        /// <summary>
        /// Actualiza o desconto existente no banco de dados com base na requisição
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
        {
            var coupon = _mapper.Map<Coupon>(request.Coupon);
            await _repository.UpdateDiscount(coupon);
            _logger.LogInformation("Discount is successfully updated. ProductName: {ProductName}", coupon.ProductName);
            return _mapper.Map<CouponModel>(coupon);
        }
        /// <summary>
        /// Exclui o desconto do banco de dados com base na requisição passada
        /// </summary>
        /// <param name="request"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
        {
            var deleted = await _repository.DeleteDiscount(request.ProductName);
            var response = new DeleteDiscountResponse
            {
                Success = deleted
            };
            return response;
        }
    }
}
