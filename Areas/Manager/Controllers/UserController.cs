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

namespace server.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class UsersController : Controller
    {
        private readonly serverContext _context;

        public UsersController(serverContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return _context.User != null ?
                        View(await _context.User.ToListAsync()) :
                        Problem("Entity set 'serverContext.User'  is null.");

        }
        // GET: Users/Search
        public async Task<IActionResult> Search(string SearchString) {
            ViewData["CurrentFilter"] = SearchString;
            var users = from user in _context.User
                        select user;
            if (!string.IsNullOrEmpty(SearchString))
            {
                users = users.Where(b => b.Username.Contains(SearchString));
            }
            return View(users);
        }


        // GET: Users/Sort
        public async Task<IActionResult> Sort(string SortOrder)
        {
            ViewData["UsernameSort"] = string.IsNullOrEmpty(SortOrder) ? "username_sort" : "";
            ViewData["StatusSort"] = SortOrder == "Status" ? "status_sort" : "status_sort";
            var users = from user in _context.User
                        select user;

            switch (SortOrder)
            {
                case "username_sort":
                default:
                    users = users.OrderBy(b => b.Username);
                    break;
                case "status_sort":
                    users = users.OrderBy(b => b.Status);
                    break;
            }
            return View(users);
        }


        // GET: Users/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Uid == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.User == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            ViewData["Uid"] = new SelectList(_context.User, "Uid", "Uid", user.Uid);
            return View(user);
        }

        // POST: Manager/User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Uid,Username,Role")] User user)
        {
            if (id != user.Uid)
            {
                return NotFound();
            }

            ViewData["Uid"] = new SelectList(_context.User, "Uid", "Uid", user.Uid);
            return View(user);
        }



        // GET: Users/Suspend/5
        public async Task<IActionResult> Suspend(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _context.User.FirstOrDefaultAsync(m => m.Uid == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Suspend/5
        [HttpPost, ActionName("Suspend")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuspendConfirmed(string id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            if (user.Status == false)
            {
                user.Status = true;
            }
            else
            {
                user.Status = false;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        private bool UserExists(string id)
        {
            return (_context.User?.Any(e => e.Uid == id)).GetValueOrDefault();
        }

    }
}