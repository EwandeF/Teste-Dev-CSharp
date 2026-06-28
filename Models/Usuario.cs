using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TesteDevCSharp.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o nome.")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o usuário.")]
        [StringLength(50)]
        [Column("Usuario")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a senha.")]
        [StringLength(255)] // alinhado com VARCHAR(255) do SQL
        public string Senha { get; set; } = string.Empty;
    }
}
