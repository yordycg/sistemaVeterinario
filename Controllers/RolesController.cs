using sistemaVeterinario.Helpers;
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
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly SistemaVeterinarioContext _context;

        public RolesController(SistemaVeterinarioContext context)
        {
            _context = context;
        }

        // GET: Roles
        public async Task<IActionResult> Index(int? pagNumber)
        {
            int pagSize = 20;
            var roles = from r in _context.Roles select r;

            return View(await PaginatedList<Role>.CreateAsync(roles, pagNumber ?? 1, pagSize));
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.IdRol == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdRol,NombreRol")] Role role)
        {
            if (ModelState.IsValid)
            {
                _context.Add(role);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRol,NombreRol")] Role role)
        {
            if (id != role.IdRol)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(role);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RoleExists(role.IdRol))
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
            return View(role);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return Json("notfound"); // Rol no encontrado
            }

            // Verificar si hay usuarios asociados a este rol
            var hasAssociatedUsers = await _context.Usuarios.AnyAsync(u => u.IdRol == id);
            if (hasAssociatedUsers)
            {
                return Json("has_users"); // El rol tiene usuarios asociados
            }

            try
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
                return Json("ok");
            }
            catch (DbUpdateException ex) // Captura excepciones de la base de datos (ej. restricción de clave foránea)
            {
                // Aquí podrías loggear 'ex' para depuración
                return Json("error_db_constraint"); // Indica un error de restricción de base de datos
            }
            catch (Exception ex) // Captura cualquier otra excepción inesperada
            {
                // Aquí podrías loggear 'ex' para depuración
                return Json("error_unexpected"); // Indica un error inesperado
            }
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.IdRol == id);
        }

        public async Task<IActionResult> ExportToExcel()
        {
            var roles = await _context.Roles.Select( r => new
            {
                r.IdRol,
                r.NombreRol
            }).ToListAsync();

            var contenidoArchivo = ExcelExporter.GenerarExcel(roles, "Roles");

            return File(
                contenidoArchivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                "roles.xlsx"
            );
        }
    }
}
