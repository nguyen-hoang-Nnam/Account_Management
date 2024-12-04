using Account_Management.Models.Models.DTO.Admin;
using Account_Management.Models.Models.DTO.Auth;
using Account_Management.Models.Models.DTO.Staff;
using Account_Management.Models.Models.DTO.User;
using Account_Management.Models.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Account_Management.Models.Models.DTO.Account;

namespace Account_Management.Models.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Account, AccountDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Staff, StaffDTO>().ReverseMap();
            CreateMap<Admin, AdminDTO>().ReverseMap();
            CreateMap<RegisterDTO, Account>();
        }
    }
}
