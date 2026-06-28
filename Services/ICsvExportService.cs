using TesteDevCSharp.Models;

namespace TesteDevCSharp.Services
{
    public interface ICsvExportService
    {
        byte[] GerarCsv(List<Endereco> enderecos);
    }
}