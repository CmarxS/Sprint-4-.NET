using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MottoMap.Models
{
    [Table("NET_C3_Funcionario")]
    public class FuncionarioEntity
    {
        [Key]
        [Column("IdFuncionario")]
        public int IdFuncionario { get; set; }
        
        [Required(ErrorMessage = "Nome é obrigatório")]
        [Column("Nome")]
        [MaxLength(100, ErrorMessage = "Nome deve ter no máximo 100 caracteres")]
        public required string Nome { get; set; }
        
        [Required(ErrorMessage = "Email é obrigatório")]
        [Column("Email")]
        [MaxLength(150, ErrorMessage = "Email deve ter no máximo 150 caracteres")]
        [EmailAddress(ErrorMessage = "Email deve ter formato válido")]
        public required string Email { get; set; }
        
        [Required(ErrorMessage = "ID da Filial é obrigatório")]
        [Column("IdFilial")]
        public int IdFilial { get; set; }
        
        [Required(ErrorMessage = "Função é obrigatória")]
        [Column("Funcao")]
        [MaxLength(80, ErrorMessage = "Função deve ter no máximo 80 caracteres")]
        public required string Funcao { get; set; }
        
        // Propriedade de navegação para FilialEntity
        [ForeignKey("IdFilial")]
        public virtual FilialEntity? Filial { get; set; }
    }
}
