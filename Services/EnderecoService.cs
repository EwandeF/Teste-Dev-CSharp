using Microsoft.EntityFrameworkCore;
using TesteDevCSharp.Data;
using TesteDevCSharp.Models;

namespace TesteDevCSharp.Services
{
    public class EnderecoService : IEnderecoService
    {
        private readonly AppDbContext _context;

        public EnderecoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Endereco>> ListarPorUsuarioAsync(int usuarioId)
        {
            return await _context.Enderecos
                .Where(e => e.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<Endereco?> BuscarPorIdAsync(int id, int usuarioId)
        {
            return await _context.Enderecos
                .FirstOrDefaultAsync(e => e.Id == id && e.UsuarioId == usuarioId);
        }

        public async Task AdicionarAsync(Endereco endereco)
        {
            PadronizarEndereco(endereco);
            _context.Enderecos.Add(endereco);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizarAsync(Endereco endereco)
        {
            PadronizarEndereco(endereco);
            _context.Enderecos.Update(endereco);
            await _context.SaveChangesAsync();
        }

        public async Task ExcluirAsync(int id, int usuarioId)
        {
            var endereco = await BuscarPorIdAsync(id, usuarioId);
            if (endereco != null)
            {
                _context.Enderecos.Remove(endereco);
                await _context.SaveChangesAsync();
            }
        }

        private static void PadronizarEndereco(Endereco endereco)
        {
            endereco.Cep = LimparCep(endereco.Cep);
            endereco.Logradouro = CapitalizarTexto(endereco.Logradouro);
            endereco.Bairro = CapitalizarTexto(endereco.Bairro);
            endereco.Cidade = CapitalizarTexto(endereco.Cidade);
            endereco.Uf = (endereco.Uf ?? string.Empty).ToUpper().Trim();
            endereco.Complemento = string.IsNullOrWhiteSpace(endereco.Complemento)
                ? null
                : CapitalizarTexto(endereco.Complemento);
        }

        // Remove tudo que não é número antes de salvar
        private static string LimparCep(string cep)
        {
            return new string(cep.Where(char.IsDigit).ToArray());
        }

        private static string CapitalizarTexto(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return string.Empty;
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo
                .ToTitleCase(texto.Trim().ToLower());
        }
    }
}

