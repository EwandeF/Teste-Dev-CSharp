using Microsoft.EntityFrameworkCore;
using TesteDevCSharp.Data;
using TesteDevCSharp.Models;

namespace TesteDevCSharp.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;

        public AccountService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> AutenticarAsync(string login, string senha)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Login == login);

                if (usuario == null) return null;

                // Verifica a senha com BCrypt
                var senhaValida = BCrypt.Net.BCrypt.Verify(senha, usuario.Senha);
                return senhaValida ? usuario : null;
            }
            catch
            {
                return null;
            }
        }
    }
}