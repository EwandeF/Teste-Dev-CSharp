using TesteDevCSharp.Models;

namespace TesteDevCSharp.Services
{
    public interface IAccountService
    {
        Task<Usuario?> AutenticarAsync(string login, string senha);
    }
}