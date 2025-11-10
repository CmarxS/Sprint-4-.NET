namespace MottoMap.DTOs.Common
{
    /// <summary>
    /// DTO para resposta paginada genérica
    /// </summary>
    /// <typeparam name="T">Tipo dos dados paginados</typeparam>
    public class PagedResponseDto<T>
    {
        /// <summary>
        /// Dados da página atual
        /// </summary>
        public IEnumerable<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// Informações de paginação
        /// </summary>
        public PaginationInfoDto Pagination { get; set; } = new PaginationInfoDto();

        /// <summary>
        /// Links HATEOAS para navegação
        /// </summary>
        public Dictionary<string, string> Links { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// Construtor para criar resposta paginada
        /// </summary>
        /// <param name="data">Dados da página</param>
        /// <param name="pageNumber">Número da página atual</param>
        /// <param name="pageSize">Tamanho da página</param>
        /// <param name="totalItems">Total de itens</param>
        public PagedResponseDto(IEnumerable<T> data, int pageNumber, int pageSize, int totalItems)
        {
            Data = data;
            Pagination = new PaginationInfoDto
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalItems > 0 ? (int)Math.Ceiling((double)totalItems / pageSize) : 0,
                HasPreviousPage = pageNumber > 1,
                HasNextPage = pageNumber < (totalItems > 0 ? (int)Math.Ceiling((double)totalItems / pageSize) : 0)
            };
        }

        /// <summary>
        /// Construtor padrão
        /// </summary>
        public PagedResponseDto()
        {
        }
    }

    /// <summary>
    /// DTO para informações de paginação
    /// </summary>
    public class PaginationInfoDto
    {
        /// <summary>
        /// Número da página atual
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Tamanho da página
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Total de itens
        /// </summary>
        public int TotalItems { get; set; }

        /// <summary>
        /// Total de páginas
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Indica se existe página anterior
        /// </summary>
        public bool HasPreviousPage { get; set; }

        /// <summary>
        /// Indica se existe próxima página
        /// </summary>
        public bool HasNextPage { get; set; }
    }

    /// <summary>
    /// DTO para resposta de erro padronizada
    /// </summary>
    public class ErrorResponseDto
    {
        /// <summary>
        /// Código do erro
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Mensagem do erro
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Detalhes adicionais do erro (opcional)
        /// </summary>
        public string? Details { get; set; }

        /// <summary>
        /// Lista de erros de validação (quando aplicável)
        /// </summary>
        public List<ValidationErrorDto> ValidationErrors { get; set; } = new List<ValidationErrorDto>();

        /// <summary>
        /// Timestamp do erro
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// DTO para erro de validação
    /// </summary>
    public class ValidationErrorDto
    {
        /// <summary>
        /// Nome do campo com erro
        /// </summary>
        public string Field { get; set; } = string.Empty;

        /// <summary>
        /// Mensagem do erro de validação
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Valor que causou o erro (opcional)
        /// </summary>
        public object? Value { get; set; }
    }

    /// <summary>
    /// DTO para resposta de sucesso simples
    /// </summary>
    public class SuccessResponseDto
    {
        /// <summary>
        /// Mensagem de sucesso
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Dados adicionais (opcional)
        /// </summary>
        public object? Data { get; set; }

        /// <summary>
        /// Timestamp da operação
        /// </summary>
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}