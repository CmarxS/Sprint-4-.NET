using System.ComponentModel.DataAnnotations;

namespace MottoMap.DTOs.Funcionario
{
    /// <summary>
    /// DTO para criação de um novo funcionário
    /// </summary>
    public class CreateFuncionarioDto
    {
        /// <summary>
        /// Nome do funcionário
        /// </summary>
        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public required string Nome { get; set; }

        /// <summary>
        /// Email do funcionário
        /// </summary>
        [Required(ErrorMessage = "Email é obrigatório")]
        [MaxLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        public required string Email { get; set; }

        /// <summary>
        /// ID da filial onde o funcionário trabalha
        /// </summary>
        [Required(ErrorMessage = "ID da Filial é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "ID da Filial deve ser maior que 0")]
        public int IdFilial { get; set; }

        /// <summary>
        /// Função do funcionário
        /// </summary>
        [Required(ErrorMessage = "Função é obrigatória")]
        [MaxLength(80, ErrorMessage = "Função deve ter no máximo 80 caracteres")]
        public required string Funcao { get; set; }
    }

    /// <summary>
    /// DTO para atualização de um funcionário existente
    /// </summary>
    public class UpdateFuncionarioDto
    {
        /// <summary>
        /// Nome do funcionário
        /// </summary>
        [Required(ErrorMessage = "Nome é obrigatório")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public required string Nome { get; set; }

        /// <summary>
        /// Email do funcionário
        /// </summary>
        [Required(ErrorMessage = "Email é obrigatório")]
        [MaxLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        public required string Email { get; set; }

        /// <summary>
        /// ID da filial onde o funcionário trabalha
        /// </summary>
        [Required(ErrorMessage = "ID da Filial é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "ID da Filial deve ser maior que 0")]
        public int IdFilial { get; set; }

        /// <summary>
        /// Função do funcionário
        /// </summary>
        [Required(ErrorMessage = "Função é obrigatória")]
        [MaxLength(80, ErrorMessage = "Função deve ter no máximo 80 caracteres")]
        public required string Funcao { get; set; }
    }

    /// <summary>
    /// DTO para resposta de funcionário (dados completos)
    /// </summary>
    public class FuncionarioResponseDto
    {
        /// <summary>
        /// ID único do funcionário
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
        /// ID da filial onde o funcionário trabalha
        /// </summary>
        public int IdFilial { get; set; }

        /// <summary>
        /// Função do funcionário
        /// </summary>
        public string Funcao { get; set; } = string.Empty;

        /// <summary>
        /// Informações da filial (se incluída)
        /// </summary>
        public FilialSummaryDto? Filial { get; set; }

        /// <summary>
        /// Links HATEOAS para operações relacionadas
        /// </summary>
        public Dictionary<string, string> Links { get; set; } = new Dictionary<string, string>();
    }

    /// <summary>
    /// DTO resumido da filial para ser usado em FuncionarioResponseDto
    /// </summary>
    public class FilialSummaryDto
    {
        /// <summary>
        /// ID da filial
        /// </summary>
        public int IdFilial { get; set; }

        /// <summary>
        /// Nome da filial
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Cidade da filial
        /// </summary>
        public string Cidade { get; set; } = string.Empty;

        /// <summary>
        /// Estado da filial
        /// </summary>
        public string Estado { get; set; } = string.Empty;
    }
}