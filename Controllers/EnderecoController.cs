using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TesteDevCSharp.Helpers;
using TesteDevCSharp.Models;
using TesteDevCSharp.Services;
using TesteDevCSharp.ViewModels;

namespace TesteDevCSharp.Controllers
{
    public class EnderecoController : Controller
    {
        private readonly IEnderecoService _enderecoService;
        private readonly IViaCepService _viaCepService;
        private readonly ICsvExportService _csvExportService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<EnderecoController> _logger;

        public EnderecoController(
            IEnderecoService enderecoService,
            IViaCepService viaCepService,
            ICsvExportService csvExportService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<EnderecoController> logger)
        {
            _enderecoService = enderecoService;
            _viaCepService = viaCepService;
            _csvExportService = csvExportService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        private int? GetUsuarioLogado() => SessionHelper.GetUsuarioId(_httpContextAccessor);

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao listar endereços do usuário {UsuarioId}", usuarioId);
                ViewBag.Erro = "Erro ao carregar endereços.";
                return View(new List<Endereco>());
            }
        }

        public IActionResult Criar()
        {
            if (GetUsuarioLogado() == null) return RedirectToAction("Login", "Account");
            return View(new EnderecoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(EnderecoViewModel vm)
        {
            var usuarioId = GetUsuarioLogado();
            if (usuarioId == null) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var endereco = new Endereco
                {
                    Cep = vm.Cep,
                    Logradouro = vm.Logradouro,
                    Complemento = vm.Complemento,
                    Bairro = vm.Bairro,
                    Cidade = vm.Cidade,
                    Uf = vm.Uf,
                    Numero = vm.Numero,
                    UsuarioId = usuarioId.Value
                };

                await _enderecoService.AdicionarAsync(endereco);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao adicionar endereço para o usuário {UsuarioId}", usuarioId);
                ViewBag.Erro = "Erro ao salvar endereço.";
                return View(vm);
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

                var vm = new EnderecoViewModel
                {
                    Id = endereco.Id,
                    Cep = endereco.Cep,
                    Logradouro = endereco.Logradouro,
                    Complemento = endereco.Complemento,
                    Bairro = endereco.Bairro,
                    Cidade = endereco.Cidade,
                    Uf = endereco.Uf,
                    Numero = endereco.Numero
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar endereço {EnderecoId} para edição", id);
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(EnderecoViewModel vm)
        {
            var usuarioId = GetUsuarioLogado();
            if (usuarioId == null) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                var endereco = new Endereco
                {
                    Id = vm.Id,
                    Cep = vm.Cep,
                    Logradouro = vm.Logradouro,
                    Complemento = vm.Complemento,
                    Bairro = vm.Bairro,
                    Cidade = vm.Cidade,
                    Uf = vm.Uf,
                    Numero = vm.Numero,
                    UsuarioId = usuarioId.Value
                };

                await _enderecoService.AtualizarAsync(endereco);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar endereço {EnderecoId}", vm.Id);
                ViewBag.Erro = "Erro ao atualizar endereço.";
                return View(vm);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir endereço {EnderecoId}", id);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> BuscarCep(string cep)
        {
            if (string.IsNullOrEmpty(cep))
                return Json(new { erro = "CEP inválido" });

            var cepLimpo = new string(cep.Where(char.IsDigit).ToArray());

            if (cepLimpo.Length != 8)
                return Json(new { erro = "CEP deve conter 8 dígitos" });

            try
            {
                var json = await _viaCepService.BuscarCepAsync(cepLimpo);
                if (json == null) return Json(new { erro = "CEP não encontrado" });
                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar CEP {Cep}", cepLimpo);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao exportar CSV para o usuário {UsuarioId}", usuarioId);
                return RedirectToAction("Index");
            }
        }
    }
}