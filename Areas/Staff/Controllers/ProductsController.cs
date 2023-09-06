using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using server.Models;
using server.Data;
using Microsoft.AspNetCore.Http;
using server.Controllers;

namespace server.Areas.Staff.Controllers
{
    [Area("Staff")]
    public class ProductsController : BaseController
    {
        public ProductsController(serverContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
		{
        }

        // GET: Staff/Products
        public async Task<IActionResult> Index()
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

			var serverContext = _context.Product.Include(p => p.Brand);
            return View(await serverContext.ToListAsync());
        }


        // GET: Staff/Products/Create
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

			ViewData["Bid"] = new SelectList(_context.Brand, "Bid", "Title");
            return View();
        }

        // POST: Staff/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Pid,Bid,Name,Price,Stock,Image,Color,Gift,Model,Warranty,Description")] Product product, IFormFile image)
        {
			if (this_user == null)
			{
				return Redirect("/");
			}
			if (this_user.Role != 2)
			{
				return Redirect("/");
			}
			if (image != null && image.Length > 0)
            {
                // Lưu trữ tệp ảnh vào thư mục hoặc cơ sở dữ liệu tùy theo yêu cầu của bạn.
                // lưu trữ tệp ảnh trong thư mục 'wwwroot/images'
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Product", image.FileName);
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                // Lưu đường dẫn tệp ảnh vào thuộc tính 'Image' của đối tượng 'Product'
                product.Image = "/images/Product/" + image.FileName;
            }
            _context.Add(product);
            await _context.SaveChangesAsync();
            return Redirect("/Staff/Products");

        }

        // GET: Staff/Products/Edit/5
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
			if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Bid"] = new SelectList(_context.Brand, "Bid", "Title", product.Bid);
            return View(product);
        }

        // POST: Staff/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Pid,Bid,Name,Price,Stock,Image,Color,Gift,Model,Warranty,Description")] Product product, IFormFile image)
        {
			if (this_user == null || this_user.Role != 2)
			{
				return Redirect("/");
			}
			if (id != product.Pid)
            {
                return NotFound();
            }

            if (ModelState.IsValid || !ModelState.IsValid)
            {
                try
                {
                    if (image != null && image.Length > 0)
                    {
                        // Lưu trữ tệp ảnh vào thư mục hoặc cơ sở dữ liệu tùy theo yêu cầu của bạn.
                        // lưu trữ tệp ảnh trong thư mục 'wwwroot/images'
                        var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Product", image.FileName);
                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }

                        // Lưu đường dẫn tệp ảnh vào thuộc tính 'Image' của đối tượng 'Product'
                        product.Image = "/images/Product/" + image.FileName;
                    }
                    else
                    {
                        // Nếu không có ảnh mới được cung cấp, giữ nguyên giá trị cũ của trường "Image"
                        var existingProduct = _context.Product.AsNoTracking().FirstOrDefault(p => p.Pid == product.Pid);
                        string img = existingProduct.Image;
                        if (img != null || img != "")
                        {
                            product.Image = img;
                        }
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Pid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            ViewData["Bid"] = new SelectList(_context.Brand, "Bid", "Bid", product.Bid);
            return RedirectToAction(nameof(Index));
        }


        private bool ProductExists(int id)
        {
            return (_context.Product?.Any(e => e.Pid == id)).GetValueOrDefault();
        }
    }
}
