using Dapper;
using Discount.Grpc.Entities;
using Discount.Grpc.Repositories;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        /// <summary>
        /// Injecção do IConfiguration para extrair a conexão do banco de dados
        /// </summary>
        private readonly IConfiguration _configuration;

        public DiscountRepository(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        //Helpers:
        /// <summary>
        /// Fabrica a conexão através do IDbConnection (recebendo como parámetro a conexão do PostgreSQL) armazenada nas configurações da API (AppSettings.json)
        /// </summary>
        /// <returns></returns>
        private IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
        }
        /// <summary>
        /// Retorna os dados de um cupon/desconto com base no produto informado e em caso de nulo, cria um novo cupon com desconto zerado
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        public async Task<Coupon> GetDiscount(string productName)
        {
            using(var conn = CreateConnection())
            {
                var sql = "SELECT * FROM Coupon WHERE ProductName=@Id";
                return await conn.QueryFirstOrDefaultAsync<Coupon>(sql, new { Id = productName })
                    ?? new Coupon { Description = "No Discount Desc", Amount = 0, ProductName = "No Discount" };
            }
        }
        /// <summary>
        /// Cria e salva um novo cupon no banco de dados e retorna como verdadeiro em caso de inserção com sucesso!
        /// </summary>
        /// <param name="coupon"></param>
        /// <returns></returns>
        public async Task<bool> CreateDiscount(Coupon coupon)
        {
            using(var conn= CreateConnection())
            {
                var sql = "INSERT INTO Coupon(ProductName, Description, Amount) VALUES(@ProductName, @Description, @Amount)";
                return await conn.ExecuteAsync(sql, coupon) > 0;
            }
        }
        /// <summary>
        /// Actualiza os dados de um coupon no banco de dados
        /// </summary>
        /// <param name="coupon"></param>
        /// <returns></returns>
        public async Task<bool> UpdateDiscount(Coupon coupon)
        {
            using (var conn = CreateConnection())
            {
                var sql = "UPDATE Coupon SET ProductName=@ProductName, Description=@Description, Amount=@Amount WHERE Id=@Id";
                return await conn.ExecuteAsync(sql, coupon) > 0;
            }
        }
        /// <summary>
        /// Exclui um coupon do banco de dados com base no ID (ProductName) informado
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        public async Task<bool> DeleteDiscount(string productName)
        {
            using (var conn = CreateConnection())
            {
                var sql = "DELETE FROM Coupon WHERE ProductName=@Id";
                return await conn.ExecuteAsync(sql, new { Id = productName }) > 0;
            }
        }
    }
}
