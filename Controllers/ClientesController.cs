using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using sistemaVeterinario.Models;

namespace sistemaVeterinario.Controllers
{
    public class ClientesController : Controller
    {
        private readonly SistemaVeterinarioContext _context;

        public ClientesController(SistemaVeterinarioContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.IdCliente == id);
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
        public async Task<IActionResult> Create([Bind("IdCliente,Run,Nombre,Telefono,Email,Direccion")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                try
                {
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

            var cliente = await _context.Clientes.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("IdCliente,Run,Nombre,Telefono,Email,Direccion")] Cliente cliente)
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

        public async Task<IActionResult> Delete(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return Json("notfound"); // Cliente no encontrado
            }

            // Verificar si hay mascotas asociadas a este cliente
            var hasAssociatedMascotas = await _context.Mascotas.AnyAsync(m => m.IdCliente == id);
            if (hasAssociatedMascotas)
            {
                return Json("has_children");
            }

            try
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
                return Json("ok");
            }
            catch (DbUpdateException ex)
            {
                return Json("error_db_constraint");
            }
            catch (Exception ex)
            {
                return Json("error_unexpected");
            }
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.IdCliente == id);
        }

        /**
         * Metodos para buscar la existencia de un RUN o Email en la DB.
         */
        [HttpGet]
        public async Task<JsonResult> ClienteRunExists(string run)
        {
            var exists = await _context.Clientes.AnyAsync(c => c.Run == run);
            return Json(exists);
        }

        [HttpGet]
        public async Task<JsonResult> ClienteEmailExists(string email, int? id)
        {
            bool exists;

            if (id.HasValue)
            {
                exists = await _context.Clientes.AnyAsync(c => c.Email == email && c.IdCliente != id.Value);
            }
            else
            {
                exists = await _context.Clientes.AnyAsync(c => c.Email == email);
            }

            return Json(exists);
        }
    }
}
