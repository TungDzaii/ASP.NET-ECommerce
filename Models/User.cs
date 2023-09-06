using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Linq;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations.Schema;
using server.Models;


namespace server.Models
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class StrongPasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var password = value as string;
            if (string.IsNullOrEmpty(password))
                return false;

            // Check length
            if (password.Length < 8)
                return false;

            // Check if it contains at least 1 uppercase letter
            if (!password.Any(char.IsUpper))
                return false;

            // Check if it contains at least 1 lowercase letter
            if (!password.Any(char.IsLower))
                return false;

            // Check if it contains at least 1 digit
            if (!password.Any(char.IsDigit))
                return false;

            // Check if it contains at least 1 special character
            if (!Regex.IsMatch(password, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
                return false;

            return true;
        }
    }

    public class User
    {
        [Key]
        public string? Uid { get; set; }
        public string Username { get; set; }
        [StrongPassword(ErrorMessage = "Password must be at least 8 characters long and contain at least 1 uppercase letter, 1 lowercase letter, 1 digit, and 1 special character.")]
        public string Password { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int Role { get; set; }
        public bool Status { get; set; }
        public bool Verified { get; set; }

        public User()
        {
            Uid = Guid.NewGuid().ToString();
            Role = 1;
            // 1: Customer
            // 2: Staff
            // 3. Manager
            Status = false;
            Verified = false;
        }
    }
}
