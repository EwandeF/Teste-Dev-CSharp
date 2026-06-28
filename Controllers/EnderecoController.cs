using Microsoft.AspNetCore.Mvc;
using TesteDevCSharp.Models;
using TesteDevCSharp.Services;

namespace TesteDevCSharp.Controllers
{
    public class EnderecoController : Controller
    {
        private readonly IEnderecoService _enderecoService;
        private readonly IViaCepService _viaCepService;
        private readonly ICsvExportService _csvExportService;

        public EnderecoController(
            IEnderecoService enderecoService,
            IViaCepService viaCepService,
            ICsvExportService csvExportService)
        {
            _enderecoService = enderecoService;
            _viaCepService = viaCepService;
            _csvExportService = csvExportService;
        }

        private int? GetUsuarioLogado() =>
            int.TryParse(HttpContext.Session.GetString("UsuarioId"), out var id) ? id : null;

        public async Task<IActionResult> Index()
        {
            var usuarioId = GetUsuarioLogado();
            if (usuarioId == null) return RedirectToAction("Login", "Account");

            try
            {
                var enderecos = await _enderecoService.ListarPorUsuarioAsync(usuarioId.Value);
                ViewBag.UsuarioNome = HttpContext.Session.GetString("UsuarioNome");
                return View(enderecos);
            }
            catch
            {
                ViewBag.Erro = "Erro ao carregar endereços.";
                return View(new List<Endereco>());
            }
        }

        public IActionResult Criar()
        {
            if (GetUsuarioLogado() == null) return RedirectToAction("Login", "Account");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(Endereco endereco)
        {
            var usuarioId = GetUsuarioLogado();
            if (usuarioId == null) return RedirectToAction("Login", "Account");

            try
            {
                endereco.UsuarioId = usuarioId.Value;
                await _enderecoService.AdicionarAsync(endereco);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Erro = "Erro ao salvar endereço.";
                return View(endereco);
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var usuarioId = GetUsuarioLogado();
            if (usuarioId == null) return RedirectToAction("Login", "Account");

            try
            {
                var endereco = await _enderecoService.BuscarPorIdAsync(id, usuarioId.Value);
                if (endereco == null) return NotFound();
                return View(endereco);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Endereco endereco)
        {
            var usuarioId = GetUsuarioLogado();
            if (usuarioId == null) return RedirectToAction("Login", "Account");

            try
            {
                endereco.UsuarioId = usuarioId.Value;
                await _enderecoService.AtualizarAsync(endereco);
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Erro = "Erro ao atualizar endereço.";
                return View(endereco);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Excluir(int id)
        {
            var usuarioId = GetUsuarioLogado();
            if (usuarioId == null) return RedirectToAction("Login", "Account");

            try
            {
                await _enderecoService.ExcluirAsync(id, usuarioId.Value);
            }
            catch
            {
                // log futuramente
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> BuscarCep(string cep)
        {
            if (string.IsNullOrEmpty(cep) || cep.Length != 8)
                return Json(new { erro = "CEP inválido" });

            try
            {
                var json = await _viaCepService.BuscarCepAsync(cep);
                if (json == null) return Json(new { erro = "CEP não encontrado" });
                return Content(json, "application/json");
            }
            catch
            {
                return Json(new { erro = "Erro ao buscar CEP" });
            }
        }

        public async Task<IActionResult> ExportarCsv()
        {
            var usuarioId = GetUsuarioLogado();
            if (usuarioId == null) return RedirectToAction("Login", "Account");

            try
            {
                var enderecos = await _enderecoService.ListarPorUsuarioAsync(usuarioId.Value);
                var bytes = _csvExportService.GerarCsv(enderecos);
                return File(bytes, "text/csv", "enderecos.csv");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }
    }
}