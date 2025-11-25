using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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

        // [Authorize] -> permite que solo usuarios autenticados accedan a este metodo.
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        //
        [Authorize(Roles  = "Admin")]
        public async Task<IActionResult> ObtenerAdminDashboardData()
        {
            try
            {
                // Obtener conteos totales de los clientes, mascotas y consultas.
                var totalClientes = await _context.Clientes.CountAsync();
                var totalMascotas = await _context.Mascotas.CountAsync();
                var totalConsultas = await _context.Consultas.CountAsync();

                // Crear query para obtener las consultas de las ultimos 6 meses.
                var ultimos6Meses = DateOnly.FromDateTime(DateTime.Now.AddMonths(-6));

                //var consultasPorMes = await _context.Consultas
                //    .Where(c => c.FechaConsulta >= ultimos6Meses) // filtrar por fecha.
                //    .GroupBy(c => new { c.FechaConsulta.Year, c.FechaConsulta.Month }) // agrupar por año y mes.
                //    .Select(g => new // asignar los resultados en un objeto anonimo.
                //    {
                //        Mes = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy-MM"), // etiqueta mas legible.
                //        Cantidad = g.Count() // contar los elementos en cada grupo para obtener el total mensual.
                //    })
                //    .OrderBy(x => x.Mes) // ordenar por mes, asi el grafico tiene el orden cronologico.
                //    .ToListAsync();

                var consultas = await _context.Consultas
                    .Where(c => c.FechaConsulta >= ultimos6Meses) // filtrar por fecha.
                    .ToListAsync();

                var consultasPorMes = consultas
                    .GroupBy(c => new { c.FechaConsulta.Year, c.FechaConsulta.Month }) // agrupar por año y mes.
                    .Select(g => new // asignar los resultados en un objeto anonimo.
                    {
                        Mes = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy-MM"), // etiqueta mas legible.
                        Cantidad = g.Count() // contar los elementos en cada grupo para obtener el total mensual.
                    })
                    .OrderBy(x => x.Mes) // ordenar por mes, asi el grafico tiene el orden cronologico.
                    .ToList();

                // Crear objeto para enviar como objeto JSON.
                var data = new { totalClientes, totalMascotas, totalConsultas, consultasPorMes };

                return Ok(data);
            }
            catch(Exception ex)
            {
                var respuestaError = new
                {
                    error = "Ocurrio un error inesperado al procesar la solicitud."
#if DEBUG
                    // Esta linea solo se ejecuta en modo DEBUG.
                    // Recomendado para evitar mostrar o exponer detalles internos...
                    ,
                    message = ex.Message
#endif
                };

                return StatusCode(500, respuestaError);
            }
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
        * - AllowAnonymous -> permite el acceso a este metodo sin necesidad de estar autenticado.
        */
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        /**
        * 
        */
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
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
            // Include() -> sirve para traer datos de tablas relacionadas, en este caso el rol del usuario.
            var user = await _context.Usuarios.Include(r => r.IdRolNavigation).FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "Email y/o password incorrectos.";
                return View();
            }

            /**
            * Claims:
            * - Sirven para almacenar informacion del usuario autenticado.
            * - Es otra forma aparte de las variables Session y TempData.
            */
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.IdUsuario.ToString()),
                new Claim(ClaimTypes.Name, user.Nombre),
                new Claim(ClaimTypes.Role, user.IdRolNavigation.NombreRol)
            };

            // ClaimsIdentity -> representa la identidad del usuario autenticado.
            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // ClaimsPrincipal -> aqui guardamos la informacion del usuario autenticado.
            var claimPrincipal = new ClaimsPrincipal(claimIdentity);

            // Iniciar sesion con autenticacion por cookies.
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal);


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

        public async Task<IActionResult> Logout()
        {
            // Limpiar las cookies de autenticacion.
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Limpiar las variables TempData y Session.
            TempData.Clear();
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}
