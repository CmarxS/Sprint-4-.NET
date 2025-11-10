using MottoMap.Models;

namespace MottoMap.Data.Repository
{
    /// <summary>
    /// Interface específica para operações do repositório de Funcionários
    /// </summary>
    public interface IFuncionarioRepository : IRepository<FuncionarioEntity>
    {
        /// <summary>
        /// Busca funcionários por filial
        /// </summary>
        /// <param name="idFilial">ID da filial</param>
        /// <param name="paginationParameters">Parâmetros de paginação</param>
        /// <returns>Página de funcionários da filial</returns>
        Task<DataPage<FuncionarioEntity>> GetByFilialAsync(int idFilial, PaginationParameters paginationParameters);
        
        /// <summary>
        /// Busca funcionário por email
        /// </summary>
        /// <param name="email">Email do funcionário</param>
        /// <returns>Funcionário encontrado ou null</returns>
        Task<FuncionarioEntity?> GetByEmailAsync(string email);
        
        /// <summary>
        /// Busca funcionários por função
        /// </summary>
        /// <param name="funcao">Função do funcionário</param>
        /// <param name="paginationParameters">Parâmetros de paginação</param>
        /// <returns>Página de funcionários com a função especificada</returns>
        Task<DataPage<FuncionarioEntity>> GetByFuncaoAsync(string funcao, PaginationParameters paginationParameters);
        
        /// <summary>
        /// Verifica se email já existe para outro funcionário
        /// </summary>
        /// <param name="email">Email a ser verificado</param>
        /// <param name="idFuncionarioAtual">ID do funcionário atual (para updates)</param>
        /// <returns>True se email já existe</returns>
        Task<bool> EmailExistsAsync(string email, int? idFuncionarioAtual = null);
    }
}