using Ordering.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Ordering.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : EntityBase
    {
        /// <summary>
        /// Retorna uma lista de todos os registos da classe passada como parámetro
        /// </summary>
        /// <returns></returns>
        Task<IReadOnlyList<T>> GetAllAsync();
        /// <summary>
        /// Retorna uma lista de registo(s) do banco de dados com base em filtros passados como parámetro
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// Retorna uma lista de registo(s) do banco de dados com base em filtros passados como parámetros, com possibilidade de ordenar as informações retornadas
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="includeString"></param>
        /// <param name="disableTracking"></param>
        /// <returns></returns>
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeString = null, bool disableTracking = true);
        /// <summary>
        /// Retorna uma lista de registo(s) do banco de dados com base em filtros passados como parámetros, com possibilidade de ordenar as informações retornadas bem como associar outras tabelas/classes nesta lista retornada
        /// </summary>
        /// <param name="predicate"></param>
        /// <param name="orderBy"></param>
        /// <param name="includes"></param>
        /// <param name="disableTracking"></param>
        /// <returns></returns>
        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<Expression<Func<T, object>>> includes = null, 
            bool disableTracking = true);
        /// <summary>
        /// Retorna um registo do banco de dados com base no ID informado como parámetro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> GetByIdAsync(int id);
        /// <summary>
        /// Insere um novo regito no banco de dados
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<T> AddAsync(T entity);
        /// <summary>
        /// Actualiza um registo do banco de dados
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> UpdateAsync(T entity);  
    }
}
