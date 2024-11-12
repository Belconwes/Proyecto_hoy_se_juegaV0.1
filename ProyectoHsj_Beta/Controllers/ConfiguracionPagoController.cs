using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProyectoHsj_Beta.Models;

namespace ProyectoHsj_Beta.Controllers
{
    public class ConfiguracionPagoController : Controller
    {
        private readonly HoySeJuegaContext _context;
        public ConfiguracionPagoController(HoySeJuegaContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.ConfiguracionPagos.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ConfiguracionPago configuracionPago)
        {
            if (ModelState.IsValid)
            {
                configuracionPago.FechaModificacion = DateTime.Now;
                _context.Add(configuracionPago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(configuracionPago);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var configuracionPago = await _context.ConfiguracionPagos.FindAsync(id);
            if (configuracionPago == null)
                return NotFound();

            return View(configuracionPago);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ConfiguracionPago configuracionPago)
        {
            if (id != configuracionPago.IdConfiguracion)
                return NotFound();

            if (ModelState.IsValid)
            {
                configuracionPago.FechaModificacion = DateTime.Now;
                _context.Update(configuracionPago);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(configuracionPago);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var configuracionPago = await _context.ConfiguracionPagos.FindAsync(id);
            if (configuracionPago == null)
                return NotFound();

            return View(configuracionPago);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var configuracionPago = await _context.ConfiguracionPagos.FindAsync(id);
            _context.ConfiguracionPagos.Remove(configuracionPago);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
