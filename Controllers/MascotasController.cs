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
    public class MascotasController : Controller
    {
        private readonly SistemaVeterinarioContext _context;

        public MascotasController(SistemaVeterinarioContext context)
        {
            _context = context;
        }

        // GET: Mascotas
        public async Task<IActionResult> Index()
        {
            var sistemaVeterinarioContext = _context.Mascotas.Include(m => m.IdClienteNavigation).Include(m => m.IdEspecieNavigation).Include(m => m.IdRazaNavigation);
            return View(await sistemaVeterinarioContext.ToListAsync());
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
                .Include(m => m.IdEspecieNavigation)
                .Include(m => m.IdRazaNavigation)
                .FirstOrDefaultAsync(m => m.IdMascota == id);
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
        public async Task<IActionResult> Create([Bind("IdMascota,IdCliente,IdEspecie,IdRaza,Nombre,Sexo,Edad")] Mascota mascota)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mascota);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "RunYNombre", mascota.IdCliente);
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "NombreEspecie", mascota.IdEspecie);
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

            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota == null)
            {
                return NotFound();
            }
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "IdCliente", "RunYNombre", mascota.IdCliente);
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "NombreEspecie", mascota.IdEspecie);
            ViewData["IdRaza"] = new SelectList(_context.Razas, "IdRaza", "NombreRaza", mascota.IdRaza);
            return View(mascota);
        }

        // POST: Mascotas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdMascota,IdCliente,IdEspecie,IdRaza,Nombre,Sexo,Edad")] Mascota mascota)
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
            ViewData["IdEspecie"] = new SelectList(_context.Especies, "IdEspecie", "NombreEspecie", mascota.IdEspecie);
            ViewData["IdRaza"] = new SelectList(_context.Razas, "IdRaza", "NombreRaza", mascota.IdRaza);
            return View(mascota);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota != null)
            {
                _context.Mascotas.Remove(mascota);
            }

            await _context.SaveChangesAsync();
            return Json("ok");
        }

        private bool MascotaExists(int id)
        {
            return _context.Mascotas.Any(e => e.IdMascota == id);
        }
    }
}
