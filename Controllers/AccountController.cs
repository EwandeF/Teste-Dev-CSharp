using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TesteDevCSharp.Helpers;
using TesteDevCSharp.Services;

namespace TesteDevCSharp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IAccountService accountService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (SessionHelper.GetUsuarioId(_httpContextAccessor) != null)
                return RedirectToAction("Index", "Endereco");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string login, string senha)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(senha))
            {
                ViewBag.Erro = "Informe o usuário e a senha.";
                return View();
            }

            try
            {
                var usuario = await _accountService.AutenticarAsync(login, senha);

                if (usuario == null)
                {
                    ViewBag.Erro = "Usuário ou senha inválidos.";
                    return View();
                }

                SessionHelper.SetUsuario(_httpContextAccessor, usuario.Id, usuario.Nome);

                return RedirectToAction("Index", "Endereco");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao realizar login para o usuário {Login}", login);
                ViewBag.Erro = "Erro ao realizar login. Tente novamente.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            SessionHelper.Limpar(_httpContextAccessor);
            return RedirectToAction("Login");
        }
    }
}