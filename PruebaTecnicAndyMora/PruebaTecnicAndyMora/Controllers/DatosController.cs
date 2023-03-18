using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicAndyMora.Models;
using Newtonsoft.Json;

namespace PruebaTecnicAndyMora.Controllers
{
    public class DatosController : Controller
    {
        private readonly DesAspContext _context;

        public DatosController(DesAspContext context)
        {
            _context = context;
        }

        // GET: Datos
        public async Task<IActionResult> Index()
        {
            var desAspContext = _context.Datos.Include(d => d.Usuario);
            return View(await desAspContext.ToListAsync());
        }
     

        // GET: Datos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Datos == null)
            {
                return NotFound();
            }

            var dato = await _context.Datos
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dato == null)
            {
                return NotFound();
            }

            return View(dato);
        }

        public async Task<JsonResult> DatosporId(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "No se encontro el dato" });
            }

            var dato = await _context.Datos
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(m => m.Usuario.Id == id);

            if (dato == null)
            {
                // Si no se encuentra el dato en la tabla Datos, buscar en la tabla Usuario
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
                if (usuario == null)
                {
                    return Json(new { success = false, message = "No se encontro el dato" });
                }
                else
                {
                    // Configurar el serializador JSON para ignorar las referencias circulares
                    var serializerSettings = new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    };

                    // Serializar el objeto Usuario a JSON con la configuración especificada
                    var json = JsonConvert.SerializeObject(new { success = true, message = "Dato encontrado", data = new { Usuario = usuario } }, serializerSettings);

                    return Json(json);
                }
            }

            // Configurar el serializador JSON para ignorar las referencias circulares
            var serializerSettings2 = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            // Serializar el objeto Dato a JSON con la configuración especificada
            var json2 = JsonConvert.SerializeObject(new { success = true, message = "Dato encontrado", data = dato }, serializerSettings2);

            return Json(json2);
        }



        // GET: Datos/Create
        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            return View();
        }

        public IActionResult VistaCombo()
        {
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre");
     
            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

           
            return View();
        }

        // POST: Datos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UsuarioId,Telefono,Direccion")] Dato dato)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dato);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", dato.UsuarioId);
            return View(dato);
        }

        // GET: Datos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Datos == null)
            {
                return NotFound();
            }

            var dato = await _context.Datos.FindAsync(id);
            if (dato == null)
            {
                return NotFound();
            }
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Nombre", dato.UsuarioId);
            return View(dato);
        }

        // POST: Datos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UsuarioId,Telefono,Direccion")] Dato dato)
        {
            if (id != dato.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dato);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DatoExists(dato.Id))
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
            ViewData["UsuarioId"] = new SelectList(_context.Usuarios, "Id", "Id", dato.UsuarioId);
            return View(dato);
        }

        // GET: Datos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Datos == null)
            {
                return NotFound();
            }

            var dato = await _context.Datos
                .Include(d => d.Usuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dato == null)
            {
                return NotFound();
            }

            return View(dato);
        }

        // POST: Datos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Datos == null)
            {
                return Problem("Entity set 'DesAspContext.Datos'  is null.");
            }
            var dato = await _context.Datos.FindAsync(id);
            if (dato != null)
            {
                _context.Datos.Remove(dato);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DatoExists(int id)
        {
          return (_context.Datos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
