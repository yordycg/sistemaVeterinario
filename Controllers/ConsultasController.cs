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
    public class ConsultasController : Controller
    {
        private readonly SistemaVeterinarioContext _context;

        public ConsultasController(SistemaVeterinarioContext context)
        {
            _context = context;
        }

        // GET: Consultas
        public async Task<IActionResult> Index()
        {
            var sistemaVeterinarioContext = _context.Consultas.Include(c => c.IdEstadoConsultaNavigation).Include(c => c.IdMascotaNavigation).Include(c => c.IdUsuarioNavigation);
            return View(await sistemaVeterinarioContext.ToListAsync());
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
            ViewData["IdEstadoConsulta"] = new SelectList(_context.EstadoConsultas, "IdEstadoConsulta", "IdEstadoConsulta");
            ViewData["IdMascota"] = new SelectList(_context.Mascotas, "IdMascota", "Nombre");
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Email");
            return View();
        }

        // POST: Consultas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["IdEstadoConsulta"] = new SelectList(_context.EstadoConsultas, "IdEstadoConsulta", "IdEstadoConsulta", consulta.IdEstadoConsulta);
            ViewData["IdMascota"] = new SelectList(_context.Mascotas, "IdMascota", "Nombre", consulta.IdMascota);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Email", consulta.IdUsuario);
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
            ViewData["IdEstadoConsulta"] = new SelectList(_context.EstadoConsultas, "IdEstadoConsulta", "IdEstadoConsulta", consulta.IdEstadoConsulta);
            ViewData["IdMascota"] = new SelectList(_context.Mascotas, "IdMascota", "Nombre", consulta.IdMascota);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Email", consulta.IdUsuario);
            return View(consulta);
        }

        // POST: Consultas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
            ViewData["IdEstadoConsulta"] = new SelectList(_context.EstadoConsultas, "IdEstadoConsulta", "IdEstadoConsulta", consulta.IdEstadoConsulta);
            ViewData["IdMascota"] = new SelectList(_context.Mascotas, "IdMascota", "Nombre", consulta.IdMascota);
            ViewData["IdUsuario"] = new SelectList(_context.Usuarios, "IdUsuario", "Email", consulta.IdUsuario);
            return View(consulta);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta != null)
            {
                _context.Consultas.Remove(consulta);
            }

            await _context.SaveChangesAsync();
            return Json("ok");
        }

        private bool ConsultaExists(int id)
        {
            return _context.Consultas.Any(e => e.IdConsulta == id);
        }
    }
}
