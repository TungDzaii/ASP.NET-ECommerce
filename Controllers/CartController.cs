using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Data;
using server.Infrastructure;
using server.Models.ViewModels;

namespace server.Controllers
{
	public class CartController : Controller
	{
		private readonly serverContext _context;
		public CartController(serverContext context)
		{
			_context = context;
		}
		public IActionResult Index()
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


			CartViewModel cartVM = new()
			{
				CartItems = cart,
				GrandTotal = cart.Sum(x => x.Quantity * x.Price)
			};
			return View(cartVM);
		}

		public async Task<IActionResult> Add(int id)
		{
			Product product = await _context.Product.FindAsync(id);
			List<CartItem> cart;
			if (HttpContext.Session.GetJson<List<CartItem>>("Cart") != null)
			{
				cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
			}
			else
			{
				cart = new List<CartItem>();
			}
			CartItem cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();
			if (cartItem == null)
			{
				if (product.Stock >= 1)
				{
					cart.Add(new CartItem(product));
				}
				else
				{
					TempData["Error"] = "This product is out of stock";
					return Redirect(Request.Headers["Referer"].ToString());
				}
			}
			else
			{
				if (cartItem.Quantity < product.Stock)
				{
					cartItem.Quantity += 1;
				}
				else
				{
					TempData["Error"] = "This product is out of stock";
					return Redirect(Request.Headers["Referer"].ToString());
				}
			}
			HttpContext.Session.SetJson("Cart", cart);
			TempData["Success"] = "The product has been added";
			return Redirect(Request.Headers["Referer"].ToString());
		}


		public async Task<IActionResult> Decrease(int id)
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

			CartItem cartItem = cart.Where(c => c.ProductId == id).FirstOrDefault();

			if (cartItem.Quantity > 1)
			{
				--cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(p => p.ProductId == id);
			}

			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
			TempData["Success"] = "The product has been removed";
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Remove(int id)
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
			cart.RemoveAll(p => p.ProductId == id);
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
			TempData["Success"] = "The product has been removed";
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> Clear(int id)
		{
			HttpContext.Session.Remove("Cart");
			return RedirectToAction("Index");
		}
	}
}
