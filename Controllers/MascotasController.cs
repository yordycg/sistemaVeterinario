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
    [Authorize]
    public class MascotasController : Controller
    {
        private readonly SistemaVeterinarioContext _context;

        public MascotasController(SistemaVeterinarioContext context)
        {
            _context = context;
        }

        // GET: Mascotas
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
            var mascotas = _context.Mascotas.Where(m => m.EsActivo)
                .Include(m => m.IdClienteNavigation)
                .Include(m => m.IdRazaNavigation)
                    .ThenInclude(r => r.IdEspecieNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                mascotas = mascotas.Where(m => m.Nombre.ToLower().Contains(search.ToLower()));
            }

            return View(await PaginatedList<Mascota>.CreateAsync(mascotas, pagNumber ?? 1, pagSize));
        }

        // GET: Mascotas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascotas
                .Include(m => m.IdClienteNavigation)
                .Include(m => m.IdRazaNavigation)
                    .ThenInclude(r => r.IdEspecieNavigation)
                .FirstOrDefaultAsync(m => m.IdMascota == id && m.EsActivo); // Solo mostrar detalles si está activa
            if (mascota == null)
            {
                return NotFound();
            }

            return View(mascota);
        }

        // GET: Mascotas/Create
        public IActionResult Create()
        {
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "RunYNombre");
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "NombreEspecie");
            ViewData["IdRaza"] = new SelectList(_context.Razas, "IdRaza", "NombreRaza");
            return View();
        }

        // POST: Mascotas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMascota,IdCliente,IdRaza,Nombre,Sexo,Edad,FechaRegistro")] Mascota mascota)
        {
            if (ModelState.IsValid)
            {
                mascota.EsActivo = true; // Asegurar que la nueva mascota esté activa por defecto
                _context.Add(mascota);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "RunYNombre", mascota.IdCliente);
            ViewData["IdRaza"] = new SelectList(_context.Razas, "IdRaza", "NombreRaza", mascota.IdRaza);
            return View(mascota);
        }

        // GET: Mascotas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascotas.Include(m => m.IdRazaNavigation).FirstOrDefaultAsync(m => m.IdMascota == id && m.EsActivo); // Solo editar si está activa
            if (mascota == null)
            {
                return NotFound();
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "RunYNombre", mascota.IdCliente);
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "NombreEspecie", mascota.IdRazaNavigation.IdEspecie);
            ViewData["IdRaza"] = new SelectList(_context.Razas.Where(r => r.IdEspecie == mascota.IdRazaNavigation.IdEspecie), "IdRaza", "NombreRaza", mascota.IdRaza);
            return View(mascota);
        }

        // POST: Mascotas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMascota,IdCliente,IdRaza,Nombre,Sexo,Edad,FechaRegistro,EsActivo")] Mascota mascota)
        {
            if (id != mascota.IdMascota)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mascota);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MascotaExists(mascota.IdMascota))
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
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "RunYNombre", mascota.IdCliente);
            ViewData["IdRaza"] = new SelectList(_context.Razas, "IdRaza", "NombreRaza", mascota.IdRaza);
            return View(mascota);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota == null)
            {
                return Json("notfound"); // Mascota no encontrada
            }

            // Verificar si hay consultas activas asociadas a esta mascota
            var hasAssociatedConsultas = await _context.Consultas.AnyAsync(c => c.IdMascota == id && c.EsActivo);
            if (hasAssociatedConsultas)
            {
                return Json("has_children"); // La mascota tiene consultas activas asociadas
            }

            try
            {
                mascota.EsActivo = false; // Marcar como inactivo (soft delete)
                _context.Update(mascota);
                await _context.SaveChangesAsync();
                return Json("ok");
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json("error_unexpected");
            }
        }

        private bool MascotaExists(int id)
        {
            return _context.Mascotas.Any(e => e.IdMascota == id && e.EsActivo); // Solo existen si están activas
        }

        public async Task<IActionResult> ExportToExcel(string search)
        {
            var consultaMascotas = _context.Mascotas.Where(m => m.EsActivo)
                .Include(m => m.IdClienteNavigation)
                .Include(m => m.IdRazaNavigation)
                    .ThenInclude(r => r.IdEspecieNavigation)
                .AsQueryable();

            string nombreArchivo = "Mascotas";

            if (!string.IsNullOrEmpty(search))
            {
                consultaMascotas = consultaMascotas.Where(m => m.Nombre.ToLower().Contains(search.ToLower()));
                nombreArchivo += $"_{search}";
            }

            var exportData = await consultaMascotas.Select(m => new {
                Nombre = m.Nombre,
                Dueño = m.IdClienteNavigation.Nombre,
                Especie = m.IdRazaNavigation.IdEspecieNavigation.NombreEspecie,
                Raza = m.IdRazaNavigation.NombreRaza,
                m.Sexo,
                m.Edad,
                m.EsActivo // Incluir el estado de actividad en el exportado
            }).ToListAsync();

            var contenidoArchivo = ExcelExporter.GenerarExcel(exportData, "Mascotas");

            return File(
                contenidoArchivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{nombreArchivo}.xlsx"
            );
        }

        public async Task<IActionResult> ExportToPdf(string search)
        {
            var consultaMascotas = _context.Mascotas.Where(m => m.EsActivo)
                .Include(m => m.IdClienteNavigation)
                .Include(m => m.IdRazaNavigation)
                    .ThenInclude(r => r.IdEspecieNavigation)
                .AsQueryable();

            string nombreArchivo = "Mascotas";

            if (!string.IsNullOrEmpty(search))
            {
                consultaMascotas = consultaMascotas.Where(m => m.Nombre.ToLower().Contains(search.ToLower()));
                nombreArchivo += $"_{search}";
            }

            var exportData = await consultaMascotas.Select(m => new {
                Nombre = m.Nombre,
                Dueño = m.IdClienteNavigation.Nombre,
                Especie = m.IdRazaNavigation.IdEspecieNavigation.NombreEspecie,
                Raza = m.IdRazaNavigation.NombreRaza,
                m.Sexo,
                m.Edad,
                m.EsActivo // Incluir el estado de actividad en el exportado
            }).ToListAsync();

            var contenidoArchivo = PdfExporter.GenerarPdf(exportData, $"Reporte de {nombreArchivo}");

            return File(
                contenidoArchivo,
                "application/pdf",
                $"{nombreArchivo}.pdf"
            );
        }

        [HttpGet]
        public async Task<IActionResult> GetRazasByEspecie(int especieId)
        {
            var razas = await _context.Razas
                .Where(r => r.IdEspecie == especieId)
                .Select(r => new { r.IdRaza, r.NombreRaza })
                .ToListAsync();
            return Json(razas);
        }
    }
}
