using Microsoft.AspNetCore.Mvc;
using System.Text;
using TesteDevCSharp.Data;
using TesteDevCSharp.Models;

namespace TesteDevCSharp.Controllers
{
    public class EnderecoController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public EnderecoController(AppDbContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // Verifica se o usuário está logado
        private int? GetUsuarioLogado()
        {
            var id = HttpContext.Session.GetString("UsuarioId");
            if (id == null) return null;
            return int.Parse(id);
        }

        // GET: /Endereco
        public IActionResult Index()
        {
            var usuarioId = GetUsuarioLogado();
            if (usuarioId == null) return RedirectToAction("Login", "Account");

            var enderecos = _context.Enderecos
                .Where(e => e.UsuarioId == usuarioId)
                .ToList();

            ViewBag.UsuarioNome = HttpContext.Session.GetString("UsuarioNome");
            return View(enderecos);
        }

        // GET: /Endereco/Criar
        public IActionResult Criar()
        {
            if (GetUsuarioLogado() == null) return RedirectToAction("Login", "Account");
            return View();
        }

        // POST: /Endereco/Criar
        [HttpPost]
        public IActionResult Criar(Endereco endereco)
        {
            var usuarioId = GetUsuarioLogado();
            if (usuarioId == null) return RedirectToAction("Login", "Account");

            endereco.UsuarioId = usuarioId.Value;
            _context.Enderecos.Add(endereco);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: /Endereco/Editar/5
        public IActionResult Editar(int id)
        {
            var usuarioId = GetUsuarioLogado();
            if (usuarioId == null) return RedirectToAction("Login", "Account");

            var endereco = _context.Enderecos
                .FirstOrDefault(e => e.Id == id && e.UsuarioId == usuarioId);

            if (endereco == null) return NotFound();
            return View(endereco);
        }

        // POST: /Endereco/Editar/5
        [HttpPost]
        public IActionResult Editar(Endereco endereco)
        {
            var usuarioId = GetUsuarioLogado();
            if (usuarioId == null) return RedirectToAction("Login", "Account");

            endereco.UsuarioId = usuarioId.Value;
            _context.Enderecos.Update(endereco);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        // POST: /Endereco/Excluir/5
        [HttpPost]
        public IActionResult Excluir(int id)
        {
            var usuarioId = GetUsuarioLogado();
            if (usuarioId == null) return RedirectToAction("Login", "Account");

            var endereco = _context.Enderecos
                .FirstOrDefault(e => e.Id == id && e.UsuarioId == usuarioId);

            if (endereco != null)
            {
                _context.Enderecos.Remove(endereco);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // GET: /Endereco/BuscarCep?cep=01310100
        public async Task<IActionResult> BuscarCep(string cep)
        {
            if (string.IsNullOrEmpty(cep))
                return Json(new { erro = "CEP inválido" });

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

            if (!response.IsSuccessStatusCode)
                return Json(new { erro = "CEP não encontrado" });

            var json = await response.Content.ReadAsStringAsync();
            return Content(json, "application/json");
        }

        // GET: /Endereco/ExportarCsv
        public IActionResult ExportarCsv()
        {
            var usuarioId = GetUsuarioLogado();
            if (usuarioId == null) return RedirectToAction("Login", "Account");

            var enderecos = _context.Enderecos
                .Where(e => e.UsuarioId == usuarioId)
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("CEP,Logradouro,Complemento,Bairro,Cidade,UF,Numero");

            foreach (var e in enderecos)
            {
                sb.AppendLine($"{e.Cep},{e.Logradouro},{e.Complemento},{e.Bairro},{e.Cidade},{e.Uf},{e.Numero}");
            }

            var bytes = Encoding.UTF8.GetBytes(sb.ToString());
            return File(bytes, "text/csv", "enderecos.csv");
        }
    }
}