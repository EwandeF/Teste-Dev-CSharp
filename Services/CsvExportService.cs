using System.Text;
using TesteDevCSharp.Models;

namespace TesteDevCSharp.Services
{
    public class CsvExportService : ICsvExportService
    {
        public byte[] GerarCsv(List<Endereco> enderecos)
        {
            var sb = new StringBuilder();

            // Cabeçalho
            sb.AppendLine("\"CEP\",\"Logradouro\",\"Número\",\"Complemento\",\"Bairro\",\"Cidade\",\"UF\"");

            foreach (var e in enderecos)
            {
                sb.AppendLine(
                    $"\"{FormatarCep(e.Cep)}\"," +
                    $"\"{Escapar(e.Logradouro)}\"," +
                    $"\"{Escapar(e.Numero)}\"," +
                    $"\"{Escapar(e.Complemento)}\"," +
                    $"\"{Escapar(e.Bairro)}\"," +
                    $"\"{Escapar(e.Cidade)}\"," +
                    $"\"{Escapar(e.Uf)}\""
                );
            }

            // BOM UTF-8 para o Excel reconhecer acentos corretamente
            var bom = Encoding.UTF8.GetPreamble();
            var conteudo = Encoding.UTF8.GetBytes(sb.ToString());

            return bom.Concat(conteudo).ToArray();
        }

        // Formata CEP para 00000-000
        private static string FormatarCep(string? cep)
        {
            if (string.IsNullOrEmpty(cep)) return string.Empty;

            var digits = new string(cep.Where(char.IsDigit).ToArray());

            if (digits.Length == 8)
                return $"{digits[..5]}-{digits[5..]}";

            return cep;
        }

        // Escapa aspas duplas dentro dos valores
        private static string Escapar(string? valor)
        {
            if (string.IsNullOrEmpty(valor)) return string.Empty;
            return valor.Replace("\"", "\"\"");
        }
    }
}