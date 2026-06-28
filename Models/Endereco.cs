using System.ComponentModel.DataAnnotations;

namespace TesteDevCSharp.Models
{
    public class Endereco
    {
        public int Id { get; set; }

        [Required]
        [StringLength(9)] // alinhado com VARCHAR(9) do SQL (suporta traço)
        public string Cep { get; set; } = string.Empty;

        [Required]
        public string Logradouro { get; set; } = string.Empty;

        public string? Complemento { get; set; }

        [Required]
        public string Bairro { get; set; } = string.Empty;

        [Required]
        public string Cidade { get; set; } = string.Empty;

        [Required]
        [StringLength(2)]
        public string Uf { get; set; } = string.Empty;

        [Required]
        public string Numero { get; set; } = string.Empty;

        public int UsuarioId { get; set; }

        public Usuario? Usuario { get; set; }
    }
}
