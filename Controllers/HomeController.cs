using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemaVeterinario.Models;

namespace sistemaVeterinario.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SistemaVeterinarioContext _context;

        public HomeController(ILogger<HomeController> logger, SistemaVeterinarioContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /**
         * Metodo que nos permite crear nuestro LOGIN.
         */
        public IActionResult Login()
        {
            return View();
        }

        /**
         * 
         */
        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            // Validadiones.
            // Verificar que los campos no esten vacios.
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                // ViewBag -> nos permite enviar datos desde el controller a la view.
                ViewBag.Error = "Por favor, ingrese su correo y contraseña.";
                return View(new {email, password});
            }

            // Hacer una consulta en la DB para verificar la existencia del usuario.
            var user = _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Email y/o password incorrectos.";
                return View();
            }

            /**
             * Variable Session:
             * - Variable temporal que se almacena mientras el usuario este loggeado.
             * - Para utilizar estas variables se debe configurar en el Program.cs
             * - SetString() -> permite guardar data en una variable session.
             * - GetString() -> permite obtener data de una variable session.
             */
            HttpContext.Session.SetString("nombre", user.Nombre);

            // TempData -> permite la comunicacion (data) entre controllers
            // durara hasta que tenga el 'login' abierto...
            TempData["nombre"] = HttpContext.Session.GetString("nombre");

            return RedirectToAction("Index"); // redireccionar al localhost/Home/Index.
        }

        public IActionResult Logout()
        {
            // Limpiar las variables TempData y Session.
            TempData.Clear();
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}
