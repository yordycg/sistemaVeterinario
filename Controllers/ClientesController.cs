using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using sistemaVeterinario.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sistemaVeterinario.Models;

namespace sistemaVeterinario.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly SistemaVeterinarioContext _context;

        public ClientesController(SistemaVeterinarioContext context)
        {
            _context = context;
        }

        // GET: Clientes
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

            // ViewData para capturar la busqueda, y poder filtrar.
            ViewData["filtro"] = search;

            int pagSize = 20; // 20 es un numero prudente, datos a mostrar por pagina.
            var clientes = _context.Clientes.Where(c => c.EsActivo).AsQueryable(); // traer lista de clientes activos.

            // Verificar que se haya realizado una busqueda, sino no hacer nada.
            if (!string.IsNullOrEmpty(search))
            {
                clientes = clientes.Where(c => c.Nombre.ToLower().Contains(search.ToLower()));
            }

            // Retornar la paginacion...
            return View(await PaginatedList<Cliente>.CreateAsync(clientes, pagNumber ?? 1, pagSize));
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.IdCliente == id && m.EsActivo); // Solo mostrar detalles si está activo
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCliente,Run,Nombre,Telefono,Email,Direccion,FechaRegistro")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    cliente.EsActivo = true; // Asegurar que el nuevo cliente esté activo por defecto
                    _context.Add(cliente);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    /**
                     * Manejo de excepciones:
                     * - Comprobar si tiene relacion con la columna PK (Run).
                     * - Comprobar si es otro atributo que sea UNIQUE.
                     */
                    if (ex.InnerException != null && ex.InnerException.Message.Contains("Duplicate entry"))
                    {
                        if (ex.InnerException.Message.Contains("for key 'clientes.Run'"))
                        {
                            ModelState.AddModelError("Run", "El RUN ingresado ya existe.");
                        }
                        else
                        {
                            // Comprobar UNIQUE.
                            ModelState.AddModelError(string.Empty, "Ha ocurrido un error al guardar los datos. Uno de los valores únicos ya existe.");
                        }
                    }
                    else
                    {
                        // Otro tipo de error.
                        ModelState.AddModelError(string.Empty, "Ha ocurrido un error inesperado al guardar en la base de datos.");
                    }
                }
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.IdCliente == id && c.EsActivo); // Solo editar si está activo
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCliente,Run,Nombre,Telefono,Email,Direccion,FechaRegistro,EsActivo")] Cliente cliente)
        {
            if (id != cliente.IdCliente)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.IdCliente))
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
            return View(cliente);
        }

        // Permitir que solo el rol 'Admin' pueda eliminar clientes.
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return Json("notfound"); // Cliente no encontrado
            }

            // Verificar si hay mascotas activas asociadas a este cliente
            var hasAssociatedMascotas = await _context.Mascotas.AnyAsync(m => m.IdCliente == id && m.EsActivo);
            if (hasAssociatedMascotas)
            {
                return Json("has_children"); // El cliente tiene mascotas activas asociadas
            }

            try
            {
                cliente.EsActivo = false; // Marcar como inactivo (soft delete)
                _context.Update(cliente);
                await _context.SaveChangesAsync();
                return Json("ok");
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json("error_unexpected");
            }
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.IdCliente == id && e.EsActivo); // Solo existen si están activos
        }

        /**
         * Metodos para buscar la existencia de un RUN o Email en la DB.
         */
        [HttpGet]
        public async Task<JsonResult> ClienteRunExists(string run)
        {
            var exists = await _context.Clientes.AnyAsync(c => c.Run == run && c.EsActivo); // Buscar solo entre activos
            return Json(exists);
        }

        [HttpGet]
        public async Task<JsonResult> ClienteEmailExists(string email, int? id)
        {
            bool exists;

            if (id.HasValue)
            {
                exists = await _context.Clientes.AnyAsync(c => c.Email == email && c.IdCliente != id.Value && c.EsActivo); // Buscar solo entre activos
            }
            else
            {
                exists = await _context.Clientes.AnyAsync(c => c.Email == email && c.EsActivo); // Buscar solo entre activos
            }

            return Json(exists);
        }

        /**
         * Metodo que nos permite exportar una tabla a excel.
         */
        public async Task<IActionResult> ExportToExcel(string search)
        {
            var consultaClientes = _context.Clientes.Where(c => c.EsActivo).AsQueryable(); // Exportar solo clientes activos
            string nombreArchivo = "Clientes";

            // Verificar si se realizo una busqueda (filtro), exportar con ese filtro.
            if (!string.IsNullOrEmpty(search))
            {
                consultaClientes = consultaClientes.Where(c => c.Nombre.ToLower().Contains(search.ToLower()));
                nombreArchivo += $"_{search}"; // agregar el 'filtro' al nombre del archivo.
            }

            // Con 'Select()' podemos elegir que datos enviar como columnas.
            // Esperamos hasta que se ejecute la consulta.
            var exportarData = await consultaClientes.Select(c => new {
                c.Run,
                c.Nombre,
                c.Telefono,
                c.Email,
                c.Direccion,
                c.EsActivo // Incluir el estado de actividad en el exportado
            }).ToListAsync();

            // Utilizamos el helper, le pasamos la lista de datos y el nombre de la hoja.
            var contenidoArchivo = ExcelExporter.GenerarExcel(exportarData, "Clientes");

            // Retornar el archivo para descargar.
            return File(
                    contenidoArchivo,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"{nombreArchivo}.xlsx"
                );
        }

        // GET: Clientes/ExportToPdf
        public async Task<IActionResult> ExportToPdf(string search)
        {
            var consultaClientes = _context.Clientes.Where(c => c.EsActivo).AsQueryable(); // Exportar solo clientes activos
            string nombreArchivo = "Clientes";

            if (!string.IsNullOrEmpty(search))
            {
                consultaClientes = consultaClientes.Where(c => c.Nombre.ToLower().Contains(search.ToLower()));
                nombreArchivo += $"_{search}";
            }

            var exportarData = await consultaClientes.Select(c => new {
                c.Run,
                c.Nombre,
                c.Telefono,
                c.Email,
                c.Direccion,
                c.EsActivo // Incluir el estado de actividad en el exportado
            }).ToListAsync();

            var contenidoArchivo = PdfExporter.GenerarPdf(exportarData, $"Reporte de {nombreArchivo}");

            return File(
                contenidoArchivo,
                "application/pdf", // Tipo para PDF
                $"{nombreArchivo}.pdf"
            );
        }
    }
}
