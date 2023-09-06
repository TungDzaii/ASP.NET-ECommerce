using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Data;
using server.Controllers;

namespace server.Areas.Staff.Controllers
{
    [Area("Staff")]
    public class BrandsController : BaseController
    {
        public BrandsController(serverContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        // GET: Staff/Brands
        public async Task<IActionResult> Index()
        {
			if (this_user == null || this_user.Role != 2)
			{
				return Redirect("/");
			}
			ViewData["User"] = this_user;
			return _context.Brand != null ?
                        View(await _context.Brand.ToListAsync()) :
                        Problem("Entity set 'serverContext.Brand'  is null.");
        }


        // GET: Staff/Brands/Create
        public IActionResult Create()
        {
			if (this_user == null)
			{
				return Redirect("/");
			}
			if (this_user.Role != 2)
			{
				return Redirect("/");
			}
			ViewData["User"] = this_user;
			return View();
        }

        // POST: Staff/Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Bid,Title")] Brand brand)
        {
			if (this_user == null)
			{
				return Redirect("/");
			}
			if (this_user.Role != 2)
			{
				return Redirect("/");
			}

			if (ModelState.IsValid)
            {
                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Staff/Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
			if (this_user == null)
			{
				return Redirect("/");
			}
			if (this_user.Role != 2)
			{
				return Redirect("/");
			}
			ViewData["User"] = this_user;

			if (id == null || _context.Brand == null)
            {
                return NotFound();
            }

            var brand = await _context.Brand.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Staff/Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Bid,Title")] Brand brand)
        {
			if (this_user == null)
			{
				return Redirect("/");
			}
			if (this_user.Role != 2)
			{
				return Redirect("/");
			}

			if (id != brand.Bid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.Bid))
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
            return View(brand);
        }

        private bool BrandExists(int id)
        {
            return (_context.Brand?.Any(e => e.Bid == id)).GetValueOrDefault();
        }
    }
}
