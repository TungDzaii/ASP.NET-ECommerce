using Microsoft.AspNetCore.Mvc;
using server.Data;
using server.Controllers;
using server.Models;
using server.Infrastructure;

namespace server.Controllers
{
    public class HomeController : BaseController
    {
        private readonly EmailSender _emailSender;

        public HomeController(serverContext context, IHttpContextAccessor httpContextAccessor, EmailSender emailSender) : base(context, httpContextAccessor)
        {
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            ViewData["User"] = this_user;
            var products = _context.Product.ToList();
            ViewData["Products"] = products;
            var brands = _context.Brand.ToList();
            ViewData["Brands"] = brands;
            ViewData["CartItems"] = GetCartItems();
            return View();
        }

        [HttpGet]
        [Route("/product/{id}")]
        public async Task<IActionResult> Product(int id)
        {
            var product = _context.Product.FirstOrDefault(p => p.Pid == id);
            product.viewCount++;
            ViewData["Product"] = _context.Product.FirstOrDefault(p => p.Pid == id);
            await _context.SaveChangesAsync();
            return View();
        }

        

        private List<CartItem> GetCartItems()
        {
            List<CartItem> cart;
            if (HttpContext.Session.GetJson<List<CartItem>>("Cart") != null)
            {
                cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            }
            else
            {
                cart = new List<CartItem>();
            }
            return cart;
        }
    }
}
