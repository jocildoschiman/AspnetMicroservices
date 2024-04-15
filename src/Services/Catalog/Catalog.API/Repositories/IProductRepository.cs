using Catalog.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.API.Repositories
{
    public interface IProductRepository
    {
        /// <summary>
        /// Retorna a lista de todos os produtos existentes no banco de dados
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetProducts();
        /// <summary>
        /// Retorna as informações de um único produto com base no ID infomado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Product> GetProduct(string id);
        /// <summary>
        /// Retorna a lista de produtos por nome informado
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetProductByName(string name);
        /// <summary>
        /// Retorna a lista de produtos por nome da categoria informada
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        Task<IEnumerable<Product>> GetProductByCategory(string categoryName);
        /// <summary>
        /// Insere um novo produto no banco de dados
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task CreateProduct(Product product);
        /// <summary>
        /// Actualiza as informações de um produto no banco de dados
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<bool> UpdateProduct(Product product);
        /// <summary>
        /// Exclui um produto do banco de dados com base no ID infomado
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteProduct(string id);
    }
}
