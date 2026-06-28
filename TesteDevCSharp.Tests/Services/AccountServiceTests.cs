using Microsoft.EntityFrameworkCore;
using TesteDevCSharp.Data;
using TesteDevCSharp.Models;
using TesteDevCSharp.Services;
using Xunit;

namespace TesteDevCSharp.Tests.Services
{
    public class AccountServiceTests
    {
        private AppDbContext CriarContexto()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AutenticarAsync_ComCredenciaisValidas_DeveRetornarUsuario()
        {
            var context = CriarContexto();
            var service = new AccountService(context);

            context.Usuarios.Add(new Usuario
            {
                Nome = "Admin",
                Login = "admin",
                Senha = BCrypt.Net.BCrypt.HashPassword("123456")
            });
            await context.SaveChangesAsync();

            var resultado = await service.AutenticarAsync("admin", "123456");

            Assert.NotNull(resultado);
            Assert.Equal("admin", resultado.Login);
        }

        [Fact]
        public async Task AutenticarAsync_ComSenhaErrada_DeveRetornarNull()
        {
            var context = CriarContexto();
            var service = new AccountService(context);

            context.Usuarios.Add(new Usuario
            {
                Nome = "Admin",
                Login = "admin",
                Senha = BCrypt.Net.BCrypt.HashPassword("123456")
            });
            await context.SaveChangesAsync();

            var resultado = await service.AutenticarAsync("admin", "senhaerrada");

            Assert.Null(resultado);
        }

        [Fact]
        public async Task AutenticarAsync_ComUsuarioInexistente_DeveRetornarNull()
        {
            var context = CriarContexto();
            var service = new AccountService(context);

            var resultado = await service.AutenticarAsync("naoexiste", "123456");

            Assert.Null(resultado);
        }
    }
}