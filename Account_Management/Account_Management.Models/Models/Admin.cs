using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.Models.Models
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string AccountId { get; set; }
        public Account Account { get; set; }

        public string Department { get; set; }
    }
}
