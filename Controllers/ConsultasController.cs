using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sistemaVeterinario.Helpers;
using sistemaVeterinario.Models;

namespace sistemaVeterinario.Controllers
{
    [Authorize]
    public class ConsultasController : Controller
    {
        private readonly SistemaVeterinarioContext _context;

        public ConsultasController(SistemaVeterinarioContext context)
        {
            _context = context;
        }

        // GET: Consultas
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
            var consultas = _context.Consultas
                .Include(c => c.IdEstadoConsultaNavigation)
                .Include(c => c.IdMascotaNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                consultas = consultas.Where(c => c.IdMascotaNavigation.Nombre.ToLower().Contains(search.ToLower()));
            }

            return View(await PaginatedList<Consulta>.CreateAsync(consultas, pagNumber ?? 1, pagSize));
        }

        // GET: Consultas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consulta = await _context.Consultas
                .Include(c => c.IdEstadoConsultaNavigation)
                .Include(c => c.IdMascotaNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .FirstOrDefaultAsync(m => m.IdConsulta == id);
            if (consulta == null)
            {
                return NotFound();
            }

            return View(consulta);
        }

        // GET: Consultas/Create
        public IActionResult Create()
        {
            ViewData["IdEstadoConsulta"] = new SelectList(_context.EstadoConsultas, "IdEstadoConsulta", "NombreEstado");
            ViewData["IdMascota"] = new SelectList(_context.Mascotas, "IdMascota", "NombreDueño");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Nombre");
            return View();
        }

        // POST: Consultas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdConsulta,IdMascota,IdUsuario,IdEstadoConsulta,FechaConsulta,Motivo,Diagnostico")] Consulta consulta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(consulta);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEstadoConsulta"] = new SelectList(_context.EstadoConsultas, "IdEstadoConsulta", "NombreEstado", consulta.IdEstadoConsulta);
            ViewData["IdMascota"] = new SelectList(_context.Mascotas, "IdMascota", "NombreDueño", consulta.IdMascota);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Nombre", consulta.IdUsuario);
            return View(consulta);
        }

        // GET: Consultas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }
            ViewData["IdEstadoConsulta"] = new SelectList(_context.EstadoConsultas, "IdEstadoConsulta", "NombreEstado", consulta.IdEstadoConsulta);
            ViewData["IdMascota"] = new SelectList(_context.Mascotas, "IdMascota", "NombreDueño", consulta.IdMascota);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Nombre", consulta.IdUsuario);
            return View(consulta);
        }

        // POST: Consultas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdConsulta,IdMascota,IdUsuario,IdEstadoConsulta,FechaConsulta,Motivo,Diagnostico")] Consulta consulta)
        {
            if (id != consulta.IdConsulta)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(consulta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConsultaExists(consulta.IdConsulta))
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
            ViewData["IdEstadoConsulta"] = new SelectList(_context.EstadoConsultas, "IdEstadoConsulta", "NombreEstado", consulta.IdEstadoConsulta);
            ViewData["IdMascota"] = new SelectList(_context.Mascotas, "IdMascota", "NombreDueño", consulta.IdMascota);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Nombre", consulta.IdUsuario);
            return View(consulta);
        }

        [Authorize(Roles = "Admin, Veterinari@, Secretari@")]
        public async Task<IActionResult> Delete(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null)
            {
                return Json("notfound"); // Consulta no encontrada
            }

            // Verificar si hay tratamientos asociados a esta consulta
            var hasAssociatedTratamientos = await _context.Tratamientos.AnyAsync(t => t.IdConsulta == id);
            if (hasAssociatedTratamientos)
            {
                return Json("has_children"); // La consulta tiene tratamientos asociados
            }

            try
            {
                _context.Consultas.Remove(consulta);
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

        private bool ConsultaExists(int id)
        {
            return _context.Consultas.Any(e => e.IdConsulta == id);
        }

        public async Task<IActionResult> ExportToExcel(string search)
        {
            var consultasQuery = _context.Consultas
                .Include(c => c.IdMascotaNavigation) // para obtener el nombre de la mascota
                .ThenInclude(m => m.IdClienteNavigation) // para obtener el nombre del dueño
                .Include(c => c.IdUsuarioNavigation) // para obtener el nombre del veterinario
                .Include(c => c.IdEstadoConsultaNavigation) // para obtener el estado
                .AsQueryable();

            string nombreArchivo = "Consultas";

            if (!string.IsNullOrEmpty(search))
            {
                consultasQuery = consultasQuery.Where(c => c.IdMascotaNavigation.Nombre.ToLower().Contains(search.ToLower()));
                nombreArchivo += $"_{search}";
            }

            var exportarData = await consultasQuery.Select(c => new {
                Fecha = c.FechaConsulta.ToString("dd-MM-yyyy"),
                Mascota = c.IdMascotaNavigation.Nombre,
                Dueño = c.IdMascotaNavigation.IdClienteNavigation.Nombre,
                Veterinario = c.IdUsuarioNavigation.Nombre,
                Motivo = c.Motivo,
                Diagnostico = c.Diagnostico,
                Estado = c.IdEstadoConsultaNavigation.NombreEstado
            }).ToListAsync();

            var contenidoArchivo = ExcelExporter.GenerarExcel(exportarData, "Consultas");

            return File(
                contenidoArchivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{nombreArchivo}.xlsx"
            );
        }

        public async Task<IActionResult> ExportToPdf(string search)
        {
            var consultasQuery = _context.Consultas
                .Include(c => c.IdMascotaNavigation)
                .ThenInclude(m => m.IdClienteNavigation)
                .Include(c => c.IdUsuarioNavigation)
                .Include(c => c.IdEstadoConsultaNavigation)
                .AsQueryable();

            string nombreArchivo = "Consultas";

            if (!string.IsNullOrEmpty(search))
            {
                consultasQuery = consultasQuery.Where(c => c.IdMascotaNavigation.Nombre.ToLower().Contains(search.ToLower()));
                nombreArchivo += $"_{search}";
            }

            var exportarData = await consultasQuery.Select(c => new {
                Fecha = c.FechaConsulta.ToString("dd-MM-yyyy"),
                Mascota = c.IdMascotaNavigation.Nombre,
                Dueño = c.IdMascotaNavigation.IdClienteNavigation.Nombre,
                Veterinario = c.IdUsuarioNavigation.Nombre,
                Motivo = c.Motivo,
                Diagnostico = c.Diagnostico,
                Estado = c.IdEstadoConsultaNavigation.NombreEstado
            }).ToListAsync();

            var contenidoArchivo = PdfExporter.GenerarPdf(exportarData, $"Reporte de {nombreArchivo}");

            return File(
                contenidoArchivo,
                "application/pdf",
                $"{nombreArchivo}.pdf"
            );
        }
    }
}
