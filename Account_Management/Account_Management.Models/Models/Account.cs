using Account_Management.Models.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.Models.Models
{
    public class Account
    {
        [Key]
        public string AccountId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public AccountRole Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public AccountStatus IsActive { get; set; }
        public string Token { get; set; }
        public DateTime TokenExpires { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpires { get; set; }

        public User User { get; set; }
    }
}
