using BCrypt.Net;
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

        // Metodo para obtener los datos del dashboard del admin.
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

        // Metodo para obtener los datos del dashboard de recepcionista.
        [Authorize(Roles = "Recepcionista")]
        public async Task<IActionResult> ObtenerRecepcionistaDashboardData()
        {
            try
            {
                var hoy = DateOnly.FromDateTime(DateTime.Now);

                // Tarjetas de Tareas Diarias
                var nuevosClientesHoy = await _context.Clientes.CountAsync(c => c.FechaRegistro == hoy);
                var nuevasMascotasHoy = await _context.Mascotas.CountAsync(m => m.FechaRegistro == hoy);
                var consultasDelDiaTotal = await _context.Consultas.CountAsync(c => c.FechaConsulta == hoy);

                // Agenda General del Día (todas las citas de todos los veterinarios)
                var agendaDelDia = await _context.Consultas
                    .Where(c => c.FechaConsulta == hoy)
                    .Include(c => c.IdMascotaNavigation)
                        .ThenInclude(m => m.IdClienteNavigation)
                    .Include(c => c.IdUsuarioNavigation)
                    .Include(c => c.IdEstadoConsultaNavigation)
                    .OrderBy(c => c.FechaConsulta)
                    .Select(c => new
                    {
                        c.IdConsulta,
                        NombreMascota = c.IdMascotaNavigation.Nombre,
                        NombreCliente = c.IdMascotaNavigation.IdClienteNavigation.Nombre,
                        NombreVeterinario = c.IdUsuarioNavigation.Nombre,
                        FechaConsulta = c.FechaConsulta,
                        //HoraConsulta = c.HoraConsulta,
                        Motivo = c.Motivo,
                        Estado = c.IdEstadoConsultaNavigation.NombreEstado
                    })
                    .ToListAsync();

                // Tabla de "Acciones Requeridas": Consultas Pendientes para confirmar
                var consultasPendientesConfirmar = await _context.Consultas
                    .Where(c => c.IdEstadoConsultaNavigation.NombreEstado == "Pendiente")
                    .Include(c => c.IdMascotaNavigation)
                        .ThenInclude(m => m.IdClienteNavigation)
                    .Include(c => c.IdUsuarioNavigation)
                    .Select(c => new
                    {
                        c.IdConsulta,
                        NombreMascota = c.IdMascotaNavigation.Nombre,
                        NombreCliente = c.IdMascotaNavigation.IdClienteNavigation.Nombre,
                        TelefonoCliente = c.IdMascotaNavigation.IdClienteNavigation.Telefono,
                        EmailCliente = c.IdMascotaNavigation.IdClienteNavigation.Email,
                        NombreVeterinario = c.IdUsuarioNavigation.Nombre,
                        FechaConsulta = c.FechaConsulta,
                        //HoraConsulta = c.HoraConsulta,
                        Motivo = c.Motivo
                    })
                    .ToListAsync();

                // Tabla de "Acciones Requeridas": Consultas Finalizadas que podrían requerir agendar un próximo control (últimos 7 días)
                var hace7Dias = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));
                var consultasFinalizadasRecientes = await _context.Consultas
                    .Where(c => c.IdEstadoConsultaNavigation.NombreEstado == "Finalizada" && c.FechaConsulta >= hace7Dias)
                    .Include(c => c.IdMascotaNavigation)
                        .ThenInclude(m => m.IdClienteNavigation)
                    .Include(c => c.IdUsuarioNavigation)
                    .Select(c => new
                    {
                        c.IdConsulta,
                        NombreMascota = c.IdMascotaNavigation.Nombre,
                        NombreCliente = c.IdMascotaNavigation.IdClienteNavigation.Nombre,
                        NombreVeterinario = c.IdUsuarioNavigation.Nombre,
                        FechaConsulta = c.FechaConsulta,
                        Motivo = c.Motivo,
                        Diagnostico = c.Diagnostico
                    })
                    .ToListAsync();


                var data = new
                {
                    nuevosClientesHoy,
                    nuevasMascotasHoy,
                    consultasDelDiaTotal,
                    agendaDelDia,
                    consultasPendientesConfirmar,
                    consultasFinalizadasRecientes
                };

                return Ok(data);
            }
            catch (Exception ex)
            {
                var respuestaError = new
                {
                    error = "Ocurrio un error inesperado al procesar la solicitud."
#if DEBUG
                    ,
                    message = ex.Message
#endif
                };

                return StatusCode(500, respuestaError);
            }
        }

        // Metodo para obtener los datos del dashboard de veterinario.
        [Authorize(Roles = "Veterinari@")]
        public async Task<IActionResult> ObtenerVetDashboardData()
        {
            try
            {
                // Obtener el ID del usuario 'Veterinari@' autenticado.
                var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userID))
                {
                    return Unauthorized(new { error = "Usuario no autenticado." });
                }

                var veterinarioID = int.Parse(userID);
                var hoy = DateOnly.FromDateTime(DateTime.Now);

                // Consultar el dia de HOY para el veterinario autenticado.
                var consultasHoy = await _context.Consultas
                    .Where(c => c.IdUsuario == veterinarioID && c.FechaConsulta == hoy)
                    .Include(c => c.IdMascotaNavigation)
                    .Include(c => c.IdEstadoConsultaNavigation)
                    .OrderBy(c => c.FechaConsulta)
                    .Select(c => new
                    {
                        c.IdConsulta,
                        NombreMascota = c.IdMascotaNavigation.Nombre,
                        FechaConsulta = c.FechaConsulta,
                        Motivo = c.Motivo,
                        Estado = c.IdEstadoConsultaNavigation.NombreEstado
                    })
                    .ToListAsync();

                // Conteo de consultas pendientes y en progreso.
                var consultasPendientesHoy = consultasHoy.Count(c => c.Estado == "Pendiente");
                var consultasEnProgresoHoy = consultasHoy.Count(c => c.Estado == "En Progreso");

                // Obtener ultimos 5 pacientes atendidos por el veterinario.
                var ultimosPacientesData = await _context.Consultas
                    .Where(c => c.IdUsuario == veterinarioID && c.IdEstadoConsultaNavigation.NombreEstado == "Finalizada")
                    .OrderByDescending(c => c.FechaConsulta)
                    .Include(c => c.IdMascotaNavigation)
                    .Select(c => new
                    {
                        c.IdMascota,
                        NombreMascota = c.IdMascotaNavigation.Nombre,
                        c.FechaConsulta,
                    })
                    .Distinct() // no repetir mascotas que tuvieran varias consultas recientes.
                    .Take(5)
                    .ToListAsync();

                // Formatear la fecha
                var ultimosPacientes = ultimosPacientesData
                    .Select(p => new
                    {
                        p.IdMascota,
                        p.NombreMascota,
                        FechaAtencion = p.FechaConsulta.ToString("dd-MM-yyyy")
                    })
                    .ToList();

                // Obtener todos los diagnosticos realizados por el veterinario.
                var diagnosticos = await _context.Consultas
                    .Where(c => c.IdUsuario == veterinarioID && !string.IsNullOrEmpty(c.Diagnostico))
                    .Select(c => c.Diagnostico)
                    .ToListAsync();

                // Obtener los diagnosticos mas comunes (Top 5).
                // Agrupar y contar ocurrencias de cada diagnostico.
                var diagnosticosComunes = diagnosticos
                    .GroupBy(d => d)
                    .Select(g => new
                    {
                        Diagnostico = g.Key,
                        Cantidad = g.Count()
                    })
                    .OrderByDescending(x => x.Cantidad)
                    .Take(5)
                    .ToList();

                // Crear objeto para enviar como objeto JSON.
                var data = new
                {
                    consultasHoy,
                    consultasPendientesHoy,
                    consultasEnProgresoHoy,
                    ultimosPacientes,
                    diagnosticosComunes
                };

                return Ok(data);
            }
            catch (Exception ex)
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

            // Hacer una consulta en la DB para verificar la existencia del usuario, solo por email.
            // Include() -> sirve para traer datos de tablas relacionadas, en este caso el rol del usuario.
            var user = await _context.Usuarios.Include(r => r.IdRolNavigation).FirstOrDefaultAsync(u => u.Email == email);

            // Verificar que el usuario exista y que la password es correcta usando BCrypt.Verify()
            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
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
