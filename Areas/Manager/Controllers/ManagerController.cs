using Microsoft.AspNetCore.Mvc;

namespace server.Areas.Manager.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
