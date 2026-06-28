using TesteDevCSharp.Models;

namespace TesteDevCSharp.Services
{
    public interface IEnderecoService
    {
        Task<List<Endereco>> ListarPorUsuarioAsync(int usuarioId);
        Task<Endereco?> BuscarPorIdAsync(int id, int usuarioId);
        Task AdicionarAsync(Endereco endereco);
        Task AtualizarAsync(Endereco endereco);
        Task ExcluirAsync(int id, int usuarioId);
    }
}