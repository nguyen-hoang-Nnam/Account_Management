using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.Models.Models.DTO.User
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public int AccountId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
