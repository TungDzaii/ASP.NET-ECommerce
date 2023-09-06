using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Data;

namespace server.Controllers
{
    public class Order_detailController : Controller
    {
        private readonly serverContext _context;

        public Order_detailController(serverContext context)
        {
            _context = context;
        }

        // GET: Order_detail
        public async Task<IActionResult> Index()
        {
            var serverContext = _context.Order_detail.Include(o => o.Order).Include(o => o.Product);
            return View(await serverContext.ToListAsync());
        }

        // GET: Order_detail/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order_detail == null)
            {
                return NotFound();
            }

            var order_detail = await _context.Order_detail
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Order_items_id == id);
            if (order_detail == null)
            {
                return NotFound();
            }

            return View(order_detail);
        }

        // GET: Order_detail/Create
        public IActionResult Create()
        {
            ViewData["Order_id"] = new SelectList(_context.Order, "Oder_id", "Oder_id");
            ViewData["Pid"] = new SelectList(_context.Product, "Pid", "Name");
            return View();
        }

        // POST: Order_detail/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Order_items_id,Order_id,Pid,quantity,total")] Order_detail order_detail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order_detail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Order_id"] = new SelectList(_context.Order, "Oder_id", "Oder_id", order_detail.Order_id);
            ViewData["Pid"] = new SelectList(_context.Product, "Pid", "Name", order_detail.Pid);
            return View(order_detail);
        }

        // GET: Order_detail/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order_detail == null)
            {
                return NotFound();
            }

            var order_detail = await _context.Order_detail.FindAsync(id);
            if (order_detail == null)
            {
                return NotFound();
            }
            ViewData["Order_id"] = new SelectList(_context.Order, "Oder_id", "Oder_id", order_detail.Order_id);
            ViewData["Pid"] = new SelectList(_context.Product, "Pid", "Name", order_detail.Pid);
            return View(order_detail);
        }

        // POST: Order_detail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Order_items_id,Order_id,Pid,quantity,total")] Order_detail order_detail)
        {
            if (id != order_detail.Order_items_id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order_detail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Order_detailExists(order_detail.Order_items_id))
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
            ViewData["Order_id"] = new SelectList(_context.Order, "Oder_id", "Oder_id", order_detail.Order_id);
            ViewData["Pid"] = new SelectList(_context.Product, "Pid", "Name", order_detail.Pid);
            return View(order_detail);
        }

        // GET: Order_detail/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order_detail == null)
            {
                return NotFound();
            }

            var order_detail = await _context.Order_detail
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Order_items_id == id);
            if (order_detail == null)
            {
                return NotFound();
            }

            return View(order_detail);
        }

        // POST: Order_detail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order_detail == null)
            {
                return Problem("Entity set 'serverContext.Order_detail'  is null.");
            }
            var order_detail = await _context.Order_detail.FindAsync(id);
            if (order_detail != null)
            {
                _context.Order_detail.Remove(order_detail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Order_detailExists(int id)
        {
          return (_context.Order_detail?.Any(e => e.Order_items_id == id)).GetValueOrDefault();
        }
    }
}
