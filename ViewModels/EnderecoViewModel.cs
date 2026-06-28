using System.ComponentModel.DataAnnotations;

namespace TesteDevCSharp.ViewModels
{
    public class EnderecoViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o CEP.")]
        [StringLength(9)]
        public string Cep { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o logradouro.")]
        [StringLength(200)]
        public string Logradouro { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Complemento { get; set; }

        [Required(ErrorMessage = "Informe o bairro.")]
        [StringLength(100)]
        public string Bairro { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a cidade.")]
        [StringLength(100)]
        public string Cidade { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe a UF.")]
        [StringLength(2)]
        public string Uf { get; set; } = string.Empty;

        [Required(ErrorMessage = "Informe o número.")]
        [StringLength(20)]
        public string Numero { get; set; } = string.Empty;
    }
}