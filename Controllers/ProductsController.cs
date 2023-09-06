using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;

namespace server.Controllers
{
    public class ProductsController : Controller
    {
        private readonly serverContext _context;

        public ProductsController(serverContext context)
        {
            _context = context;
        }

        // GET: Manager/Products
        public async Task<IActionResult> Index()
        {
            var serverContext = _context.Product.Include(p => p.Brand);
            return View(await serverContext.ToListAsync());
        }

        // GET: Manager/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(m => m.Pid == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }



        // GET: Manager/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(m => m.Pid == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Manager/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'serverContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.Pid == id)).GetValueOrDefault();
        }
    }
}
