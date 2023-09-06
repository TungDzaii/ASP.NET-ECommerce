using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using server.Data;
using server.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Cryptography;
using System.Text;
using server.Infrastructure;
using server.Controllers;
using System.Linq;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace server.Controllers
{

    public class RegisterController : BaseController
    {
        private readonly EmailSender _emailSender;

        public RegisterController(serverContext context, IHttpContextAccessor httpContextAccessor, EmailSender emailSender) : base(context, httpContextAccessor)
        {
            _emailSender = emailSender;
        }
        
        [HttpGet]
        [Route("Register")]
        public IActionResult Index()
        {
            var message = TempData["Message"] as string;
            TempData.Remove("Message"); // Remove the message from TempData to prevent it from persisting across requests

            ViewBag.Message = message; // Pass the message to the view

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Register")]
        public async Task<IActionResult> Register([Bind("Username,Password,Email,FirstName,LastName,Phone")] User user)
        {
            if (ModelState.IsValid)
            {
                string id;
                bool duplication;

                do
                {
                    id = Guid.NewGuid().ToString();
                    duplication = _context.User.Any(u => u.Uid == id);
                }
                while (duplication);

                if (_context.User.Any(u => u.Email == user.Email))
                {
                    TempData["msg"] = "Email already taken.";
                    return RedirectToAction("Index", "Register");
                }
                else if (_context.User.Any(u => u.Username == user.Username))
                {
                    TempData["msg"] = "Username already taken.";
                    return RedirectToAction("Index", "Register");
                }

                else{
                    string hashedPassword = HashPassword(user.Password);
                    user.Password = hashedPassword;
                    user.Uid = id;
                    
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    string token = GenerateJwtToken(user);
                    _emailSender.SendEmail(user.Email, token);
                    TempData["msg"] = "Registration succeeded! A verification email has been sent to your email address " + user.Email + ". Please follow the instructions to activate your account.";
                    return RedirectToAction("Index", "Register");
                }
            }
            else
            {
                // Registration failed
                var errorMessages = new List<string>();
                foreach (var modelStateValue in ModelState.Values)
                {
                    foreach (var error in modelStateValue.Errors)
                    {
                        errorMessages.Add(error.ErrorMessage);
                    }
                }
                TempData["msg"] = errorMessages[0];

                return RedirectToAction("Index", "Register");
            }
        }

        private string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
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

        public User GetUserFromJwtToken(string tokenString)
        {
            try{
                // Verify and decode the JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("slkdjapowue1-qopwasjd012hueboiqasklncbvoilwjrnb`92wk`j1ni2w8qs9pd"));

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = true,
                    ValidIssuer = "your-issuer",
                    ValidateAudience = true,
                    ValidAudience = "your-audience",
                };

                SecurityToken validatedToken;
                
                var principal = tokenHandler.ValidateToken(tokenString, validationParameters, out validatedToken);
                if (principal == null){
                    return null;
                }
                // Extract user information from the token claims
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
                
                var username = principal.FindFirst(ClaimTypes.Name).Value;
                
                // Create a user object with the extracted information
                var found_user = _context.User.FirstOrDefault(u => u.Uid == userId);
                var user = new User
                {
                    Uid = userId,
                    Username = username,
                    FirstName = found_user.FirstName,
                    LastName = found_user.LastName,
                    Email = found_user.Email,
                    Phone = found_user.Phone,
                };

                return user;
            }
            catch (SecurityTokenException ex)
            {
                // Token validation failed
                TempData["msg"] = $"Token validation failed: {ex.Message}";
                return null;
                // Handle the error or perform any necessary actions
            }
        }

        [HttpGet]
        [Route("/verify/{token}")]
        public async Task<IActionResult> VerifyUser(string token)
        {
            var user = GetUserFromJwtToken(token);
            if (user != null){
                user.Verified = true;
                await _context.SaveChangesAsync();
                TempData["msg"] = "User verified successfully!";
                return View();
            }
            TempData["msg"] = "Something went wrong. Please try again.";
            return View();
        }

    }
}