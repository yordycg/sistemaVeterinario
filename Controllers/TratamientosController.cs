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
    public class TratamientosController : Controller
    {
        private readonly SistemaVeterinarioContext _context;

        public TratamientosController(SistemaVeterinarioContext context)
        {
            _context = context;
        }

        // GET: Tratamientos
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
            /*
             * Carga anticipada (Eager Loading) con Include y ThenInclude:
             *
             * El objetivo de esta consulta es cargar una lista de Tratamientos y, para cada uno,
             * traer toda la información relacionada (Consulta, Mascota y Cliente) en una
             * única y eficiente consulta a la base de datos.
             *
             * 1. .Include(t => t.IdConsultaNavigation):
             *    - Por cada 'Tratamiento' (t) que se obtiene, este comando le dice a Entity Framework:
             *      "Incluye también el objeto 'Consulta' completo que está asociado a este tratamiento".
             *    - Sin esto, `tratamiento.IdConsultaNavigation` sería nulo.
             *
             * 2. .ThenInclude(c => c.IdMascotaNavigation):
             *    - 'ThenInclude' continúa la cadena desde la última entidad incluida. En este caso,
             *      continúa desde 'Consulta' (c).
             *    - Le dice: "YA que incluiste la Consulta, AHORA, para cada una de esas consultas,
             *      incluye también el objeto 'Mascota' asociado".
             *    - La ruta de datos hasta ahora es: Tratamiento -> Consulta -> Mascota.
             *
             * 3. .ThenInclude(m => m.IdClienteNavigation):
             *    - De nuevo, 'ThenInclude' continúa desde la última entidad incluida, que ahora es
             *      'Mascota' (m).
             *    - Le dice: "Y finalmente, para cada una de esas mascotas, incluye también el
             *      objeto 'Cliente' (el dueño) asociado".
             *    - La ruta de datos completa es: Tratamiento -> Consulta -> Mascota -> Cliente.
             *
             * El resultado es que la variable 'tratamientos' contendrá una lista de todos los
             * tratamientos, y podrás navegar por toda la cadena de objetos (ej: `tratamiento.IdConsultaNavigation.IdMascotaNavigation.IdClienteNavigation.Nombre`)
             * sin provocar nuevas consultas a la base de datos, lo que es muy eficiente.
             */
            var tratamientos = _context.Tratamientos.Where(t => t.EsActivo)
                .Include(t => t.IdConsultaNavigation)
                    .ThenInclude(c => c.IdMascotaNavigation)
                        .ThenInclude(m => m.IdClienteNavigation)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                tratamientos = tratamientos.Where(t => t.Descripcion.ToLower().Contains(search.ToLower()));
            }

            return View(await PaginatedList<Tratamiento>.CreateAsync(tratamientos, pagNumber ?? 1, pagSize));
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
                .FirstOrDefaultAsync(m => m.IdTratamiento == id && m.EsActivo); // Solo mostrar detalles si está activo
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
        public async Task<IActionResult> Create([Bind("IdTratamiento,IdConsulta,Descripcion,Medicamento,FechaRegistro,EsActivo")] Tratamiento tratamiento)
        {
            if (ModelState.IsValid)
            {
                tratamiento.EsActivo = true; // Asegurar que el nuevo tratamiento esté activo por defecto
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

            var tratamiento = await _context.Tratamientos.FirstOrDefaultAsync(t => t.IdTratamiento == id && t.EsActivo); // Solo editar si está activo
            if (tratamiento == null)
            {
                return NotFound();
            }
            var consultas = _context.Consultas.Where(c => c.EsActivo).Include(c => c.IdMascotaNavigation); // Solo mostrar consultas activas
            ViewData["IdConsulta"] = new SelectList(consultas, "IdConsulta", "MascotaYFecha", tratamiento.IdConsulta);
            return View(tratamiento);
        }

        // POST: Tratamientos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdTratamiento,IdConsulta,Descripcion,Medicamento,FechaRegistro,EsActivo")] Tratamiento tratamiento)
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

            try
            {
                tratamiento.EsActivo = false; // Marcar como inactivo (soft delete)
                _context.Update(tratamiento);
                await _context.SaveChangesAsync();
                return Json("ok");
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json("error_unexpected");
            }
        }

        private bool TratamientoExists(int id)
        {
            return _context.Tratamientos.Any(e => e.IdTratamiento == id && e.EsActivo); // Solo existen si están activos
        }

        

        public async Task<IActionResult> ExportToExcel(string search)
        {
            var tratamientosQuery = _context.Tratamientos.Where(t => t.EsActivo)
                .Include(t => t.IdConsultaNavigation)
                .ThenInclude(c => c.IdMascotaNavigation) // Para obtener la mascota de la consulta
                .AsQueryable();

            string nombreArchivo = "Tratamientos";

            if (!string.IsNullOrEmpty(search))
            {
                tratamientosQuery = tratamientosQuery.Where(t => t.Descripcion.ToLower().Contains(search.ToLower()));
                nombreArchivo += $"_{search}";
            }

            var exportData = await tratamientosQuery.Select(t => new {
                t.Descripcion,
                t.Medicamento,
                Mascota = t.IdConsultaNavigation.IdMascotaNavigation.Nombre,
                FechaConsulta = t.IdConsultaNavigation.FechaConsulta.ToString("dd-MM-yyyy"),
                Activo = t.EsActivo // Incluir el estado de actividad en el exportado
            }).ToListAsync();

            var contenidoArchivo = ExcelExporter.GenerarExcel(exportData, "Tratamientos");

            return File(
                contenidoArchivo,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"{nombreArchivo}.xlsx"
            );
        }

        public async Task<IActionResult> ExportToPdf(string search)
        {
            var tratamientosQuery = _context.Tratamientos.Where(t => t.EsActivo)
                .Include(t => t.IdConsultaNavigation)
                .ThenInclude(c => c.IdMascotaNavigation)
                .AsQueryable();

            string nombreArchivo = "Tratamientos";

            if (!string.IsNullOrEmpty(search))
            {
                tratamientosQuery = tratamientosQuery.Where(t => t.Descripcion.ToLower().Contains(search.ToLower()));
                nombreArchivo += $"_{search}";
            }

            var exportData = await tratamientosQuery.Select(t => new {
                t.Descripcion,
                t.Medicamento,
                Mascota = t.IdConsultaNavigation.IdMascotaNavigation.Nombre,
                FechaConsulta = t.IdConsultaNavigation.FechaConsulta.ToString("dd-MM-yyyy"),
                Activo = t.EsActivo // Incluir el estado de actividad en el exportado
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