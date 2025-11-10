using System.ComponentModel.DataAnnotations;

namespace MottoMap.Models
{
    /// <summary>
    /// Classe genérica para representar dados paginados
    /// </summary>
    /// <typeparam name="T">Tipo dos dados a serem paginados</typeparam>
    public class DataPage<T>
    {
        /// <summary>
        /// Lista dos itens da página atual
        /// </summary>
        public IEnumerable<T> Data { get; set; } = new List<T>();
        
        /// <summary>
        /// Número da página atual (baseado em 1)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Número da página deve ser maior que 0")]
        public int PageNumber { get; set; }
        
        /// <summary>
        /// Quantidade de itens por página
        /// </summary>
        [Range(1, 100, ErrorMessage = "Tamanho da página deve estar entre 1 e 100")]
        public int PageSize { get; set; }
        
        /// <summary>
        /// Total de itens em todas as páginas
        /// </summary>
        public int TotalItems { get; set; }
        
        /// <summary>
        /// Total de páginas disponíveis
        /// </summary>
        public int TotalPages { get; set; }
        
        /// <summary>
        /// Indica se existe uma página anterior
        /// </summary>
        public bool HasPreviousPage { get; set; }
        
        /// <summary>
        /// Indica se existe uma próxima página
        /// </summary>
        public bool HasNextPage { get; set; }
        
        /// <summary>
        /// Número da primeira página
        /// </summary>
        public int FirstPage { get; set; } = 1;
        
        /// <summary>
        /// Número da última página
        /// </summary>
        public int LastPage { get; set; }
        
        /// <summary>
        /// Links HATEOAS para navegação
        /// </summary>
        public Dictionary<string, string> Links { get; set; } = new Dictionary<string, string>();
        
        /// <summary>
        /// Construtor para criar uma página de dados
        /// </summary>
        /// <param name="data">Dados da página atual</param>
        /// <param name="pageNumber">Número da página atual</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <param name="totalItems">Total de itens</param>
        public DataPage(IEnumerable<T> data, int pageNumber, int pageSize, int totalItems)
        {
            Data = data ?? new List<T>();
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = totalItems > 0 ? (int)Math.Ceiling((double)totalItems / pageSize) : 0;
            HasPreviousPage = pageNumber > 1;
            HasNextPage = pageNumber < TotalPages;
            LastPage = TotalPages;
            Links = new Dictionary<string, string>();
        }
        
        /// <summary>
        /// Construtor padrão para serialização
        /// </summary>
        public DataPage()
        {
            Data = new List<T>();
            Links = new Dictionary<string, string>();
        }
        
        /// <summary>
        /// Adiciona links HATEOAS para navegação entre páginas
        /// </summary>
        /// <param name="baseUrl">URL base do endpoint</param>
        public void AddNavigationLinks(string baseUrl)
        {
            // Link para a própria página
            Links["self"] = $"{baseUrl}?pageNumber={PageNumber}&pageSize={PageSize}";
            
            // Link para a primeira página
            if (TotalPages > 0)
            {
                Links["first"] = $"{baseUrl}?pageNumber=1&pageSize={PageSize}";
                Links["last"] = $"{baseUrl}?pageNumber={TotalPages}&pageSize={PageSize}";
            }
            
            // Link para a página anterior (se existir)
            if (HasPreviousPage)
            {
                Links["prev"] = $"{baseUrl}?pageNumber={PageNumber - 1}&pageSize={PageSize}";
            }
            
            // Link para a próxima página (se existir)
            if (HasNextPage)
            {
                Links["next"] = $"{baseUrl}?pageNumber={PageNumber + 1}&pageSize={PageSize}";
            }
        }
    }
    
    /// <summary>
    /// Classe para parâmetros de paginação nas requisições
    /// </summary>
    public class PaginationParameters
    {
        /// <summary>
        /// Número da página (padrão: 1)
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Número da página deve ser maior que 0")]
        public int PageNumber { get; set; } = 1;
        
        /// <summary>
        /// Quantidade de itens por página (padrão: 10, máximo: 100)
        /// </summary>
        [Range(1, 100, ErrorMessage = "Tamanho da página deve estar entre 1 e 100")]
        public int PageSize { get; set; } = 10;
        
        /// <summary>
        /// Campo para ordenação (opcional)
        /// </summary>
        public string? SortBy { get; set; }
        
        /// <summary>
        /// Direção da ordenação: asc ou desc (padrão: asc)
        /// </summary>
        public string SortDirection { get; set; } = "asc";
        
        /// <summary>
        /// Termo para busca/filtro (opcional)
        /// </summary>
        public string? SearchTerm { get; set; }
        
        /// <summary>
        /// Valida se a direção da ordenação é válida
        /// </summary>
        /// <returns>True se válida, False caso contrário</returns>
        public bool IsValidSortDirection()
        {
            return SortDirection.ToLower() == "asc" || SortDirection.ToLower() == "desc";
        }
    }
}