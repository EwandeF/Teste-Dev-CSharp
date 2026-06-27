using Microsoft.AspNetCore.Mvc;
using TesteDevCSharp.Data;

namespace TesteDevCSharp.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("UsuarioId") != null)
                return RedirectToAction("Index", "Endereco");

            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public IActionResult Login(string login, string senha)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(senha))
            {
                ViewBag.Erro = "Informe o usuário e a senha.";
                return View();
            }

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Login == login && u.Senha == senha);

            if (usuario == null)
            {
                ViewBag.Erro = "Usuário ou senha inválidos.";
                return View();
            }

            HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
            HttpContext.Session.SetString("UsuarioNome", usuario.Nome);

            return RedirectToAction("Index", "Endereco");
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}