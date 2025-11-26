using sistemaVeterinario.Helpers;
using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sistemaVeterinario.Models;

namespace sistemaVeterinario.Controllers
{
    // Podemos definir que el 'Admin' es el �nico rol que puede acceder a este controlador.
    [Authorize(Roles = "Admin")]
    public class UsuariosController : Controller
    {
        private readonly SistemaVeterinarioContext _context;

        public UsuariosController(SistemaVeterinarioContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index(int? pagNumber, string search, string filter)
        {
            if (search == null)
            {
                search = filter;
            }
            else
            {
                pagNumber = 1;
            }

            ViewData["filtro"] = search;

            int pagSize = 20;
            var usuarios = _context.Usuarios.Where(u => u.EsActivo)
                .Include(u => u.IdEstadoUsuarioNavigation)
                .Include(u => u.IdRolNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                usuarios = usuarios.Where(u => u.Nombre.ToLower().Contains(search.ToLower()));
            }

            return View(await PaginatedList<Usuario>.CreateAsync(usuarios, pagNumber ?? 1, pagSize));
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.IdEstadoUsuarioNavigation)
                .Include(u => u.IdRolNavigation)
                .FirstOrDefaultAsync(m => m.IdUsuario == id && m.EsActivo); // Solo mostrar detalles si está activo
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            ViewData["IdEstadoUsuario"] = new SelectList(_context.EstadoUsuarios, "IdEstadoUsuario", "NombreEstado");
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "NombreRol");
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdUsuario,IdRol,IdEstadoUsuario,Nombre,Email,Password,FechaRegistro,EsActivo")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Asegurar que el nuevo usuario esté activo por defecto y tenga el estado "Activo"
                usuario.EsActivo = true;
                usuario.IdEstadoUsuario = 1; // 1 = Activo
                
                // Hashear password antes de guardarla.
                usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstadoUsuario"] = new SelectList(_context.EstadoUsuarios, "IdEstadoUsuario", "NombreEstado", usuario.IdEstadoUsuario);
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "NombreRol", usuario.IdRol);
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id && u.EsActivo); // Solo editar si está activo
            if (usuario == null)
            {
                return NotFound();
            }
            ViewData["IdEstadoUsuario"] = new SelectList(_context.EstadoUsuarios, "IdEstadoUsuario", "NombreEstado", usuario.IdEstadoUsuario);
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "NombreRol", usuario.IdRol);
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,IdRol,IdEstadoUsuario,Nombre,Email,Password,FechaRegistro,EsActivo")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener el usuario existente (DB) para comparar.
                    // AsNoTracking() -> para evitar problemas de seguimiento de entidades.
                    var usuarioExistente = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.IdUsuario == id);

                    // Hashear password solo si ha sido modificado.
                    if (usuarioExistente != null && usuarioExistente.Password != usuario.Password)
                    {
                        usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
                    }

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.IdUsuario))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstadoUsuario"] = new SelectList(_context.EstadoUsuarios, "IdEstadoUsuario", "NombreEstado", usuario.IdEstadoUsuario);
            ViewData["IdRol"] = new SelectList(_context.Roles, "IdRol", "NombreRol", usuario.IdRol);
            return View(usuario);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return Json("notfound"); // Usuario no encontrado
            }

            // Verificar si hay consultas activas asociadas a este usuario
            var hasAssociatedConsultas = await _context.Consultas.AnyAsync(c => c.IdUsuario == id && c.EsActivo);
            if (hasAssociatedConsultas)
            {
                return Json("has_children"); // El usuario tiene consultas activas asociadas
            }

            try
            {
                usuario.EsActivo = false; // Marcar como inactivo (soft delete)
                _context.Update(usuario);
                await _context.SaveChangesAsync();
                return Json("ok");
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json("error_unexpected");
            }
        }

        [HttpGet]
        public async Task<JsonResult> UsuarioEmailExists(string email, int? id)
        {
            bool exists;

            if (id.HasValue)
            {
                exists = await _context.Usuarios.AnyAsync(u => u.Email == email && u.IdUsuario != id.Value && u.EsActivo); // Buscar solo entre activos
            }
            else
            {
                exists = await _context.Usuarios.AnyAsync(u => u.Email == email && u.EsActivo); // Buscar solo entre activos
            }

            return Json(exists);
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id && e.EsActivo); // Solo existen si están activos
        }

        public async Task<IActionResult> ExportToExcel(string search)
        {
            var usuariosQuery = _context.Usuarios.Where(u => u.EsActivo)
                .Include(u => u.IdEstadoUsuarioNavigation)
                .Include(u => u.IdRolNavigation)
                .AsQueryable();

            string nombreArchivo = "Usuarios";

            if (!string.IsNullOrEmpty(search))
            {
                usuariosQuery = usuariosQuery.Where(u => u.Nombre.ToLower().Contains(search.ToLower()));
                nombreArchivo += $"_{search}";
            }

            var exportData = await usuariosQuery.Select(u => new {
                u.Nombre,
                u.Email,
                Rol = u.IdRolNavigation.NombreRol,
                Estado = u.IdEstadoUsuarioNavigation.NombreEstado,
                u.EsActivo // Incluir el estado de actividad en el exportado
            }).ToListAsync();

            var contenidoArchivo = ExcelExporter.GenerarExcel(exportData, "Usuarios");

            return File(
                contenidoArchivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{nombreArchivo}.xlsx"
            );
        }

        public async Task<IActionResult> ExportToPdf(string search)
        {
            var usuariosQuery = _context.Usuarios.Where(u => u.EsActivo)
                .Include(u => u.IdEstadoUsuarioNavigation)
                .Include(u => u.IdRolNavigation)
                .AsQueryable();

            string nombreArchivo = "Usuarios";

            if (!string.IsNullOrEmpty(search))
            {
                usuariosQuery = usuariosQuery.Where(u => u.Nombre.ToLower().Contains(search.ToLower()));
                nombreArchivo += $"_{search}";
            }

            var exportData = await usuariosQuery.Select(u => new {
                u.Nombre,
                u.Email,
                Rol = u.IdRolNavigation.NombreRol,
                Estado = u.IdEstadoUsuarioNavigation.NombreEstado,
                u.EsActivo // Incluir el estado de actividad en el exportado
            }).ToListAsync();

            var contenidoArchivo = PdfExporter.GenerarPdf(exportData, $"Reporte de {nombreArchivo}");

            return File(
                contenidoArchivo,
                "application/pdf",
                $"{nombreArchivo}.pdf"
            );
        }
    }
}