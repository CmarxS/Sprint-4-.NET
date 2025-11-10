using Microsoft.EntityFrameworkCore;
using MottoMap.Data.AppData;
using MottoMap.Models;

namespace MottoMap.Data.Repository
{
    /// <summary>
    /// Implementação do repositório de Filiais
    /// </summary>
    public class FilialRepository : BaseRepository<FilialEntity>, IFilialRepository
    {
        public FilialRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<DataPage<FilialEntity>> GetByCidadeAsync(string cidade, PaginationParameters paginationParameters)
        {
            var query = _dbSet.Where(f => f.Cidade.Contains(cidade));

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

            return new DataPage<FilialEntity>(items, paginationParameters.PageNumber, paginationParameters.PageSize, totalItems);
        }

        public async Task<DataPage<FilialEntity>> GetByEstadoAsync(string estado, PaginationParameters paginationParameters)
        {
            var query = _dbSet.Where(f => f.Estado == estado.ToUpper());

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

            return new DataPage<FilialEntity>(items, paginationParameters.PageNumber, paginationParameters.PageSize, totalItems);
        }

        public async Task<FilialEntity?> GetWithRelationsAsync(int id)
        {
            return await _dbSet
                .Include(f => f.Funcionarios)
                .Include(f => f.Motos)
                .FirstOrDefaultAsync(f => f.IdFilial == id);
        }

        public async Task<FilialStatsDto> GetStatsAsync(int id)
        {
            var filial = await _dbSet
                .Include(f => f.Funcionarios)
                .Include(f => f.Motos)
                .FirstOrDefaultAsync(f => f.IdFilial == id);

            if (filial == null)
            {
                return new FilialStatsDto();
            }

            return new FilialStatsDto
            {
                IdFilial = filial.IdFilial,
                NomeFilial = filial.Nome,
                TotalFuncionarios = filial.Funcionarios.Count,
                TotalMotos = filial.Motos.Count
            };
        }

        protected override IQueryable<FilialEntity> ApplySearch(IQueryable<FilialEntity> query, string searchTerm)
        {
            return query.Where(f => 
                f.Nome.Contains(searchTerm) || 
                f.Endereco.Contains(searchTerm) || 
                f.Cidade.Contains(searchTerm) ||
                f.Estado.Contains(searchTerm) ||
                (f.CEP != null && f.CEP.Contains(searchTerm)));
        }

        protected override IQueryable<FilialEntity> ApplySort(IQueryable<FilialEntity> query, string sortBy, string sortDirection)
        {
            var isDescending = sortDirection.ToLower() == "desc";

            return sortBy.ToLower() switch
            {
                "nome" => isDescending ? query.OrderByDescending(f => f.Nome) : query.OrderBy(f => f.Nome),
                "endereco" => isDescending ? query.OrderByDescending(f => f.Endereco) : query.OrderBy(f => f.Endereco),
                "cidade" => isDescending ? query.OrderByDescending(f => f.Cidade) : query.OrderBy(f => f.Cidade),
                "estado" => isDescending ? query.OrderByDescending(f => f.Estado) : query.OrderBy(f => f.Estado),
                "cep" => isDescending ? query.OrderByDescending(f => f.CEP) : query.OrderBy(f => f.CEP),
                _ => query.OrderBy(f => f.IdFilial)
            };
        }
    }
}