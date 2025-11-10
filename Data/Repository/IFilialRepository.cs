using MottoMap.Models;

namespace MottoMap.Data.Repository
{
    /// <summary>
    /// Interface específica para operações do repositório de Filiais
    /// </summary>
    public interface IFilialRepository : IRepository<FilialEntity>
    {
        /// <summary>
        /// Busca filiais por cidade
        /// </summary>
        /// <param name="cidade">Nome da cidade</param>
        /// <param name="paginationParameters">Parâmetros de paginação</param>
        /// <returns>Página de filiais da cidade</returns>
        Task<DataPage<FilialEntity>> GetByCidadeAsync(string cidade, PaginationParameters paginationParameters);
        
        /// <summary>
        /// Busca filiais por estado
        /// </summary>
        /// <param name="estado">Sigla do estado</param>
        /// <param name="paginationParameters">Parâmetros de paginação</param>
        /// <returns>Página de filiais do estado</returns>
        Task<DataPage<FilialEntity>> GetByEstadoAsync(string estado, PaginationParameters paginationParameters);
        
        /// <summary>
        /// Obtém filial com seus funcionários e motos
        /// </summary>
        /// <param name="id">ID da filial</param>
        /// <returns>Filial com funcionários e motos</returns>
        Task<FilialEntity?> GetWithRelationsAsync(int id);
        
        /// <summary>
        /// Obtém estatísticas da filial (quantidade de funcionários e motos)
        /// </summary>
        /// <param name="id">ID da filial</param>
        /// <returns>Objeto com estatísticas da filial</returns>
        Task<FilialStatsDto> GetStatsAsync(int id);
    }

    /// <summary>
    /// DTO para estatísticas da filial
    /// </summary>
    public class FilialStatsDto
    {
        public int IdFilial { get; set; }
        public string NomeFilial { get; set; } = string.Empty;
        public int TotalFuncionarios { get; set; }
        public int TotalMotos { get; set; }
    }
}