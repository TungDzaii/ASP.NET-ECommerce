using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using server.Controllers;
using server.Data;
using server.Models;

namespace server.Areas.Staff.Controllers
{
	[Area("Staff")]
	public class StaffController : BaseController
    {

        public StaffController(serverContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }
        
        public IActionResult Index()
        {
            Console.WriteLine(this_user.Role);
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
    }
}