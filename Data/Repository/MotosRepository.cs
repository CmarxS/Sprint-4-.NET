using Microsoft.EntityFrameworkCore;
using MottoMap.Data.AppData;
using MottoMap.Models;

namespace MottoMap.Data.Repository
{
    /// <summary>
    /// Implementação do repositório de Motos
    /// </summary>
    public class MotosRepository : BaseRepository<MotosEntity>, IMotosRepository
    {
        public MotosRepository(ApplicationContext context) : base(context)
        {
        }

        public override async Task<MotosEntity?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(m => m.Filial)
                .FirstOrDefaultAsync(m => m.IdMoto == id);
        }

        public override async Task<DataPage<MotosEntity>> GetAllAsync(PaginationParameters paginationParameters)
        {
            var query = _dbSet.Include(m => m.Filial).AsQueryable();

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

            return new DataPage<MotosEntity>(items, paginationParameters.PageNumber, paginationParameters.PageSize, totalItems);
        }

        public async Task<DataPage<MotosEntity>> GetByFilialAsync(int idFilial, PaginationParameters paginationParameters)
        {
            var query = _dbSet
                .Include(m => m.Filial)
                .Where(m => m.IdFilial == idFilial);

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

            return new DataPage<MotosEntity>(items, paginationParameters.PageNumber, paginationParameters.PageSize, totalItems);
        }

        public async Task<MotosEntity?> GetByPlacaAsync(string placa)
        {
            return await _dbSet
                .Include(m => m.Filial)
                .FirstOrDefaultAsync(m => m.Placa == placa);
        }

        public async Task<DataPage<MotosEntity>> GetByMarcaAsync(string marca, PaginationParameters paginationParameters)
        {
            var query = _dbSet
                .Include(m => m.Filial)
                .Where(m => m.Marca.Contains(marca));

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

            return new DataPage<MotosEntity>(items, paginationParameters.PageNumber, paginationParameters.PageSize, totalItems);
        }

        public async Task<DataPage<MotosEntity>> GetByAnoAsync(int ano, PaginationParameters paginationParameters)
        {
            var query = _dbSet
                .Include(m => m.Filial)
                .Where(m => m.Ano == ano);

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

            return new DataPage<MotosEntity>(items, paginationParameters.PageNumber, paginationParameters.PageSize, totalItems);
        }

        public async Task<DataPage<MotosEntity>> GetByQuilometragemRangeAsync(int quilometragemMin, int quilometragemMax, PaginationParameters paginationParameters)
        {
            var query = _dbSet
                .Include(m => m.Filial)
                .Where(m => m.Quilometragem >= quilometragemMin && m.Quilometragem <= quilometragemMax);

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

            return new DataPage<MotosEntity>(items, paginationParameters.PageNumber, paginationParameters.PageSize, totalItems);
        }

        public async Task<bool> PlacaExistsAsync(string placa, int? idMotoAtual = null)
        {
            if (idMotoAtual.HasValue)
            {
                // Para atualizações - verificar se existe outra moto com a mesma placa
                var count = await _dbSet
                    .Where(m => m.Placa == placa && m.IdMoto != idMotoAtual.Value)
                    .CountAsync();
                return count > 0;
            }
            else
            {
                // Para criações - verificar se existe qualquer moto com a placa
                var count = await _dbSet
                    .Where(m => m.Placa == placa)
                    .CountAsync();
                return count > 0;
            }
        }

        protected override IQueryable<MotosEntity> ApplySearch(IQueryable<MotosEntity> query, string searchTerm)
        {
            return query.Where(m => 
                m.Marca.Contains(searchTerm) || 
                m.Modelo.Contains(searchTerm) || 
                m.Placa.Contains(searchTerm) ||
                (m.Cor != null && m.Cor.Contains(searchTerm)) ||
                (m.Filial != null && m.Filial.Nome.Contains(searchTerm)));
        }

        protected override IQueryable<MotosEntity> ApplySort(IQueryable<MotosEntity> query, string sortBy, string sortDirection)
        {
            var isDescending = sortDirection.ToLower() == "desc";

            return sortBy.ToLower() switch
            {
                "marca" => isDescending ? query.OrderByDescending(m => m.Marca) : query.OrderBy(m => m.Marca),
                "modelo" => isDescending ? query.OrderByDescending(m => m.Modelo) : query.OrderBy(m => m.Modelo),
                "ano" => isDescending ? query.OrderByDescending(m => m.Ano) : query.OrderBy(m => m.Ano),
                "placa" => isDescending ? query.OrderByDescending(m => m.Placa) : query.OrderBy(m => m.Placa),
                "cor" => isDescending ? query.OrderByDescending(m => m.Cor) : query.OrderBy(m => m.Cor),
                "quilometragem" => isDescending ? query.OrderByDescending(m => m.Quilometragem) : query.OrderBy(m => m.Quilometragem),
                "filial" => isDescending ? query.OrderByDescending(m => m.Filial!.Nome) : query.OrderBy(m => m.Filial!.Nome),
                _ => query.OrderBy(m => m.IdMoto)
            };
        }
    }
}