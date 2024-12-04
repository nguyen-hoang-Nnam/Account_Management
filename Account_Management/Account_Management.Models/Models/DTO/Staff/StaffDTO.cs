using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Account_Management.Models.Models.DTO.Staff
{
    public class StaffDTO
    {
        public int StaffId { get; set; }
        public int AccountId { get; set; }
        public string Position { get; set; }
        public DateTime HiredDate { get; set; }
    }

}
