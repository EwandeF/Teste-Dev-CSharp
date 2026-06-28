using Microsoft.AspNetCore.Mvc;
using TesteDevCSharp.Services;

namespace TesteDevCSharp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UsuarioId") != null)
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

                HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
                HttpContext.Session.SetString("UsuarioNome", usuario.Nome);

                return RedirectToAction("Index", "Endereco");
            }
            catch
            {
                ViewBag.Erro = "Erro ao realizar login. Tente novamente.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}