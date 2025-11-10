using Microsoft.EntityFrameworkCore;
using MottoMap.Data.AppData;
using MottoMap.Models;
using System.Linq.Expressions;

namespace MottoMap.Data.Repository
{
    /// <summary>
    /// Implementação base do repositório genérico
    /// </summary>
    /// <typeparam name="T">Tipo da entidade</typeparam>
    public class BaseRepository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<DataPage<T>> GetAllAsync(PaginationParameters paginationParameters)
        {
            var query = _dbSet.AsQueryable();

            // Aplicar filtro de busca se fornecido
            if (!string.IsNullOrEmpty(paginationParameters.SearchTerm))
            {
                query = ApplySearch(query, paginationParameters.SearchTerm);
            }

            // Aplicar ordenação se fornecida
            if (!string.IsNullOrEmpty(paginationParameters.SortBy))
            {
                query = ApplySort(query, paginationParameters.SortBy, paginationParameters.SortDirection);
            }

            // Contar total de itens
            var totalItems = await query.CountAsync();

            // Aplicar paginação
            var items = await query
                .Skip((paginationParameters.PageNumber - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .ToListAsync();

            return new DataPage<T>(items, paginationParameters.PageNumber, paginationParameters.PageSize, totalItems);
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<bool> DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null)
                return false;

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            return await _dbSet.FindAsync(id) != null;
        }

        /// <summary>
        /// Método virtual para aplicar busca/filtro - deve ser sobrescrito pelas classes filhas
        /// </summary>
        /// <param name="query">Query base</param>
        /// <param name="searchTerm">Termo de busca</param>
        /// <returns>Query com filtro aplicado</returns>
        protected virtual IQueryable<T> ApplySearch(IQueryable<T> query, string searchTerm)
        {
            // Implementação padrão - retorna query sem filtro
            return query;
        }

        /// <summary>
        /// Método virtual para aplicar ordenação - deve ser sobrescrito pelas classes filhas
        /// </summary>
        /// <param name="query">Query base</param>
        /// <param name="sortBy">Campo para ordenação</param>
        /// <param name="sortDirection">Direção da ordenação</param>
        /// <returns>Query com ordenação aplicada</returns>
        protected virtual IQueryable<T> ApplySort(IQueryable<T> query, string sortBy, string sortDirection)
        {
            // Implementação padrão - retorna query sem ordenação específica
            return query;
        }
    }
}