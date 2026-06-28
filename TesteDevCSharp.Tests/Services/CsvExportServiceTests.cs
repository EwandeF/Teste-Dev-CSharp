using TesteDevCSharp.Models;
using TesteDevCSharp.Services;
using Xunit;

namespace TesteDevCSharp.Tests.Services
{
    public class CsvExportServiceTests
    {
        private readonly CsvExportService _service = new();

        [Fact]
        public void GerarCsv_DeveRetornarBytesComCabecalho()
        {
            var enderecos = new List<Endereco>();
            var resultado = _service.GerarCsv(enderecos);
            var texto = System.Text.Encoding.UTF8.GetString(resultado);
            Assert.Contains("CEP", texto);
            Assert.Contains("Logradouro", texto);
        }

        [Fact]
        public void GerarCsv_DeveFormatarCepComTracao()
        {
            var enderecos = new List<Endereco>
            {
                new() {
                    Cep = "01310100",
                    Logradouro = "Avenida Paulista",
                    Numero = "1000",
                    Bairro = "Bela Vista",
                    Cidade = "São Paulo",
                    Uf = "SP",
                    UsuarioId = 1
                }
            };

            var resultado = _service.GerarCsv(enderecos);
            var texto = System.Text.Encoding.UTF8.GetString(resultado);
            Assert.Contains("01310-100", texto);
        }

        [Fact]
        public void GerarCsv_DeveEscaparAspasNosValores()
        {
            var enderecos = new List<Endereco>
            {
                new() {
                    Cep = "01310100",
                    Logradouro = "Rua \"Teste\"",
                    Numero = "1",
                    Bairro = "Bairro",
                    Cidade = "Cidade",
                    Uf = "SP",
                    UsuarioId = 1
                }
            };

            var resultado = _service.GerarCsv(enderecos);
            var texto = System.Text.Encoding.UTF8.GetString(resultado);
            Assert.Contains("\"\"", texto);
        }

        [Fact]
        public void GerarCsv_ComComplementoNulo_NaoDeveQuebrар()
        {
            var enderecos = new List<Endereco>
            {
                new() {
                    Cep = "01310100",
                    Logradouro = "Avenida Paulista",
                    Numero = "1000",
                    Complemento = null,
                    Bairro = "Bela Vista",
                    Cidade = "São Paulo",
                    Uf = "SP",
                    UsuarioId = 1
                }
            };

            var resultado = _service.GerarCsv(enderecos);
            Assert.NotNull(resultado);
            Assert.True(resultado.Length > 0);
        }
    }
}