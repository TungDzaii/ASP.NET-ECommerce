using Microsoft.AspNetCore.Mvc;
using server.Models;
using server.Models.ViewModels;

namespace server.Infrastructure.Components
{
    public class CartIconViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            CartIconViewModel cartIconVM;
            if (cart == null || cart.Count == 0)
            {
                cartIconVM = null;
            }
            else
            {
                cartIconVM = new()
                {
                    NumberOfItems = cart.Sum(x => x.Quantity),
                    Total = cart.Sum(x => x.Quantity * x.Price)
                };
            }
            return View(cartIconVM);
        }
    }
}
