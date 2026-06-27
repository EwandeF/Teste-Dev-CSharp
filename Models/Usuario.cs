using System.ComponentModel.DataAnnotations;

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
        public string UsuarioLogin { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a senha.")]
        [StringLength(100)]
        public string Senha { get; set; } = string.Empty;
    }
}