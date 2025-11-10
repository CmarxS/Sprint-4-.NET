using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottoMap.Models
{
    [Table("NET_C3_Filial")]
    public class FilialEntity
    {
        [Key]
        [Column("IdFilial")]
        public int IdFilial { get; set; }
        
        [Required(ErrorMessage = "Nome da filial é obrigatório")]
        [Column("Nome")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public required string Nome { get; set; }
        
        [Required(ErrorMessage = "Endereço é obrigatório")]
        [Column("Endereco")]
        [MaxLength(200, ErrorMessage = "Endereço deve ter no máximo 200 caracteres")]
        public required string Endereco { get; set; }
        
        [Required(ErrorMessage = "Cidade é obrigatória")]
        [Column("Cidade")]
        [MaxLength(80, ErrorMessage = "Cidade deve ter no máximo 80 caracteres")]
        public required string Cidade { get; set; }
        
        [Required(ErrorMessage = "Estado é obrigatório")]
        [Column("Estado")]
        [MaxLength(2, ErrorMessage = "Estado deve ter exatamente 2 caracteres")]
        [MinLength(2, ErrorMessage = "Estado deve ter exatamente 2 caracteres")]
        public required string Estado { get; set; }
        
        [Column("CEP")]
        [MaxLength(10, ErrorMessage = "CEP deve ter no máximo 10 caracteres")]
        public string? CEP { get; set; }
        
        // Propriedade de navegação para Funcionários
        public virtual ICollection<FuncionarioEntity> Funcionarios { get; set; } = new List<FuncionarioEntity>();
        
        // Propriedade de navegação para Motos
        public virtual ICollection<MotosEntity> Motos { get; set; } = new List<MotosEntity>();
    }
}
