using Microsoft.AspNetCore.Mvc;
using server.Data;
using server.Controllers;
using server.Models;
using server.Infrastructure;
using server.Models.ViewModels;

namespace server.Controllers
{
    public class CheckOutController : BaseController
    {
        public CheckOutController(serverContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {
        }

        public IActionResult Index()
        {
            ViewData["User"] = this_user;
            List<CartItem> cart;
			if (HttpContext.Session.GetJson<List<CartItem>>("Cart") != null)
			{
				cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
			}
			else
			{
				cart = new List<CartItem>();
			}


			CartViewModel cartVM = new()
			{
				CartItems = cart,
				GrandTotal = cart.Sum(x => x.Quantity * x.Price)
			};
			return View(cartVM);
        }
        
        
    }
}
