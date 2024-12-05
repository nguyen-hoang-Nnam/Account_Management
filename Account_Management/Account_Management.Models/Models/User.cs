using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.Models.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string AccountId { get; set; }
        public Account Account { get; set; }

        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
