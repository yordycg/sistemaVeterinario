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
    // Podemos definir que el 'Admin' es el único rol que puede acceder a este controlador.
    [Authorize(Roles = "Admin")]
    public class UsuariosController : Controller
    {
        private readonly SistemaVeterinarioContext _context;

        public UsuariosController(SistemaVeterinarioContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var sistemaVeterinarioContext = _context.Usuarios.Include(u => u.IdEstadoUsuarioNavigation).Include(u => u.IdRolNavigation);
            return View(await sistemaVeterinarioContext.ToListAsync());
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
                .FirstOrDefaultAsync(m => m.IdUsuario == id);
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
        public async Task<IActionResult> Create([Bind("IdUsuario,IdRol,IdEstadoUsuario,Nombre,Email,Password")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
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

            var usuario = await _context.Usuarios.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("IdUsuario,IdRol,IdEstadoUsuario,Nombre,Email,Password")] Usuario usuario)
        {
            if (id != usuario.IdUsuario)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

            // Verificar si hay consultas asociadas a este usuario
            var hasAssociatedConsultas = await _context.Consultas.AnyAsync(c => c.IdUsuario == id);
            if (hasAssociatedConsultas)
            {
                return Json("has_children"); // El usuario tiene consultas asociadas
            }

            try
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
                return Json("ok");
            }
            catch (DbUpdateException ex)
            {
                // Log the exception
                return Json("error_db_constraint");
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
                exists = await _context.Usuarios.AnyAsync(u => u.Email == email && u.IdUsuario != id.Value);
            }
            else
            {
                exists = await _context.Usuarios.AnyAsync(u => u.Email == email);
            }

            return Json(exists);
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.IdUsuario == id);
        }
    }
}