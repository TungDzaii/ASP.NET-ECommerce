using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Data;

namespace server.Areas.Staff.Controllers
{
    [Area("Staff")]
    public class OrdersController : Controller
    {
        private readonly serverContext _context;

        public OrdersController(serverContext context)
        {
            _context = context;
        }

        // GET: Staff/Orders
        public async Task<IActionResult> Index()
        {
            var serverContext = _context.Order.Include(o => o.Address).Include(o => o.User);
            return View(await serverContext.ToListAsync());
        }

        // GET: Staff/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Address)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Oder_id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Staff/Orders/Create
        public IActionResult Create()
        {
            ViewData["Address_id"] = new SelectList(_context.Address, "Address_id", "Address_id");
            ViewData["Uid"] = new SelectList(_context.User, "Uid", "Uid");
            return View();
        }

        // POST: Staff/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Oder_id,Uid,Address_id,Created_at,Updated_at,Status,Total")] Order order)
        {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Staff/Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["Address_id"] = new SelectList(_context.Address, "Address_id", "Address_id", order.Address_id);
            ViewData["Uid"] = new SelectList(_context.User, "Uid", "Uid", order.Uid);
            return View(order);
        }

        // POST: Staff/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Oder_id,Updated_at,Status")] Order order)
        {

            if (order.Status != null)
            {
                try
                {
                    var foundOrder = _context.Order.AsNoTracking().FirstOrDefault(o => o.Oder_id == order.Oder_id);
                    order.Address_id = foundOrder.Address_id;
                    order.Uid = foundOrder.Uid;
                    order.Created_at = foundOrder.Created_at;
                    order.Updated_at = DateTime.Now.ToString();
                    order.Total = foundOrder.Total;

                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Oder_id))
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
            return View(order);
        }

        // GET: Staff/Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.Address)
                .Include(o => o.User)
                .FirstOrDefaultAsync(m => m.Oder_id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Staff/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null)
            {
                return Problem("Entity set 'serverContext.Order'  is null.");
            }
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Order?.Any(e => e.Oder_id == id)).GetValueOrDefault();
        }
    }
}
