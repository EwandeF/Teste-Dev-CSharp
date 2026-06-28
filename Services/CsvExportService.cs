using System.Text;
using TesteDevCSharp.Models;

namespace TesteDevCSharp.Services
{
    public class CsvExportService : ICsvExportService
    {
        public byte[] GerarCsv(List<Endereco> enderecos)
        {
            var sb = new StringBuilder();
            sb.AppendLine("CEP,Logradouro,Complemento,Bairro,Cidade,UF,Numero");

            foreach (var e in enderecos)
            {
                sb.AppendLine($"{e.Cep},{e.Logradouro},{e.Complemento},{e.Bairro},{e.Cidade},{e.Uf},{e.Numero}");
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}