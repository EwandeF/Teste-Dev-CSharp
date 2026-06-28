using Microsoft.EntityFrameworkCore;
using TesteDevCSharp.Data;
using TesteDevCSharp.Models;

namespace TesteDevCSharp.Services
{
    public class AccountService : IAccountService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<AccountService> _logger;

        public AccountService(AppDbContext context, ILogger<AccountService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Usuario?> AutenticarAsync(string login, string senha)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Login == login);

                if (usuario == null) return null;

                var senhaValida = BCrypt.Net.BCrypt.Verify(senha, usuario.Senha);
                return senhaValida ? usuario : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao autenticar usuário {Login}", login);
                return null;
            }
        }
    }
}
