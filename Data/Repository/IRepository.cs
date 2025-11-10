using MottoMap.Models;

namespace MottoMap.Data.Repository
{
    /// <summary>
    /// Interface genérica para operações básicas de repositório
    /// </summary>
    /// <typeparam name="T">Tipo da entidade</typeparam>
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Obtém todos os registros com paginação
        /// </summary>
        /// <param name="paginationParameters">Parâmetros de paginação</param>
        /// <returns>Página de dados</returns>
        Task<DataPage<T>> GetAllAsync(PaginationParameters paginationParameters);
        
        /// <summary>
        /// Obtém um registro por ID
        /// </summary>
        /// <param name="id">ID do registro</param>
        /// <returns>Registro encontrado ou null</returns>
        Task<T?> GetByIdAsync(int id);
        
        /// <summary>
        /// Adiciona um novo registro
        /// </summary>
        /// <param name="entity">Entidade a ser adicionada</param>
        /// <returns>Entidade adicionada</returns>
        Task<T> AddAsync(T entity);
        
        /// <summary>
        /// Atualiza um registro existente
        /// </summary>
        /// <param name="entity">Entidade a ser atualizada</param>
        /// <returns>Entidade atualizada</returns>
        Task<T> UpdateAsync(T entity);
        
        /// <summary>
        /// Remove um registro por ID
        /// </summary>
        /// <param name="id">ID do registro a ser removido</param>
        /// <returns>True se removido com sucesso</returns>
        Task<bool> DeleteAsync(int id);
        
        /// <summary>
        /// Verifica se um registro existe
        /// </summary>
        /// <param name="id">ID do registro</param>
        /// <returns>True se existe</returns>
        Task<bool> ExistsAsync(int id);
    }
}