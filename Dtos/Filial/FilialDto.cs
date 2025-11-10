using System.ComponentModel.DataAnnotations;

namespace MottoMap.DTOs.Filial
{
    /// <summary>
    /// DTO para criação de uma nova filial
    /// </summary>
    public class CreateFilialDto
    {
        /// <summary>
        /// Nome da filial
        /// </summary>
        [Required(ErrorMessage = "Nome da filial é obrigatório")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public required string Nome { get; set; }

        /// <summary>
        /// Endereço da filial
        /// </summary>
        [Required(ErrorMessage = "Endereço é obrigatório")]
        [MaxLength(200, ErrorMessage = "Endereço deve ter no máximo 200 caracteres")]
        public required string Endereco { get; set; }

        /// <summary>
        /// Cidade da filial
        /// </summary>
        [Required(ErrorMessage = "Cidade é obrigatória")]
        [MaxLength(80, ErrorMessage = "Cidade deve ter no máximo 80 caracteres")]
        public required string Cidade { get; set; }

        /// <summary>
        /// Estado da filial (sigla)
        /// </summary>
        [Required(ErrorMessage = "Estado é obrigatório")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Estado deve ter exatamente 2 caracteres")]
        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "Estado deve conter apenas letras maiúsculas")]
        public required string Estado { get; set; }

        /// <summary>
        /// CEP da filial (opcional)
        /// </summary>
        [MaxLength(10, ErrorMessage = "CEP deve ter no máximo 10 caracteres")]
        [RegularExpression(@"^\d{5}-?\d{3}$", ErrorMessage = "CEP deve estar no formato 00000-000 ou 00000000")]
        public string? CEP { get; set; }
    }

    /// <summary>
    /// DTO para atualização de uma filial existente
    /// </summary>
    public class UpdateFilialDto
    {
        /// <summary>
        /// Nome da filial
        /// </summary>
        [Required(ErrorMessage = "Nome da filial é obrigatório")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public required string Nome { get; set; }

        /// <summary>
        /// Endereço da filial
        /// </summary>
        [Required(ErrorMessage = "Endereço é obrigatório")]
        [MaxLength(200, ErrorMessage = "Endereço deve ter no máximo 200 caracteres")]
        public required string Endereco { get; set; }

        /// <summary>
        /// Cidade da filial
        /// </summary>
        [Required(ErrorMessage = "Cidade é obrigatória")]
        [MaxLength(80, ErrorMessage = "Cidade deve ter no máximo 80 caracteres")]
        public required string Cidade { get; set; }

        /// <summary>
        /// Estado da filial (sigla)
        /// </summary>
        [Required(ErrorMessage = "Estado é obrigatório")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "Estado deve ter exatamente 2 caracteres")]
        [RegularExpression(@"^[A-Z]{2}$", ErrorMessage = "Estado deve conter apenas letras maiúsculas")]
        public required string Estado { get; set; }

        /// <summary>
        /// CEP da filial (opcional)
        /// </summary>
        [MaxLength(10, ErrorMessage = "CEP deve ter no máximo 10 caracteres")]
        [RegularExpression(@"^\d{5}-?\d{3}$", ErrorMessage = "CEP deve estar no formato 00000-000 ou 00000000")]
        public string? CEP { get; set; }
    }

    /// <summary>
    /// DTO para resposta de filial (dados básicos)
    /// </summary>
    public class FilialResponseDto
    {
        /// <summary>
        /// ID único da filial
        /// </summary>
        public int IdFilial { get; set; }

        /// <summary>
        /// Nome da filial
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Endereço da filial
        /// </summary>
        public string Endereco { get; set; } = string.Empty;

        /// <summary>
        /// Cidade da filial
        /// </summary>
        public string Cidade { get; set; } = string.Empty;

        /// <summary>
        /// Estado da filial
        /// </summary>
        public string Estado { get; set; } = string.Empty;

        /// <summary>
        /// CEP da filial
        /// </summary>
        public string? CEP { get; set; }

        /// <summary>
        /// Links HATEOAS para operações relacionadas
        /// </summary>
        public Dictionary<string, string> Links { get; set; } = new Dictionary<string, string>();
    }

    /// <summary>
    /// DTO para resposta detalhada da filial com relacionamentos
    /// </summary>
    public class FilialDetailResponseDto : FilialResponseDto
    {
        /// <summary>
        /// Funcionários da filial
        /// </summary>
        public List<FuncionarioSummaryDto> Funcionarios { get; set; } = new List<FuncionarioSummaryDto>();

        /// <summary>
        /// Motos da filial
        /// </summary>
        public List<MotoSummaryDto> Motos { get; set; } = new List<MotoSummaryDto>();

        /// <summary>
        /// Estatísticas da filial
        /// </summary>
        public FilialStatsDto Stats { get; set; } = new FilialStatsDto();
    }

    /// <summary>
    /// DTO resumido do funcionário para ser usado em FilialDetailResponseDto
    /// </summary>
    public class FuncionarioSummaryDto
    {
        /// <summary>
        /// ID do funcionário
        /// </summary>
        public int IdFuncionario { get; set; }

        /// <summary>
        /// Nome do funcionário
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Email do funcionário
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Função do funcionário
        /// </summary>
        public string Funcao { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO resumido da moto para ser usado em FilialDetailResponseDto
    /// </summary>
    public class MotoSummaryDto
    {
        /// <summary>
        /// ID da moto
        /// </summary>
        public int IdMoto { get; set; }

        /// <summary>
        /// Marca da moto
        /// </summary>
        public string Marca { get; set; } = string.Empty;

        /// <summary>
        /// Modelo da moto
        /// </summary>
        public string Modelo { get; set; } = string.Empty;

        /// <summary>
        /// Placa da moto
        /// </summary>
        public string Placa { get; set; } = string.Empty;

        /// <summary>
        /// Ano da moto
        /// </summary>
        public int Ano { get; set; }
    }

    /// <summary>
    /// DTO para estatísticas da filial
    /// </summary>
    public class FilialStatsDto
    {
        /// <summary>
        /// Total de funcionários na filial
        /// </summary>
        public int TotalFuncionarios { get; set; }

        /// <summary>
        /// Total de motos na filial
        /// </summary>
        public int TotalMotos { get; set; }
    }
}