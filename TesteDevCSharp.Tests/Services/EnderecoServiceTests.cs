using Microsoft.EntityFrameworkCore;
using TesteDevCSharp.Data;
using TesteDevCSharp.Models;
using TesteDevCSharp.Services;
using Xunit;

namespace TesteDevCSharp.Tests.Services
{
    public class EnderecoServiceTests
    {
        private AppDbContext CriarContexto()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AdicionarAsync_DeveAdicionarEndereco()
        {
            var context = CriarContexto();
            var service = new EnderecoService(context);

            var endereco = new Endereco
            {
                Cep = "01310100",
                Logradouro = "Avenida Paulista",
                Numero = "1000",
                Bairro = "Bela Vista",
                Cidade = "São Paulo",
                Uf = "SP",
                UsuarioId = 1
            };

            await service.AdicionarAsync(endereco);

            var resultado = await context.Enderecos.FirstOrDefaultAsync();
            Assert.NotNull(resultado);
            Assert.Equal("Avenida Paulista", resultado.Logradouro);
        }

        [Fact]
        public async Task AdicionarAsync_DeveLimparTracoDoCep()
        {
            var context = CriarContexto();
            var service = new EnderecoService(context);

            var endereco = new Endereco
            {
                Cep = "01310-100",
                Logradouro = "Avenida Paulista",
                Numero = "1000",
                Bairro = "Bela Vista",
                Cidade = "São Paulo",
                Uf = "SP",
                UsuarioId = 1
            };

            await service.AdicionarAsync(endereco);

            var resultado = await context.Enderecos.FirstOrDefaultAsync();
            Assert.Equal("01310100", resultado!.Cep);
        }

        [Fact]
        public async Task ListarPorUsuarioAsync_DeveRetornarSoEnderecoDoUsuario()
        {
            var context = CriarContexto();
            var service = new EnderecoService(context);

            context.Enderecos.AddRange(
                new Endereco { Cep = "01310100", Logradouro = "Rua A", Numero = "1", Bairro = "B", Cidade = "C", Uf = "SP", UsuarioId = 1 },
                new Endereco { Cep = "01310200", Logradouro = "Rua B", Numero = "2", Bairro = "B", Cidade = "C", Uf = "SP", UsuarioId = 2 }
            );
            await context.SaveChangesAsync();

            var resultado = await service.ListarPorUsuarioAsync(1);

            Assert.Single(resultado);
            Assert.Equal("Rua A", resultado[0].Logradouro);
        }

        [Fact]
        public async Task ExcluirAsync_DeveRemoverEndereco()
        {
            var context = CriarContexto();
            var service = new EnderecoService(context);

            var endereco = new Endereco
            {
                Cep = "01310100",
                Logradouro = "Rua A",
                Numero = "1",
                Bairro = "B",
                Cidade = "C",
                Uf = "SP",
                UsuarioId = 1
            };

            context.Enderecos.Add(endereco);
            await context.SaveChangesAsync();

            await service.ExcluirAsync(endereco.Id, 1);

            var resultado = await context.Enderecos.ToListAsync();
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task BuscarPorIdAsync_NaoDeveRetornarEnderecoDeOutroUsuario()
        {
            var context = CriarContexto();
            var service = new EnderecoService(context);

            var endereco = new Endereco
            {
                Cep = "01310100",
                Logradouro = "Rua A",
                Numero = "1",
                Bairro = "B",
                Cidade = "C",
                Uf = "SP",
                UsuarioId = 1
            };

            context.Enderecos.Add(endereco);
            await context.SaveChangesAsync();

            var resultado = await service.BuscarPorIdAsync(endereco.Id, 2);
            Assert.Null(resultado);
        }
    }
}