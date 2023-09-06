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
    public abstract class BaseController : Controller
    {
        public readonly serverContext _context;
        public readonly IHttpContextAccessor _httpContextAccessor;
        public User this_user;

        public BaseController(serverContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            this_user = CheckLoginStatus();
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
                    Role = found_user.Role,
					Status = found_user.Status,
					Verified = found_user.Verified,
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

        public User CheckLoginStatus()
        {
            var tokenString = _httpContextAccessor.HttpContext.Session.GetString("JwtToken");
            if (string.IsNullOrEmpty(tokenString))
            {
                return null;
            }
            
            var user = GetUserFromJwtToken(tokenString);
            return user;
        }
    }
}
