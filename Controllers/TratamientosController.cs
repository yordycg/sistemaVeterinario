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
    [Authorize]
    public class TratamientosController : Controller
    {
        private readonly SistemaVeterinarioContext _context;

        public TratamientosController(SistemaVeterinarioContext context)
        {
            _context = context;
        }

        // GET: Tratamientos
        public async Task<IActionResult> Index()
        {
            var sistemaVeterinarioContext = _context.Tratamientos.Include(t => t.IdConsultaNavigation);
            return View(await sistemaVeterinarioContext.ToListAsync());
        }

        // GET: Tratamientos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamientos
                .Include(t => t.IdConsultaNavigation)
                .FirstOrDefaultAsync(m => m.IdTratamiento == id);
            if (tratamiento == null)
            {
                return NotFound();
            }

            return View(tratamiento);
        }

        // GET: Tratamientos/Create
        public IActionResult Create()
        {
            var consultas = _context.Consultas.Include(c => c.IdMascotaNavigation);
            ViewData["IdConsulta"] = new SelectList(consultas, "IdConsulta", "MascotaYFecha");
            return View();
        }

        // POST: Tratamientos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdTratamiento,IdConsulta,Descripcion,Medicamento")] Tratamiento tratamiento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tratamiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var consultas = _context.Consultas.Include(c => c.IdMascotaNavigation);
            ViewData["IdConsulta"] = new SelectList(consultas, "IdConsulta", "MascotaYFecha", tratamiento.IdConsulta);
            return View(tratamiento);
        }

        // GET: Tratamientos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamientos.FindAsync(id);
            if (tratamiento == null)
            {
                return NotFound();
            }
            var consultas = _context.Consultas.Include(c => c.IdMascotaNavigation);
            ViewData["IdConsulta"] = new SelectList(consultas, "IdConsulta", "MascotaYFecha", tratamiento.IdConsulta);
            return View(tratamiento);
        }

        // POST: Tratamientos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTratamiento,IdConsulta,Descripcion,Medicamento")] Tratamiento tratamiento)
        {
            if (id != tratamiento.IdTratamiento)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tratamiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TratamientoExists(tratamiento.IdTratamiento))
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
            var consultas = _context.Consultas.Include(c => c.IdMascotaNavigation);
            ViewData["IdConsulta"] = new SelectList(consultas, "IdConsulta", "MascotaYFecha", tratamiento.IdConsulta);
            return View(tratamiento);
        }

        [Authorize(Roles = "Admin, Veterinari@")]
        public async Task<IActionResult> Delete(int id)
        {
            var tratamiento = await _context.Tratamientos.FindAsync(id);
            if (tratamiento == null)
            {
                return Json("notfound"); // Tratamiento no encontrado
            }

            // No hay tablas que referencien a Tratamientos, por lo que no se necesita has_children check.

            try
            {
                _context.Tratamientos.Remove(tratamiento);
                await _context.SaveChangesAsync();
                return Json("ok");
            }
            catch (DbUpdateException ex)
            {
                // Log the exception
                return Json("error_db_constraint"); // Podría ocurrir si hay alguna restricción inesperada
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json("error_unexpected");
            }
        }

        private bool TratamientoExists(int id)
        {
            return _context.Tratamientos.Any(e => e.IdTratamiento == id);
        }
    }
}