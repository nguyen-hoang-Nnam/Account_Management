using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.Models.Models
{
    public class Staff
    {
        public int StaffId { get; set; }
        public string AccountId { get; set; }
        public Account Account { get; set; }

        public string Position { get; set; }
        public DateTime HiredDate { get; set; }
    }
}
