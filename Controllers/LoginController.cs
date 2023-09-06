using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.Data;
using server.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using server.Controllers;

namespace server.Controllers
{
    public class LoginController : BaseController
    {
        public LoginController(serverContext context, IHttpContextAccessor httpContextAccessor) : base(context, httpContextAccessor)
        {

        }

        [HttpGet]
        [Route("Login")]
        public IActionResult Index()
        {
            var message = TempData["Message"] as string;
            TempData.Remove("Message"); // Remove the message from TempData to prevent it from persisting across requests

            ViewBag.Message = message; // Pass the message to the view

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            var user = _context.User.FirstOrDefault(u => u.Username == username);
            
			if (user != null && VerifyPassword(password, user.Password))
            {
				
				var tokenString = GenerateJwtToken(user);
				

				HttpContext.Session.SetString("JwtToken", tokenString);

                if (user.Role == 1)
                {
					Console.WriteLine(user.Role);
					return RedirectToAction("Index", "Home");
                }
                else if (user.Role == 2)
                {
					Console.WriteLine(user.Role);
					return Redirect("/staff/staff");
                }
                else
                {
					return Redirect("/Manager/Users");
                }
            }

            // Login failed
            // You can handle the failed login attempt here, such as displaying an error message on the login page.
            TempData["Error"] = "Invalid username or password";
            return RedirectToAction("Index", "Login");
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {
            // Decode the Base64-encoded stored password to get the byte array
            byte[] hashedBytes = Convert.FromBase64String(storedPassword);

            // Extract the salt from the hashed password
            byte[] salt = new byte[16];
            Array.Copy(hashedBytes, 0, salt, 0, 16);

            // Hash the entered password with the same salt
            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000);
            byte[] enteredHash = pbkdf2.GetBytes(20);

            // Compare the entered hash with the stored hash
            for (int i = 0; i < 20; i++)
            {
                if (hashedBytes[i + 16] != enteredHash[i])
                {
                    return false;
                }
            }

            return true;
        }

        public string GenerateJwtToken(User user)
        {
            // Create claims for the user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Uid),
                new Claim(ClaimTypes.Name, user.Username),
            };

            // Generate a security key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("slkdjapowue1-qopwasjd012hueboiqasklncbvoilwjrnb`92wk`j1ni2w8qs9pd"));

            // Generate a signing credential using the key
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            // Create a JWT token
            var token = new JwtSecurityToken(
                issuer: "your-issuer",
                audience: "your-audience",
                claims: claims,
                expires: DateTime.Now.AddDays(1), // Token expiration time
                signingCredentials: signingCredentials
            );

            // Serialize the token to a string
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        [HttpGet("/logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("JwtToken");
            return Redirect("/login");
        }
    }
}
